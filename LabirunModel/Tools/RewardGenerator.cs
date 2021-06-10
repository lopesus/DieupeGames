using LabirunModel.Labirun;

namespace LabirunModel.Tools
{
    public class RewardGenerator
    {
        public Seed EnergyMazeSeed;
        //public Seed PromoEnergyMazeSeed;

        public RewardGenerator(Seed energyMazeSeed)
        {
            this.EnergyMazeSeed = energyMazeSeed;
        }

        public double GetMazeReward(long mazeId)
        {
            var suite = new SuiteArithmetique1(EnergyMazeSeed.U1, EnergyMazeSeed.R);
            var terme = suite.Terme(mazeId);
            return terme;
        }

        public double GetLevelReward(long mazeId, int level)
        {
            var somme = GetMazeReward(mazeId);

            var suite = new SuiteArithmetique1Alpha(somme, EnergyMazeSeed.N, EnergyMazeSeed.Alpha, 5);
            var terme = suite.Terme(level);
            return terme;
        }
    }
}