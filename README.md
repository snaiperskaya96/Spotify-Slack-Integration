## Spotify-Slack Integration

You didn't ask for it but it's here.

### How-to
You'll need to create a new Spotify app in the Spotify Web API dashboard. Grab client id and secret and put them into their environment variable.
  - SPLACK_SPOTIFY_CLIENT_ID
  - SPLACK_SPOTIFY_SECRET_ID 

Then head to https://api.slack.com/custom-integrations/legacy-tokens, make sure you are logged into your workspace and scroll down to Legacy token generator (hey I know... it's just easier with the legacy one...), generate your token and put it into the SPLACK_SLACK_AUTH_TOKEN environment variable.

Last but not least you'll need your user ID. I usually get it by right clicking on your user name in slack and copy link. The link looks like this `https://myslack.slack.com/team/UFXXXXXX` and the last bit being your id. Put it into SPLACK_SLACK_USER_ID and everything should work just fine™.

Note that I couldn't be bothered to add any sort of check, so if it doesn't work just double check your environment variables.

### Supported platforms
No idea, it should run on everything that supports .NET core
