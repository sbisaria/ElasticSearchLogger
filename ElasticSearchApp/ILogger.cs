﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp
{
    public interface ILogger
    {
        void WriteLog(LogEntry logEntry);
    }
}
