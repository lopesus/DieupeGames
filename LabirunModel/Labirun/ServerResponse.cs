using LabirunModel.Labirun.Enums;
using LabirunModel.Labirun.Response;

namespace LabirunModel.Labirun
{
    public partial class ServerResponse
    {
        public ItemId PickedItem { get; set; }
        public LeaderBoardResult LeaderBoardResult { get; set; }
        public bool MazeUnlocked { get; set; }
        public CreatedMaze CreatedMaze { get; set; }
    }
}