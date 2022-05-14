using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Translator.Controllers;

namespace Trainslator
{
    public class Functions
    {
        private static readonly string BaiduTranslateUrl = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        private static readonly string OxfordFileName = "《牛津英汉词典》.txt";

        public static string[] OxfordAllLines = ReadOxfordAllLines();

        private static string[] ReadOxfordAllLines()
        {
            return File.ReadAllLines(@"D:\Users\Pandadigi\Source\Repos\Translator\《牛津英汉词典》.txt", Encoding.Default);
        }

        public static string[] CleanupList()
        {
            return OxfordAllLines.Where(s =>string.IsNullOrEmpty(s) || !IsEn(s)).ToArray();
        }

        private static Dictionary<string,string> dictionary = File.Exists("dictionary.txt")
            ? JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("dictionary.txt"))
            : new Dictionary<string, string>();

        public static string Translate(string key)
        {
//            var list1 = OxfordAllLines.Where(l => l.IndexOf(key, StringComparison.Ordinal) != -1).ToList();
//            foreach (var line in list1)
//            {
//                var s =
//                    Regex.Matches(line, @"\w+[^\.]")
//                        .Cast<Match>()
//                        .Select(match => match.Value.Trim('，', ' ', '\t'))
//                        .FirstOrDefault();
//                // 英文要求全词匹配
//                if (IsEn(key) ? s == key.Trim('，', ' ', '\t') : !string.IsNullOrEmpty(s))
//                {
//                    return Regex.Matches(line, @"\w+\.*")
//                        .Cast<Match>()
//                        .Select(m => m.Value.Trim('，', ' ', '\t'))
//                        .Where(str => !str.EndsWith("."))
//                        .Where(str => IsEn(key) ? !IsEn(str) : IsEn(str))
//                        .Union(
//                            Regex.Matches(key, @"\d*")
//                                .Cast<Match>()
//                                .Select(m => m.Value))
//                        .Where(m => !string.IsNullOrEmpty(m))
//                        .ToArray();
//                }
//            }

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            var response = BaiduTranslate(key);
            if (response != null && response.trans_result.Any())
            {
                var contents = response.trans_result.Select(
                    p =>
                        (IsEn(key) ? p.src.ToLower() : p.dst.ToLower()).Trim())
                    .Where(p => !String.IsNullOrEmpty(p))
                    .ToArray();
                if (contents.Any())
                {

//                    if (!File.Exists(OxfordFileName))
//                    {
//                        File.Create(OxfordFileName);
//                    }
//                    File.AppendAllLines(OxfordFileName,
//                        contents,
//                        Encoding.Default);
//
//                    var list = OxfordAllLines.ToList();
//                    list.AddRange(contents);
//                    OxfordAllLines = list.ToArray();

                    dictionary.Add(key,contents.FirstOrDefault());
                    File.WriteAllText("dictionary.txt", JsonConvert.SerializeObject(dictionary));
                    return contents.FirstOrDefault();
                }
            }
            return string.Empty;
        }

        private static BaiduResponse BaiduTranslate(string key)
        {
            if (String.IsNullOrEmpty(key)) return null;
            string q = key.Trim();
            bool isEN = IsEn(key);
            string from = isEN ? "en" : "zh";
            string to = !isEN ? "en" : "zh";
            string appid = "20160419000019154";
            string pwd = "bFSBOkx78njR5WI2vqUv";
            string salt = new Random(10).Next((int)(Math.Pow(10, 5))).ToString() +
                          new Random(10).Next((int)(Math.Pow(10, 5))).ToString();
            var originSign = appid + q + salt + pwd;
            MD5 md5 = new MD5CryptoServiceProvider();
            var output = md5.ComputeHash(Encoding.UTF8.GetBytes(originSign));
            string sign = BitConverter.ToString(output).Replace("-", "").ToLower();

            string url = $"{BaiduTranslateUrl}?q={q}&from={@from}&to={to}&appid={appid}&salt={salt}&sign={sign}";

            string htmlStr = String.Empty;

            Stream responseStream = CreateGetHttpResponse(url).GetResponseStream();
            if (responseStream != null)
            {
                using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                {
                    htmlStr = reader.ReadToEnd();
                }
                responseStream.Close();
            }
            var response = JsonConvert.DeserializeObject<BaiduResponse>(htmlStr);
            return response;
        }

        public static bool IsEn(string key)
        {
            return key[0] >= 'A' && key[0] <= 'z';
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            return request.GetResponse() as HttpWebResponse;
        }
    }
}