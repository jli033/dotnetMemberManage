using System;
using System.Diagnostics;

namespace SharedUtilitys.Environments
{
    public class NetuseHelper
    {
        public static NetuseHelper GetInstance()
        {
            return new NetuseHelper();
        }

        public bool SetNetuse(string dir, string userName, string password)
        {
            try
            {
                const string format = @" use {0} {2} /user:{1}";
                var result = startCommand(String.Format(format, dir, userName, password));
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool startCommand(string option)
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "net",
                        CreateNoWindow = false,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = option
                    }
                };

                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
