using System;

namespace LabirunModel.Tools
{
    public class SuiteArithmetique1Alpha
    {
        public SuiteArithmetique1Alpha(double Somme, int n, double alpha, int roundTo)
        {
            RoundTo = roundTo;
            U1 = GetU1(Somme, n, alpha);
            R = GetR(U1, n, alpha);
        }

        double GetU1(double Somme, int n, double alpha)
        {
            var u1 = 2 * Somme / (n * (1 + alpha));
            return u1;
        }

        double GetR(double u1, int n, double alpha)
        {
            var r = (alpha * u1 - u1) / (n - 1);
            return r;
        }


        public double Terme(long n)
        {
            var t = U1 + (n - 1) * R;
            return t.RoundUpToNearestMultiOf(RoundTo);
        }

        public double Somme(long n)
        {
            if (n < 1)
            {
                throw new Exception("n must be >=1");
            }

            var un = Terme(n);

            var temp = n * (U1 + un) / 2;
            return temp;
        }

        public double U1;
        public double R;
        public int RoundTo;
    }
}