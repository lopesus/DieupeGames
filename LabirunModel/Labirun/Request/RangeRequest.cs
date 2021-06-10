using System.Collections.Generic;

namespace LabirunModel.Labirun.Request
{
    public class RangeRequest
    {
        //public int Start { get; set; }
        //public int Count { get; set; }

        public long PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
        public int ItemsToSkip { get; set; }

        //for cache service init
        public List<CreatedMaze> CreatedMazes { get; set; }

        public RangeRequest()
        {

        }

        public RangeRequest(int pageNumber, int itemsPerPage=20)
        {
            PageNumber = pageNumber;
            ItemsPerPage = itemsPerPage;

            if (PageNumber < 1)
            {
                PageNumber = 1;
            }

            ItemsToSkip = (int) ((PageNumber - 1) * ItemsPerPage);
        }
    }
}