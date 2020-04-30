using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLogic.Spectra
{
    public abstract class Spectrum
    {
        public abstract double GetValueForSpectrum(Complex number);
    }
}
