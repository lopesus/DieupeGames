namespace LabirunModel.Labirun
{
    public partial class PlayerExp
    {
        public int Id { get; set; }

        public int ExpLevel { get; set; }
        public int Point { get; set; }
        public int PointToNextLevel { get; set; }
        public int U1 { get; set; }
        public int Incr { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Labirun.PlayerExp.ExpLevel)}: {ExpLevel}, {nameof(Labirun.PlayerExp.Point)}: {Point}, {nameof(Labirun.PlayerExp.PointToNextLevel)}: {PointToNextLevel}";
        }
    }
}