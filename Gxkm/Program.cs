using System;
using System.Windows.Forms;
using System.Reflection;
using Gxkm.Util;

namespace Gxkm
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex;
            if ((ex = (e.ExceptionObject as Exception)) == null)
            {
                return;
            }
            LogWriter.Log(ex, "Current Domain Unhandled Exception - Unknown", null, false);
            try
            {
                MessageBoxEx.Show("程序当前遇到未处理的异常", "提示", MessageBoxButtons.OK, new[] { "确定" });
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// C#将DLL嵌入到exe当中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            var rm = new System.Resources.ResourceManager("Gxkm" + ".Properties.Resources", Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }
    }
}
