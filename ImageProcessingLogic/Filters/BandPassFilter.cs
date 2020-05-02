using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Filters
{
    public class BandPassFilter : Filter
    {
        double radiusOut;
        double radiusIn;
        public BandPassFilter(double radiusOut, double radiusIn)
        {
            this.radiusOut = radiusOut;
            this.radiusIn = radiusIn;
        }
        public override void ApplyFilter(List<List<Complex>> transform)
        {
            int centerI = transform.Count / 2;
            int centerJ = transform[0].Count / 2;
            for (int i = 0; i < transform.Count; i++)
            {
                for (int j = 0; j < transform.Count; j++)
                {
                    int iDifference = i - centerI;
                    int jDifference = j - centerJ;
                    double distance = Math.Sqrt((iDifference * iDifference) + (jDifference * jDifference));
                    if (0 < distance && distance < radiusIn)
                    {
                        transform[i][j] = Complex.GetZero();
                    }
                    if (distance > radiusOut)
                    {
                        transform[i][j] = Complex.GetZero();
                    }
                }
            }
        }
    }
}
