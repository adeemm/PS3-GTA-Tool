using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3_GTA_Tool
{
    class ModelChanger
    {
        public static int NumberOffsets = 0;
        public static uint ZeroOffset = 0;
        public static uint ModelStartOffset = 844103680;
        public static int ModelMinaseNumber = 20;
        public static uint ModelOffsetOne = 0;
        public static bool found;

        public static uint ContainsSequence_by_KranK_ModZ(byte[] toSearch, byte[] toFind, uint StartOffset, int bytes)
        {
            uint startOffset;
            int num = 0;
            while (true)
            {
                if (num + (int)toFind.Length < (int)toSearch.Length)
                {
                    bool flag = true;
                    int num1 = 0;
                    while (num1 < (int)toFind.Length)
                    {
                        if (toSearch[num + num1] == toFind[num1])
                        {
                            num1++;
                        }
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        num = num + bytes;
                    }
                    else
                    {
                        NumberOffsets = NumberOffsets + 1;
                        startOffset = StartOffset + (uint)num;
                        break;
                    }
                }
                else
                {
                    startOffset = 0;
                    break;
                }
            }
            return startOffset;
        }

        public static uint Search(byte[] Search, uint Start, int Length, int bytes)
        {
            uint num;
            byte[] numArray = Form1.PS3.Extension.ReadBytes(Start, Length);
            uint num1 = ContainsSequence_by_KranK_ModZ(numArray, Search, Start, bytes);
            if (!num1.Equals(ZeroOffset))
            {
                int num2 = 0;
                byte[] search = Search;
                for (int i = 0; i < (int)search.Length; i++)
                {
                    if (search[i] == 1)
                    {
                        num2++;
                    }
                }
                num = num1 + (uint)num2;
            }
            else
            {
                num = 0;
            }
            return num;
        }

        public static void find()
        {
            uint num = ModelChanger.Search(new byte[] { 24, 206, 87, 208 }, 844103680, 2097152, 4);
            if (num != ModelChanger.ZeroOffset)
            {
                uint modelMinaseNumber = num - (uint)ModelChanger.ModelMinaseNumber;
                ModelChanger.ModelOffsetOne = modelMinaseNumber;
                ModelChanger.found = true;
            }
            else
            {
                ModelChanger.ModelOffsetOne = 0;
            }
        }

        public static void changeModel(string selected)
        {
            byte[] numArray;
            if (selected.Equals("FRANKLIN"))
            {
                byte[] numArray1 = new byte[] { 25, 2, 255, 247 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray1);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray1);
            }
            else if (selected.Equals("TREVOR"))
            {
                byte[] numArray2 = new byte[] { 25, 3, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray2);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray2);

            }
            else if (selected.Equals("MR. ROGERS"))
            {
                byte[] numArray3 = new byte[] { 27, 14, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray3);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray3);

            }
            else if (selected.Equals("PARAMEDIC"))
            {
                byte[] numArray4 = new byte[] { 27, 13, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray4);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray4);

            }
            else if (selected.Equals("ALIEN"))
            {
                byte[] numArray5 = new byte[] { 27, 10, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray5);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray5);

            }
            else if (selected.Equals("ASTRONAUT"))
            {
                byte[] numArray6 = new byte[] { 27, 12, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray6);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray6);

            }
            else if (selected.Equals("MONKEY"))
            {
                byte[] numArray7 = new byte[] { 27, 85, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray7);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray7);

            }
            else if (selected.Equals("PROSTITUTE 3"))
            {
                byte[] numArray8 = new byte[] { 27, 73, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray8);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray8);

            }
            else if (selected.Equals("PROSTITUTE 2"))
            {
                byte[] numArray9 = new byte[] { 27, 72, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray9);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray9);

            }
            else if (selected.Equals("PROSTITUTE 1"))
            {
                byte[] numArray10 = new byte[] { 27, 71, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray10);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray10);

            }
            else if (selected.Equals("LOS SANTOS MECHANIC 4"))
            {
                byte[] numArray11 = new byte[] { 27, 81, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray11);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray11);

            }
            else if (selected.Equals("LOS SANTOS MECHANIC 3"))
            {
                byte[] numArray12 = new byte[] { 27, 70, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray12);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray12);

            }
            else if (selected.Equals("LOS SANTOS MECHANIC 2"))
            {
                byte[] numArray13 = new byte[] { 27, 69, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray13);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray13);

            }
            else if (selected.Equals("LOS SANTOS MECHANIC 1"))
            {
                byte[] numArray14 = new byte[] { 27, 68, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray14);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray14);

            }
            else if (selected.Equals("WAITER"))
            {
                byte[] numArray15 = new byte[] { 27, 67, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray15);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray15);

            }
            else if (selected.Equals("VALET"))
            {
                byte[] numArray16 = new byte[] { 27, 66, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray16);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray16);

            }
            else if (selected.Equals("COAST GUARD"))
            {
                byte[] numArray17 = new byte[] { 27, 65, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray17);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray17);

            }
            else if (selected.Equals("SWAT TEAM"))
            {
                byte[] numArray18 = new byte[] { 27, 64, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray18);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray18);

            }
            else if (selected.Equals("INMATE 2"))
            {
                byte[] numArray19 = new byte[] { 27, 57, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray19);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray19);

            }
            else if (selected.Equals("INMATE 1"))
            {
                byte[] numArray20 = new byte[] { 25, 227, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray20);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray20);

            }
            else if (selected.Equals("LSPD PILOT 2"))
            {
                byte[] numArray21 = new byte[] { 27, 15, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray21);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray21);

            }
            else if (selected.Equals("LSPD PILOT 1"))
            {
                byte[] numArray22 = new byte[] { 27, 56, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray22);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray22);

            }
            else if (selected.Equals("EXTERMINATOR"))
            {
                byte[] numArray23 = new byte[] { 27, 55, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray23);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray23);

            }
            else if (selected.Equals("STATUE GUY"))
            {
                byte[] numArray24 = new byte[] { 27, 82, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray24);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray24);

            }
            else if (selected.Equals("MIME"))
            {
                byte[] numArray25 = new byte[] { 27, 54, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray25);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray25);

            }
            else if (selected.Equals("MILITARY SOLDIER 4"))
            {
                byte[] numArray26 = new byte[] { 27, 53, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray26);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray26);

            }
            else if (selected.Equals("MILITARY SOLDIER 3"))
            {
                byte[] numArray27 = new byte[] { 27, 6, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray27);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray27);

            }
            else if (selected.Equals("MILITARY SOLDIER 2"))
            {
                byte[] numArray28 = new byte[] { 27, 52, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray28);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray28);

            }
            else if (selected.Equals("MILITARY SOLDIER 1"))
            {
                byte[] numArray29 = new byte[] { 27, 51, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray29);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray29);

            }
            else if (selected.Equals("BOUNCER 1"))
            {
                byte[] numArray30 = new byte[] { 27, 49, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray30);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray30);

            }
            else if (selected.Equals("HOODED THUG"))
            {
                byte[] numArray31 = new byte[] { 27, 40, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray31);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray31);

            }
            else if (selected.Equals("LSPD"))
            {
                byte[] numArray32 = new byte[] { 27, 39, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray32);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray32);

            }
            else if (selected.Equals("CITY WORKER 3"))
            {
                byte[] numArray33 = new byte[] { 27, 48, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray33);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray33);

            }
            else if (selected.Equals("CITY WORKER 2"))
            {
                byte[] numArray34 = new byte[] { 27, 38, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray34);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray34);

            }
            else if (selected.Equals("CITY WORKER 1"))
            {
                byte[] numArray35 = new byte[] { 27, 37, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray35);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray35);

            }
            else if (selected.Equals("CLOWN"))
            {
                byte[] numArray36 = new byte[] { 27, 36, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray36);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray36);

            }
            else if (selected.Equals("CHEF"))
            {
                byte[] numArray37 = new byte[] { 27, 35, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray37);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray37);

            }
            else if (selected.Equals("MERRY WEATHER 2"))
            {
                byte[] numArray38 = new byte[] { 27, 34, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray38);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray38);

            }
            else if (selected.Equals("MERRY WEATHER 1"))
            {
                byte[] numArray39 = new byte[] { 27, 33, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray39);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray39);

            }
            else if (selected.Equals("BUS BOY"))
            {
                byte[] numArray40 = new byte[] { 27, 32, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray40);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray40);

            }
            else if (selected.Equals("PEDESTRIAN 1"))
            {
                numArray = new byte[] { 27, 83, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray);

            }
            else if (selected.Equals("PEDESTRIAN 2"))
            {
                byte[] numArray41 = new byte[] { 27, 80, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray41);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray41);

            }
            else if (selected.Equals("PEDESTRIAN 3"))
            {
                numArray = new byte[] { 27, 25, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray);

            }
            else if (selected.Equals("UPS WORKER 2"))
            {
                byte[] numArray42 = new byte[] { 27, 24, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray42);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray42);

            }
            else if (selected.Equals("UPS WORKER 1"))
            {
                byte[] numArray43 = new byte[] { 27, 23, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray43);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray43);

            }
            else if (selected.Equals("HILLBILLY"))
            {
                byte[] numArray44 = new byte[] { 27, 22, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray44);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray44);

            }
            else if (selected.Equals("YANKTON POLICE"))
            {
                byte[] numArray45 = new byte[] { 27, 21, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray45);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray45);

            }
            else if (selected.Equals("DOCTOR 1"))
            {
                byte[] numArray46 = new byte[] { 27, 20, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray46);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray46);

            }
            else if (selected.Equals("SHERIFF 3"))
            {
                byte[] numArray47 = new byte[] { 27, 50, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray47);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray47);

            }
            else if (selected.Equals("SHERIFF 2"))
            {
                byte[] numArray48 = new byte[] { 27, 19, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray48);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray48);

            }
            else if (selected.Equals("SHERIFF 1"))
            {
                byte[] numArray49 = new byte[] { 27, 19, 255, 240 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray49);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray49);

            }
            else if (selected.Equals("PRISON GUARD"))
            {
                byte[] numArray50 = new byte[] { 27, 18, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray50);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray50);

            }
            else if (selected.Equals("GO POSTAL WORKER"))
            {
                byte[] numArray51 = new byte[] { 27, 17, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray51);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray51);

            }
            else if (selected.Equals("GARDENER"))
            {
                byte[] numArray52 = new byte[] { 27, 9, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray52);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray52);

            }
            else if (selected.Equals("AMIGO"))
            {
                byte[] numArray53 = new byte[] { 27, 8, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray53);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray53);

            }
            else if (selected.Equals("MILITARY GENERAL"))
            {
                byte[] numArray54 = new byte[] { 27, 7, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray54);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray54);

            }
            else if (selected.Equals("RAJ"))
            {
                byte[] numArray55 = new byte[] { 27, 5, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray55);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray55);

            }
            else if (selected.Equals("BUTCHER 2"))
            {
                byte[] numArray56 = new byte[] { 27, 84, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray56);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray56);

            }
            else if (selected.Equals("BUTCHER 1"))
            {
                byte[] numArray57 = new byte[] { 27, 4, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray57);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray57);

            }
            else if (selected.Equals("MECHANIC"))
            {
                byte[] numArray58 = new byte[] { 27, 3, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray58);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray58);

            }
            else if (selected.Equals("SECRET SERVICE 2"))
            {
                byte[] numArray59 = new byte[] { 27, 2, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray59);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray59);

            }
            else if (selected.Equals("SECRET SERVICE 1"))
            {
                byte[] numArray60 = new byte[] { 27, 1, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray60);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray60);

            }
            else if (selected.Equals("BARBER QUEER"))
            {
                byte[] numArray61 = new byte[] { 27, 0, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray61);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray61);

            }
            else if (selected.Equals("TREY BAKER BIKER GUY"))
            {
                byte[] numArray62 = new byte[] { 25, 239, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray62);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray62);

            }
            else if (selected.Equals("WHITE GUY THUG"))
            {
                byte[] numArray63 = new byte[] { 25, 238, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray63);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray63);

            }
            else if (selected.Equals("MOUNTAIN BIKER"))
            {
                byte[] numArray64 = new byte[] { 25, 237, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray64);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray64);

            }
            else if (selected.Equals("BUSINESS MAN 1"))
            {
                byte[] numArray65 = new byte[] { 25, 236, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray65);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray65);

            }
            else if (selected.Equals("BURGER SHOT WORKER"))
            {
                byte[] numArray66 = new byte[] { 25, 235, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray66);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray66);

            }
            else if (selected.Equals("PEDESTRIAN 4"))
            {
                byte[] numArray67 = new byte[] { 25, 234, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray67);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray67);

            }
            else if (selected.Equals("BUSINESS LADY 1"))
            {
                byte[] numArray68 = new byte[] { 25, 233, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray68);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray68);

            }
            else if (selected.Equals("MOHAWK BIKER"))
            {
                byte[] numArray69 = new byte[] { 25, 232, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray69);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray69);

            }
            else if (selected.Equals("MALE STRIPPER"))
            {
                byte[] numArray70 = new byte[] { 25, 231, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray70);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray70);

            }
            else if (selected.Equals("LEATHER BIKER"))
            {
                byte[] numArray71 = new byte[] { 25, 230, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray71);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray71);

            }
            else if (selected.Equals("HALO MAN"))
            {
                byte[] numArray72 = new byte[] { 25, 229, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray72);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray72);

            }
            else if (selected.Equals("PEDESTRIAN 5"))
            {
                byte[] numArray73 = new byte[] { 25, 228, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray73);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray73);

            }
            else if (selected.Equals("MONKEY SUIT"))
            {
                byte[] numArray74 = new byte[] { 25, 226, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray74);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray74);

            }
            else if (selected.Equals("PEDESTRIAN 6"))
            {
                byte[] numArray75 = new byte[] { 25, 225, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray75);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray75);

            }
            else if (selected.Equals("DUCK DYNASTY"))
            {
                byte[] numArray76 = new byte[] { 25, 224, 255, 245 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray76);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray76);

            }
            else if (selected.Equals("PEDESTRIAN 7"))
            {
                byte[] numArray77 = new byte[] { 26, 236, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray77);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray77);

            }
            else if (selected.Equals("LIFE GUARD 1"))
            {
                byte[] numArray78 = new byte[] { 26, 232, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray78);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray78);

            }
            else if (selected.Equals("BUSINESS MAN 2"))
            {
                byte[] numArray79 = new byte[] { 26, 232, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray79);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray79);

            }
            else if (selected.Equals("LOST AND DAMMED BIKER 1"))
            {
                byte[] numArray80 = new byte[] { 26, 212, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray80);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray80);

            }
            else if (selected.Equals("NURSE"))
            {
                byte[] numArray81 = new byte[] { 26, 238, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray81);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray81);

            }
            else if (selected.Equals("TRANSIT TRAM DRIVER 1"))
            {
                byte[] numArray82 = new byte[] { 26, 255, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray82);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray82);

            }
            else if (selected.Equals("DRUNK BUM 1"))
            {
                byte[] numArray83 = new byte[] { 25, 101, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray83);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray83);

            }
            else if (selected.Equals("IAA SECURITY"))
            {
                byte[] numArray84 = new byte[] { 27, 78, 255, 242 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray84);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray84);

            }
            else if (selected.Equals("DANE COOK JASON SHIRT"))
            {
                byte[] numArray85 = new byte[] { 27, 63, 255, 243 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray85);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray85);

            }
            else if (selected.Equals("HITLER"))
            {
                byte[] numArray86 = new byte[] { 27, 11, 255, 240 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray86);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray86);

            }
            else if (selected.Equals("DAVE CHAPPELLE"))
            {
                byte[] numArray87 = new byte[] { 26, 161, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray87);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray87);

            }
            else if (selected.Equals("CORN ROW THUG"))
            {
                byte[] numArray88 = new byte[] { 26, 162, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray88);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray88);

            }
            else if (selected.Equals("PEDESTRIAN 8"))
            {
                byte[] numArray89 = new byte[] { 26, 163, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray89);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray89);

            }
            else if (selected.Equals("PEDESTRIAN 9"))
            {
                byte[] numArray90 = new byte[] { 26, 164, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray90);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray90);

            }
            else if (selected.Equals("BEACH PED 1"))
            {
                byte[] numArray91 = new byte[] { 26, 165, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray91);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray91);

            }
            else if (selected.Equals("BEACH PED 2"))
            {
                byte[] numArray92 = new byte[] { 26, 166, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray92);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray92);

            }
            else if (selected.Equals("BEACH PED 3"))
            {
                byte[] numArray93 = new byte[] { 26, 167, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray93);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray93);

            }
            else if (selected.Equals("PEDESTRIAN 10"))
            {
                byte[] numArray94 = new byte[] { 26, 168, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray94);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray94);

            }
            else if (selected.Equals("COLLEGE STUDENT"))
            {
                byte[] numArray95 = new byte[] { 26, 169, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray95);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray95);

            }
            else if (selected.Equals("PEDESTRIAN 11"))
            {
                byte[] numArray96 = new byte[] { 26, 160, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray96);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray96);

            }
            else if (selected.Equals("EMO 1"))
            {
                byte[] numArray97 = new byte[] { 26, 170, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray97);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray97);

            }
            else if (selected.Equals("PEDESTRIAN 12"))
            {
                byte[] numArray98 = new byte[] { 26, 171, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray98);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray98);

            }
            else if (selected.Equals("EMO 2"))
            {
                byte[] numArray99 = new byte[] { 26, 172, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray99);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray99);

            }
            else if (selected.Equals("GYM GUY"))
            {
                byte[] numArray100 = new byte[] { 26, 173, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray100);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray100);

            }
            else if (selected.Equals("BUSINESS MAN 3"))
            {
                byte[] numArray101 = new byte[] { 26, 174, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray101);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray101);

            }
            else if (selected.Equals("DICKLESS MAN"))
            {
                byte[] numArray102 = new byte[] { 26, 175, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray102);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray102);

            }
            else if (selected.Equals("CLINT EASTWOOD"))
            {
                byte[] numArray103 = new byte[] { 26, 177, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray103);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray103);

            }
            else if (selected.Equals("OLD MAN 1"))
            {
                byte[] numArray104 = new byte[] { 26, 178, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray104);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray104);

            }
            else if (selected.Equals("CHINESE 1"))
            {
                byte[] numArray105 = new byte[] { 26, 179, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray105);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray105);

            }
            else if (selected.Equals("OLD MAN 2"))
            {
                byte[] numArray106 = new byte[] { 26, 180, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray106);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray106);

            }
            else if (selected.Equals("OLD MAN 3"))
            {
                byte[] numArray107 = new byte[] { 26, 181, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray107);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray107);

            }
            else if (selected.Equals("DRUNK BUM 2"))
            {
                byte[] numArray108 = new byte[] { 26, 182, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray108);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray108);

            }
            else if (selected.Equals("DRUNK BUM 3"))
            {
                byte[] numArray109 = new byte[] { 26, 183, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray109);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray109);

            }
            else if (selected.Equals("DRUNK BUM 4"))
            {
                byte[] numArray110 = new byte[] { 26, 184, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray110);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray110);

            }
            else if (selected.Equals("PEDESTRIAN 13"))
            {
                byte[] numArray111 = new byte[] { 26, 185, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray111);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray111);

            }
            else if (selected.Equals("REDNECK 1"))
            {
                byte[] numArray112 = new byte[] { 26, 176, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray112);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray112);

            }
            else if (selected.Equals("HEIST ROBBER 1"))
            {
                byte[] numArray113 = new byte[] { 26, 193, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray113);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray113);

            }
            else if (selected.Equals("CHINESE 2"))
            {
                byte[] numArray114 = new byte[] { 26, 194, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray114);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray114);

            }
            else if (selected.Equals("CHINESE 3"))
            {
                byte[] numArray115 = new byte[] { 26, 195, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray115);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray115);

            }
            else if (selected.Equals("OLD MAN 4"))
            {
                byte[] numArray116 = new byte[] { 26, 196, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray116);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray116);

            }
            else if (selected.Equals("CHINESE 4"))
            {
                byte[] numArray117 = new byte[] { 26, 197, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray117);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray117);

            }
            else if (selected.Equals("MATLOCK"))
            {
                byte[] numArray118 = new byte[] { 26, 198, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray118);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray118);

            }
            else if (selected.Equals("MOB MAN 1"))
            {
                byte[] numArray119 = new byte[] { 26, 199, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray119);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray119);

            }
            else if (selected.Equals("SKATER 1"))
            {
                byte[] numArray120 = new byte[] { 26, 200, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray120);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray120);

            }
            else if (selected.Equals("COREY FELDMAN"))
            {
                byte[] numArray121 = new byte[] { 26, 201, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray121);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray121);

            }
            else if (selected.Equals("HUMANE LAB WORKER"))
            {
                byte[] numArray122 = new byte[] { 26, 192, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray122);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray122);

            }
            else if (selected.Equals("THUG 1"))
            {
                byte[] numArray123 = new byte[] { 26, 224, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray123);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray123);

            }
            else if (selected.Equals("TYRESE"))
            {
                byte[] numArray124 = new byte[] { 26, 225, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray124);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray124);

            }
            else if (selected.Equals("FRANKLINS AUNT"))
            {
                byte[] numArray125 = new byte[] { 26, 226, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray125);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray125);

            }
            else if (selected.Equals("MAID"))
            {
                byte[] numArray126 = new byte[] { 26, 227, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray126);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray126);

            }
            else if (selected.Equals("BUSINESS LADY 2"))
            {
                byte[] numArray127 = new byte[] { 26, 228, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray127);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray127);

            }
            else if (selected.Equals("PEDESTRIAN 14"))
            {
                byte[] numArray128 = new byte[] { 26, 229, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray128);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray128);

            }
            else if (selected.Equals("BUSINESS LADY 3"))
            {
                byte[] numArray129 = new byte[] { 26, 230, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray129);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray129);

            }
            else if (selected.Equals("VANILLA STRIPPER"))
            {
                byte[] numArray130 = new byte[] { 26, 231, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray130);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray130);

            }
            else if (selected.Equals("LSPD FEMALE"))
            {
                byte[] numArray131 = new byte[] { 26, 233, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray131);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray131);

            }
            else if (selected.Equals("NURSE IN SCRUBS"))
            {
                byte[] numArray132 = new byte[] { 26, 234, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray132);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray132);

            }
            else if (selected.Equals("GARDENER FEMALE"))
            {
                byte[] numArray133 = new byte[] { 26, 235, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray133);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray133);

            }
            else if (selected.Equals("FEMALE PARK RANGER"))
            {
                byte[] numArray134 = new byte[] { 26, 237, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray134);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray134);

            }
            else if (selected.Equals("SHERIFF 4"))
            {
                byte[] numArray135 = new byte[] { 26, 239, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray135);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray135);

            }
            else if (selected.Equals("PROSTITUTE 4"))
            {
                byte[] numArray136 = new byte[] { 26, 241, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray136);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray136);

            }
            else if (selected.Equals("CASHIER"))
            {
                byte[] numArray137 = new byte[] { 26, 242, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray137);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray137);

            }
            else if (selected.Equals("AMMUNATION WORKER"))
            {
                byte[] numArray138 = new byte[] { 26, 243, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray138);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray138);

            }
            else if (selected.Equals("SHERIFF 5"))
            {
                byte[] numArray139 = new byte[] { 26, 244, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray139);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray139);

            }
            else if (selected.Equals("SHERIFF 6"))
            {
                byte[] numArray140 = new byte[] { 26, 244, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray140);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray140);

            }
            else if (selected.Equals("HANDY MAN 1"))
            {
                byte[] numArray141 = new byte[] { 26, 246, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray141);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray141);

            }
            else if (selected.Equals("TRANSIT TRAM DRIVER 2"))
            {
                byte[] numArray142 = new byte[] { 26, 247, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray142);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray142);

            }
            else if (selected.Equals("STRIP CLUB BOUNCER"))
            {
                byte[] numArray143 = new byte[] { 26, 248, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray143);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray143);
            }
            else if (selected.Equals("CITY WORKER 4"))
            {
                byte[] numArray144 = new byte[] { 26, 249, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray144);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray144);

            }
            else if (selected.Equals("PEDESTRIAN 15"))
            {
                byte[] numArray145 = new byte[] { 26, 240, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray145);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray145);

            }
            else if (selected.Equals("DOCTOR 2"))
            {
                byte[] numArray146 = new byte[] { 26, 250, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray146);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray146);

            }
            else if (selected.Equals("FIB AGENT 1"))
            {
                byte[] numArray147 = new byte[] { 26, 251, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray147);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray147);

            }
            else if (selected.Equals("BUSINESS MAN 4"))
            {
                byte[] numArray148 = new byte[] { 26, 252, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray148);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray148);

            }
            else if (selected.Equals("HANDY MAN 2"))
            {
                byte[] numArray149 = new byte[] { 26, 253, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray149);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray149);

            }
            else if (selected.Equals("CITY WORKER 5"))
            {
                byte[] numArray150 = new byte[] { 26, 254, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray150);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray150);

            }
            else if (selected.Equals("JANITOR"))
            {
                byte[] numArray151 = new byte[] { 26, 255, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray151);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray151);

            }
            else if (selected.Equals("FAT CHICK 1"))
            {
                byte[] numArray152 = new byte[] { 26, 1, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray152);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray152);

            }
            else if (selected.Equals("FAT CHICK 2 THONG"))
            {
                byte[] numArray153 = new byte[] { 26, 2, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray153);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray153);

            }
            else if (selected.Equals("FAT CHICK 3"))
            {
                byte[] numArray154 = new byte[] { 26, 3, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray154);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray154);

            }
            else if (selected.Equals("CHINESE 5"))
            {
                byte[] numArray155 = new byte[] { 26, 5, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray155);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray155);

            }
            else if (selected.Equals("BIKINI GIRL 1"))
            {
                byte[] numArray156 = new byte[] { 26, 21, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray156);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray156);

            }
            else if (selected.Equals("BUSINESS LADY 4"))
            {
                byte[] numArray157 = new byte[] { 26, 22, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray157);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray157);

            }
            else if (selected.Equals("BUSINESS LADY 5"))
            {
                byte[] numArray158 = new byte[] { 26, 24, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray158);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray158);

            }
            else if (selected.Equals("MICHAELS WIFE"))
            {
                byte[] numArray159 = new byte[] { 26, 53, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray159);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray159);

            }
            else if (selected.Equals("BIKINI GIRL 2"))
            {
                byte[] numArray160 = new byte[] { 26, 54, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray160);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray160);

            }
            else if (selected.Equals("FAT GUY 1"))
            {
                byte[] numArray161 = new byte[] { 26, 69, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray161);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray161);

            }
            else if (selected.Equals("FAT GUY 2"))
            {
                byte[] numArray162 = new byte[] { 26, 70, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray162);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray162);

            }
            else if (selected.Equals("FAT GUY 3"))
            {
                byte[] numArray163 = new byte[] { 26, 71, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray163);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray163);
            }
            else if (selected.Equals("FAT GUY 4"))
            {
                byte[] numArray164 = new byte[] { 26, 72, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray164);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray164);

            }
            else if (selected.Equals("GEORGE LOPEZ"))
            {
                byte[] numArray165 = new byte[] { 26, 81, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray165);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray165);

            }
            else if (selected.Equals("DR DRE"))
            {
                byte[] numArray166 = new byte[] { 26, 82, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray166);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray166);

            }
            else if (selected.Equals("SAMUEL JACKSON"))
            {
                byte[] numArray167 = new byte[] { 26, 89, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray167);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray167);

            }
            else if (selected.Equals("THUG 2"))
            {
                byte[] numArray168 = new byte[] { 26, 96, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray168);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray168);

            }
            else if (selected.Equals("THUG 3"))
            {
                byte[] numArray169 = new byte[] { 26, 97, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray169);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray169);

            }
            else if (selected.Equals("FAT GUY 5"))
            {
                byte[] numArray170 = new byte[] { 26, 100, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray170);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray170);

            }
            else if (selected.Equals("DRUNK BUM 5"))
            {
                byte[] numArray171 = new byte[] { 26, 101, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray171);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray171);

            }
            else if (selected.Equals("DRAG QUEER 1"))
            {
                byte[] numArray172 = new byte[] { 26, 103, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray172);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray172);

            }
            else if (selected.Equals("DRAG QUEER 2"))
            {
                byte[] numArray173 = new byte[] { 26, 104, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray173);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray173);

            }
            else if (selected.Equals("CULT GUY"))
            {
                byte[] numArray174 = new byte[] { 26, 105, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray174);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray174);

            }
            else if (selected.Equals("MR T"))
            {
                byte[] numArray175 = new byte[] { 26, 112, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray175);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray175);

            }
            else if (selected.Equals("FRO WITH TATS"))
            {
                byte[] numArray176 = new byte[] { 26, 117, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray176);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray176);

            }
            else if (selected.Equals("BACKPACKER"))
            {
                byte[] numArray177 = new byte[] { 26, 133, 255, 246 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray177);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray177);

            }
            else if (selected.Equals("EMO 3"))
            {
                byte[] numArray178 = new byte[] { 25, 193, 87, 208 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray178);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray178);

            }
            else if (selected.Equals("RAMBO"))
            {
                byte[] numArray179 = new byte[] { 25, 202, 87, 208 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray179);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray179);

            }
            else if (selected.Equals("PATIENT 1 DEAD"))
            {
                byte[] numArray180 = new byte[] { 25, 195, 87, 208 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray180);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray180);

            }
            else if (selected.Equals("PATIENT 2"))
            {
                byte[] numArray181 = new byte[] { 25, 196, 87, 208 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray181);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray181);

            }
            else if (selected.Equals("BOUNCER 2"))
            {
                byte[] numArray182 = new byte[] { 25, 196, 87, 208 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray182);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray182);

            }
            else if (selected.Equals("MAN IN BOXERS"))
            {
                byte[] numArray183 = new byte[] { 25, 197, 87, 208 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray183);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray183);

            }
            else if (selected.Equals("LOST AND DAMMED BIKER 2"))
            {
                byte[] numArray184 = new byte[] { 25, 166, 87, 208 };
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne, numArray184);
                Form1.PS3.SetMemory(ModelChanger.ModelOffsetOne + 12, numArray184);

            }
        }
    }
}