using System.Collections.Generic;

namespace LabirunModel.Labirun.Request
{
    public class UnlockUpgradeRequest
    {
        public string Token { get; set; }
        public int TotalCost { get; set; }
        public List<UpGradeRequest> ToUnlock { get; set; }

        public UnlockUpgradeRequest()
        {
            
        }
        public UnlockUpgradeRequest(string token)
        {
            Token = token;
        }
    }
}