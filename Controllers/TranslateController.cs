using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json;
using Trainslator;

namespace Translator.Controllers
{
    public class TranslateController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return Functions.OxfordAllLines;
        }

        public JsonResult<string> Get(string key)
        {
            return Json(TitleCase(Functions.Translate(key)));
        }

        private static string TitleCase(string s)
        {
            return s[0].ToString().ToUpper() + s.Substring(1).ToString();
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