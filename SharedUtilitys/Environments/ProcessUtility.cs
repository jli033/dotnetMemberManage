using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SharedUtilitys.Environments
{
    public class ProcessUtility
    {
        public static ProcessUtility GetInstance()
        {
            return new ProcessUtility();
        }

        public bool IsMultipleStartup(bool isDisplayMessage)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                if (isDisplayMessage)
                {
                    MessageBox.Show(String.Format("{0}は起動中です。", Application.ProductName), 
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return true;
            }

            return false;
        }
    }
}
