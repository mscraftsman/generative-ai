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
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<GenerativeAIAuthenticationHandler<GenerativeAIOptions>>();
            services.AddSingleton<IGenerativeModelService, GenerativeModelService>();

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
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

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
