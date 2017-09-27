using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearchApp
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            var hotel = new StringBuilder();
            hotel.Append("\n");
            hotel.Append($"Id:\t\t{Id}");
            hotel.Append("\n");
            hotel.Append($"Name:\t\t{Name}");
            hotel.Append("\n");
            hotel.Append($"Type:\t\t{Type}");
            hotel.Append("\n");
            hotel.Append($"Description:\t{Description}");
            hotel.Append("\n");
            return hotel.ToString();
        }
    }
}
