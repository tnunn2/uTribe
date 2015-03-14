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

    public class AuthQuery
    {
        public string applicationKey { get; set; }
        public string privateKey { get; set; }
        public string authenticationToken { get; set; }
        public List<string> roles { get; set; }
        public long timeout { get; set; }
        public Policy policies { get; set; }
    }

    public class Policy
    {
        public DBPolicy database { get; set;}
        public Dictionary<String, TablePolicy> tables {get; set;}
    }

    public class DBPolicy
    {
        public List<string> listTables { get; set; }
        public List<string> deleteTable { get; set; }
        public bool createTable { get; set; }
        public List<string> updateTable { get; set; }
    }

    public class TablePolicy
    {
        public string allow { get; set; }
    }

    public class PutItemQuery
    {
        public string applicationKey { get; set; }
        public string privateKey { get; set; }
        public string authenticationToken { get; set; }
        public string table { get; set; }
        public Dictionary<String, String> item { get; set; }
    }

}