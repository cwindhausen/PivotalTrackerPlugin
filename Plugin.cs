using System;
using System.Diagnostics;
using System.Windows.Forms;
using PivotalTrackerPlugin.Properties;

namespace PivotalTrackerPlugin
{
    public class MyExtension : WholeTomatoSoftware.SourceLinks.IPlugin
    {
        /// <summary>Return true if your extension provides a configuration GUI.</summary>
        public bool CanConfigure()
        {
            Debug.WriteLine("SourceLinks calling CanConfigure()");
            return true;
        }

        /// <summary>Implement your configuration GUI here as a modal dialog.</summary>
        public void Configure()
        {
            Debug.WriteLine("SourceLinks calling Configure()");

            using (var dialog = new ConfigDlg(Settings.Default.Token))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    Settings.Default.Save();
            }
        }

        /// <summary>Assemble a tooltip string using the marker text as input.</summary>
        /// <remarks>You can return either a straight up string or Flow Document XML</remarks>
        /// <param name="storyId">The Pivotal story ID</param>
        public string GetTooltip(string storyId)
        {
            Debug.WriteLine("SourceLinks calling GetTooltip(\"" + storyId + "\")");

            try
            {
                if (string.IsNullOrEmpty(Settings.Default.Token))
                    return "You need to configure your access token under Tools/Options/SourceLinks";

                var story = Story.Download(storyId, Settings.Default.Token);
                return story.ToString();
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>Provide a name to appear in the plug-in dropdown in SourceLinks configuration.</summary>
        public string Name
        {
            get
            {
                Debug.WriteLine("SourceLinks getting Name property");
                return "Pivotal Tracker Extension";
            }
        }

        /// <summary>Provide a URL to open when the user double-clicks marked text. Use %s as a placeholder for marker text.</summary>
        /// <remarks>This method is called every time the user double-clicks, allowing you to specify a state-dependent URL if you wish.</remarks>
        public string OverrideUrl
        {

            get
            {
                Debug.WriteLine("SourceLinks getting OverrideUrl property");
                return "https://www.pivotaltracker.com/story/show/%s";
            }
        }
    }
}
