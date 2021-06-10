namespace LabirunModel.Labirun
{
    public partial class VirtualCurrencies
    {
        //public int Id { get; set; }

        public long Coin { get; set; }
        public long Gems { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Labirun.VirtualCurrencies.Coin)}: {Coin}, {nameof(Labirun.VirtualCurrencies.Gems)}: {Gems}";
        }
    }
}