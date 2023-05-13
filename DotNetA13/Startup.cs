using DotNetA13.Services;
using DotNetA13_MLE.Context;
using DotNetA13_MLE.Dao;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetA13
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddFile("movie_app.log");
            });

            // Concrete Services
            services.AddTransient<IMovieService, MovieService>();
            services.AddSingleton<IRepository, Repository>();
            services.AddDbContextFactory<MovieContext>();

            return services.BuildServiceProvider();
        }
    }
}
