namespace LabirunModel.Labirun.Request
{
    public class BasicRequest
    {
        public string Token { get; set; }
        public string SessionId { get; set; }

        public BasicRequest()
        {
            
        }
        public BasicRequest(string token)
        {
            Token = token;
        }
    }
}