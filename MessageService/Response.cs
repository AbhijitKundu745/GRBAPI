using System.Collections.Generic;

namespace MessageService
{
    public class MSGResponse
    {
        public string status { get; set; }
        public string msgid { get; set; }
        public string message { get; set; }
        public List<ResponseData> data { get; set; }
    }
    public class ResponseData
    {
        public string id { get; set; }
        public string mobile { get; set; }
        public string status { get; set; }
    }
}
