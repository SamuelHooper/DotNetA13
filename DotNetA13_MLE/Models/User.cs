namespace DotNetA13_MLE.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Age { get; set; }
        public string Gender { get; set; }
        public string ZipCode { get; set; }

        public virtual Occupation Occupation { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }

        public string DisplayInfo()
        {
            return $"{Name}, {Age} | {Occupation}";
        }

        public override string ToString()
        {
            return $"{Name}, {Gender} {Age} | Zipcode: {ZipCode} | {Occupation}";
        }
    }
}
