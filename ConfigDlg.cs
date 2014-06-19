using System.Diagnostics;
using System.Windows.Forms;

namespace PivotalTrackerPlugin
{
    /// <summary>The configuration dialog for this Source Links plugin</summary>
    public partial class ConfigDlg : Form
    {
        public ConfigDlg(string accessToken = "")
        {
            InitializeComponent();
            token.Text = accessToken;
        }

        private void OnHyperlink(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
