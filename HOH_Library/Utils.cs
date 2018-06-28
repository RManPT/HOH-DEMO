using System.Collections.Generic;
using System;

namespace HOH_Library
{
    public class Utils
    {
      

        public Utils()
        {
  
        }

        public static string TimeToStr(int counter)
        {
            int seg;
            int min = counter / 60;
            if (min != 0) seg = counter % (min * 60); else seg = counter;
            string minstr, segstr;
            if (min < 10) minstr = "0" + min.ToString(); else minstr = min.ToString();
            if (seg < 10) segstr = "0" + seg.ToString(); else segstr = seg.ToString();

            return minstr + ":" + segstr;
        }

        public static string TimeToStr(int min, int seg)
        {
            string minstr, segstr;
            if (min < 10) minstr = "0" + min.ToString(); else minstr = min.ToString();
            if (seg < 10) segstr = "0" + seg.ToString(); else segstr = seg.ToString();

            return minstr + ":" + segstr;
        }

        public static int TimeToCounter(int min, int seg)
        {
            return min * 60 + seg;
        }

        public static int Levenshtein(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}