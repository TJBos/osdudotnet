# osdudotnet

A simple client for the [OSDU](https://community.opengroup.org/osdu) data platform, written in C#.

## Contents

- [Clients](#clients)
  - [SimpleOsduClient](#simpleosduclient)
  - [AwsServicePrincipalOsduClient](#awsserviceprincipalosduclient)
  - [AwsOsduClient](#awsosduclient)
- [Currently supported methods](#currently-supported-methods)
- [Tests](#tests)
- [Usage](#usage)
  - [Instantiating the SimpleOsduClient](#instantiating-the-simpleosduclient)
  - [Instantiating the AwsServicePrincipalOsduClient](#instantiating-the-awsosduclient)
  - [Instantiating the AwsOsduClient](#instantiating-the-awsosduclient)
  - [Using the client](#using-the-client)
    - [Search for records by query](#search-for-records-by-query)
    - [Search with paging](#search-with-paging)
    - [Get a record](#get-a-record)
    - [Upsert records](#upsert-records)
    - [List groupmembership for the current user](#list-groups)
    - [List membership of a particular group](#list-membership)
    - [Add a user to a particular group](#add-group)
- [Release Notes](release-notes.md)

## Clients

Choose the client that best meets your needs. The same methods are all supported for each.

### SimpleOsduClient

BYOT: Bring your own token. Great for backend service or business logic that supplements a
front-end application.

This client assumes you are obtaining a token yourself (e.g. via your application's
login form or other mechanism. With this SimpleOsduClient, you simply provide that token.
With this simplicity, you are also then responsible for refreeshing the token as needed and
re-instantiating the client with the new token or by providing the authentication client id and secret as well as a refresh token and token url
and allowing the client to attempt the refresh automatically.

### AwsOsduServicePrincipalClient

Good for batch tasks that don't have an interactive front-end. Token management is handled
with the AWS SDK directly through the Cognito service. You have to supply additional arguments for this.

For OSDU on AWS, this client is usually simpler than the AwsOsduClient as long as you have IAM credentials to access the necessary resources. You only need to provide the OSDU resource_prefix, region, and a named AWS profile that has the necessary IAM access. Alternatively, you can provide the Cognito Service Principal 'ClientWithCredentials' client id and corresponding client secret, as well as the Cognito Token Url.

This client takes care of automatic refresh.

### AwsOsduClient

Good for batch tasks that don't have an interactive front-end or desktop clients or other local or server-side instances where users can authenticate with their personal credentials. It is not recommended to store passwords unencrypted in code, configuration or storage. Token management is handled
with the AWS SDK directly through the Cognito service. You have to supply additional arguments for this.

For OSDU on AWS, this client is useful in the case where you may want to perform actions as a specific OSDU user rather than as the ServicePrincipal. To instantiate this client, you need to provide the Cognito Userpool Id, as well as the Cognito Client Id and Client Secret. The client authenticates by passing in the username and password of the Cognito user.
If the Cognito Token Url is provided, automatic refresh is taken care of.

## Currently supported methods

- [search]
  - Query
  - QueryWithPaging
  - QuickQuery
- [storage]
  - GetRecord
  - GetRecords
  - GetAllRecordVersions
  - GetRecordVersion
  - StoreRecords
  - DeleteRecord
- [dataset]
  - GetStorageInstructions
  - RegisterDatasetFile
  - GetRetrievalInstructions
- [ingestion]
  - IngestManifest
  - CheckIngestionRunStatus
- [schema]
  - Kinds

## Tests

To run the tests, add an appsettings.tests.json file under the root of the /Tests directory, with the JSON keys corresponding to the OsduConfiguration class fields.

## Usage

### Instantiating the SimpleOsduClient

```csharp
using OsduLib.Client;

string dataPartitionId = 'osdu';
string apiBaseUrl = "https://your.api.base_url.com";
string token = "token-received-from-front-end-app";

var client = new SimpleOsduClient(dataPartitionId, apiBaseUrl, token);

// Or pass in a refresh token and refresh token url (usually ending in e.g. oauth2/token), as well as the authentication client id and secret like so:
var client = new SimpleOsduClient(dataPartitionId, apiBaseUrl, refreshToken, refreshUrl, clientId, clientSecret);
```

### Instantiating the AwsServicePrincipalOsduClient

```csharp
using OsduLib.Client;
using OsduLib.Models;

// create a new config object. BaseApiUrl and DataPartitionId are required. Then provide either a profile, region, and resource prefix OR the cognito svp client id, secret and token url.

var config = new OsduAWSEnvironment {
    BaseApiUrl = "https://your-api-base-url.com",
    DataPartitionId = "osdu",
    Region = "us-east-1",
    Profile = "my-named-aws-cli-profile",
    ResourcePrefix = "osdur3m11",
    ServicePrincipalClientId = "client-with-credentials-client-id",
    ServicePrincipalClientSecret = "client-with-credentials-client-secret",
    TokenUrl = ".../oauth2/token"
};

var client = new AwsServicePrincipalOsduClient(config);
```

### Instantiating the AwsOsduClient

```csharp
using OsduLib.Client;
using OsduLib.Models;

// create a new config object. BaseApiUrl and DataPartitionId, Region, Cognito pool id, client id and secret are required. If the token url is provided, the client can automatically refresh (this is the only way, since username and password are not stored in memory).

var config = new OsduAWSEnvironment {
    BaseApiUrl = "https://your-api-base-url.com",
    DataPartitionId = "osdu",
    Region = "us-east-1",
    UserPoolId = "Cognito pool id",
    UserPoolClientId = "client-id",
    UserPoolClientSecret = "client-secret",
    TokenUrl = ".../oauth2/token" // optional
};

var client = new AwsOsduClient(config, "myCognitoUserName", "myCognitoPassword");
```

### Automatically re-authorizing the client

Each client will automatically attempt to re-authorize when its access token expires. In order for this re-authorization to succeed, you will need to supply the client with additional parameters

#### Simple Client:

1. OSDU_CLIENTWITHSECRET_ID
1. OSDU_CLIENTWITHSECRET_SECRET
1. REFRESH_TOKEN
1. REFRESH_URL

#### AWS Client:

1. REFRESH_URL

#### Service Principal:

N/A--this client can re-authorize with just the variables needed for it to instantiate

### Using the client

Below are just a few usage examples. See all the implemented method signatures in the corresponding services under the /Services directory.

#### Search for records by query

```csharp

```

#### Get a record

```csharp
string recordId = "opendes:doc:123456789";
var result = await client.Storage.GetRecord(recordId);
// { 'id': 'opendes:doc:123456789', 'kind': ..., 'data': {...}, 'acl': {...}, .... }
```

#### Create/update records

TODO: create more documentation for services...
