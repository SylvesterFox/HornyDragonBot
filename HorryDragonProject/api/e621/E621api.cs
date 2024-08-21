using System.Net;

namespace HorryDragonProject.api.e621{
    public class E621api
    {
        private HttpWebRequest _request;
        private string _address;

        public string Response { get; set; }
        public E621api(string address)
        {
            _address = address;
        }

        public void Run() {
            _request = (HttpWebRequest)WebRequest.Create(_address);
            _request.Method = "Get";

           try
           {
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null) Response = new StreamReader(stream).ReadToEnd();
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex);
           }
        }
    }

}

