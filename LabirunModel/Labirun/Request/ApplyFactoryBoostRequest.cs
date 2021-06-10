using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class ApplyFactoryBoostRequest
    {
        public string Token { get; set; }
        public BoostSlot Slot { get; set; }
        public ItemId BoostId { get; set; }

        public ApplyFactoryBoostRequest()
        {
            
        }
        public ApplyFactoryBoostRequest(string token)
        {
            Token = token;
        }
    }
}