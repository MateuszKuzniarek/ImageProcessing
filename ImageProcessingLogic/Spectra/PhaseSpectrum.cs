using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Spectra
{
    public class PhaseSpectrum : Spectrum
    {
        public override double GetValueForSpectrum(Complex number)
        {
            return Math.Atan(number.Imaginary / number.Real);
        }
    }
}
