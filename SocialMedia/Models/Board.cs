using System.Collections.Generic;

namespace SocialMedia.Models
{
  public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Name { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>();
    }
}
