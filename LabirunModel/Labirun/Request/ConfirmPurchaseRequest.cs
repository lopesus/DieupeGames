using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class ConfirmPurchaseRequest
    {
        public string Token { get; set; }
        public AppStoreEnum AppStoreEnum{ get; set; }
        public string StoreId{ get; set; }
        public string ProductId{ get; set; }

        public string Receipt{ get; set; }

        //IsFake: boolean{ get; set; }
       

        public AndroidReceiptData AndroidReceiptData{ get; set; }

        public ConfirmPurchaseRequest()
        {
            
        }
        public ConfirmPurchaseRequest(string token)
        {
            Token = token;
        }
    }
}