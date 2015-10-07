using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTestProject.Common
{
    public interface IServiceLocator
    {
        T Get<T>();
    }
}
