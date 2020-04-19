using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic
{
    public class ColorChannel
    {
        public const int Blue = 0;
        public const int Green = 1;
        public const int Red = 2;
        public static List<int> All { get; } = new List<int>() { Blue, Red, Green };
    }
}
