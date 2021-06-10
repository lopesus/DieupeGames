using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LabirunModel.Tools
{
    public static class BaseConverter{
        private const string BASE_DIGITS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Convert a base 10 number to a different base.
        /// </summary>
        /// <param name = "number">Number to convert</param>
        /// <param name = "toBase">Base to use</param>
        /// <returns>String</returns>
        public static string ConvertToBase( long number, int toBase)
        {
            Debug.Assert(toBase >= 2 && toBase <= BASE_DIGITS.Length);
            if (number == 0) return "0";
            if (number < 0) throw new ArgumentOutOfRangeException("number", number, "Number cannot be negative");

            var baseChars = BASE_DIGITS.ToCharArray();
            var result = new Stack<char>();
            while (number != 0)
            {
                result.Push(baseChars[number % toBase]);
                number /= toBase;
            }

            return new string(result.ToArray());
        }

        /// <summary>
        /// Convert a string in a different base to a base 10 number
        /// </summary>
        /// <param name = "number">Number to convert</param>
        /// <param name = "fromBase">Base to convert from</param>
        /// <returns>Int64</returns>
        public static long ConvertFromBase( string number, int fromBase)
        {
            Debug.Assert(fromBase >= 2 && fromBase <= BASE_DIGITS.Length);
            long result = 0;
            long multiplier = 1;
            for (var i = number.Length - 1; i >= 0; i--)
            {
                result += BASE_DIGITS.IndexOf(number[i]) * multiplier;
                multiplier *= fromBase;
            }

            return result;
        }


    }
}