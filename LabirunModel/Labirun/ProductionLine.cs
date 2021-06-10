using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class ProductionLine
    {
        public int Id { get; set; }
        public ProductionLineState State { get; set; }
        public int UpgradeCount { get; set; }
        public int ProdBonus { get; set; }
        public int FruitCollected { get; set; }

        public Product Product { get; set; }
        //SeedCount: number{ get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Labirun.ProductionLine.Id)}: {Id}, {nameof(Labirun.ProductionLine.State)}: {State}, {nameof(Labirun.ProductionLine.ProdBonus)}: {ProdBonus}, \r\n{nameof(Labirun.ProductionLine.Product)}: {Product},";
        }
    }
}