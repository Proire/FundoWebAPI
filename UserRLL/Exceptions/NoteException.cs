using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRLL.Exceptions
{
    public class NoteException(string message) : ApplicationException(message)
    {
    }
}
