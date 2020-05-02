using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Spectra
{
    public class AmplitudeSpectrum : Spectrum
    {
        public override double GetValueForSpectrum(Complex number)
        {
            return number.GetAbsouluteValue().Real;
            //return Math.Sqrt(number.Real * number.Real + number.Imaginary * number.Imaginary);
        }
    }
}
