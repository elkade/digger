using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroDigger.Logging
{
    interface ILogger
    {
        void Log(string message);
        ILogger Instance { get; set; }
    }
}
