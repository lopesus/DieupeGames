using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class BuyItemRequest
    {
        public BuyItemRequest(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
        public ItemId Id { get; set; }

        public BuyItemRequest()
        {
            
        }
    }
}