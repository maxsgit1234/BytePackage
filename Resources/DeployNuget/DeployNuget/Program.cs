using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeployNuget
{
    class Program
    {
        static void Main(string[] args)
        {
            string version = "1.0.2";
            string dir = Path.GetFullPath(@"..\..\..\..\..");
            string slnFile = dir + "\\Source\\BytePack.sln";
            string stdFile = dir + "\\Source\\BytePack.NET\\BytePack.NET.csproj";
            string bldDir = dir + "\\Resources\\DeployNuget\\Build";
            string outDir = dir + "\\Resources\\DeployNuget\\NuPkg";
            string nuDir = dir + "\\Resources\\DeployNuget\\DeployNuget";
            string stdText = File.ReadAllText(stdFile);

            // Update version numbers in each of the 3 files.
            stdText = ReplaceTag(stdText, "Version", version);
            File.WriteAllText(stdFile, stdText);

            // Build solution
            InstallMethods.BuildSln(
                slnFile, true, true, bldDir, null, Console.WriteLine);

            // Copy Nuget files to an output directory...
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            File.Copy(
                bldDir + "\\Release\\BytePack." + version + ".nupkg",
                outDir + "\\BytePack." + version + ".nupkg", true);
            
            Console.WriteLine("DONE");
        }

        private static string ReplaceTag(string text, string tag, string value)
        {
            int k0 = text.IndexOf("<" + tag + ">");
            int k1 = text.IndexOf("</" + tag + ">", k0);

            int ix0 = k0 + tag.Length + 2;
            int ix1 = k1;

            string ret = text.Substring(0, ix0)
                + value + text.Substring(ix1, text.Length - ix1);
            return ret;
        }

    }
}
