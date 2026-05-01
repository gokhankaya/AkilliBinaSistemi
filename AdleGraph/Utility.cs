using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdleGraph
{
    public class Utility
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int Between(int minimumValue, int maximumValue, bool includeMaxValue = true)
        {
            if (!includeMaxValue)
                maximumValue = maximumValue - 1;

            return Vector.Random(1, 0, maximumValue)[0];
        }

        public static int Between2(int minimumValue, int maximumValue, bool includeMaxValue = true)
        {
            if (!includeMaxValue)
                maximumValue = maximumValue - 1;

            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            // We are using Math.Max, and substracting 0.00000000001, 
            // to ensure "multiplier" will always be between 0.0 and .99999999999
            // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // We need to add one to the range, to allow for the rounding done with Math.Floor
            int range = maximumValue - minimumValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);
        }

        /// <summary>
        /// weights bir çizelgedeki bir düğümden diğer düğüme olan kenarların ağırlıklarını tutan dizidir.
        /// Dizi içindeki değerler int olarak tutulur. Yani kenar değeri 0.1, 0.9 ise sırası ile 10 ve 90 olarak verilir.
        /// weights toplamı 100 olmalıdır.
        /// 0 değeri verilirse işleme alınmaz.
        /// </summary>
        /// <param name="weights">1-100 arası değerler alır</param>
        /// <returns></returns>
        public static int RandomWeighted(int[] weights)
        {
            int result = 0, total = 0;
            int randVal = Vector.Random(1, 1, 101)[0]; //new Random((int)DateTime.Now.Ticks).Next(0, 101);
            for (result = 0; result < weights.Length; result++)
            {
                total += weights[result];
                if (total >= randVal) break;
            }
            return result;
        }
    }
}
