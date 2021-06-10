namespace LabirunModel.Labirun.Request
{
    public class UpdateNameAndEmailRequest
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UpdateNameAndEmailRequest()
        {
            
        }
        public UpdateNameAndEmailRequest(string token)
        {
            Token = token;
        }
    }
}