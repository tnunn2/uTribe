﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace urTribeWebAPI.Controllers
{
    public class EventsController : ApiController
    {
        // GET: api/Events
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Events/5
        public string GetCreate(int id)
        {
            return "value = " + id;
        }

        // POST: api/Events
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Events/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Events/5
        public void Delete(int id)
        {
        }
    }
}
