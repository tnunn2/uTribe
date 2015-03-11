using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// ReSharper disable InconsistentNaming

namespace urTribeWebAPI.Models
{

    public class CreateTableQuery
    {
        public string applicationKey { get; set; }
        public string authenticationToken { get; set; }
        public string table { get; set; }
        public TableSchema key { get; set; }
        public int provisionType { get; set; }
        public int provisionLoad { get; set; }
    }

    public class TableSchema
    {
        public Key primary { get; set; }
        public Key secondary { get; set; }
    }

    public class Key
    {
        public string name { get; set; }
        public string dataType { get; set; }
    }
}