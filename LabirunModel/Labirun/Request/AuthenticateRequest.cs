namespace LabirunModel.Labirun.Request
{
    public class AuthenticateRequest
    {
        //for first time anonymous auth
        public string DeviceId { get; set; }
        public string FaceBookId { get; set; }
        public string KartRidgeId { get; set; }
        public string SteamId { get; set; }

        public string Token { get; set; }


        //[Required]
        public string Username { get; set; }

        //[Required]
        public string Password { get; set; }

        public bool IsTester { get; set; }

        public AuthenticateRequest()
        {
            
        }
    }
}