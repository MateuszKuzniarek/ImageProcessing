using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic
{
    public class HistogramData
    {
        public int[] RedData { get; } = new int[256];
        public int[] GreenData { get; } = new int[256];
        public int[] BlueData { get; } = new int[256];
    }
}
