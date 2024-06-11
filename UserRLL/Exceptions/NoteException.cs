using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRLL.Exceptions
{
    public class NoteException : ApplicationException
    {
        public NoteException(string message) : base(message) { }
    }
}
