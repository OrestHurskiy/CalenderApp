﻿using BookingRoom.Helpers;
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
            new InjectionFactory(x => LogManager.GetLogger(AppSettingsHelper.GetAppSetting(AppSetingsConst.LoggerName))));

            var uriPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            string path = new Uri(uriPath).LocalPath + AppSettingsHelper.GetAppSetting(AppSetingsConst.MapPath);
            container.RegisterType<X509Certificate2>(new InjectionConstructor(
                 path,
                 AppSettingsHelper.GetAppSetting(AppSetingsConst.Password),
                 X509KeyStorageFlags.Exportable
                 ));

            container.RegisterType<CustomCredentialInitializer>(new InjectionConstructor(
                AppSettingsHelper.GetAppSetting(AppSetingsConst.EmailService),
                new[] { CalendarService.Scope.Calendar },
                container.Resolve<X509Certificate2>()
                ));

            container.RegisterType<ServiceAccountCredential>(
                new InjectionConstructor(container.Resolve<CustomCredentialInitializer>()));

            container.RegisterType<CustomInitializer>(
                new InjectionConstructor(container.Resolve<ServiceAccountCredential>(),
                AppSettingsHelper.GetAppSetting(AppSetingsConst.ApplicationName)));

            container.RegisterType<CalendarService>(new InjectionConstructor(container.Resolve<CustomInitializer>()));

            container.RegisterType<BookingService>(new InjectionConstructor(container.Resolve<CalendarService>(), container.Resolve<ILog>()));
        }
    }
}
