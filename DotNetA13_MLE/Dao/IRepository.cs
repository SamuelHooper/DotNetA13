using DotNetA13_MLE.Models;

namespace DotNetA13_MLE.Dao
{
    public interface IRepository
    {
        void AddMovie(Movie movie);
        void AddRating(UserMovie rating);
        void AddUser(User user);
        void DeleteMovie(Movie movie);
        void SaveChanges();
        List<Genre> GetAllGenres();
        List<Movie> GetAllMovies();
        List<UserMovie> GetAllRatings();
        List<Occupation> GetAllOccupations();
        List<User> GetAllUsers();
        List<Movie> GetMovies(int numMovies);
        List<Movie> SearchMovies(string searchString);
        List<User> SearchUsers(string searchString);
    }
}
