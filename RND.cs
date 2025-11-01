using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TemaEGC_04
{
    internal class RND
    {

        private Random rndGen;

        private const int minValue = -25;
        private const int maxValue = 25;
        private const int low_coord = -50;
        private const int high_coord = 50;
        public RND()
        {
            rndGen = new Random();
        }

        public int RandomInt()
        {
            return rndGen.Next(minValue, maxValue);
        }

        public Vector3 RandomVector3()
        {
            int x = rndGen.Next(low_coord, high_coord);
            int y = rndGen.Next(low_coord, high_coord);
            int z = rndGen.Next(low_coord, high_coord);
            return new Vector3(x, y, z);
        }

        public int RandomInt(int Mval) { 
            int i = rndGen.Next(Mval);
            return i;
        }

        public int RandomInt(int minV, int maxV)
        {
            return rndGen.Next(minV, maxV);
        }
    }
}
