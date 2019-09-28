using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ReplayRecorder
{
    class Program
    {
        static void Main(string[] args)
        {
            string wowsPath = @"C:\Games\World_of_Warships_EU";
            string replayPath = @"C:\Games\World_of_Warships_EU\replays\0.8.8.1";
            string replayFilename = @"20190926_001620_PASC020-Des-Moines-1948_47_Sleeping_Giant.wowsreplay";

            string obsPath = @"C:\Program Files\obs-studio\bin\64bit\";
            string obsProfile = "Game";
            string obsCollection = "Test";

            int typicalWoWSwsStartupTime = 25;


            Console.WriteLine("Starting WoWS Replay...");
            startWoWsReplay(wowsPath, replayPath, replayFilename);

            //Waiting typical startup time
            Console.WriteLine("Waiting for WoWS Startup...");
            Thread.Sleep(typicalWoWSwsStartupTime * 1000);

            Console.WriteLine("Starting OBS...");
            startRecordingOBS(obsPath, obsProfile, obsCollection);

            //wait for WOWS to stop
            waitWoWSEnd();

            Console.WriteLine("Killing OBS...");
            killOBS();
        }

        static private void startWoWsReplay(string wowsPath, string replayPath, string replayFilename)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "WorldOfWarships.exe";
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = wowsPath;
            startInfo.Arguments = PathAddBackslash(replayPath) + replayFilename;


            process.StartInfo = startInfo;
            process.Start();
        }

        static private void startRecordingOBS(string obsPath, string obsProfile, string obsCollection)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "obs64.exe";
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = PathAddBackslash(obsPath);

            startInfo.Arguments = @" --startrecording --minimize-to-tray --profile """+ obsProfile + @""" --collection """ + obsCollection + @""" ";


            process.StartInfo = startInfo;
            process.Start();
        }

        static private void waitWoWSEnd()
        {
            Boolean isRunning = true;
            while (isRunning)
            {
                isRunning = false;
                Thread.Sleep(1000);

                foreach (var process in Process.GetProcessesByName("WorldOfWarships64"))
                {
                    isRunning = true;
                    Console.WriteLine("Wows is running");
                    continue;
                }
            }

            Console.WriteLine("Wows not running anymore");
        }

        static private void killOBS()
        {
            foreach (var process in Process.GetProcessesByName("obs64"))
            {
                process.Kill();
            }
        }


        static string PathAddBackslash(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            path = path.TrimEnd();

            if (PathEndsWithDirectorySeparator())
                return path;

            return path + GetDirectorySeparatorUsedInPath();

            bool PathEndsWithDirectorySeparator()
            {
                if (path.Length == 0)
                    return false;

                char lastChar = path[path.Length - 1];
                return lastChar == Path.DirectorySeparatorChar
                    || lastChar == Path.AltDirectorySeparatorChar;
            }

            char GetDirectorySeparatorUsedInPath()
            {
                if (path.Contains(Path.AltDirectorySeparatorChar))
                    return Path.AltDirectorySeparatorChar;

                return Path.DirectorySeparatorChar;
            }
        }
    }
}
