namespace urTribeWebAPI.Messaging
{
    public class CreateTableQuery
    {
        public string applicationKey { get; set; }
        public string authenticationToken { get; set; }
        public string table { get; set; }
        public TableSchema key { get; set; }
        public int provisionType { get; set; }
        public int provisionLoad { get; set; }
        public Throughput throughput { get; set; }
    }
}
