using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDevOps.Core.IO;

namespace WebAPIDevOps.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            NLogHelper.DefineLogProfile(true, true);
            NLogHelper.EnableLogging();
            WebServer server = new WebServer();
            server.Initialize();
            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
