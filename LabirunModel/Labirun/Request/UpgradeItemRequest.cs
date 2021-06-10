using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class UpgradeItemRequest
    {
        public string Token { get; set; }
        public ItemId Id { get; set; }

        public UpgradeItemRequest()
        {
            
        }
        public UpgradeItemRequest(string token)
        {
            Token = token;
        }
    }
}