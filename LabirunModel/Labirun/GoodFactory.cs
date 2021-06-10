using System;
using System.Collections.Generic;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class GoodFactory
    {
        // public long Id { get; set; }
        public BuildingId BuildingId { get; set; }
        public List<FactoryBoost> Boost { get; set; }
        public List<ProductionLine> ProductionLines { get; set; }

        public int UpgradeCount { get; set; }

        //percentage , example 10 % for every production line
        public int ProductionBonus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public int RemainTime { get; set; }
        public ProductionTime ProductionTime { get; set; }
        public ProductionStatus Status { get; set; }

        public GoodFactory()
        {
            Boost = new List<FactoryBoost>()
            {
                new FactoryBoost()
                {
                    Slot = BoostSlot.Daily1,
                    EndTime = DateTime.UtcNow,
                },

                new FactoryBoost()
                {
                    Slot = BoostSlot.Daily2,
                    EndTime = DateTime.UtcNow,
                },

                new FactoryBoost()
                {
                    Slot = BoostSlot.Daily3,
                    EndTime = DateTime.UtcNow,
                },

                new FactoryBoost()
                {
                    Slot = BoostSlot.Daily4,
                    EndTime = DateTime.UtcNow,
                },

                new FactoryBoost()
                {
                    Slot = BoostSlot.Monthly1,
                    EndTime = DateTime.UtcNow,
                },

                new FactoryBoost()
                {
                    Slot = BoostSlot.Monthly2,
                    EndTime = DateTime.UtcNow,
                },

                new FactoryBoost()
                {
                    Slot = BoostSlot.Monthly3,
                    EndTime = DateTime.UtcNow,
                },

                new FactoryBoost()
                {
                    Slot = BoostSlot.Monthly200,
                    EndTime = DateTime.UtcNow,
                },
            };
            ProductionLines = new List<ProductionLine>();
        }

        public override string ToString()
        {
            return
                $"{nameof(ProductionBonus)}: {ProductionBonus},   {nameof(ProductionTime)}: {ProductionTime}, \r\n {nameof(Status)}: {Status}, {nameof(UpgradeCount)}: {UpgradeCount}";
        }
    }
}