using System.Collections.Generic;

namespace LabirunModel.Labirun.Response
{
    public class PublicMazeResponse
    {
        public List<CreatedMaze> Mazes { get; set; }
        public long TotalMazeCount { get; set; }
        public int MaxPage { get; set; }
        public PublicMazeResponse(List<CreatedMaze> mazes)
        {
            Mazes = mazes;
        }
    }
}