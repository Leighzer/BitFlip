using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitFlip.Asm
{
    public class Program
    {
        private static readonly Dictionary<string, string> s_switchMappings = new Dictionary<string, string>()
        {
            { "--c", "command" },
            { "--p", "sourcefilepath" },
            { "--0", "outputfilepath" }
        };

        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var host = builder.Build();
            var assembler = host.Services.GetRequiredService<Assembler>();

            assembler.Process(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.Sources.Clear();
                    config.SetBasePath(AppContext.BaseDirectory)
                          .AddCommandLine(args, s_switchMappings);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // register all your services here you'd like to use dependency injection with
                    services.AddTransient<Assembler>();
                });
        }
    }
}
