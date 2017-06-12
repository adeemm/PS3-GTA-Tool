using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;

namespace PS3_GTA_Tool
{
    class SPCheats
    {
        public static PS3API PS3 = Form1.PS3;
        public static uint CheatStartOffset = 0x32000000;
        public static byte[] CheatLongFoundsBytes = new byte[10000000];
        public static byte[] CheatBytes = new byte[] { 0xBD, 0x19, 0x99, 0x9A, 0x3E, 0x2E, 0x14, 0x7B, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01 };
        public static uint CheatOffset = 0;
        public static bool found;

        public static void find()
        {
            PS3.GetMemory(CheatStartOffset, CheatLongFoundsBytes);
            uint OffsetPlace = ContainsSequence(CheatLongFoundsBytes, CheatBytes, CheatStartOffset);
            if (OffsetPlace == 0u)
            {
                CheatOffset = 0x0;
                found = false;
            }
            else
            {
                uint realoffset = OffsetPlace;
                CheatOffset = realoffset + 0x0B;
                string OffsetString = Convert.ToString(realoffset, 16);
                found = true;
            }
        }

        public static uint ContainsSequence(byte[] toSearch, byte[] toFind, uint StartOffset)
        {
            int num = 0;
            while (num + toFind.Length < toSearch.Length)
            {
                bool flag = true;
                for (int index = 0; index <= toFind.Length - 1; index++)
                {
                    if (Convert.ToInt32(toSearch[num + index]) != Convert.ToInt32(toFind[index]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return StartOffset + Convert.ToUInt32(num);
                }
                num += 4;
            }
            return 0u;
        }  
    }
}
