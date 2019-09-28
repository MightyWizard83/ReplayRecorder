using System;

namespace ReplayRecorder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "obs64.exe";
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = @"C:\Program Files\obs-studio\bin\64bit\";

            startInfo.Arguments = @" --startrecording --minimize-to-tray --profile ""Game"" --collection ""Test"" ";


            process.StartInfo = startInfo;
            process.Start();

        }
    }
}
