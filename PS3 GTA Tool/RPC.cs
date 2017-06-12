using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PS3Lib;

public static class RPC
{
    public static PS3API PS3 = PS3_GTA_Tool.Form1.PS3;
    public static uint SFA1;
    public static uint EFA1;
    public static uint SFA2;
    public static uint EFA2;
    public static uint SFA3;
    public static uint EFA3;
    public static uint BFA1;
    public static uint BAB1;
    public static uint BFA2;
    public static uint BAB2;
    public static uint BFA3;
    public static uint BAB3;
    public static uint rpcEnableOffset;
    public static uint rpcEnableValue;

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
        PS3.Extension.WriteUInt32(0x1002004c, func_address);
        Thread.Sleep(50);
        return PS3.Extension.ReadInt32(0x10020050);
    }

    public static uint CBAB(uint F, uint T)
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

    public static void Enable()
    {
        byte[] buffer = new byte[] { 0xf8, 0x21, 0xff, 0x91, 0x7c, 8, 2, 0xa6, 0xf8, 1, 0, 0x80, 60, 0x60, 0x10, 2, 0x81, 0x83, 0, 0x4c, 0x2c, 12, 0, 0, 0x41, 130, 0, 100, 0x80, 0x83, 0, 4, 0x80, 0xa3, 0, 8, 0x80, 0xc3, 0, 12, 0x80, 0xe3, 0, 0x10, 0x81, 3, 0, 20, 0x81, 0x23, 0, 0x18, 0x81, 0x43, 0, 0x1c, 0x81, 0x63, 0, 0x20, 0xc0, 0x23, 0, 0x24, 0xc0, 0x43, 0, 40, 0xc0, 0x63, 0, 0x2c, 0xc0, 0x83, 0, 0x30, 0xc0, 0xa3, 0, 0x34, 0xc0, 0xc3, 0, 0x38, 0xc0, 0xe3, 0, 60, 0xc1, 3, 0, 0x40, 0xc1, 0x23, 0, 0x48, 0x80, 0x63, 0, 0, 0x7d, 0x89, 3, 0xa6, 0x4e, 0x80, 4, 0x21, 60, 0x80, 0x10, 2, 0x38, 160, 0, 0, 0x90, 0xa4, 0, 0x4c, 0x90, 100, 0, 80, 0xe8, 1, 0, 0x80, 0x7c, 8, 3, 0xa6, 0x38, 0x21, 0, 0x70 };
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

    public static byte[] ReverseBytes(byte[] toReverse)
    {
        Array.Reverse(toReverse);
        return toReverse;
    }

    public static void WriteSingle(uint address, float input)
    {
        byte[] array = new byte[4];
        BitConverter.GetBytes(input).CopyTo(array, 0);
        Array.Reverse(array, 0, 4);
        PS3.SetMemory(address, array);
    }
    public static void WriteSingle(uint address, float[] input)
    {
        int length = input.Length;
        byte[] array = new byte[length * 4];
        for (int i = 0; i < length; i++)
        {
            ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (int)(i * 4));
        }
        PS3.SetMemory(address, array);
    }
}