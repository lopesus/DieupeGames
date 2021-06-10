using System;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class Bank
    {
        public BuildingId BuildingId { get; set; }
        public bool IsActive { get; set; }

        public long DailyPayout { get; set; }


        //public float BonusInterest { get; set; }
        public DateTime StartTime { get; set; }

        public override string ToString()
        {
            return $"   {nameof(DailyPayout)}: {DailyPayout}   \r\n";
            // $"   {nameof(BonusInterest)}: {BonusInterest}";
        }
    }
}