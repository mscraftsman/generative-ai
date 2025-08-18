using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mscc.GenerativeAI.Web
{
    /// <summary>
    /// Extension methods for setting up Google Gemini services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class GenerativeAIServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Google Gemini features to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddGenerativeAI(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Todo: Provide default options.
            //services.AddOptions<GenerativeAIOptions>()
            //    .Configure(options =>
            //    {
            //    });
            //services.AddTransient<GenerativeAIAuthenticationHandler<GenerativeAIOptions>>();
            //services.AddSingleton<IGenerativeAIOptions, GenerativeAIOptions>();
            services.AddHttpClient();
            services.AddTransient<IGenerativeModelService, GenerativeModelService>();

            return services;
        }

        /// <summary>
        /// Adds Google Gemini features to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="namedConfigurationSection">The <see cref="IConfiguration"/> section with service options.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddGenerativeAI(this IServiceCollection services, IConfiguration namedConfigurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (namedConfigurationSection == null) throw new ArgumentNullException(nameof(namedConfigurationSection));

            services.Configure<GenerativeAIOptions>(namedConfigurationSection);
            services.AddGenerativeAI();

            return services;
        }
        
        /// <summary>
        /// Adds Google Gemini features to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configSectionPath">The path to the section with service options.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddGenerativeAI(this IServiceCollection services, string configSectionPath)
        {
            services.AddOptions<GenerativeAIOptions>()
                .BindConfiguration(configSectionPath)
                //.ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddGenerativeAI();

            return services;
        }

        /// <summary>
        /// Adds Google Gemini features to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="options">A <see cref="GenerativeAIOptions" /> instance to configure the service.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddGenerativeAI(this IServiceCollection services, GenerativeAIOptions options)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (options == null) throw new ArgumentNullException(nameof(options));

            services.AddOptions<GenerativeAIOptions>()
                .Configure(o =>
                {
                    o.Credentials = options.Credentials;
                    o.ProjectId = options.ProjectId;
                    o.Region = options.Region ?? options.Location;
                });
            services.AddGenerativeAI();

            return services;
        }

        /// <summary>
        /// Adds Google Gemini features to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{GenerativeAIOptions}" /> to configure the provided <see cref="GenerativeAIOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddGenerativeAI(this IServiceCollection services, Action<GenerativeAIOptions> setupAction = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            services.AddGenerativeAI();
            services.ConfigureGenerativeAI(setupAction);

            return services;
        }

        public static void ConfigureGenerativeAI(this IServiceCollection services, Action<GenerativeAIOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
