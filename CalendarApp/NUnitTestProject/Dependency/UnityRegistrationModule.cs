using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using GoogleCalendarService;
using GoogleCalendarService.GoogleConnection;
using log4net;
using Microsoft.Practices.Unity;
using NUnitTestProject.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTestProject.Dependency
{
    public class UnityRegistrationModule : IContainerRegistrationModule<IUnityContainer>
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IServiceLocator, CustomUnityServiceLocator>();

            //register here;
            container.RegisterType<ILog>(
            new InjectionFactory(x => LogManager.GetLogger(ConfigurationManager.AppSettings["LoggerName"])));

            var uriPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            string path = new Uri(uriPath).LocalPath + System.Configuration.ConfigurationManager.AppSettings["MapPath"];
            container.RegisterType<X509Certificate2>(new InjectionConstructor(
                 path,
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

            container.RegisterType<MeetingBooking>(new InjectionConstructor(container.Resolve<CalendarService>(), container.Resolve<ILog>()));
        }
    }
}
