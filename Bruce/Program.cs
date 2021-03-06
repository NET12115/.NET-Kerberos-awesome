using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Kerberos.NET.CommandLine.Dns;
using Kerberos.NET.Dns;

namespace Kerberos.NET.CommandLine
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DnsQuery.RegisterImplementation(new PlatformIndependentDnsClient());

            var assembly = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName);

            string loadingModule = null;

            if (!"bruce".Equals(assembly, StringComparison.InvariantCultureIgnoreCase))
            {
                loadingModule = assembly;
            }

            var argvIndex = Environment.CommandLine.IndexOf(' ');
            string argv = null;

            if (argvIndex >= 0)
            {
                argv = Environment.CommandLine.Substring(argvIndex + 1);
            }

            var shell = new BruceConsoleShell()
            {
                CommandLine = argv,
                LoadingModule = loadingModule,
                Verbose = args.Any(a => string.Equals("--verbose", a, StringComparison.InvariantCultureIgnoreCase)),
                Silent = args.Any(a => string.Equals("--silent", a, StringComparison.InvariantCultureIgnoreCase))
            };

            shell.Start().Wait();
        }
    }
}
