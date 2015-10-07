using Microsoft.Practices.Unity;
using System;

namespace NUnitTestProject.Dependency
{
    public class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new
            Lazy<IUnityContainer>(() =>
            {
                var container = new UnityContainer();
                RegisterTypes(container);
                return container;
            });

        public static IUnityContainer GetUnityContainer()
        {
            return Container.Value;
        }
        public static void RegisterTypes(IUnityContainer container)
        {
            UnityRegistrationModule module = new UnityRegistrationModule();
            module.Register(container);
        }
    }
}
