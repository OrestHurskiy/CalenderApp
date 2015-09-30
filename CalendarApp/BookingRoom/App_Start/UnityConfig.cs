using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using BookingRoom.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using log4net;
using GoogleCalendarService.GoogleConnection;
using GoogleCalendarService;

namespace BookingRoom.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ILog>(
            new InjectionFactory(x => LogManager.GetLogger(System.Configuration.ConfigurationManager.AppSettings["LoggerName"])));

            container.RegisterType<X509Certificate2>(new InjectionConstructor(
                System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["MapPath"]),
                System.Configuration.ConfigurationManager.AppSettings["password"],
                X509KeyStorageFlags.Exportable
                ));

            container.RegisterType<CustomCredentialInitializer>(new InjectionConstructor(
                System.Configuration.ConfigurationManager.AppSettings["emailService"],
                new[] { CalendarService.Scope.Calendar },
                container.Resolve<X509Certificate2>()
                ));

            container.RegisterType<ServiceAccountCredential>(
                new InjectionConstructor(container.Resolve<CustomCredentialInitializer>()));

            container.RegisterType<CustomInitializer>(
                new InjectionConstructor(container.Resolve<ServiceAccountCredential>(),
                System.Configuration.ConfigurationManager.AppSettings["applicationName"]));

            container.RegisterType<CalendarService>(new InjectionConstructor(container.Resolve<CustomInitializer>()));

            container.RegisterType<MeetingBooking>(new InjectionConstructor(container.Resolve<CalendarService>(),container.Resolve<ILog>()));
        }
    }
}
