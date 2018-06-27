using System.Collections.Generic;

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
    }
}