using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PS3Lib;

namespace PS3_GTA_Tool
{
    class Hook
    {
        public static PS3API PS3 = Form1.PS3;
        public static uint SFA4 = 0x186E390;
        public static uint a = SFA4 - 0x20;

        public static int Call(uint func_address, params object[] parameters)
        {
            int length = parameters.Length;
            int index = 0;
            uint num3 = 0;
            uint num4 = 0;
            uint num5 = 0;
            uint num6 = 0;
            while (index < length)
            {
                if (parameters[index] is int)
                {
                    PS3.Extension.WriteInt32(0x10020000 + (num3 * 4), (int)parameters[index]);
                    num3++;
                }
                else if (parameters[index] is uint)
                {
                    PS3.Extension.WriteUInt32(0x10020000 + (num3 * 4), (uint)parameters[index]);
                    num3++;
                }
                else
                {
                    uint num7;
                    if (parameters[index] is string)
                    {
                        num7 = 0x10022000 + (num4 * 0x400);
                        PS3.Extension.WriteString(num7, Convert.ToString(parameters[index]));
                        PS3.Extension.WriteUInt32(0x10020000 + (num3 * 4), num7);
                        num3++;
                        num4++;
                    }
                    else if (parameters[index] is float)
                    {
                        WriteSingle(0x10020024 + (num5 * 4), (float)parameters[index]);
                        num5++;
                    }
                    else if (parameters[index] is float[])
                    {
                        float[] input = (float[])parameters[index];
                        num7 = 0x10021000 + (num6 * 4);
                        WriteSingle(num7, input);
                        PS3.Extension.WriteUInt32(0x10020000 + (num3 * 4), num7);
                        num3++;
                        num6 += (uint)input.Length;
                    }
                }
                index++;
            }
            PS3.Extension.WriteUInt32(0x1002004C, func_address);
            for (; ; )
            {
                if (PS3.Extension.ReadUInt32(0x1002004C) == 0)
                {
                    System.Threading.Thread.Sleep(8);
                    break;
                }
            }
            return PS3.Extension.ReadInt32(0x10020050);
        }


        public static bool IsEnable()
        {
            bool reslut = false;
            if (PS3.GetBytes(a, 4).SequenceEqual(new byte[] { 0x3D, 0x60, 0x10, 0x05 }))
                reslut = true;
            return reslut;
        }

        static byte[] patchjmp(uint is_player_online)
        {
            is_player_online += 0xC;
            //uint a = 0x186E370;

            uint bytes = a - is_player_online;
            byte[] f = new byte[4];
            byte[] result = new byte[4];
            f[3] = (byte)(bytes >> 24);
            f[2] = (byte)(bytes >> 16);
            f[1] = (byte)(bytes >> 8);
            f[0] = (byte)(bytes);

            result[3] = (byte)(f[0] + 1);
            result[2] = f[1];
            result[1] = f[2];
            result[0] = 0x49;

            return result;
        }


        public static void Enable(uint is_player_online)
        {
            byte[] buffer2 = new byte[] { 0xf8, 0x21, 0xfd, 0xA1, 0x7c, 8, 2, 0xa6, 0xf8, 1, 0x02, 0x70, 60, 0x60, 0x10, 2, 0x81, 0x83, 0, 0x4c, 0x2c, 12, 0, 0, 0x41, 130, 0, 100, 0x80, 0x83, 0, 4, 0x80, 0xa3, 0, 8, 0x80, 0xc3, 0, 12, 0x80, 0xe3, 0, 0x10, 0x81, 3, 0, 20, 0x81, 0x23, 0, 0x18, 0x81, 0x43, 0, 0x1c, 0x81, 0x63, 0, 0x20, 0xc0, 0x23, 0, 0x24, 0xc0, 0x43, 0, 40, 0xc0, 0x63, 0, 0x2c, 0xc0, 0x83, 0, 0x30, 0xc0, 0xa3, 0, 0x34, 0xc0, 0xc3, 0, 0x38, 0xc0, 0xe3, 0, 60, 0xc1, 3, 0, 0x40, 0xc1, 0x23, 0, 0x48, 0x80, 0x63, 0, 0, 0x7d, 0x89, 3, 0xa6, 0x4e, 0x80, 4, 0x21, 60, 0x80, 0x10, 2, 0x38, 160, 0, 0, 0x90, 0xa4, 0, 0x4c, 0x90, 100, 0, 80, 0xe8, 1, 0x02, 0x70, 0x7c, 8, 3, 0xa6, 0x38, 0x21, 0x02, 0x60, 0x38, 0x60, 0x00, 0x03, 0x4E, 0x80, 0x00, 0x20 };

            PS3.SetMemory(a, new byte[] { 0x3D, 0x60, 0x10, 0x05, 0x81, 0x6B, 0x00, 0x00, 0x7D, 0x69, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x20 });
            PS3.SetMemory(SFA4, buffer2);
            PS3.Extension.WriteUInt32(0x10050000, SFA4);
            byte[] on = patchjmp(is_player_online);
            PS3.SetMemory(is_player_online, new byte[] { 0xF8, 0x21, 0xFF, 0x91, 0x7C, 0x08, 0x02, 0xA6, 0xF8, 0x01, 0x00, 0x80, on[0], on[1], on[2], on[3] });
            PS3.SetMemory(is_player_online + 0x18, new byte[] { 0x7C, 0x08, 0x03, 0xA6, 0x38, 0x21, 0x00, 0x70, 0x4E, 0x80, 0x00, 0x20 });
        }

