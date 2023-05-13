using DotNetA13.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetA13
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = LoggerFactory.Create(b => b.AddFile("app.log"));
            var logger = factory.CreateLogger<Program>();

            try
            {
                var startup = new Startup();
                var serviceProvider = startup.ConfigureServices();
                var service = serviceProvider.GetService<IMovieService>();

                service?.Invoke();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}