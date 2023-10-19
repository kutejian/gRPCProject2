using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRPC.Framework
{
    public class JWTTokenHelper
    {
        public class JWTTokenResult
        {
            public bool result { get; set; }
            public string token { get; set; }
        }
        public async static Task<string> GetJWTToken()
        {
            //string result = await PostWebQuest();
            string result = await PostClient();
            return JsonConvert.DeserializeObject<JWTTokenResult>(result).token;
        }
        #region HttpClient实现Post请求
        /// <summary>
        /// HttpClient实现Post请求
        /// </summary>
        private async static Task<string> PostClient()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"Name","Steven" },
                {"Password","1234" }
            };
            string url = "http://localhost:5052/Authentication?name=Steven&password=1234";
            HttpClientHandler handler = new HttpClientHandler();
            using (var http = new HttpClient(handler))
            {
                var content = new FormUrlEncodedContent(dic);
                var response = await http.PostAsync(url, content);
                Console.WriteLine(response.StatusCode); //确保HTTP成功状态值
                return await response.Content.ReadAsStringAsync();
            }
        }
        #endregion
    }
}
