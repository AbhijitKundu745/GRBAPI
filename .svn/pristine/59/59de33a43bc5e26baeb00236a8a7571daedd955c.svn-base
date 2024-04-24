using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MessageService
{
    public class MessageController
    {
        string url { get; set; }

        public MessageController(string url)
        {
            this.url = url;
        }

        public MessageController(MSGRequest request)
        {
            if (request == null)
                throw new System.Exception("invalid message request");

            StringBuilder builder = new StringBuilder();
            builder.Append(request.url);
            builder.Append("?apikey=");
            builder.Append(request.APIKey);
            builder.Append("&senderid=");
            builder.Append(request.SenderId);
            builder.Append("&number=");
            builder.Append(request.MobileNo);
            builder.Append("&message=");
            builder.Append(request.Message);
            builder.Append("&format=");
            builder.Append(request.Format);
            this.url = builder.ToString();
        }

        public MSGResponse send()
        {
            MSGResponse MSGResponse = null;
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url);
                response.Wait();
                var result = response.Result;
                var content = response.Result.Content;
                string responseBody = content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(responseBody))
                {
                    JavaScriptSerializer _serializer = new JavaScriptSerializer();
                    MSGResponse = _serializer.Deserialize<MSGResponse>(responseBody);
                }
                else
                    throw new System.Exception("message sending failed");
            }
            return MSGResponse;
        }

        public async Task<MSGResponse> sendAsync()
        {
            MSGResponse MSGResponse = null;
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url);
                response.Wait();
                var result = response.Result;
                string responseBody = await response.Result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(responseBody))
                {
                    JavaScriptSerializer _serializer = new JavaScriptSerializer();
                    MSGResponse = _serializer.Deserialize<MSGResponse>(responseBody);
                }
                else
                    throw new System.Exception("message sending failed");
            }
            return MSGResponse;
        }
    }
}
