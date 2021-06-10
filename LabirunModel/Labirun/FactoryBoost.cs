using System;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class FactoryBoost
    {
       // public int Id { get; set; }
        public BoostSlot Slot { get; set; }

        public ItemDuration Duration { get; set; }

        public DateTime EndTime { get; set; }
        public ItemId ItemId { get; set; }
        public int Value { get; set; }
    }
}