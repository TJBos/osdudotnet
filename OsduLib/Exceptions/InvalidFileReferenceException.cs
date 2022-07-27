using OsduLib.Models.Dataset;

namespace OsduLib.Exceptions;
public class InvalidFileReferenceException : Exception
{
    public readonly IList<RetrievalInstructionError> InstructionErrors;
    public InvalidFileReferenceException(string message, IList<RetrievalInstructionError> errors): base(message){
        InstructionErrors = errors;
    }
}