using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;

namespace PS3_GTA_Tool
{
    class Weapons
    {
        public static PS3API PS3 = Form1.PS3;
        public static bool found = false;
        public static uint WeaponsOffset = 0;
        public static byte[] WeaponsLongFoundsBytes = new byte[20000000];
        public static byte[] WeaponsSSSBytes = new byte[] { 0x08, 0xD4, 0xBE, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x76, 0xD0, 0x47, 0x10 };
        public static byte[] WeaponsBytes = new byte[] { 0x99, 0xB5, 0x07, 0xEA, 0x89, 0xD6, 0x50, 0xBF };
        public static uint WeaponsStartOffset = 0x33000000;
        public static uint WeaponsOffsetUsed = 0;
        public static byte[] SecondWeaponsLongFoundsBytes = new byte[1000000];
        public static byte[] KnifeBytes = new byte[] { 0x99, 0xB5, 0x07, 0xEA };
        public static uint KnifeOffset = 0;
        public static byte[] NightBytes = new byte[] { 0x67, 0x8B, 0x81, 0xB1 };
        public static uint NightOffset = 0;
        public static byte[] HammerBytes = new byte[] { 0x4E, 0x87, 0x5F, 0x73 };
        public static uint HammerOffset = 0;
        public static byte[] BatBytes = new byte[] { 0x95, 0x8A, 0x4A, 0x8F };
        public static uint BatOffset = 0;
        public static byte[] GolfBytes = new byte[] { 0x44, 0x0E, 0x47, 0x88 };
        public static uint GolfOffset = 0;
        public static byte[] CrowbarBytes = new byte[] { 0x84, 0xBD, 0x7B, 0xFD };
        public static uint CrowbarOffset = 0;
        public static byte[] PistolBytes = new byte[] { 0x1B, 0x06, 0xD5, 0x71 };
        public static uint PistolOffset = 0;
        public static byte[] CpistolBytes = new byte[] { 0x5E, 0xF9, 0xFE, 0xC4 };
        public static uint CpistolOffset = 0;
        public static byte[] AppistolBytes = new byte[] { 0x22, 0xD8, 0xFE, 0x39 };
        public static uint AppistolOffset = 0;
        public static byte[] Pistol50Bytes = new byte[] { 0x99, 0xAE, 0xEB, 0x3B };
        public static uint Pistol50Offset = 0;
        public static byte[] MsmgBytes = new byte[] { 0x13, 0x53, 0x22, 0x44 };
        public static uint MsmgOffset = 0;
        public static byte[] SmgBytes = new byte[] { 0x2B, 0xE6, 0x76, 0x6B };
        public static uint SmgOffset = 0;
        public static byte[] AsmgBytes = new byte[] { 0xEF, 0xE7, 0xE2, 0xDF };
        public static uint AsmgOffset = 0;
        public static byte[] ArifleBytes = new byte[] { 0xBF, 0xEF, 0xFF, 0x6D };
        public static uint ArifleOffset = 0;
        public static byte[] CrifleBytes = new byte[] { 0x83, 0xBF, 0x02, 0x78 };
        public static uint CrifleOffset = 0;
        public static byte[] AdrifleBytes = new byte[] { 0xAF, 0x11, 0x3F, 0x99 };
        public static uint AdrifleOffset = 0;
        public static byte[] MgBytes = new byte[] { 0x9D, 0x07, 0xF7, 0x64 };
        public static uint MgOffset = 0;
        public static byte[] CmgBytes = new byte[] { 0x7F, 0xD6, 0x29, 0x62 };
        public static uint CmgOffset = 0;
        public static byte[] PshotBytes = new byte[] { 0x1D, 0x07, 0x3A, 0x89 };
        public static uint PshotOffset = 0;
        public static byte[] SshotBytes = new byte[] { 0x78, 0x46, 0xA3, 0x18 };
        public static uint SshotOffset = 0;
        public static byte[] AshotBytes = new byte[] { 0xE2, 0x84, 0xC5, 0x27 };
        public static uint AshotOffset = 0;
        public static byte[] BshotBytes = new byte[] { 0x9D, 0x61, 0xE5, 0x0F };
        public static uint BshotOffset = 0;
        public static byte[] StunBytes = new byte[] { 0x36, 0x56, 0xC8, 0xC1 };
        public static uint StunOffset = 0;
        public static byte[] SniperBytes = new byte[] { 0x05, 0xFC, 0x3C, 0x11 };
        public static uint SniperOffset = 0;
        public static byte[] HsniperBytes = new byte[] { 0x0C, 0x47, 0x2F, 0xE2 };
        public static uint HsniperOffset = 0;
        public static byte[] RsniperBytes = new byte[] { 0x33, 0x05, 0x8E, 0x22 };
        public static uint RsniperOffset = 0;
        public static byte[] GlauncherBytes = new byte[] { 0xA2, 0x84, 0x51, 0x0B };
        public static uint GlauncherOffset = 0;
        public static byte[] SglauncherBytes = new byte[] { 0x4D, 0xD2, 0xDC, 0x56 };
        public static uint SglauncherOffset = 0;
        public static byte[] RpgBytes = new byte[] { 0xB1, 0xCA, 0x77, 0xB1 };
        public static uint RpgOffset = 0;
        public static byte[] ProcketBytes = new byte[] { 0x16, 0x62, 0x18, 0xFF };
        public static uint ProcketOffset = 0;
        public static byte[] ArocketBytes = new byte[] { 0x13, 0x57, 0x92, 0x79 };
        public static uint ArocketOffset = 0;
        public static byte[] StingerBytes = new byte[] { 0x68, 0x76, 0x52, 0xCE };
        public static uint StingerOffset = 0;
        public static byte[] MinigunBytes = new byte[] { 0x42, 0xBF, 0x8A, 0x85 };
        public static uint MinigunOffset = 0;
        public static byte[] GrenadeBytes = new byte[] { 0x93, 0xE2, 0x20, 0xBD };
        public static uint GrenadeOffset = 0;
        public static byte[] StickyBytes = new byte[] { 0x2C, 0x37, 0x31, 0xD9 };
        public static uint StickyOffset = 0;
        public static byte[] SgrenadeBytes = new byte[] { 0xFD, 0xBC, 0x8A, 0x50 };
        public static uint SgrenadeOffset = 0;
        public static byte[] BzgasBytes = new byte[] { 0xA0, 0x97, 0x3D, 0x5E };
        public static uint BzgasOffset = 0;
        public static byte[] MolotovBytes = new byte[] { 0x24, 0xB1, 0x70, 0x70 };
        public static uint MolotovOffset = 0;
        public static byte[] FireextBytes = new byte[] { 0x06, 0x0E, 0xC5, 0x06 };
        public static uint FireextOffset = 0;
        public static byte[] PetrolBytes = new byte[] { 0x34, 0xA6, 0x7B, 0x97 };
        public static uint PetrolOffset = 0;
        public static byte[] DigiBytes = new byte[] { 0xFD, 0xBA, 0xDC, 0xED };
        public static uint DigiOffset = 0;
        public static byte[] BriefBytes = new byte[] { 0x88, 0xC7, 0x8E, 0xB7 };
        public static uint BriefOffset = 0;
        public static byte[] Brief2Bytes = new byte[] { 0x01, 0xB7, 0x9F, 0x17 };
        public static uint Brief2Offset = 0;
        public static byte[] BallBytes = new byte[] { 0x23, 0xC9, 0xF9, 0x5C };
        public static uint BallOffset = 0;
        public static byte[] FlareBytes = new byte[] { 0x49, 0x7F, 0xAC, 0xC3 };
        public static uint FlareOffset = 0;
        public static byte[] TankBytes = new byte[] { 0x73, 0xF7, 0xC0, 0x4B };
        public static uint TankOffset = 0;
        public static byte[] SpaceBytes = new byte[] { 0xF8, 0xA3, 0x93, 0x9F };
        public static uint SpaceOffset = 0;
        public static byte[] PlaneBytes = new byte[] { 0xCF, 0x08, 0x96, 0xE0 };
        public static uint PlaneOffset = 0;
        public static byte[] PlaserBytes = new byte[] { 0xEF, 0xFD, 0x01, 0x4B };
        public static uint PlaserOffset = 0;
        public static byte[] PbulletBytes = new byte[] { 0x4B, 0x13, 0x9B, 0x2D };
        public static uint PbulletOffset = 0;
        public static byte[] PbuzzardBytes = new byte[] { 0x46, 0xB8, 0x9C, 0x8E };
        public static uint PbuzzardOffset = 0;
        public static byte[] PhunterBytes = new byte[] { 0x9F, 0x1A, 0x91, 0xDE };
        public static uint PhunterOffset = 0;
        public static byte[] PlazerBytes = new byte[] { 0xE2, 0x82, 0x2A, 0x29 };
        public static uint PlazerOffset = 0;
        public static byte[] ElaserBytes = new byte[] { 0x5D, 0x66, 0x60, 0xAB };
        public static uint ElaserOffset = 0;

