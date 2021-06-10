using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class StartProductionRequest
    {
        public string Token { get; set; }
        public ProductionTime Time { get; set; }

        public StartProductionRequest()
        {
            
        }

        public StartProductionRequest(string token)
        {
            Token = token;
        }
    }
}