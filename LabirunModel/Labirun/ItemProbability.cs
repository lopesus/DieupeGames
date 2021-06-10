using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class ItemProbability
    {
        public ItemId Id;
        public int Probability;

        public ItemProbability()
        {

        }
        public ItemProbability(ItemId id, int probability)
        {
            Id = id;
            Probability = probability;
        }
    }
}