using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public class GenericViewItem:InventoryItem
    {
        public bool HasProbability { get; set; }
        public int Probability { get; set; }

        public GenericViewItem()
        {

        }
        public GenericViewItem(ItemId id, int count)
        {
            ItemId = id;
            Count = count;
            HasProbability = false;
        }

        public GenericViewItem(ItemId id, bool hasProbability, int probability)
        {
            ItemId = id;
            HasProbability = hasProbability;
            Probability = probability;
        }
    }
}