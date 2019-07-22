using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Gxkm.Util
{
    public static class LogWriter
    {
        public static void Log(Exception ex, string title, object aditional = null, bool isFallback = false)
        {
            try
            {
                string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gxkm\\Logs");
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                string text2 = Path.Combine(text, DateTime.Now.ToString("yy_MM_dd") + ".txt");
                string text3 = Path.Combine(text, DateTime.Now.ToString("yy_MM_dd hh_mm_ss_fff") + ".txt");
                FileStream fileStream = null;
                bool flag = false;
                try
                {
                    fileStream = new FileStream(text2, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
                catch (Exception)
                {
                    flag = true;
                    fileStream = new FileStream(text3, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
                fileStream.Dispose();
                using (FileStream fileStream2 = new FileStream(flag ? text3 : text2, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream2))
                    {
                        streamWriter.WriteLine("► Title - " + Environment.NewLine + "\t" + title);
                        streamWriter.WriteLine("▬ Message - " + Environment.NewLine + "\t" + ex.Message);
                        streamWriter.WriteLine(string.Format("○ Type - {0}\t{1}", Environment.NewLine, ex.GetType()));
                        TextWriter textWriter = streamWriter;
                        string format = "♦ [Version] Date/Hour - {0}\t[{1}] {2}";
                        object[] array = new object[3];
                        array[0] = Environment.NewLine;
                        int num = 1;
                        array[num] = Environment.Version;
                        array[2] = DateTime.Now;
                        textWriter.WriteLine(string.Format(format, array));
                        streamWriter.WriteLine("▲ Source - " + Environment.NewLine + "\t" + ex.Source);
                        streamWriter.WriteLine(string.Format("▼ TargetSite - {0}\t{1}", Environment.NewLine, ex.TargetSite));
                        BadImageFormatException ex2 = ex as BadImageFormatException;
                        if (ex2 != null)
                        {
                            streamWriter.WriteLine("► Fuslog - " + Environment.NewLine + "\t" + ex2.FusionLog);
                        }
                        if (aditional != null)
                        {
                            streamWriter.WriteLine(string.Format("◄ Aditional - {0}\t{1}", Environment.NewLine, aditional));
                        }
                        streamWriter.WriteLine("♠ StackTrace - " + Environment.NewLine + ex.StackTrace);
                        if (ex.InnerException != null)
                        {
                            streamWriter.WriteLine();
                            streamWriter.WriteLine("▬▬ Message - " + Environment.NewLine + "\t" + ex.InnerException.Message);
                            streamWriter.WriteLine(string.Format("○○ Type - {0}\t{1}", Environment.NewLine, ex.InnerException.GetType()));
                            streamWriter.WriteLine("▲▲ Source - " + Environment.NewLine + "\t" + ex.InnerException.Source);
                            streamWriter.WriteLine(string.Format("▼▼ TargetSite - {0}\t{1}", Environment.NewLine, ex.InnerException.TargetSite));
                            streamWriter.WriteLine("♠♠ StackTrace - " + Environment.NewLine + ex.InnerException.StackTrace);
                            if (ex.InnerException.InnerException != null)
                            {
                                streamWriter.WriteLine();
                                streamWriter.WriteLine("▬▬▬ Message - " + Environment.NewLine + "\t" + ex.InnerException.InnerException.Message);
                                streamWriter.WriteLine(string.Format("○○○ Type - {0}\t{1}", Environment.NewLine, ex.InnerException.InnerException.GetType()));
                                streamWriter.WriteLine("▲▲▲ Source - " + Environment.NewLine + "\t" + ex.InnerException.InnerException.Source);
                                streamWriter.WriteLine(string.Format("▼▼▼ TargetSite - {0}\t{1}", Environment.NewLine, ex.InnerException.InnerException.TargetSite));
                                streamWriter.WriteLine("♠♠♠ StackTrace - " + Environment.NewLine + "\t" + ex.InnerException.InnerException.StackTrace);
                                if (ex.InnerException.InnerException.InnerException != null)
                                {
                                    streamWriter.WriteLine();
                                    streamWriter.WriteLine("▬▬▬▬ Message - " + Environment.NewLine + "\t" + ex.InnerException.InnerException.InnerException.Message);
                                    streamWriter.WriteLine(string.Format("○○○○ Type - {0}\t{1}", Environment.NewLine, ex.InnerException.InnerException.InnerException.GetType()));
                                    streamWriter.WriteLine("▲▲▲▲ Source - " + Environment.NewLine + "\t" + ex.InnerException.InnerException.InnerException.Source);
                                    streamWriter.WriteLine(string.Format("▼▼▼▼ TargetSite - {0}\t{1}", Environment.NewLine, ex.InnerException.InnerException.InnerException.TargetSite));
                                    streamWriter.WriteLine("♠♠♠♠ StackTrace - " + Environment.NewLine + "\t" + ex.InnerException.InnerException.InnerException.StackTrace);
                                }
                            }
                        }
                        streamWriter.WriteLine();
                        streamWriter.WriteLine("----------------------------------");
                        streamWriter.WriteLine();
                    }
                }
            }
            catch (Exception)
            {
                if (!isFallback)
                {
                    LogWriter.Log(ex, title, aditional, true);
                }
            }
        }
    }
}