        public static bool find()
        {
            PS3.GetMemory(WeaponsStartOffset, WeaponsLongFoundsBytes);
            uint OffsetPlace = ContainsSequence(WeaponsLongFoundsBytes, WeaponsBytes, WeaponsStartOffset);
            if (OffsetPlace == 0u)
            {
                WeaponsOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace;
                WeaponsOffset = realoffset - 0x10;
            }
            PS3.GetMemory(WeaponsOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace1 = ContainsSequence(SecondWeaponsLongFoundsBytes, KnifeBytes, WeaponsOffset);
            if (OffsetPlace1 == 0u)
            {
                KnifeOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace1;
                KnifeOffset = realoffset;

            }
            PS3.GetMemory(KnifeOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace2 = ContainsSequence(SecondWeaponsLongFoundsBytes, NightBytes, KnifeOffset);
            if (OffsetPlace2 == 0u)
            {
                NightOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace2;
                NightOffset = realoffset;
            }
            PS3.GetMemory(NightOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace3 = ContainsSequence(SecondWeaponsLongFoundsBytes, HammerBytes, NightOffset);
            if (OffsetPlace3 == 0u)
            {
                HammerOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace3;
                HammerOffset = realoffset;
            }
            PS3.GetMemory(HammerOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace4 = ContainsSequence(SecondWeaponsLongFoundsBytes, BatBytes, HammerOffset);
            if (OffsetPlace4 == 0u)
            {
                BatOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace4;
                BatOffset = realoffset;
            }
            PS3.GetMemory(BatOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace5 = ContainsSequence(SecondWeaponsLongFoundsBytes, GolfBytes, BatOffset);
            if (OffsetPlace5 == 0u)
            {
                GolfOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace5;
                GolfOffset = realoffset;
            }
            PS3.GetMemory(GolfOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace6 = ContainsSequence(SecondWeaponsLongFoundsBytes, CrowbarBytes, GolfOffset);
            if (OffsetPlace6 == 0u)
            {
                CrowbarOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace6;
                CrowbarOffset = realoffset;
            }
            PS3.GetMemory(CrowbarOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace7 = ContainsSequence(SecondWeaponsLongFoundsBytes, PistolBytes, CrowbarOffset);
            if (OffsetPlace7 == 0u)
            {
                PistolOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace7;
                PistolOffset = realoffset;

            }
            PS3.GetMemory(PistolOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace8 = ContainsSequence(SecondWeaponsLongFoundsBytes, CpistolBytes, PistolOffset);
            if (OffsetPlace8 == 0u)
            {
                CpistolOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace8;
                CpistolOffset = realoffset;
            }
            PS3.GetMemory(CpistolOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace9 = ContainsSequence(SecondWeaponsLongFoundsBytes, AppistolBytes, CpistolOffset);
            if (OffsetPlace9 == 0u)
            {
                AppistolOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace9;
                AppistolOffset = realoffset;
            }
            PS3.GetMemory(AppistolOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace10 = ContainsSequence(SecondWeaponsLongFoundsBytes, Pistol50Bytes, AppistolOffset);
            if (OffsetPlace10 == 0u)
            {
                Pistol50Offset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace10;
                Pistol50Offset = realoffset;
            }
            PS3.GetMemory(Pistol50Offset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace11 = ContainsSequence(SecondWeaponsLongFoundsBytes, MsmgBytes, Pistol50Offset);
            if (OffsetPlace11 == 0u)
            {
                MsmgOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace11;
                MsmgOffset = realoffset;
            }
            PS3.GetMemory(MsmgOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace12 = ContainsSequence(SecondWeaponsLongFoundsBytes, SmgBytes, MsmgOffset);
            if (OffsetPlace12 == 0u)
            {
                SmgOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace12;
                SmgOffset = realoffset;
            }
            PS3.GetMemory(SmgOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace13 = ContainsSequence(SecondWeaponsLongFoundsBytes, AsmgBytes, SmgOffset);
            if (OffsetPlace13 == 0u)
            {
                AsmgOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace13;
                AsmgOffset = realoffset;
            }
            PS3.GetMemory(AsmgOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace14 = ContainsSequence(SecondWeaponsLongFoundsBytes, ArifleBytes, AsmgOffset);
            if (OffsetPlace14 == 0u)
            {
                ArifleOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace14;
                ArifleOffset = realoffset;
            }
            PS3.GetMemory(ArifleOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace15 = ContainsSequence(SecondWeaponsLongFoundsBytes, CrifleBytes, ArifleOffset);
            if (OffsetPlace15 == 0u)
            {
                CrifleOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace15;
                CrifleOffset = realoffset;
            }
            PS3.GetMemory(CrifleOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace16 = ContainsSequence(SecondWeaponsLongFoundsBytes, AdrifleBytes, CrifleOffset);
            if (OffsetPlace16 == 0u)
            {
                AdrifleOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace16;
                AdrifleOffset = realoffset;
            }
            PS3.GetMemory(AdrifleOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace17 = ContainsSequence(SecondWeaponsLongFoundsBytes, MgBytes, AdrifleOffset);
            if (OffsetPlace17 == 0u)
            {
                MgOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace17;
                MgOffset = realoffset;
            }
            PS3.GetMemory(MgOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace18 = ContainsSequence(SecondWeaponsLongFoundsBytes, CmgBytes, MgOffset);
            if (OffsetPlace18 == 0u)
            {
                CmgOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace18;
                CmgOffset = realoffset;
            }
            PS3.GetMemory(CmgOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace19 = ContainsSequence(SecondWeaponsLongFoundsBytes, PshotBytes, CmgOffset);
            if (OffsetPlace19 == 0u)
            {
                PshotOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace19;
                PshotOffset = realoffset;
            }
            PS3.GetMemory(PshotOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace20 = ContainsSequence(SecondWeaponsLongFoundsBytes, SshotBytes, PshotOffset);
            if (OffsetPlace20 == 0u)
            {
                SshotOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace20;
                SshotOffset = realoffset;
            }
            PS3.GetMemory(SshotOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace21 = ContainsSequence(SecondWeaponsLongFoundsBytes, AshotBytes, SshotOffset);
            if (OffsetPlace21 == 0u)
            {
                AshotOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace21;
                AshotOffset = realoffset;
            }
            PS3.GetMemory(AshotOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace22 = ContainsSequence(SecondWeaponsLongFoundsBytes, BshotBytes, AshotOffset);
            if (OffsetPlace22 == 0u)
            {
                BshotOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace22;
                BshotOffset = realoffset;
            }
            PS3.GetMemory(BshotOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace23 = ContainsSequence(SecondWeaponsLongFoundsBytes, StunBytes, BshotOffset);
            if (OffsetPlace23 == 0u)
            {
                StunOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace23;
                StunOffset = realoffset;
            }
            PS3.GetMemory(StunOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace24 = ContainsSequence(SecondWeaponsLongFoundsBytes, SniperBytes, StunOffset);
            if (OffsetPlace24 == 0u)
            {
                SniperOffset = 0x0;
                return false;

            }
            else
            {
                uint realoffset = OffsetPlace24;
                SniperOffset = realoffset;
            }
            PS3.GetMemory(SniperOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace25 = ContainsSequence(SecondWeaponsLongFoundsBytes, HsniperBytes, SniperOffset);
            if (OffsetPlace25 == 0u)
            {
                HsniperOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace25;
                HsniperOffset = realoffset;
            }
            PS3.GetMemory(HsniperOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace26 = ContainsSequence(SecondWeaponsLongFoundsBytes, RsniperBytes, HsniperOffset);
            if (OffsetPlace26 == 0u)
            {
                RsniperOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace26;
                RsniperOffset = realoffset;
            }
            PS3.GetMemory(RsniperOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace27 = ContainsSequence(SecondWeaponsLongFoundsBytes, GlauncherBytes, RsniperOffset);
            if (OffsetPlace27 == 0u)
            {
                GlauncherOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace27;
                GlauncherOffset = realoffset;
            }
            PS3.GetMemory(GlauncherOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace28 = ContainsSequence(SecondWeaponsLongFoundsBytes, SglauncherBytes, GlauncherOffset);
            if (OffsetPlace28 == 0u)
            {
                SglauncherOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace28;
                SglauncherOffset = realoffset;
            }
            PS3.GetMemory(SglauncherOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace29 = ContainsSequence(SecondWeaponsLongFoundsBytes, RpgBytes, SglauncherOffset);
            if (OffsetPlace29 == 0u)
            {
                RpgOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace29;
                RpgOffset = realoffset;
            }
            PS3.GetMemory(RpgOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace30 = ContainsSequence(SecondWeaponsLongFoundsBytes, ProcketBytes, RpgOffset);
            if (OffsetPlace30 == 0u)
            {
                ProcketOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace30;
                ProcketOffset = realoffset;
            }
            PS3.GetMemory(ProcketOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace31 = ContainsSequence(SecondWeaponsLongFoundsBytes, ArocketBytes, ProcketOffset);
            if (OffsetPlace31 == 0u)
            {
                ArocketOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace31;
                ArocketOffset = realoffset;
            }
            PS3.GetMemory(ArocketOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace32 = ContainsSequence(SecondWeaponsLongFoundsBytes, StingerBytes, ArocketOffset);
            if (OffsetPlace32 == 0u)
            {
                StingerOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace32;
                StingerOffset = realoffset;
            }
            PS3.GetMemory(StingerOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace33 = ContainsSequence(SecondWeaponsLongFoundsBytes, MinigunBytes, StingerOffset);
            if (OffsetPlace33 == 0u)
            {
                MinigunOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace33;
                MinigunOffset = realoffset;
            }
            PS3.GetMemory(MinigunOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace34 = ContainsSequence(SecondWeaponsLongFoundsBytes, GrenadeBytes, MinigunOffset);
            if (OffsetPlace34 == 0u)
            {
                GrenadeOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace34;
                GrenadeOffset = realoffset;
            }
            PS3.GetMemory(GrenadeOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace35 = ContainsSequence(SecondWeaponsLongFoundsBytes, StickyBytes, GrenadeOffset);
            if (OffsetPlace35 == 0u)
            {
                StickyOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace35;
                StickyOffset = realoffset;
            }
            PS3.GetMemory(StickyOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace36 = ContainsSequence(SecondWeaponsLongFoundsBytes, SgrenadeBytes, StickyOffset);
            if (OffsetPlace36 == 0u)
            {
                SgrenadeOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace36;
                SgrenadeOffset = realoffset;
            }
            PS3.GetMemory(SgrenadeOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace37 = ContainsSequence(SecondWeaponsLongFoundsBytes, BzgasBytes, SgrenadeOffset);
            if (OffsetPlace37 == 0u)
            {
                BzgasOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace37;
                BzgasOffset = realoffset;
            }
            PS3.GetMemory(BzgasOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace38 = ContainsSequence(SecondWeaponsLongFoundsBytes, MolotovBytes, BzgasOffset);
            if (OffsetPlace38 == 0u)
            {
                MolotovOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace38;
                MolotovOffset = realoffset;
            }
            PS3.GetMemory(MolotovOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace39 = ContainsSequence(SecondWeaponsLongFoundsBytes, FireextBytes, MolotovOffset);
            if (OffsetPlace39 == 0u)
            {
                FireextOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace39;
                FireextOffset = realoffset;
            }
            PS3.GetMemory(FireextOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace40 = ContainsSequence(SecondWeaponsLongFoundsBytes, PetrolBytes, FireextOffset);
            if (OffsetPlace40 == 0u)
            {
                PetrolOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace40;
                PetrolOffset = realoffset;
            }
            PS3.GetMemory(PetrolOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace41 = ContainsSequence(SecondWeaponsLongFoundsBytes, DigiBytes, PetrolOffset);
            if (OffsetPlace41 == 0u)
            {
                DigiOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace41;
                DigiOffset = realoffset;
            }
            PS3.GetMemory(DigiOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace42 = ContainsSequence(SecondWeaponsLongFoundsBytes, BriefBytes, DigiOffset);
            if (OffsetPlace42 == 0u)
            {
                BriefOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace42;
                BriefOffset = realoffset;
            }
            PS3.GetMemory(BriefOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace43 = ContainsSequence(SecondWeaponsLongFoundsBytes, Brief2Bytes, BriefOffset);
            if (OffsetPlace43 == 0u)
            {
                Brief2Offset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace43;
                Brief2Offset = realoffset;
            }
            PS3.GetMemory(Brief2Offset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace44 = ContainsSequence(SecondWeaponsLongFoundsBytes, BallBytes, Brief2Offset);
            if (OffsetPlace44 == 0u)
            {
                BallOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace44;
                BallOffset = realoffset;
            }
            PS3.GetMemory(BallOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace45 = ContainsSequence(SecondWeaponsLongFoundsBytes, FlareBytes, BallOffset);
            if (OffsetPlace45 == 0u)
            {
                FlareOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace45;
                FlareOffset = realoffset;

            }
            PS3.GetMemory(FlareOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace46 = ContainsSequence(SecondWeaponsLongFoundsBytes, TankBytes, FlareOffset);
            if (OffsetPlace46 == 0u)
            {
                TankOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace46;
                TankOffset = realoffset;
            }
            PS3.GetMemory(TankOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace47 = ContainsSequence(SecondWeaponsLongFoundsBytes, SpaceBytes, TankOffset);
            if (OffsetPlace47 == 0u)
            {
                SpaceOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace47;
                SpaceOffset = realoffset;
            }
            PS3.GetMemory(SpaceOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace48 = ContainsSequence(SecondWeaponsLongFoundsBytes, PlaneBytes, SpaceOffset);
            if (OffsetPlace48 == 0u)
            {
                PlaneOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace48;
                PlaneOffset = realoffset;
            }
            PS3.GetMemory(PlaneOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace49 = ContainsSequence(SecondWeaponsLongFoundsBytes, PlaserBytes, PlaneOffset);
            if (OffsetPlace49 == 0u)
            {
                PlaserOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace49;
                PlaserOffset = realoffset;
            }
            PS3.GetMemory(PlaserOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace50 = ContainsSequence(SecondWeaponsLongFoundsBytes, PbulletBytes, PlaserOffset);
            if (OffsetPlace50 == 0u)
            {
                PbulletOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace50;
                PbulletOffset = realoffset;
            }
            PS3.GetMemory(PbulletOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace51 = ContainsSequence(SecondWeaponsLongFoundsBytes, PbuzzardBytes, PbulletOffset);
            if (OffsetPlace51 == 0u)
            {
                PbuzzardOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace51;
                PbuzzardOffset = realoffset;
            }
            PS3.GetMemory(PbuzzardOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace52 = ContainsSequence(SecondWeaponsLongFoundsBytes, PhunterBytes, PbuzzardOffset);
            if (OffsetPlace52 == 0u)
            {
                PhunterOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace52;
                PhunterOffset = realoffset;
            }
            PS3.GetMemory(PhunterOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace53 = ContainsSequence(SecondWeaponsLongFoundsBytes, PlazerBytes, PhunterOffset);
            if (OffsetPlace53 == 0u)
            {
                PlazerOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace53;
                PlazerOffset = realoffset;
            }
            PS3.GetMemory(PlazerOffset, SecondWeaponsLongFoundsBytes);
            uint OffsetPlace54 = ContainsSequence(SecondWeaponsLongFoundsBytes, ElaserBytes, PlazerOffset);
            if (OffsetPlace54 == 0u)
            {
                ElaserOffset = 0x0;
                return false;
            }
            else
            {
                uint realoffset = OffsetPlace54;
                ElaserOffset = realoffset;
                string OffsetString = Convert.ToString(realoffset, 16);
                found = true;
                return true;
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
