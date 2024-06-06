using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserModelLayer
{
    public class ResponseModel
    {
        public bool status { get; set; } = true;
        public string message { get; set; } = string.Empty;
        public string data { get; set; } = string.Empty ;
    }
}
