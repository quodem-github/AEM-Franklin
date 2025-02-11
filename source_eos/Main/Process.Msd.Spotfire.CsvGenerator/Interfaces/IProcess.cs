using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator.Interfaces
{
    public interface IProcess
    {
        void WriteLineApp(string message, int level);
    }
}