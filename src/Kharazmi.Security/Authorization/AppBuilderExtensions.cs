using System;
using Owin;

namespace Kharazmi.Security.Authorization
{
    /// <summary>
    ///     Extension methods for setting up authorization services in an <see cref="IAppBuilder" />.
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        ///     Adds authorization services to the specified <see cref="IAppBuilder" />.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to add services to.</param>
        /// <returns>The <see cref="IAppBuilder" /> so that additional calls can be chained.</returns>
        public static IAppBuilder UseAuthorization(this IAppBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(paramName: nameof(app));
            }

            return UseAuthorization(app: app, options: new AuthorizationOptions());
        }

        /// <summary>
        ///     Adds authorization services to the specified <see cref="IAppBuilder" />.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to add services to.</param>
        /// <param name="options">The <see cref="AuthorizationOptions" /> to configure the <paramref name="app" /> with.</param>
        /// <returns>The <see cref="IAppBuilder" /> so that additional calls can be chained.</returns>
        public static IAppBuilder UseAuthorization(this IAppBuilder app, AuthorizationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(paramName: nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(paramName: nameof(options));
            }

            return app.Use(typeof(ResourceAuthorizationMiddleware), options);
        }

        /// <summary>
        ///     Adds authorization services to the specified <see cref="IAppBuilder" />.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to add services to.</param>
        /// <param name="configure">An action delegate to configure the provided <see cref="AuthorizationOptions" />.</param>
        /// <returns>The <see cref="IAppBuilder" /> so that additional calls can be chained.</returns>
        public static IAppBuilder UseAuthorization(this IAppBuilder app, Action<AuthorizationOptions> configure)
        {
            if (app == null)
            {
                throw new ArgumentNullException(paramName: nameof(app));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(paramName: nameof(configure));
            }

            var options = new AuthorizationOptions();
            configure(obj: options);
            return UseAuthorization(app: app, options: options);
        }
    }
}