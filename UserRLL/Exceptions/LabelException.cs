﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRLL.Exceptions
{
    public class LabelException : ApplicationException
    {
        public LabelException(string message):base(message) { }
    }
}