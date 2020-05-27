using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Filters
{
    public class EdgeDetectionFilter : Filter
    {
        double radius;
        double height;
        double angle;
        public EdgeDetectionFilter(double radius, double height, double angle)
        {
            this.radius = radius;
            this.height = height;
            this.angle = angle;
        }
        public override void ApplyFilter(List<List<Complex>> transform)
        {

            double a1 = (-1.0 * height) / 255.0;
            double radians = Math.Atan(a1);
            double newAngle1 = radians * (180 / Math.PI) + angle;
            radians = newAngle1 * (Math.PI / 180);
            a1 = Math. Tan(radians);

            double a2 = (1.0 * height) / 255.0; 
            radians = Math.Atan(a2);
            double newAngle2 = radians * (180 / Math.PI) + angle;
            radians = newAngle2 * (Math.PI / 180);
            a2 = Math.Tan(radians);

            int centerI = transform.Count / 2;
            int centerJ = transform[0].Count / 2;
            for (int i = 0; i < transform.Count; i++)
            {
                for (int j = 0; j < transform.Count; j++)
                {
                    int iDifference = i - centerI;
                    int jDifference = j - centerJ;
                    double distance = Math.Sqrt((iDifference * iDifference) + (jDifference * jDifference));
                    if(newAngle2 >= 90 && newAngle1 < 90)
                    {
                        if (((a1 * (i - centerI)) > (centerJ - j) && (a2 * (i - centerI)) > (centerJ - j)) || (distance > 0 && distance < radius))
                        {
                            transform[i][j] = Complex.GetZero();
                        }
                        else if (((a1 * (i - centerI)) < (centerJ - j) && (a2 * (i - centerI)) < (centerJ - j)) || (distance > 0 && distance < radius))
                        {
                            transform[i][j] = Complex.GetZero();
                        }
                    }
                    else
                    {
                        if (((a1 * (i - centerI)) < (centerJ - j) && (a2 * (i - centerI)) > (centerJ - j)) || (distance > 0 && distance < radius))
                        {
                            transform[i][j] = Complex.GetZero();
                        }
                        else if (((a1 * (i - centerI)) > (centerJ - j) && (a2 * (i - centerI)) < (centerJ - j)) || (distance > 0 && distance < radius))
                        {
                            transform[i][j] = Complex.GetZero();
                        }
                    }
                }
            }
        }
    }
}
