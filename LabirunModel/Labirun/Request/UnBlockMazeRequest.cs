namespace LabirunModel.Labirun.Request
{
    public class UnBlockMazeRequest
    {
        public string Token { get; set; }
        public int MazeId { get; set; }

        public UnBlockMazeRequest()
        {
            
        }
        public UnBlockMazeRequest(string token)
        {
            Token = token;
        }
    }
}