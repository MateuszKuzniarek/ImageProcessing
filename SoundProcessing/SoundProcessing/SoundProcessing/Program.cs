using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundProcessing
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<SoundFragment> sound = new List<SoundFragment>();
            sound.Add(new SoundFragment(440, 2.5f));
            sound.Add(new SoundFragment(700, 2.5f));
            sound.Add(new SoundFragment(440, 3.5f));
            SoundGenerator waveGenerator = new SoundGenerator(sound);
            waveGenerator.Save("./test.wav");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SoundProcessing());
        }
    }
}
