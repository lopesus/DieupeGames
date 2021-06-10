namespace LabirunModel.Labirun
{
    public partial class Maze
    {
        public int MazeId { get; set; }
        public int Level { get; set; }
        public bool IsOpen { get; set; }
        public int OpenCost { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(MazeId)}: {MazeId}, {nameof(Level)}: {Level}, {nameof(IsOpen)}: {IsOpen}, {nameof(OpenCost)}: {OpenCost}";
        }
    }
}