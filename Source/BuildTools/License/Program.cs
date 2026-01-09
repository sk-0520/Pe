using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Args;
using Microsoft.Extensions.Logging;

namespace License
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("START!");
            try {
                var options = GetLicenseOptions(args, loggerFactory);
                var app = new LicenseApplication(options, loggerFactory);
                await app.ExecuteAsync(default);
                logger.LogInformation("END!");
            } catch(System.Exception ex) {
                logger.LogError(ex, ex.Message);
            }
        }

        #region function

        private static LicenseOptions GetLicenseOptions(string[] args, ILoggerFactory loggerFactory)
        {
            var commandLineConverter = new CommandLineConverter(loggerFactory);
            return commandLineConverter.Convert<LicenseOptions>(new CommandLineParser(), args);
        }

        #endregion
    }
}
