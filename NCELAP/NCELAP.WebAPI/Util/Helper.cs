using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Util
{
    public interface ITest { string GetEnvironment(); }

    public class Helper : ITest
    {
        private static IConfiguration _config;
        public Helper(IConfiguration configuration)
        {
            Configuration = configuration;
            _config = configuration;
        }

        private readonly IConfiguration Configuration;
        private string environment;
        public string GetEnvironment()
        {
            string currentenvironment = Configuration.GetSection("Environments").GetSection("current").Value;
            return currentenvironment;
        }

        public string GetApiServiceEnvironment()
        {
            string currentenvironment = Configuration.GetSection("Environments").GetSection("current").Value;
            string currentenvironmentApiServiceEnvironment = String.Empty;
            //string currentEnv = Configuration.GetSection("ApiServiceEnvironments").GetSection("current").Value;

            if (currentenvironment == "dev")
            {
                currentenvironmentApiServiceEnvironment = Configuration.GetSection("ApiServiceEnvironments").GetSection("dev").Value;
            }
            else if (currentenvironment == "sat")
            {
                currentenvironmentApiServiceEnvironment = Configuration.GetSection("ApiServiceEnvironments").GetSection("sat").Value;
            }
            else if (currentenvironment == "cherry")
            {
                currentenvironmentApiServiceEnvironment = Configuration.GetSection("ApiServiceEnvironments").GetSection("cherry").Value;
            }
            else
            {
                // prod
                currentenvironmentApiServiceEnvironment = Configuration.GetSection("ApiServiceEnvironments").GetSection("prod").Value;
            }

            return currentenvironmentApiServiceEnvironment;
        }

        public static DateTime GetNigerianDateTime()
        {
            var applicationDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time"));
            return applicationDate;
        }

        public static string GetYearMonthDay()
        {
            // var yearMonthDay = DateTime.Now.Year.ToString() + "-" +  + "-" + DateTime.Now.Day.ToString();
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString();
            var day = DateTime.Now.Day.ToString();

            if (day.Length == 1)
            {
                day = "0" + day;
            }

            if (month.Length == 1)
            {
                month = "0" + month;
            }
            var yearMonthDay = year + "-" + month + "-" + day;

            return yearMonthDay;
        }

        public static string GetYearMonthDayHourMinuteSeconds()
        {
            // var yearMonthDay = DateTime.Now.Year.ToString() + "-" +  + "-" + DateTime.Now.Day.ToString();
            var currentDateTime = DateTime.Now;

            var year = currentDateTime.Year.ToString();
            var month = currentDateTime.Month.ToString();
            var day = currentDateTime.Day.ToString();
            var hour = currentDateTime.Hour.ToString();
            var minute = currentDateTime.Minute.ToString();
            var second = currentDateTime.Second.ToString();

            if (day.Length == 1)
            {
                day = "0" + day;
            }

            if (month.Length == 1)
            {
                month = "0" + month;
            }

            string yearMonthDayHourMinuteSecond = year + month + day + hour + minute + second + "-" + Helper.RandomNumbers(4);

            return yearMonthDayHourMinuteSecond;
        }

        public static string GetYearMonthDayNoSpaces()
        {
            var yearMonthDay = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            return yearMonthDay;
        }

        public string GetEnvironmentUrl()
        {
            environment = this.GetEnvironment();
            string environmentUrl = Configuration.GetSection("Environments").GetSection(environment).Value;
            return environmentUrl;
        }

        public static string FormatIntegerCounter(int serialCount)
        {
            int serialCountLength = serialCount.ToString().Length;
            string result;
            switch (serialCountLength)
            {
                case 1:
                    result = "000" + serialCount.ToString();
                    break;
                case 2:
                    result = "00" + serialCount.ToString();
                    break;
                case 3:
                    result = "0" + serialCount.ToString();
                    break;
                default:
                    result = serialCount.ToString();
                    break;
            }

            return result;
        }

        private static Random random = new Random();
        public static string SHA512Hash(string secret, string code, string license)
        {
            var key = secret + code + license;
            var data = System.Text.Encoding.UTF8.GetBytes(key);
            using (SHA512 shaM = new SHA512Managed())
            {
                var hash = shaM.ComputeHash(data);
                return GetStringFromHash(hash);
            }
        }

        //public static string PrepareMailFromTemplate(string templateFilePath)
        //{

        //}

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString().ToLower();
        }

        public static string RandomNumbers(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomAlhpaNumeric(int length)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string ComputeSHA512(string rawData)
        {
            // Create a SHA256   
            var sha512 = new SHA512Managed();
            var encryptedSha512 = sha512.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            sha512.Clear();
            var hashed = BitConverter.ToString(encryptedSha512).Replace("-", "").ToLower();
            return hashed;
        }
    }

    public class RemitaAppSetting
    {
        public  string merchantId { get; set; }
        public  string APIKey { get; set; }
        public  string baseUrl { get; set; }
        public string domain { get; set; }

    }
    public class DisableActivityHandler : DelegatingHandler
    {
        public DisableActivityHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Activity.Current = null;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
