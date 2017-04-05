using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LinqToTwitter;

namespace TwitterAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public List<string> Get()
        {
            List<string> tweets = new List<string>();

            var tweetList = GetTwitterFeeds();
            foreach (var t in tweetList)
            {
                tweets.Add(t.Text.ToString());
            }

            return tweets;
        }

        public static List<Status> GetTwitterFeeds()
        {
            string screenname = "iamdatabear";

            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore()
                {
                    ConsumerKey = "INSERT_CONSUMER_KEY",
                    ConsumerSecret = "INSERT_CONSUMER_SECRET",
                    OAuthToken = "INSERT_OAUTH_TOKEN",
                    OAuthTokenSecret = "INSERT_OAUTH_TOKEN_SECRET"
                }
            };

            var twitterCtx = new TwitterContext(auth);
            var ownTweets = new List<Status>();

            var statusResponse = new List<Status>();

            int rateLimitStatus = twitterCtx.RateLimitRemaining;

            if (rateLimitStatus != 0)
            {
                statusResponse = (from tweet in twitterCtx.Status
                                  where tweet.Type == StatusType.User
                                  && tweet.ScreenName == screenname
                                  && tweet.Count == 500
                                  select tweet).ToList();

                if (statusResponse.Count > 0)
                {
                    ownTweets = statusResponse;
                }
            }

            return ownTweets;
        }   
    }
}
