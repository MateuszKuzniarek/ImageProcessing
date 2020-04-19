using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Transforms
{
    public abstract class TransformStrategy
    {
        abstract public List<Complex> TransformSignal(List<Complex> signal);

        protected Complex GetWCoefficient(double upperCoefficient, double lowerCoefficient)
        {
            Complex result = new Complex();
            double exponent = (2.0 * Math.PI * upperCoefficient) / lowerCoefficient;
            result.Real = Math.Cos(exponent);
            result.Imaginary = Math.Sin(exponent);
            return result;
        }
    }
}
