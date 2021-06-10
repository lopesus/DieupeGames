using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class UpGradeRequest
    {
        public string Token { get; set; }
        public UpgradeId Id { get; set; }
        public int Count { get; set; }

        public UpGradeRequest()
        {
            
        }
        public UpGradeRequest(string token)
        {
            Token = token;
        }
    }
}