using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class Mine
    {
        //public long Id { get; set; }

        public BuildingId BuildingId;
        public int ProdPerHour;
        public int Capacity;

        public int StartTime;

        //ProductionTime: ProductionTime;
        public ProductionStatus Status;
        public ItemUpgrade CapacityUpgrade;
        public ItemUpgrade ProductionUpgrade;
    }
}