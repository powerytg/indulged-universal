using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Utils
{
    public static class RandomUtils
    {
        public static float NextFloat(this Random rand)
        {
            return (float)rand.NextDouble();
        }
        public static float NextFloat(this Random rand, float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        } 
    }
}
