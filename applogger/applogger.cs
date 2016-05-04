/**
 * Author: Luc Gommans, 2016
 * 
 * License: MIT (see the file named `LICENSE`)
 * 
 * Todo: make stuff configurable (logging and saving interval, window filter, storage directory, language; etc.); proper command line argument parsing.
 * 
 * Application icon from: http://www.fatcow.com/free-icons
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
        private Dictionary<string, WindowData> runningProgramData;
        private List<WindowData> closedProgramData;
        private NotifyIcon notifyIcon;
        private Dictionary<string, string> translations;
        private string lastFilename;

        public applogger()
        {
            InitializeComponent();
            logging = false;
            timer_logger.Interval = 1000 * 5;
            timer_saver.Interval = 1000 * 60 * 5;
            translations = Translations.getTranslations("nl");
            applicationStartTime = DateTime.Now.ToString("HH-mm").Replace("-", translations["hour symbol"]);
            runningProgramData = new Dictionary<string, WindowData>();
            closedProgramData = new List<WindowData>();
            lastFilename = getFilename();

            if (!Directory.Exists(getFolder()))
            {
                Directory.CreateDirectory(getFolder());
            }

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Click += notifyIcon_Click;
            notifyIcon.BalloonTipClicked += notifyIcon_Click;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1] == "bg")
            {
                this.WindowState = FormWindowState.Minimized;
                startLogging();
                this.ShowInTaskbar = false;
            }
        }

        private void stopLogging()
        {
            timer_logger.Stop();
            timer_saver.Stop();
            button_toggle_start.Text = translations["btn start logging"];
            button_open_log.Enabled = true;
            saveLog();
            label_status.Text = translations["status: idle"];
            logging = false;
        }

        private void startLogging()
        {
            timer_logger.Start();
            timer_saver.Start();
            label_status.Text = translations["status: starting..."];
            button_toggle_start.Text = translations["btn stop logging"];
            button_open_log.Enabled = false;
            log();
            logging = true;
        }

        private void button_toggle_start_Click(object sender, EventArgs e)
        {
            if (logging)
            {
                stopLogging();
            }
            else
            {
                startLogging();
            }
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
            if (getFilename() != lastFilename)
            {
                // If the filename changes, the current closed programs is longer relevant because it has been written to yesterday's log.
                lastFilename = getFilename();
                closedProgramData.Clear();
            }

            // Set each program's status to not open...
            foreach (KeyValuePair<string, WindowData> kvp in runningProgramData)
            {
                kvp.Value.stillOpen = false;
            }

            // ...then check which ones are still open.
            foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
            {
                if (!filter(window.Value))
                {
                    // We only care about certain windows; ignore the others completely
                    continue;
                }

                if (!runningProgramData.ContainsKey(window.Value))
                {
                    // a new window appeared! Keep track of this.
                    WindowData windowData = new WindowData(window.Value);
                    windowData.stillOpen = true;
                    runningProgramData.Add(window.Value, windowData);
                }
                else
                {
                    // We already knew this window, so just mark it as "still open".
                    runningProgramData[window.Value].stillOpen = true;
                }

                runningProgramData[window.Value].runningTime++;
            }

            // Create a list of each window we can remove...
            List<string> closedPrograms = new List<string>();
            foreach (KeyValuePair<string, WindowData> kvp in runningProgramData)
            {
                if (kvp.Value.stillOpen == false)
                {
                    closedProgramData.Add(kvp.Value);
                    closedPrograms.Add(kvp.Key);
                }
            }

            // ... and remove them.
            foreach (string closedProgram in closedPrograms)
            {
                runningProgramData.Remove(closedProgram);
            }

            // Update the status field (filename may have changed).
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
            // Programs contain an int for their "open time", which is increased by 1 every time the program is seen.
            // Thus, the program's open time is (opentime * tick_interval).
            // This method converts the number of ticks to a "hh:mm" format (e.g. 01:03 for 1 hour and 3 minutes).
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

        private void saveLog()
        {
            // Build a string with the data we want to write.
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

            // Data string complete, write it!
            try
            {
                File.WriteAllText(getFilename(), data);
            }
            catch (Exception)
            {
                alert(translations["error writing log"]);
            }
        }

        private void timer_saver_Tick(object sender, EventArgs e)
        {
            saveLog();
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
                // Hide to tray upon minimizing
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500, translations["program name"] + " " + translations["active"], translations["click to open"], ToolTipIcon.Info);
                this.Hide();
            }
        }

        private void showOurselves()
        {
            // Tray icon was clicked! Hide the tray icon and show the form.
            notifyIcon.Visible = false;
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            showOurselves();
        }
    }
}
