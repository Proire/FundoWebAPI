using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRLL.Exceptions
{
    public class NoteLabelException(string msg) : ApplicationException(msg)
    {
    }
}
