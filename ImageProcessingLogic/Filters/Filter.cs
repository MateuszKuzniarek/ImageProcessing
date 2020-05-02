using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Filters
{
    public abstract class Filter
    {
        public abstract void ApplyFilter(List<List<Complex>> transform);
        public abstract void CreateMask(List<List<Complex>> transform, int[][] mask);
    }
}
