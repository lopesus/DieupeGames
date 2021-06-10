using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class InventoryItem
    {
        public ItemId ItemId { get; set; }
        public ItemCategory Category { get; set; }
        public ItemType Type { get; set; }
        public ItemAcquisition AcquisitionType { get; set; }
        public ItemDuration Duration { get; set; }

        public int Count { get; set; }

        //number of part to convert this into a full item
        public int PartCount { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(ItemId)}: {ItemId} {nameof(Count)}: {Count} \r\n" +
                $"{nameof(Category)}: {Category}, {nameof(Type)}: {Type} \r\n" +
                $"{nameof(Duration)}: {Duration},  \r\n";
        }

    }
}