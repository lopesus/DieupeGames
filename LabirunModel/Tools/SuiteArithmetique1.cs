using System;

namespace LabirunModel.Tools
{
    public class SuiteArithmetique1
    {
        public SuiteArithmetique1(double u1, double r)
        {
            U1 = u1;
            R = r;
        }


        public double Terme(long n)
        {
            return U1 + (n - 1) * R;
        }

        public double Somme(int n)
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
    }
}