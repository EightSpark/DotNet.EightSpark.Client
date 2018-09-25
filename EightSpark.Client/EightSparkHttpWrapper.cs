using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EightSpark.Client
{
    public class EightSparkHttpWrapper : IEightSparkHttpWrapper
    {
        public RuleValue CallEightSparkApi(string url)
        {
            var result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            request.ReadWriteTimeout = 10000;
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                using (var reader = new System.IO.StreamReader(responseStream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }

            var deserializedResult = JsonConvert.DeserializeObject<RuleValue>(result);
            return deserializedResult;
        }

        public async Task<RuleValue> CallEightSparkApiAsync(string url)
        {
            var result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            request.ReadWriteTimeout = 10000;
            using (var response = await request.GetResponseAsync())
            using (var responseStream = response.GetResponseStream())
            {
                using (var reader = new System.IO.StreamReader(responseStream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }

            var deserializedResult = JsonConvert.DeserializeObject<RuleValue>(result);
            return deserializedResult;
        }
    }
}