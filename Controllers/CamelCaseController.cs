using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using static Trainslator.Functions;

namespace Trainslator.Controllers
{
    public class CamelCaseController : ApiController
    {
        // GET api/<controller>
        public JsonResult<string> Get(string key)
        {
            var str = string.Concat(new[]{ Translate(key) }.Select(s=>s[0].ToString().ToUpper()+s.Substring(1).ToString()));
            return Json(str.Length > 1 && IsEn(str) ? str[0].ToString().ToLower() + str.Substring(1) : str);
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