using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;

namespace SpotifySlackIntegration
{
    class TrayModel
    { 
        private SpotifyWebAPI Api = null;
        private HttpClient SlackClient = new HttpClient();
        public void OnLoad()
        {
            string SlackToken = Environment.GetEnvironmentVariable("SPLACK_SLACK_AUTH_TOKEN");
            SlackClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SlackToken);
            AuthroizationRetrieverClient AuthClient = new AuthroizationRetrieverClient();
            AuthClient.RetrieveAuthCode((string Token, string Type) =>
            {
                Api = new SpotifyWebAPI
                {
                    AccessToken = Token,
                    TokenType = Type
                };
                Thread InstanceCaller = new Thread(new ThreadStart(this.Execute));
                InstanceCaller.Start();
            });
        }

        protected bool IsNotAd(PlaybackContext Context)
        {
            return Context.Item != null 
                    && Context.Item.Artists.Count > 0 
                    && Context.Item.Artists[0].Name != null;
        }

        protected async void Execute()
        {
            while (true)
            {
                Thread.Sleep(30000);
                PlaybackContext Context = await Api.GetPlayingTrackAsync();
                if(!Context.HasError() && Context.IsPlaying && IsNotAd(Context))
                {
                    List<string> Artists = new List<string>();
                    Context.Item.Artists.ForEach(Artist => Artists.Add(Artist.Name));
                    string SlackMessage = String.Format("Listening To: {0} by {1} ({2})", 
                                            Context.Item.Name, 
                                            String.Join(", ", Artists), 
                                            Context.Item.Album.Name);

                    string UserId = Environment.GetEnvironmentVariable("SPLACK_SLACK_USER_ID");

                    JObject Profile = new JObject();
                    Profile["status_text"] = SlackMessage;
                    Profile["status_emoji"] = ":notes:";
                    Profile["status_expiration"] = (int)((Context.Item.DurationMs - Context.ProgressMs) / 1000);

                    Dictionary<string, string> Values = new Dictionary<string, string>
                    {
                        { "user", UserId },
                        { "profile", Profile.ToString() }
                    };

                    FormUrlEncodedContent Content = new FormUrlEncodedContent(Values);
                    HttpResponseMessage Response = await SlackClient.PostAsync("https://slack.com/api/users.profile.set", Content);
                    //Console.WriteLine(await Response.Content.ReadAsStringAsync());
                }
            }
        }        
    }
}