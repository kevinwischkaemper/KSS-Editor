using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC1_Router
{
    public static class UnitConverter
    {
        public static string ConvertMilsToFracInches(double mils)
        {
            double inches = mils / 25.4000;
            int wholeInches = (int)inches;
            double remainder = inches - wholeInches;
            for (double i = 0.0000; i < 1.0625; i = i + 0.0625)
            {
                if (i > remainder)
                {
                    double high = i;
                    double low = i - 0.0625;
                    if ((high - remainder) > (remainder - low))
                        remainder = low;
                    else
                        remainder = high;
                    break;
                }
            }
            var numerator = (int)(16.0000 / (1.0000 / remainder));
            string fractionalInches;
            if (numerator % 2 == 0)
            {
                if (numerator % 4 == 0)
                {
                    if (numerator % 8 == 0)
                        fractionalInches = $"{(numerator / 8).ToString()}/2";
                    else
                        fractionalInches = $"{(numerator / 4).ToString()}/4";
                }
                else
                    fractionalInches = $"{(numerator / 2).ToString()}/8";
            }
            else
                fractionalInches = $"{numerator.ToString()}/16";
            if (numerator == 0)
                return wholeInches.ToString();
            else
                return $"{wholeInches.ToString()} {fractionalInches}";
        }
    }
}
