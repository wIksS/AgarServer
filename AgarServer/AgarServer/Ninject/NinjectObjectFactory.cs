using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class NinjectObjectFactory
    {
        private static StandardKernel kernel = new StandardKernel(new NinjectBindings());

        public static T GetObject<T>()
        {
            return kernel.Get<T>();
        }
    }
}