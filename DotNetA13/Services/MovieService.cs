using DotNetA13_MLE.Dao;
using DotNetA13_MLE.Models;
using Microsoft.Extensions.Logging;
using Trivial.CommandLine;

namespace DotNetA13.Services
{
    public class MovieService : IMovieService
    {
        private readonly ILogger<MovieService> _logger;
        private readonly IRepository _repository;
        private readonly List<string> _menuOptions;

        public MovieService(ILogger<MovieService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _menuOptions = new List<string>()
            {
                "Add new movie",
                "Edit movie",
                "Display movies",
                "Search movies",
                "Delete movie",
                "Add new user",
                "Rate movie",
                "List top rated movie by occupation",
                "Exit Program"
            };
        }

        // Main Program code goes here
        public void Invoke()
        {
            _logger.LogInformation("Program start");
            int menuOption;

            do
            {
                Console.WriteLine("\n----- Movie Library Application -----");
                menuOption = MenuSelection(_menuOptions);

                if (menuOption == 0)
                {
                    AddMovie();
                }
                else if (menuOption == 1)
                {
                    EditMovie();
                }
                else if (menuOption == 2)
                {
                    DisplayMovies();
                }
                else if (menuOption == 3)
                {
                    DisplaySearchResults();
                }
                else if (menuOption == 4)
                {
                    DeleteMovie();
                }
                else if (menuOption == 5)
                {
                    AddUser();
                }
                else if (menuOption == 6)
                {
                    AddRating();
                }
                else if (menuOption == 7)
                {
                    ListTopRatedMovies();
                }
            } while (menuOption != _menuOptions.Count - 1);
            _logger.LogInformation("Program end");
        }

        private void AddMovie()
        {
            _logger.LogInformation("Adding a new movie");
            Movie newMovie;

            Console.Write("Enter a movie title: ");
            string title = ValidateInput();

            Console.WriteLine($"When is the release date of {title}?");
            DateTime releaseDate = ValidateDateTime();

            newMovie = new Movie(title, releaseDate);

            AddGenres(newMovie);

            _repository.AddMovie(newMovie);
            Console.WriteLine($"Sucessfully added {newMovie}");
        }

        private void AddGenres(Movie movie)
        {
            var genres = _repository.GetAllGenres();
            var genreNames = new List<string>();
            var genreChoices = new List<string>()
            {
                "Yes", " ", "No"
            };
            var genreIds = new List<int>();
            int addGenre;

            foreach (var genre in genres)
            {
                genreNames.Add($"{genre.Name}");
            }

            Console.WriteLine($"What genre(s) is {movie.Title}?");
            do
            {
                genreIds.Add(MenuSelection(genreNames));
                Console.WriteLine("Add another genre?");
                addGenre = MenuSelection(genreChoices);
            } while (addGenre <= 1);

            foreach (var id in genreIds)
            {
                var newMovieGenre = new MovieGenre
                {
                    Movie = movie,
                    Genre = genres[id]
                };

                movie.MovieGenres.Add(newMovieGenre);
            }
        }

        private void AddRating()
        {
            _logger.LogInformation("Rating a movie");

            var ratings = new List<string>()
            {
                "5", " ", "4", " ", "3", " ", "2", " ", "1"
            };

            var movie = SelectMovie($"Which movie would you like to rate?");
            var user = SelectUser($"Which user is rating {movie.Title}?");

            int result = MenuSelection(ratings);
            long rating;
            if (result == 0 || result == 1)
            {
                rating = 5;
            }
            else if (result == 2 || result == 3)
            {
                rating = 4;
            }
            else if (result == 4 || result == 5)
            {
                rating = 3;
            }
            else if (result == 6 || result == 7)
            {
                rating = 2;
            }
            else
            {
                rating = 1;
            }

            var movieRating = new UserMovie()
            {
                Rating = rating,
                RatedAt = DateTime.Now,
                User = user,
                Movie = movie
            };

            _repository.AddRating(movieRating);
            Console.WriteLine(movieRating);
        }

        private void AddUser()
        {
            _logger.LogInformation("Adding a new user");
            User newUser;
            string gender = "M";

            Console.Write("Enter user name: ");
            string name = ValidateInput();

            Console.Write("Enter user age: ");
            int age = ValidateInt("Not a valid age!");

            var genders = new List<string>()
            {
                "Male", "Female", "Other"
            };
            Console.WriteLine("Select user gender: ");
            var genderIndex = MenuSelection(genders);
            if (genderIndex == 0)
            {
                gender = "M";
            }
            else if (genderIndex == 1)
            {
                gender = "F";
            }
            else if (genderIndex == 2)
            {
                gender = "O";
            }

            Console.Write("Enter user zip code: ");
            string zipcode = ValidateInput();

            var occupations = _repository.GetAllOccupations();
            var occupationNames = new List<string>();
            foreach (var occupation in occupations)
            {
                occupationNames.Add(occupation.Name);
            }
            Console.WriteLine("Select user Occupation");
            var userOccupation = occupations[MenuSelection(occupationNames)];
            newUser = new User()
            {
                Name = name,
                Age = Convert.ToInt64(age),
                Gender = gender,
                ZipCode = zipcode,
                Occupation = userOccupation
            };
            _repository.AddUser(newUser);
            Console.WriteLine($"Sucessfully added {newUser}");
        }

        private void DeleteMovie()
        {
            _logger.LogInformation("Deleting a movie");
            var selectedMovie = SelectMovie("Which movie would you like to delete?");

            Console.WriteLine($"Are you sure you want to delete {selectedMovie.Title}?");
            var confirmationOptions = new List<string>()
            {
                "Confirm", " ", "Cancel"
            };

            int result = MenuSelection(confirmationOptions);

            if (result <= 1)
            {
                _repository.DeleteMovie(selectedMovie);
                Console.WriteLine("Deleted");
            }
            else
            {
                _logger.LogInformation("Canceled movie delete");
                Console.WriteLine("Canceled");
            }
        }

