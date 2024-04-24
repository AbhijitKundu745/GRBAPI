using System;

namespace MessageService
{
    public class MSGRequest
    {

        private string _url;
        public string url
        {
            get { return _url; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("url is required to send message");
                _url = value;
            }
        }

        private string _apiKey;
        public string APIKey
        {
            get { return _apiKey; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("API Key is required to send message");
                _apiKey = value;
            }
        }

        private string _senderId;
        public string SenderId
        {
            get { return _senderId; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("sender Id is required to send message");
                _senderId = value;
            }
        }

        private string _mobileNo;
        public string MobileNo
        {
            get { return _mobileNo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Mobile number is required to send message");
                _mobileNo = value;
            }
        }

        public string Message { get; set; }
        public string Format { get; private set; } = "json";
    }
}
