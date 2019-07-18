using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

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

            // 启用 ie11 模式，低版本ie显示有问题
            IEVersion.SetWebBrowserFeatures(11);

            // 打开题库网站
            webBrowser1.Url = new Uri("https://gongxukemu.cn/search.html");
            webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;
        }

        /// <summary>
        /// 去广告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var web = (WebBrowser)sender;

                var javascript = @"
var style = document.createElement('style'); 
style.type = 'text/css'; 
style.innerHTML ='.side_right_box,.breadcrumb,.header-top,.footer { display: none; } .search_box {border: 1px solid #000;}'; 
document.getElementsByTagName('head')[0].appendChild(style);
";
                webBrowser1.Document.InvokeScript("eval", new object[] { javascript });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 改请求头
        /// </summary>
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
            var taobao = "https://shop495155500.taobao.com";

            var result = MsgBox.Show(
$@"全国公需科目考试题库 无限搜索版
用于查询全国各地区历年公需科目考试答案。

开源地址：{url}
Copyright ©2019 爱脑图团队", "关于",

$@"收集题库，运维网站都不容易。
请有钱的老哥，还是去支持下题库官网的作者吧。
我看他淘宝店的收入，才几百块钱，可怜的同行。
你评职了是要赚大钱的，请去支持这个帮你节省时间的家伙吧。

题库淘宝地址：{taobao}", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, false,
                new[] { "打开开源项目", "打赏题库作者", "哪儿也不去" });
            if (result == DialogResult.Yes)
            {
                OpenUrl(url);
            }
            else if (result == DialogResult.No)
            {
                OpenUrl(taobao);
            }
        }

        /// <summary>
        /// 打开url，默认用chrome浏览器
        /// </summary>
        /// <param name="url"></param>
        private void OpenUrl(string url)
        {
            var path = string.Empty;

            var chromePath = @"{0}:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            if (File.Exists(string.Format(chromePath, "C")))
            {
                path = string.Format(chromePath, "C");
            }
            else if (File.Exists(string.Format(chromePath, "D")))
            {
                path = string.Format(chromePath, "D");
            }

            if (string.IsNullOrEmpty(path))
            {
                path = "iexplore.exe";
            }

            Process.Start(path, url);
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