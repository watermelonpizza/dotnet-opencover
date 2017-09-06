using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace dotnet_opencover
{
    class Program
    {
        static string OpenCoverExecutable = "OpenCover.Console.exe";

        static int Main(string[] args)
        {
            var userProfile = Environment.GetEnvironmentVariable("userprofile");

            if (string.IsNullOrEmpty(userProfile))
            {
                Console.Error.WriteLine($"Couldn't find userprofile folder at environment variable 'userprofile'. Used to find packages in %userprofile%/.nuget/packages/");
                return 1;
            }

            var pathToExe = Path.Combine(userProfile, ".nuget", "packages", "opencover");

            if (!Directory.Exists(pathToExe))
            {
                Console.Error.WriteLine($"Couldn't find opencover folder at {pathToExe}. Have you used dotnet restore on a project with the opencover dependancy?");
                return 2;
            }

            // Can specify a specific version using "dotnet opencover --opencover-verison x.x.x <opencover arguments>"
            if (args?.Length > 1 && args[0] == "--opencover-version")
            {
                var availableVersions = Directory.GetDirectories(pathToExe);

                pathToExe = Path.Combine(pathToExe, args[1]);

                if (!Directory.Exists(pathToExe))
                {
                    Console.Error.WriteLine($"Couldn't find the opencover version at {pathToExe}. Version specified: {args[1]}. Available: '{string.Join("', '", availableVersions)}'");
                }

                args = args.Skip(2).ToArray();
            }
            else
            {
                pathToExe = Path.Combine(pathToExe, Directory.GetDirectories(pathToExe).OrderByDescending(d => d).First());
            }

            // Older versions don't use the nuget tools folder system
            if (Directory.Exists(Path.Combine(pathToExe, "tools")))
            {
                pathToExe = Path.Combine(pathToExe, "tools");
            }

            pathToExe = Path.Combine(pathToExe, OpenCoverExecutable);

            if (!File.Exists(pathToExe))
            {
                Console.Error.WriteLine($"Couldn't find {OpenCoverExecutable} at {pathToExe}");
                return 3;
            }            

            string[] escapedArgs = new string[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains(" "))
                    escapedArgs[i] = $@"""{args[i]}""";
                else
                    escapedArgs[i] = args[i];
            }

            ProcessStartInfo psi = new ProcessStartInfo
            {
                Arguments = string.Join(" ", escapedArgs),
                FileName = pathToExe
            };

            var process = Process.Start(psi);
            process.WaitForExit();

            return process.ExitCode;
        }
    }
}
