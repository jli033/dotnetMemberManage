using System;
using System.Threading;
using System.Windows.Forms;

namespace SharedUtilitys.Exceptions
{
    public class ExceptionManager
    {
        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var message = String.Format("エラー詳細{0}{1}{0}{2}", Environment.NewLine, e.Exception.Message, e.Exception.StackTrace);

            var f = new ErrorMessageForm { ErrorDetail = message };
            f.ShowDialog();
            f.Dispose();

            Application.Exit();
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                ((Exception)e.ExceptionObject).Message,
                @"UnhandledException",
                MessageBoxButtons.OK,
                MessageBoxIcon.Stop);

            Environment.Exit(-1);
        }
    }
}
