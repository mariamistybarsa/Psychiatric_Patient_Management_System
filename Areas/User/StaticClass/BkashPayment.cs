using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Psychiatrist_Management_System.Areas.User.StaticClass
{
   

    public static class BkashPayment
    {
        private static readonly string BKS_URL = "https://tokenized.sandbox.bka.sh/v1.2.0-beta/tokenized/checkout";
        private static readonly string BKS_KEY = "4f6o0cjiki2rfm34kfdadl1eqq";   // Your App Key
        private static readonly string BKS_SEC = "2is7hdktrekvrbljjh44ll3d9l1dtjo4pasmjvs5vl5qr3fug4b";   // Your App Secret
        private static readonly string BKS_USER = "sandboxTokenizedUser02";  // Your Username
        private static readonly string BKS_PASS = "sandboxTokenizedUser02@12345";  // Your Password
        private static readonly string DOMAIN = "https://localhost:7005/User/BookAppointment/";

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string?> GetAuthToken()
        {
            var url = $"{BKS_URL}/token/grant";
            var body = new
            {
                app_key = BKS_KEY,
                app_secret = BKS_SEC
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            request.Headers.Add("username", BKS_USER);
            request.Headers.Add("password", BKS_PASS);

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);
            if (json["statusCode"]?.ToString() == "0000")
            {
                return json["id_token"]?.ToString();
            }
            return null;
        }

        public static async Task<JObject?> CreatePaymentLink(string amount)
        {
            var token = await GetAuthToken();
            if (token == null) return null;

            var url = $"{BKS_URL}/create";
            var body = new
            {
                mode = "0011",
                amount = amount,
                payerReference = "test for api",
                callbackURL = DOMAIN + "PaymentStatus/",
                currency = "BDT",
                intent = "sale",
                merchantInvoiceNumber = "Inv" + new Random().Next(100000, 999999)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", token);
            request.Headers.Add("X-APP-Key", BKS_KEY);

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return JObject.Parse(content);
        }

        public static async Task<JObject?> GetPaymentDetails(string paymentID)
        {
            var token = await GetAuthToken();
            if (token == null) return null;

            var url = $"{BKS_URL}/execute";
            var body = new { paymentID = paymentID };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", token);
            request.Headers.Add("X-APP-Key", BKS_KEY);

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);
            if (json["statusCode"]?.ToString() == "0000")
            {
                return json;
            }

            return new JObject
            {
                ["success"] = false,
                ["statusMessage"] = json["statusMessage"]
            };
        }
    }
}
