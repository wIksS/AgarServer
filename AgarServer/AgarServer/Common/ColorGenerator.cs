using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class ColorGenerator : IColorGenerator
    {
        private Random random;

        public ColorGenerator()
        {
            this.random = new Random();
        }

        public string GetColor()
        {
            var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"

            return color;
        }
    }
}