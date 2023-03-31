
using Microsoft.WindowsAzure.Storage.Table;

namespace RestApi
{
    public class MyEntity : TableEntity
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string BlobURLs { get; set; }
        public string Type { get; set; }
    }



}
