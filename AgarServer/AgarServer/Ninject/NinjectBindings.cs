using AgarServer.Collision;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class NinjectBindings : NinjectModule
    {   
        public override void Load()
        {
            Bind<IColorGenerator>().To<ColorGenerator>();
            Bind<IPlayerColliser>().To<CircleColliser>();
            Bind<IShapesGenerator>().To<StaticShapesGenerator>();
            Bind<IShapesColliser>().To<ShapesColliser>();
        }
    }
}