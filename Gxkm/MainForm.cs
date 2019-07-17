using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace Gxkm
{
    public partial class MainForm : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        private void MainForm_Load(object sender, EventArgs e)
        {
            var wb = (SHDocVw.WebBrowser)webBrowser1.ActiveXInstance;
            wb.BeforeNavigate2 += Wb_BeforeNavigate2;
        }

        public MainForm()
        {
            InitializeComponent();

            webBrowser1.Url = new Uri("https://gongxukemu.cn/search.html");
        }

        private void Wb_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            var postDataText = System.Text.Encoding.ASCII.GetString(PostData as byte[]);

            var url = URL.ToString();
            var cookieStr = webBrowser1.Document.Cookie;

            var cookstr = cookieStr.Split(';');
            foreach (var str in cookstr)
            {
                var cookieNameValue = str.Split('=');
                var name = cookieNameValue[0].Trim().ToString();
                var data = cookieNameValue[1].Trim().ToString();

                // 核心代码，修改查询次数
                if ("showTime".Equals(name)) { data = "1"; }

                InternetSetCookie(url, name, data);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("确定退出？", "友情提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = "https://github.com/topcss/gxkmtk";

            var result = MessageBox.Show($@"
全国公需科目考试题库 无限搜索版

请有钱的老哥，还是去支持下官网的作者吧。
我看他淘宝店的收入，才几百块钱，可怜的同行。

{url}
Copyright ©2019 爱脑图团队
", "关于", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", url);
                }
                catch (Exception)
                {
                    Process.Start("iexplore.exe", url);
                }
            }
        }


        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定退出？", "友情提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}