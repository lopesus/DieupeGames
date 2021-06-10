using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class Product
    {
        public ItemId Id { get; set; }
        public float UnitPrice { get; set; }
        public int ProdPerHour { get; set; }
        public int UpgradeCount { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Labirun.Product.Id)}: {Id}, {nameof(Labirun.Product.UnitPrice)}: {UnitPrice}, {nameof(Labirun.Product.ProdPerHour)}: {ProdPerHour}, {nameof(Labirun.Product.UpgradeCount)}: {UpgradeCount}";
        }
    }
}