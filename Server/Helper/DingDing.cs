using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Server.Helper
{
    class DingDing
    {
        public static void Send(string WebHook,string secret,string content)
        {
            string result = "";
            //获取毫秒时间戳
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long shijianchuo = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000);
            //获取签名值
            string stringToSign = shijianchuo + "\n" + secret;
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(stringToSign);
            string sign;
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                sign = HttpUtility.UrlEncode(Convert.ToBase64String(hashmessage), Encoding.UTF8);
            }
            string url = WebHook + "&timestamp=" + shijianchuo + "&sign=" + sign;

            var obj = new
            {
                msgtype = "text",
                text = new
                {
                    content = content,
                }

            };

            string json = JsonConvert.SerializeObject(obj);
            Console.WriteLine(url);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=utf-8";
            //Console.WriteLine(json);

            var bytes = Encoding.UTF8.GetBytes(json);
            req.ContentLength = bytes.Length;
            using (Stream requestStream = req.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            //Console.WriteLine(result);

            //Console.ReadKey();

        }
    }
}
