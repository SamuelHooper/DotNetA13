using DotNetA13_MLE.Context;
using DotNetA13_MLE.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetA13_MLE.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void AddRating(UserMovie rating)
        {
            _context.UserMovies.Add(rating);
            _context.SaveChanges();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public List<Genre> GetAllGenres()
        {
            return _context.Genres.ToList();
        }

        public List<Movie> GetAllMovies()
        {
            return _context.Movies.ToList();
        }

        public List<UserMovie> GetAllRatings()
        {
            return _context.UserMovies.ToList();
        }

        public List<Occupation> GetAllOccupations()
        {
            return _context.Occupations.ToList();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public List<Movie> GetMovies(int numMovies)
        {
            return _context.Movies.Take(numMovies).ToList();
        }

        public List<Movie> SearchMovies(string searchString)
        {
            var allMovies = GetAllMovies();
            var searchMovies = allMovies.Where(m => m.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return searchMovies.ToList();
        }

        public List<User> SearchUsers(string searchString)
        {
            var allUsers = GetAllUsers();
            var searchUsers = allUsers.Where(u => u.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return searchUsers.ToList();
        }
    }
}
