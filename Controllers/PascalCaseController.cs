using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using static Trainslator.Functions;

namespace Trainslator.Controllers
{
    public class PascalCaseController : ApiController
    {
        // GET api/<controller>
        public JsonResult<string> Get(string key)
        {
            return
                Json(
                    string.Concat(new[] {Translate(key)}
                        .SelectMany(s => s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                        .Select(s =>
                            s.Length > 1 && IsEn(s)
                                ? TitleCase(s)
                                : s))
                        .Replace("-", string.Empty)
                        .Replace("=", string.Empty)
                        .Replace("+", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty)
                        .Replace("/", string.Empty)
                        .Replace(".", string.Empty)
                        .Replace("\\", string.Empty)
                        .Replace(" ", string.Empty)
                        .Replace(",", string.Empty)
                        .Replace("-", string.Empty)
                        .Replace(" ", string.Empty)
                        .Replace(" ", string.Empty));
        }

        private static string TitleCase(string s)
        {
            return s[0].ToString().ToUpper() + s.Substring(1).ToString();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}