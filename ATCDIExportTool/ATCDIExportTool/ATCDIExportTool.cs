using Bentley.MstnPlatformNET;
using System;
using System.Windows.Forms;

namespace ATCDIExportTool
{
    [AddIn(MdlTaskID = "ATCDIExportTool")]
    public sealed class ATCDIExportTool : AddIn
    {
        public static ATCDIExportTool exportTool = null;

        private ATCDIExportTool(System.IntPtr mdlDesc) : base(mdlDesc)
        {
            exportTool = this;
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
        }

        protected override int Run(string[] commandLine)
        {
            return 0;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
        }

    }
}
