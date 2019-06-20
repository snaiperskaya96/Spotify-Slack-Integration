using System;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace SpotifySlackIntegration
{
    class AuthroizationRetrieverClient
    {
        public delegate void AuthCodeReceivedDelegate(string AccessToken, string TokenType);

        private AuthCodeReceivedDelegate OnAuthCodeReceived = null;
        public void RetrieveAuthCode(AuthCodeReceivedDelegate OnAuthCodeReceived)
        {
            this.OnAuthCodeReceived = OnAuthCodeReceived;
            string ClientId = Environment.GetEnvironmentVariable("SPLACK_SPOTIFY_CLIENT_ID");
            string SecretId = Environment.GetEnvironmentVariable("SPLACK_SPOTIFY_SECRET_ID");

            AuthorizationCodeAuth Auth = new AuthorizationCodeAuth(ClientId, SecretId, "http://localhost:4002", "http://localhost:4002", Scope.UserReadCurrentlyPlaying);
            Auth.AuthReceived += AuthOnAuthReceived;
            Auth.Start();
            Auth.OpenBrowser();
        }

        private async void AuthOnAuthReceived(object Sender, AuthorizationCode Payload)
        {
            AuthorizationCodeAuth Auth = (AuthorizationCodeAuth) Sender;
            Auth.Stop();

            Token ReceivedToken = await Auth.ExchangeCode(Payload.Code);
            OnAuthCodeReceived(ReceivedToken.AccessToken, ReceivedToken.TokenType);
        }
    }
}