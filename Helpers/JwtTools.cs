using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LabirunModel.Labirun;
using LabirunModel.Tools;
using learnCore;

namespace LabirunServer.Helpers
{
    public static class JwtTools
    {
        /// <summary>
        /// in api controller  var identity = HttpContext.User.Identity as ClaimsIdentity;
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static async Task<PlayerData> GetCurrentPlayer(ClaimsIdentity identity, MongoDBContext context)
        {
            if (identity != null)
            {
                var claims = identity.Claims.ToList();
                var name = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (name != null)
                {
                    var id = name.Value;
                    var res = await context.Players.Find(p => p.Id == id);
                    return res;
                }
            }

            return null;
            //}

        }



        public static string CreateJwtToken(PlayerData player, string secret)
        {
            return "";
            //try
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Encoding.ASCII.GetBytes(secret);
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new Claim[]
            //        {
            //            new Claim(ClaimTypes.Name, player.Id)
            //        }),
            //        Expires = DateTime.UtcNow.AddDays(1),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            //            SecurityAlgorithms.HmacSha256Signature)
            //    };
            //    var token = tokenHandler.CreateToken(tokenDescriptor);
            //    var stringToken = tokenHandler.WriteToken(token);

            //    return stringToken;
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.Message);
            //    return null;
            //}
        }


        /// <summary>
        /// in api controller  var identity = HttpContext.User.Identity as ClaimsIdentity;
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetPlayerIdFromClaims(ClaimsIdentity identity)
        {
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims.ToList();
                var name = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (name != null) return name.Value;
                // or
                //var value = identity.FindFirst("ClaimName").Value;
            }

            return null;
        }



        //public static async Task<PlayerData> GetCurrentPlayer0(string token, string key, MongoDBContext context)
        //{
        //    if (token.IsNotEmptyString() && key.IsNotEmptyString())
        //    {
        //        var id = EncryptionTools.DecryptString(token, key);
        //        var res = await context.PlayerData.Find(p => p.Id == id);
        //        return res;
        //    }

        //    return null;
        //}
    }
}