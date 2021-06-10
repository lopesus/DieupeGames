using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class ConvertBoosterRequest
    {
        public string Token { get; set; }
        public ItemId Id { get; set; }

        public ConvertBoosterRequest()
        {
            
        }
        public ConvertBoosterRequest(string token)
        {
            Token = token;
        }
    }
}