using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Filters
{
    public class SpectrumFilter : Filter
    {
        private readonly double k;
        private readonly double l;

        public SpectrumFilter(double k, double l)
        {
            this.k = k;
            this.l = l;
        }

        public override void ApplyFilter(List<List<Complex>> transform)
        {
            int N = transform.Count;
            int M = transform[0].Count;
            for (int i = 0; i < transform.Count; i++)
            {
                for (int j = 0; j < transform.Count; j++)
                {
                    Complex temp = new Complex(0, 0);
                    temp.Real = Math.Cos((-i * k * 2 * Math.PI) / N + (-j * l * 2 * Math.PI) / M + (k + l) * Math.PI);
                    temp.Imaginary = Math.Sin((-i * k * 2 * Math.PI) / N + (-j * l * 2 * Math.PI) / M + (k + l) * Math.PI);
                    transform[i][j] = Complex.Multiply(transform[i][j], temp);
                }
            }
        }
    }
}
