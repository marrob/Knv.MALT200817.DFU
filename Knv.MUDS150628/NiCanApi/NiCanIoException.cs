using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knv.MUDS150628.NiCanApi
{
    public class NiCanIoException : Exception
    {
        public NiCanIoException()
        {
        }

        public NiCanIoException(int status):base(NiCanTools.StatusToString(status))
        {

        }

        public NiCanIoException(string message)
            : base(message)
        {
        }

        public NiCanIoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
