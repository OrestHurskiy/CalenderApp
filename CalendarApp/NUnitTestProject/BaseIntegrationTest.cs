using System;
using NUnit.Framework;
using NUnitTestProject.Common;
using Microsoft.Practices.Unity;

namespace NUnitTestProject
{
    [TestFixture]
    public class BaseIntegrationTest
    {
        protected IServiceLocator _serviceLocator;
        protected IUnityContainer _unityContainer;

        [TestFixtureSetUp]
        public void Initialize()
        {           
            _unityContainer = Dependency.UnityConfig.GetUnityContainer();
            _serviceLocator = _unityContainer.Resolve<IServiceLocator>();
        }

    }
}