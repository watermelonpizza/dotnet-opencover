using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace dotnet_opencover
{
    class Program
    {
        static string DotNugetFolder = ".nuget";
        static string NugetPackagesFolder = "packages";
        static string OpenCoverFolderName = "opencover";
        static string ToolFolder = "tools";
        static string OpenCoverExecutable = "OpenCover.Console.exe";

        static int Main(string[] args)
        {
            var userProfile = Environment.GetEnvironmentVariable("userprofile");

            var pathToExe = Path.Combine(userProfile, DotNugetFolder);
            pathToExe = Path.Combine(pathToExe, NugetPackagesFolder);
            pathToExe = Path.Combine(pathToExe, OpenCoverFolderName);

            if (!Directory.Exists(pathToExe))
            {
                Console.Error.WriteLine($"Couldn't find opencover folder at {pathToExe}. Have you used dotnet restore on a project with the opencover dependancy?");
                return 1;
            }

            // Get the highest value (will have argument later)
            pathToExe = Path.Combine(pathToExe, Directory.GetDirectories(pathToExe).OrderByDescending(d => d).First());

            pathToExe = Path.Combine(pathToExe, ToolFolder);
            pathToExe = Path.Combine(pathToExe, OpenCoverExecutable);

            if (!File.Exists(pathToExe))
            {
                Console.Error.WriteLine($"Couldn't find {OpenCoverExecutable} at {pathToExe}");
                return 1;
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
