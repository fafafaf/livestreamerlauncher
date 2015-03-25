using System;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace LiveStreamerLauncher
{
    public partial class Main : Form
    {
        readonly string url = @"https://api.twitch.tv/kraken/streams/?game=Supreme+Commander:+Forged+Alliance";

        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {               
                Stream stream = (Stream)comboBox1.SelectedItem;
                startStream(stream.channel.url);
            }
        }

        private void startStream(string streamUrl)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = true;
            startInfo.FileName = "livestreamer.exe";
            startInfo.Arguments = String.Format("{0} {1}", streamUrl, "best");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process.Start(startInfo);

        }

        private void Main_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Help().ShowDialog();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var twitch = new JavaScriptSerializer().Deserialize<Twitch>(webClient.DownloadString(url));

                    comboBox1.Items.Clear();
                    foreach (var stream in twitch.streams)
                    {
                        comboBox1.Items.Add(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Stream stream = (Stream)comboBox1.SelectedItem;
                Process.Start(stream.channel.url + "/chat");
            }
        }        
    }
}
