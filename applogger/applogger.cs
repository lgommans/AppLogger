/**
 * Author: Luc Gommans, 2016
 * 
 * License: MIT (see the file named `LICENSE`)
 * 
 * Todo: make stuff configurable (logging and saving interval, window filter, storage directory, language; etc.); proper command line argument parsing.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace applogger
{
    public partial class applogger : Form
    {
        private bool logging;
        private string applicationStartTime;
        private string applicationStartDay;
        private Dictionary<string, WindowData> runningProgramData;
        private List<WindowData> closedProgramData;
        private NotifyIcon notifyIcon;
        private Dictionary<string, string> translations;

        public applogger()
        {
            InitializeComponent();
            logging = false;
            timer_logger.Interval = 1000 * 5;
            timer_saver.Interval = 1000 * 60 * 5;
            translations = Translations.getTranslations("nl");
            applicationStartTime = DateTime.Now.ToString("HH-mm").Replace("-", translations["hour symbol"]);
            applicationStartDay = DateTime.Now.ToString("yyyy-MM-dd");
            runningProgramData = new Dictionary<string, WindowData>();
            closedProgramData = new List<WindowData>();

            if (!Directory.Exists(getFolder()))
            {
                Directory.CreateDirectory(getFolder());
            }

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Click += notifyIcon_Click;
        }

        private void button_toggle_start_Click(object sender, EventArgs e)
        {
            if (logging)
            {
                timer_logger.Stop();
                button_toggle_start.Text = translations["btn start logging"];
                button_open_log.Enabled = true;
                timer_saver_Tick(null, null);
                label_status.Text = translations["status: idle"];
            }
            else
            {
                timer_logger.Start();
                label_status.Text = translations["status: starting..."];
                button_toggle_start.Text = translations["btn stop logging"];
                button_open_log.Enabled = false;
                log();
            }
            logging = !logging;
        }

        private bool filter(string windowName)
        {
            List<string> filters = new List<string>();
            filters.Add(" - Word");
            filters.Add(" - PowerPoint");
            filters.Add(" - Excel");
            filters.Add(" - Adobe Acrobat");
            foreach (string s in filters)
            {
                if (windowName.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }

        private void log()
        {
            string currentDay = DateTime.Now.ToString("yyyy-MM-dd");
            if (applicationStartDay != currentDay)
            {
                closedProgramData.Clear();
            }

            foreach (KeyValuePair<string, WindowData> kvp in runningProgramData)
            {
                kvp.Value.stillOpen = false;
            }

            foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
            {
                if (!filter(window.Value))
                {
                    continue;
                }

                if (!runningProgramData.ContainsKey(window.Value))
                {
                    WindowData windowData = new WindowData(window.Value);
                    windowData.stillOpen = true;
                    runningProgramData.Add(window.Value, windowData);
                }
                else
                {
                    runningProgramData[window.Value].stillOpen = true;
                }

                runningProgramData[window.Value].runningTime++;
            }

            List<string> closedPrograms = new List<string>();

            foreach (KeyValuePair<string, WindowData> kvp in runningProgramData)
            {
                if (kvp.Value.stillOpen == false)
                {
                    closedProgramData.Add(kvp.Value);
                    closedPrograms.Add(kvp.Key);
                }
            }

            foreach (string closedProgram in closedPrograms)
            {
                runningProgramData.Remove(closedProgram);
            }

            string filename = getFilename();
            label_status.Text = translations["status: logging to "] + getFilename();
        }

        private string getFolder()
        {
            string userprofile = System.Environment.GetEnvironmentVariable("USERPROFILE");
            return userprofile + @"\Documents\" + translations["journal"];
        }

        private string getFilename()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return getFolder() + @"\applog_" + date + "_" + applicationStartTime + ".doc";
        }

        private void timer_logger_Tick(object sender, EventArgs e)
        {
            log();
        }

        private void button_open_log_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(getFilename());
            }
            catch (Exception)
            {
                try
                {
                    Process.Start(getFolder());
                    alert(translations["404, opening folder"]);
                }
                catch (Exception)
                {
                    alert(translations["failed to open"]);
                }
            }
        }

        private string ticksToTime(int ticks)
        {
            int milliseconds = ticks * timer_logger.Interval;
            double seconds = milliseconds / 1000.0;
            double minutes = seconds / 60.0;
            int hours = (int) Math.Floor(minutes / 60.0);
            minutes -= hours * 60;
            string strHours = hours.ToString();
            if (hours < 10)
            {
                strHours = "0" + strHours;
            }
            string strMinutes = ((int)Math.Round(minutes)).ToString();
            if (strMinutes.Length == 1)
            {
                strMinutes = "0" + strMinutes;
            }
            return strHours + ":" + strMinutes;
        }

        private void timer_saver_Tick(object sender, EventArgs e)
        {
            string data = translations["log saved at "] + DateTime.Now.ToString("HH:mm") + "\r\n";
            data += translations["column info"] + "\r\n";
            foreach (WindowData window in closedProgramData)
            {
                data += window.starttime.ToString("HH:mm") + "   " + ticksToTime(window.runningTime) + "   " + window.windowTitle + "\r\n";
            }
            foreach (KeyValuePair<string, WindowData> kvp in runningProgramData)
            {
                data += kvp.Value.starttime.ToString("HH:mm") + "   " + ticksToTime(kvp.Value.runningTime) + "*  " + kvp.Key + "\r\n";
            }
            data += "* " + translations["incomplete time; app still open"];
            try
            {
                File.WriteAllText(getFilename(), data);
            }
            catch (Exception)
            {
                alert(translations["error writing log"]);
            }
        }

        private void alert(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button_open_folder_Click(object sender, EventArgs e)
        {
            Process.Start(getFolder());
        }

        private void applogger_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500, translations["program name"] + " " + translations["active"], translations["click to open"], ToolTipIcon.Info);
                this.Hide();
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Show();
        }
    }
}
