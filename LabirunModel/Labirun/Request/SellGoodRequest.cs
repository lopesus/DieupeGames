using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class SellGoodRequest
    {
        public string Token { get; set; }
        public ItemId Id { get; set; }
        public int Count { get; set; }

        public SellGoodRequest()
        {
            
        }

        public SellGoodRequest(string token)
        {
            Token = token;
        }
    }
}