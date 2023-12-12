using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace init_edgehub
{
    public class Sas
    {
        const int DEFAULT_TOKEN_EXPIRY_MINS = 60;

        static string Sign(string requestString, string key)
        {
            HMACSHA256 algorithm = new(Convert.FromBase64String(key));
            return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(requestString)));
        }

        internal static string GetSignature(string encodedURI, string key, long expiry)
        {
            return Sign($"{encodedURI}\n{expiry}", key);
        }

        public static string GetToken(string encodedURI, string key, long expiry = 0)
        {
            long expiryValue = expiry == 0 ? DateTimeOffset.UtcNow.AddMinutes(DEFAULT_TOKEN_EXPIRY_MINS).ToUnixTimeSeconds() : expiry;
            string sig = WebUtility.UrlEncode(Sign($"{encodedURI}\n{expiryValue}", key));

            return $"SharedAccessSignature sr={encodedURI}&sig={sig}&se={expiryValue}";
        }

        internal static string GetDeviceToken(string hostname, string deviceId, string key, long expiry = 0)
        {
            return GetToken(GetDeviceResourceURI(hostname, deviceId), key, expiry);
        }

        internal static string GetDeviceToken(string hostname, string deviceId, string moduleId, string key, long expiry = 0)
        {
            return GetToken(GetDeviceResourceURI(hostname, deviceId, moduleId), key, expiry);
        }

        private static string GetDeviceResourceURI(string hostname, string deviceId)
        {
            return WebUtility.UrlEncode(FormattableString.Invariant($"{hostname}/devices/{WebUtility.UrlEncode(deviceId)}"));
        }

        private static string GetDeviceResourceURI(string hostname, string deviceId, string moduleId)
        {
            return WebUtility.UrlEncode(FormattableString.Invariant($"{hostname}/devices/{WebUtility.UrlEncode(deviceId)}/modules/{WebUtility.UrlEncode(moduleId)}"));
        }
    }
}