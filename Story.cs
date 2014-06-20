using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using PivotalTrackerPlugin.Properties;

namespace PivotalTrackerPlugin
{
    public class Story
    {
        public int          id;
        public int          project_id;
        public string       name;
        public string       description;
        public StoryType    story_type;
        public CurrentState current_state;
        public float        estimate;
        public DateTime     accepted_at;
        public DateTime     deadline;
        public int          requested_by_id;
        public int          owned_by_id;
        public int[]        owner_ids;
        public int[]        label_ids;
        public int[]        task_ids;
        public int[]        follower_ids;
        public int[]        comment_ids;
        public DateTime     created_at;
        public DateTime     updated_at;
        public int          integration_id;
        public string       external_id;
        public Uri          url;
        public ObjectKind   kind;

        public enum StoryType    {Feature, Bug, Chore, Release};
        public enum CurrentState {Accepted, Delivered, Finished, Started, Rejected, Unstarted, Unscheduled};
        public enum ObjectKind   {Story};

        private static readonly JavaScriptSerializer jss = new JavaScriptSerializer();

        public static Story Download(string storyId, string token)
        {
            var baseUri   = new Uri(Settings.Default.ApiUrl);
            var uri       = new Uri(baseUri, "stories/" + storyId);
            var webClient = new WebClient();

            webClient.Headers.Add("X-TrackerToken", token);

            var json  = webClient.DownloadString(uri);
            var story = jss.Deserialize<Story>(json);

            return story;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(1024);

            sb.Append(FormatLine("Title",       name,                     sb.Length > 0));
            sb.Append(FormatLine("Type",        story_type,               sb.Length > 0));
            sb.Append(FormatLine("State",       current_state,            sb.Length > 0));
            sb.Append(FormatLine("Created",     created_at.ToString("d"), sb.Length > 0));
            sb.Append(FormatLine("Description", Shorten(description),     sb.Length > 0));

            sb.Insert(0, "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"><Paragraph>");
            sb.Append("</Paragraph></FlowDocument>");

            return sb.ToString();
        }

        /// <summary>Shortens the string to something appropriate</summary>
        private static string Shorten(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Take the first line of the string
            var separators = new[]{"\r\n", "\r", "\n"};
            var parts      = input.Split(separators, 1, StringSplitOptions.RemoveEmptyEntries);
            var shortDesc  = parts[0];

            if (shortDesc.Length <= Settings.Default.MaxLength) // If not too long...
                return shortDesc;

            var index  = shortDesc.LastIndexOf(' ', Settings.Default.MaxLength);

            if (index == -1) // If no space character found...
                return shortDesc.Substring(0, Settings.Default.MaxLength) + '…'; // Just lop off the end

            return shortDesc.Substring(0, index) + " …"; // Truncate at the nearest space character
        }

        private static string FormatLine(string title, object content, bool lineBreak = false)
        {
            if (string.IsNullOrWhiteSpace(content.ToString()))
                return string.Empty;

            return string.Format("{0}<Span Foreground=\"Navy\"><Bold>{1}:</Bold></Span> {2}", lineBreak ? "<LineBreak />" : string.Empty, title, content);
        }
    }

}