        private static byte[] ReverseBytes(byte[] toReverse)
        {
            Array.Reverse(toReverse);
            return toReverse;
        }

        private static void WriteSingle(uint address, float input)
        {
            byte[] array = new byte[4];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 4);
            PS3.SetMemory(address, array);
        }

        private static void WriteSingle(uint address, float[] input)
        {
            int length = input.Length;
            byte[] array = new byte[length * 4];
            for (int i = 0; i < length; i++)
            {
                ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (int)(i * 4));
            }
            PS3.SetMemory(address, array);
        }

        private static uint CBAB(uint F, uint T)
        {
            if (F > T)
            {
                return (0x4c000000 - (F - T));
            }
            if (F < T)
            {
                return ((T - F) + 0x48000000);
            }
            return 0x48000000;
        }

        public static void LoopToggle(bool toggle)
        {
            uint SFA1 = RPC.SFA1;
            uint EFA1 = RPC.EFA1 + 0x20;
            uint SFA2 = RPC.SFA2;
            uint EFA2 = RPC.EFA2 + 0x20;
            uint SFA3 = RPC.SFA3;
            uint EFA3 = RPC.EFA3 + 0x20;
            uint BFA1 = RPC.BFA1;
            uint BAB1 = RPC.BAB1;
            uint BFA2 = RPC.BFA2;
            uint BAB2 = RPC.BAB2;
            uint BFA3 = RPC.BFA3;
            uint BAB3 = RPC.BAB3;

            if (toggle)
            {
                byte[] buffer = new byte[] { 0xF8, 0x21, 0xFF, 0x41, 0x7C, 0x08, 0x02, 0xA6, 0xF8, 0x01, 0x00, 0xD0, 0x3D, 0x80, 0x10, 0x01, 0x80, 0x6C, 0x00, 0x08, 0x80, 0x8C, 0x00, 0x04, 0x81, 0x4C, 0x00, 0x00, 0x38, 0xA0, 0x00, 0x01, 0x80, 0xCC, 0x00, 0x08, 0x7D, 0x49, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x21, 60, 0x60, 0x10, 4, 0x81, 0x83, 0, 0x4c, 0x2c, 12, 0, 0, 0x41, 130, 0, 100, 0x80, 0x83, 0, 4, 0x80, 0xa3, 0, 8, 0x80, 0xc3, 0, 12, 0x80, 0xe3, 0, 0x10, 0x81, 3, 0, 20, 0x81, 0x23, 0, 0x18, 0x81, 0x43, 0, 0x1c, 0x81, 0x63, 0, 0x20, 0xc0, 0x23, 0, 0x24, 0xc0, 0x43, 0, 40, 0xc0, 0x63, 0, 0x2c, 0xc0, 0x83, 0, 0x30, 0xc0, 0xa3, 0, 0x34, 0xc0, 0xc3, 0, 0x38, 0xc0, 0xe3, 0, 60, 0xc1, 3, 0, 0x40, 0xc1, 0x23, 0, 0x48, 0x80, 0x63, 0, 0, 0x7d, 0x89, 3, 0xa6, 0x4e, 0x80, 4, 0x21, 60, 0x80, 0x10, 4, 0x38, 160, 0, 0, 0x60, 0x00, 0x00, 0x00, 0x90, 100, 0, 80, 0xE8, 0x01, 0x00, 0xD0, 0x7C, 0x08, 0x03, 0xA6, 0x38, 0x21, 0x00, 0xC0 };
                PS3.SetMemory(SFA1, buffer);
                PS3.SetMemory(SFA2, buffer);
                PS3.SetMemory(SFA3, buffer);
                PS3.Extension.WriteUInt32(EFA1, CBAB(EFA1, BAB1));
                PS3.Extension.WriteUInt32(BFA1, CBAB(BFA1, SFA1));
                PS3.Extension.WriteUInt32(EFA2, CBAB(EFA2, BAB2));
                PS3.Extension.WriteUInt32(BFA2, CBAB(BFA2, SFA2));
                PS3.Extension.WriteUInt32(EFA3, CBAB(EFA3, BAB3));
                PS3.Extension.WriteUInt32(BFA3, CBAB(BFA3, SFA3));

            }
            else
            {
                byte[] buffer = new byte[] { 0xF8, 0x21, 0xFF, 0x41, 0x7C, 0x08, 0x02, 0xA6, 0xF8, 0x01, 0x00, 0xD0, 0x3D, 0x80, 0x10, 0x01, 0x80, 0x6C, 0x00, 0x08, 0x80, 0x8C, 0x00, 0x04, 0x81, 0x4C, 0x00, 0x00, 0x38, 0xA0, 0x00, 0x01, 0x80, 0xCC, 0x00, 0x08, 0x7D, 0x49, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x21, 60, 0x60, 0x10, 4, 0x81, 0x83, 0, 0x4c, 0x2c, 12, 0, 0, 0x41, 130, 0, 100, 0x80, 0x83, 0, 4, 0x80, 0xa3, 0, 8, 0x80, 0xc3, 0, 12, 0x80, 0xe3, 0, 0x10, 0x81, 3, 0, 20, 0x81, 0x23, 0, 0x18, 0x81, 0x43, 0, 0x1c, 0x81, 0x63, 0, 0x20, 0xc0, 0x23, 0, 0x24, 0xc0, 0x43, 0, 40, 0xc0, 0x63, 0, 0x2c, 0xc0, 0x83, 0, 0x30, 0xc0, 0xa3, 0, 0x34, 0xc0, 0xc3, 0, 0x38, 0xc0, 0xe3, 0, 60, 0xc1, 3, 0, 0x40, 0xc1, 0x23, 0, 0x48, 0x80, 0x63, 0, 0, 0x7d, 0x89, 3, 0xa6, 0x4e, 0x80, 4, 0x21, 60, 0x80, 0x10, 4, 0x38, 160, 0, 0, 0x90, 0xa4, 0, 0x4c, 0x90, 100, 0, 80, 0xE8, 0x01, 0x00, 0xD0, 0x7C, 0x08, 0x03, 0xA6, 0x38, 0x21, 0x00, 0xC0 };
                PS3.SetMemory(SFA1, buffer);
                PS3.SetMemory(SFA2, buffer);
                PS3.SetMemory(SFA3, buffer);
                PS3.Extension.WriteUInt32(EFA1, CBAB(EFA1, BAB1));
                PS3.Extension.WriteUInt32(BFA1, CBAB(BFA1, SFA1));
                PS3.Extension.WriteUInt32(EFA2, CBAB(EFA2, BAB2));
                PS3.Extension.WriteUInt32(BFA2, CBAB(BFA2, SFA2));
                PS3.Extension.WriteUInt32(EFA3, CBAB(EFA3, BAB3));
                PS3.Extension.WriteUInt32(BFA3, CBAB(BFA3, SFA3));
            }
        }

        public static int Loop(uint func_address, params object[] parameters)
        {
            int length = parameters.Length;
            int index = 0;
            uint num3 = 0;
            uint num4 = 0;
            uint num5 = 0;
            uint num6 = 0;
            while (index < length)
            {
                if (parameters[index] is int)
                {
                    PS3.Extension.WriteInt32(0x10040000 + (num3 * 4), (int)parameters[index]);
                    num3++;
                }
                else if (parameters[index] is uint)
                {
                    PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), (uint)parameters[index]);
                    num3++;
                }
                else
                {
                    uint num7;
                    if (parameters[index] is string)
                    {
                        num7 = 0x10042000 + (num4 * 0x400);
                        PS3.Extension.WriteString(num7, Convert.ToString(parameters[index]));
                        PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), num7);
                        num3++;
                        num4++;
                    }
                    else if (parameters[index] is float)
                    {
                        WriteSingle(0x10040024 + (num5 * 4), (float)parameters[index]);
                        num5++;
                    }
                    else if (parameters[index] is float[])
                    {
                        float[] input = (float[])parameters[index];
                        num7 = 0x10041000 + (num6 * 4);
                        WriteSingle(num7, input);
                        PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), num7);
                        num3++;
                        num6 += (uint)input.Length;
                    }
                }
                index++;
            }
            PS3.Extension.WriteUInt32(0x1004004c, func_address);
            Thread.Sleep(10);
            return PS3.Extension.ReadInt32(0x10040050);
        }
    }
}
