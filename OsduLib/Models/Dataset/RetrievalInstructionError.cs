using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.Dataset
{
    public class RetrievalInstructionError
    {
        public string FileId;
        public RetrievalInstruction RetrievalInstruction;
        public Exception Error;
    }
}
