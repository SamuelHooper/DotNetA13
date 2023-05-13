using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetA13_MLE.Models
{
    public class UserMovie
    {
        public long Id { get; set; }
        public long Rating { get; set; }
        public DateTime RatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }

        public override string ToString()
        {
            return $"{User.DisplayInfo()} rated {Movie.Title} a {Rating}/5 on {RatedAt:MM/dd/yyyy}";
        }
    }
}
