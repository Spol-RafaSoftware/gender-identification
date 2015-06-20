using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;

namespace GenderIdentification
{
    public class getMaxFreq
    {
        public int MaxFreq(Complex[] spectrum)
        {
            int freq = 0;
            double max = 0.0;
            for (int i = 0; i < spectrum.Length; i++)
            {
                if (max < spectrum[i].Magnitude)
                {
                    freq = i;
                    max = spectrum[i].Magnitude;
                }
            }
            return freq;
        }
    }
}
