using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Filters
{
    public class HighPassFilter : Filter
    {
        private readonly double radius;

        public HighPassFilter(double radius)
        {
            this.radius = radius;
        }

        public override void ApplyFilter(List<List<Complex>> transform)
        {
            for (int i = 0; i < transform.Count; i++)
            {
                for (int j = 0; j < transform.Count; j++)
                {
                    /*int pw = transform.Count / 2;
                    int ps = transform[0].Count / 2;
                    int w = i;
                    int k = j;
                    double one = (pw - w)/
                    int jDifference = j - centerJ;
                    if (Math.Sqrt((iDifference * iDifference) + (jDifference * jDifference)) <= radius)
                    {
                        transform[i][j] = Complex.GetZero();
                    }*/
                    int centerI = transform.Count / 2;
                    int centerJ = transform[0].Count / 2;
                    int iDifference = i - centerI;
                    int jDifference = j - centerJ;
                    if (Math.Sqrt((iDifference * iDifference) + (jDifference * jDifference)) < radius)
                    {
                        transform[i][j] = Complex.GetZero();
                    }
                    else
                    {

                    }
                }
            }
        }

        public override void CreateMask(List<List<Complex>> transform, int[][] mask)
        {
            throw new NotImplementedException();
        }
    }
}
