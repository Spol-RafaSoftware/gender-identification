using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;

namespace GenderIdentification
{
    public class FFT
    {
        public Complex[] fft(double[] input, int nfUp, int nfDown)
        {
            int N = input.Length;
            Complex[] output = new Complex[nfUp];
            Console.WriteLine("length : " + input.Length);
            for (int k = nfDown; k < nfUp; k++)
            {
                Complex a = new Complex();
                Complex b = new Complex();
                Complex angle;
                for (int n = 0; n <= N / 2 - 1; n++)
                {
                    angle = -Complex.ImaginaryOne * 2 * Math.PI * n * k * 2 / N;
                    a = a + input[2 * n] * Complex.Exp(angle);
                    b = b + input[2 * n + 1] * Complex.Exp(angle);
                }
                Complex koef = -Complex.ImaginaryOne * 2 * Math.PI * k / N;
                output[k] = a + Complex.Exp(koef) * b;
            }
            return output;
        }
    }
}
