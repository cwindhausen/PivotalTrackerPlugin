using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace PivotalTrackerPlugin
{
    public class Story
    {
        private static readonly Uri baseUri = new Uri("https://www.pivotaltracker.com/services/v5/");

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

        public static Story Download(string storyId, string token)
        {
            var uri       = new Uri(baseUri, "stories/" + storyId);
            var webClient = new WebClient();

            // Adding certain headers to fool the api server into returning 200 OK
            webClient.Headers.Add("X-TrackerToken", token);
            //webClient.Headers.Add("Accept", "*/*");
            //webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36");
            //webClient.Headers.Add("Cookie", "devicePixelRatio=1; ident=exists; is_human=true; company_history=%5B%5B%22http%3A//community.pivotaltracker.com/pivotal%22%2C%22Pivotal%20Labs%22%5D%5D; __utma=131036942.1761068177.1400259677.1402071789.1402321828.34; __utmb=131036942.19.9.1402325139711; __utmc=131036942; __utmz=131036942.1402071740.31.2.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided); t_session=BAh7CkkiD3Nlc3Npb25faWQGOgZFVEkiJTE4NmI4YzllZTRiYjhlM2NkMzlkY2FkYzBjMWJkZmY3BjsAVEkiEF9jc3JmX3Rva2VuBjsARkkiMW1Wd21JejF5cnJlMVFoUDhkL3JrUy9xVGJOY0FmUSs2SDk3TDkxT3dnbkU9BjsARkkiFXNpZ25pbl9wZXJzb25faWQGOwBGaQMaOBFJIhRsYXN0X2xvZ2luX2RhdGUGOwBGVTogQWN0aXZlU3VwcG9ydDo6VGltZVdpdGhab25lWwhJdToJVGltZQ20kRzAAAAAMgY6CXpvbmVJIghVVEMGOwBUSSIfUGFjaWZpYyBUaW1lIChVUyAmIENhbmFkYSkGOwBUSXU7Bw2tkRzAAAAAMgY7CEkiCFVUQwY7AFRJIg9leHBpcmVzX2F0BjsARkl1OwcND5YcgO7RGXQHOgtvZmZzZXRpADsISSIIVVRDBjsAVA%3D%3D--1283b166bb03bfd1e29ef22a5764fd2b68e0982b");

            var json  = webClient.DownloadString(uri);
            var story = JsonConvert.DeserializeObject<Story>(json);

            return story;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(1024);

            sb.Append("<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");
            sb.Append("<Paragraph>");
            sb.Append("<Span Foreground=\"Navy\"><Bold>Title:</Bold></Span> ");
            sb.Append(name);
            sb.Append("<LineBreak /><Span Foreground=\"Navy\"><Bold>Type:</Bold></Span> ");
            sb.Append(story_type);
            sb.Append("<LineBreak /><Span Foreground=\"Navy\"><Bold>State:</Bold></Span> ");
            sb.Append(current_state);
            sb.Append("<LineBreak /><Span Foreground=\"Navy\"><Bold>Created:</Bold></Span> ");
            sb.Append(created_at.ToString("d"));
            sb.Append("<LineBreak /><Span Foreground=\"Navy\"><Bold>Description:</Bold></Span> ");
            sb.Append(short_description);
            sb.Append("</Paragraph>");
            sb.Append("</FlowDocument>");

            return sb.ToString();
        }

        /// <summary>Shortens the description string to something less than 256 characters</summary>
        public string short_description
        {
            get
            {
                var separators = new[]{"\r\n", "\r", "\n"};
                var parts      = description.Split(separators, 1, StringSplitOptions.RemoveEmptyEntries);
                var result     = parts.Length == 0 ? "<empty>" : parts.Length > 256 ? parts[0].Substring(0, 256) : parts[0];

                return result;
            }
        }
    }

}
