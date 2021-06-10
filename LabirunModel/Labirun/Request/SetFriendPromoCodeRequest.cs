namespace LabirunModel.Labirun.Request
{
    public  class SetFriendPromoCodeRequest
    {
        public string Token { get; set; }
        public string PlayerId { get; set; }

        public SetFriendPromoCodeRequest()
        {
            
        }
        public SetFriendPromoCodeRequest(string token)
        {
            Token = token;
        }
    }
}