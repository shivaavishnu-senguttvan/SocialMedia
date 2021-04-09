using System.Collections.Generic;

namespace SocialMedia.ViewModels
{
    public class BoardList
    {
        public List<Board> Boards = new List<Board>();

        public class Board
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string Name { get; set; }
        }
    }
}
