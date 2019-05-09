using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Selenium {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        ChromeDriver chromeDriver;
        private void Button1_Click(object sender, EventArgs e) {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            var perfLogPrefs = new ChromePerformanceLoggingPreferences();
            var tracingCategories = "toplevel,disabled-by-default-devtools.timeline.frame,blink.console,disabled-by-default-devtools.timeline,benchmark";
            perfLogPrefs.AddTracingCategories(new string[] { tracingCategories });
            options.PerformanceLoggingPreferences = perfLogPrefs;
            options.SetLoggingPreference("performance", LogLevel.All);
             chromeDriver = new ChromeDriver(chromeDriverService, options);
            chromeDriver.Navigate().GoToUrl("https://www.facebook.com/");
            chromeDriver.FindElementByClassName("inputtext").Click();
            
            var logs = chromeDriver.Manage().Logs.GetLog("performance");
            object s;
            File.WriteAllText("result/json.txt", "");

            foreach (var item in logs) {
                if (item.Message.ToString().Contains("https://www.facebook.com/ajax/bz") && item.Message.ToString().Contains("postData")) {
                    s = JsonConvert.SerializeObject(item);
                    File.AppendAllText("result/json.txt", s.ToString() + "\r\n\r\n");
                }
            }
            MessageBox.Show(File.ReadAllText("result/json.txt"));
        }

    }
}