        private void DisplayMovies()
        {
            _logger.LogInformation("Displaying movies");
            Console.WriteLine("How many movies would you like to display?");
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out int movieCount) || movieCount <= 0)
                {
                    Console.WriteLine("Not a valid amount!");
                }
                else
                {
                    List<Movie> movies;
                    if (movieCount <= 1000)
                    {
                        movies = _repository.GetMovies(movieCount);
                    }
                    else
                    {
                        movies = _repository.GetAllMovies();
                    }
                    var movieNames = new List<string>();

                    for (int i = 0; i < movieCount && i < movies.Count; i++)
                    {
                        movieNames.Add($"{movies[i].Title} | {movies[i].GetFormattedGenres()}");
                    }
                    MenuSelection(movieNames);
                    break;
                }
            }
        }

        private void DisplaySearchResults()
        {
            _logger.LogInformation("Searching for a movie");
            SelectMovie("What movie are you looking for?");
        }

        private void EditMovie()
        {
            _logger.LogInformation("Editing a movie");
            var editMenuOptions = new List<string>()
            {
                "Movie title", "Movie release date", "Movie genres", "Cancel"
            };

            var selectedMovie = SelectMovie("Which movie would you like to edit?");
            Console.WriteLine($"What would you like to update about {selectedMovie.Title}?");
            int editOption = MenuSelection(editMenuOptions);
            if (editOption == 0)
            {
                Console.WriteLine("Enter the updated movie name:");
                selectedMovie.Title = ValidateInput();
            }
            else if (editOption == 1)
            {
                Console.WriteLine("Enter the updated movie release date:");
                selectedMovie.ReleaseDate = ValidateDateTime();
            }
            else if (editOption == 2)
            {
                selectedMovie.MovieGenres = new List<MovieGenre>();
                AddGenres(selectedMovie);
            }
            _repository.SaveChanges();
        }

        private void ListTopRatedMovies()
        {
            _logger.LogInformation("Listing top rated movies");

            var users = _repository.GetAllUsers();

            var occ = users.GroupBy(x => x.Occupation)
                .Select(x => new { Occupation = x.Key, UserCount = x.Count() });

            foreach (var o in occ)
            {
                var ratings = _repository.GetAllRatings();

                var topRatedMovie = ratings
                    .Where(rating => rating.User.Occupation.Name == o.Occupation.Name)
                    .GroupBy(rating => rating.Movie.Title)
                    .OrderByDescending(ratingForMovie =>
                    {
                        int ratingCount = 0;
                        double totalRating = 0.0;
                        foreach (var rating in ratingForMovie)
                        {
                            totalRating += rating.Rating;
                            ratingCount++;
                        }
                        // Small algorithm to weigh movies by the number of ratings received
                        return (totalRating / ratingCount) * (1.0 + (ratingCount / 50.0));
                    })
                    .First();

                int ratingCount = 0;
                double totalRating = 0.0;
                foreach (var rating in topRatedMovie)
                {
                    totalRating += rating.Rating;
                    ratingCount++;
                }
                double avgRating = totalRating / ratingCount;

                Console.WriteLine($"Occupation: {o.Occupation} | {o.UserCount} Users");
                Console.WriteLine($"\t\tTop rated movie: {topRatedMovie.Key}");
                Console.WriteLine($"\t\tRated {ratingCount} times | Average rating: {avgRating:f2}");
            }
        }

        private int MenuSelection(List<string> menuOptions)
        {
            var menu = new Trivial.Collection.SelectionData<string>();
            foreach (var option in menuOptions)
            {
                menu.Add(option);
            }
            // Create a options for display.
            var options = new SelectionConsoleOptions
            {
                Question = "Please select an option: ",
                Tips = "Tips: You can use arrow key to select and press ENTER key to select",
                SelectedPrefix = ">",
                Prefix = "",
                Column = 2,
                MaxRow = 20,
            };

            int result = DefaultConsole.Select(menu, options).Index;
            Console.WriteLine(); // Spacer line
            return result;
        }

        private Movie SelectMovie(string question)
        {
            List<Movie> movies;
            do
            {
                Console.WriteLine(question);
                movies = _repository.SearchMovies(Console.ReadLine());
                if (movies.Count != 0)
                {
                    break;
                }
                Console.WriteLine("No results found.");
            } while (true);

            var movieNames = new List<string>();
            foreach (var movie in movies)
            {
                movieNames.Add($"{movie.Title} | {movie.GetFormattedGenres()}");
            }
            return movies[MenuSelection(movieNames)];
        }

        private User SelectUser(string question)
        {
            List<User> users;
            do
            {
                Console.WriteLine(question);
                users = _repository.SearchUsers(Console.ReadLine());
                if (users.Count != 0)
                {
                    break;
                }
                Console.WriteLine("No users found.");
            } while (true);

            var userNames = new List<string>();
            foreach (var user in users)
            {
                userNames.Add($"{user}");
            }
            return users[MenuSelection(userNames)];
        }

        private static DateTime ValidateDateTime()
        {
            while (true)
            {
                try
                {
                    return DateTime.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid date!");
                }
            }
        }

        private static string ValidateInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput))
                {
                    return userInput;
                }
                Console.WriteLine("Cannot be null or empty!");
            }
        }

        private static int ValidateInt(string errorMessage)
        {
            while (true)
            {
                try
                {
                    int result = int.Parse(Console.ReadLine());
                    if (result > 0)
                    {
                        return result;
                    }
                    Console.WriteLine(errorMessage);
                }
                catch (Exception)
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }
    }
}