using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public class HostCookies
    {
        private Cookie[] _cookies;
        private string _hostName;

        public Cookie[] Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }

        public string HostName
        {
            get { return _hostName; }
            set { _hostName = value; }
        }

        public void Print()
        {
            string user = Environment.GetEnvironmentVariable("USERNAME");
            Console.WriteLine("--- Cookie (User: {0}) ---", user);
            Console.WriteLine("Domain         : {0}", HostName);
            Console.WriteLine("Cookies (JSON) :\n{0}", ToJSON());
            Console.WriteLine();
        }


        /*
         * [X] Exception: Object reference not set to an instance of an object.

   at SharpChrome.Cookie.ToJSON()
   at SharpChrome.HostCookies.ToJSON()
   at SharpChrome.HistoricUrl.Print()
   at SharpChrome.Program.Main(String[] args)
[*] Assembly 'SharpChrome' with commands 'history' completed

         */
        public string ToJSON()
        {
            if (this.Cookies != null && this.Cookies.Length > 0)
            {
                List<string> jsonCookies = new List<string>();
                //string[] jsonCookies = new string[this.Cookies.Length];
                int j = 0;
                //Console.WriteLine("Cookies length: {0}", this.Cookies.Length);
                for (int i = 0; i < this.Cookies.Length; i++)
                {
                    //Console.WriteLine("Cookie {0}: {1}", i, this.Cookies[i]);
                    if (this.Cookies[i] != null)
                    {
                        //this.Cookies[i].Id = j + 1;
                        jsonCookies.Add(this.Cookies[i].ToJSON());
                        j++;
                    }
                }
                return "[\n" + String.Join(",\n", jsonCookies.ToArray()) + "\n]";
            }
            return "";
        }

        public static HostCookies FilterHostCookies(HostCookies[] hostCookies, string url)
        {
            HostCookies results = new HostCookies();
            if (hostCookies == null)
                return results;
            if (url == "" || url == null || url == string.Empty)
                return results;
            List<String> hostPermutations = new List<String>();
            // First retrieve the domain from the url
            string domain = url;
            // determine if url or raw domain name
            if (domain.IndexOf('/') != -1)
            {
                domain = domain.Split('/')[2];
            }
            results.HostName = domain;
            string[] domainParts = domain.Split('.');
            for (int i = 0; i < domainParts.Length; i++)
            {
                if ((domainParts.Length - i) < 2)
                {
                    // We've reached the TLD. Break!
                    break;
                }
                string[] subDomainParts = new string[domainParts.Length - i];
                Array.Copy(domainParts, i, subDomainParts, 0, subDomainParts.Length);
                string subDomain = String.Join(".", subDomainParts);
                hostPermutations.Add(subDomain);
                hostPermutations.Add("." + subDomain);
            }
            List<Cookie> cookies = new List<Cookie>();
            foreach (string sub in hostPermutations)
            {
                // For each permutation
                foreach (HostCookies hostInstance in hostCookies)
                {
                    // Determine if the hostname matches the subdomain perm
                    if (hostInstance.HostName.ToLower() == sub.ToLower())
                    {
                        // If it does, cycle through
                        foreach (Cookie cookieInstance in hostInstance.Cookies)
                        {
                            // No dupes
                            if (!cookies.Contains(cookieInstance))
                            {
                                cookies.Add(cookieInstance);
                            }
                        }
                    }
                }
            }
            results.Cookies = cookies.ToArray();
            return results;

        }
    }
}
