using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LabirunModel.Tools;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DieupeGames.Helpers
{
    public static class ServerTools
    {
       
        public static string ExtractUsername(this string facebookName)
        {
            if (facebookName.IsEmptyString())
            {
                return "-";
            }

            try
            {
                var split = facebookName.Split(" -_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
               // var first = split.First();
                var result = split.First();

                for (int i = 1; i < split.Length ; i++)
                {
                    var name = split[i];
                    result += $".{name[0]}";
                }

                //result += first;
                return result.ToLower();
            }
            catch (Exception)
            {
                return "-";
            }
        }


        public static byte[] ConvertToBytes(this string s)
        {
            // From string to byte array
            byte[] buffer = Encoding.UTF8.GetBytes(s);
            return buffer;
        }

        public static string ConvertToString(this byte[] bin)
        {
            // From byte array to string
            string s = Encoding.UTF8.GetString(bin, 0, bin.Length);
            return s;
        }

        public static string ToJsonDotNetCore<T>(this T obj) where T : class
        {
            if (obj == null) return null;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(obj, options);
            return json;

        }
        public static T FromJson<T>(this string jsonString)
        {
            if (jsonString.IsNotEmptyString())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,// JsonNamingPolicy.CamelCase,
                    //WriteIndented = true
                };

                var jsonModel = JsonSerializer.Deserialize<T>(jsonString, options);
                return jsonModel;
            }

            return default;
        }


        public static string GenerateRandomSalt()
        {
            //https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var stringSalt = salt.ConvertToString();
            return stringSalt;
        }

        public static string HashPassword(string passWord, string salt)
        {
            var saltByte = salt.ConvertToBytes();

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passWord,
                salt: saltByte,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
