using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using OsduLib.Models;

namespace OsduLib.Services.Authentication;

public class UserCredentialService
{
    private readonly string? _clientSecret;
    private readonly IAmazonCognitoIdentityProvider _cognitoIdentityProviderClient;
    private readonly Func<CognitoUserPool> _cognitoUserPoolResolver;

    public UserCredentialService(
        OsduAWSEnvironment osduEnv
        , IAmazonCognitoIdentityProvider cognitoIdentityProviderClient
    )
    {
        _clientSecret = osduEnv.UserPoolClientSecret;
        _cognitoIdentityProviderClient = cognitoIdentityProviderClient;
        // delays resolution of user pool ids from environment data
        _cognitoUserPoolResolver = () => new CognitoUserPool(
            osduEnv.UserPoolId,
            osduEnv.UserPoolClientId,
            _cognitoIdentityProviderClient,
            _clientSecret
        );
    }

    public async Task<UserCredentials> GetUserCredentials(string username, string password)
    {
        var cognitoUserPool = _cognitoUserPoolResolver();
        var user = new CognitoUser(username, cognitoUserPool.ClientID, cognitoUserPool, _cognitoIdentityProviderClient,
            _clientSecret);
        var authRequest = new InitiateSrpAuthRequest
        {
            Password = password
        };
        var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
        return new UserCredentials(user, authResponse);
    }


    public class UserCredentials : ITokenProvider
    {
        private readonly CognitoUser _cognitoUser;

        public UserCredentials(CognitoUser cognitoUser, AuthFlowResponse authResponse)
        {
            _cognitoUser = cognitoUser;
            AuthResponse = authResponse;
        }

        public AuthFlowResponse AuthResponse { get; private set; }

        public Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrEmpty(AuthResponse.AuthenticationResult?.AccessToken));
        }

        public Task<TokenResponse> GetToken()
        {
            return Task.FromResult(new TokenResponse
            {
                AccessToken = AuthResponse.AuthenticationResult.AccessToken,
                ExpiresIn = AuthResponse.AuthenticationResult.ExpiresIn,
                TokenType = AuthResponse.AuthenticationResult.TokenType,
                Username = _cognitoUser.Username
            });
        }

        public async Task RespondToChallenge(string codeOrPassword)
        {
            if (AuthResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                AuthResponse = await _cognitoUser.RespondToNewPasswordRequiredAsync(
                    new RespondToNewPasswordRequiredRequest
                    {
                        SessionID = AuthResponse.SessionID,
                        NewPassword = codeOrPassword
                    });
            else if (AuthResponse.ChallengeName == ChallengeNameType.SMS_MFA)
                AuthResponse = await _cognitoUser.RespondToSmsMfaAuthAsync(new RespondToSmsMfaRequest
                {
                    SessionID = AuthResponse.SessionID,
                    MfaCode = codeOrPassword
                }).ConfigureAwait(false);
        }

        public bool RequiresChallengeCode()
        {
            return AuthResponse.AuthenticationResult == null &&
                   (AuthResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED ||
                    AuthResponse.ChallengeName == ChallengeNameType.SMS_MFA);
        }
    }
}