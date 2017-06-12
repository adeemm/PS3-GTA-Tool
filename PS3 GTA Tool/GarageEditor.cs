using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;

namespace PS3_GTA_Tool
{
    class GarageEditor
    {
        public static PS3API PS3 = Form1.PS3;
        public static Dictionary<uint, string> carhashes = new Dictionary<uint, string>();
        public static bool updating = false;
        public const uint BODYSTYLE = 211;
        public const uint CHASSIS = 71;
        public const uint GARAGEITEMLENGTH = 400;
        public const uint INSURANCE = 287;
        public const uint CARTYPEOFFSET = 176;
        public const uint REPAIROFFSET = 285;
        public const uint PRIM_COLOR = 31;
        public const uint SEC_COLOR = 35;
        public const uint PEARL_COLOR = 39;
        public const uint TIRESMOKE_ENABLED = 131;
        public const uint TIRESMOKE_R = 163;
        public const uint TIRESMOKE_G = 167;
        public const uint TIRESMOKE_B = 171;
        public const uint HORN = 107;
        public const uint RIMS = 143;
        public const uint REARRIM = 147;
        public const uint RIMCLASS = 191;
        public const uint RIMCOLOR = 43;
        public const uint BULLETPROOF = 195;
        //const uint ROLLCAGE = 71;
        public const uint WINDOW = 175;
        public const uint LICENSE = 12;
        public const uint LICENSECOLOR = 11;
        public const uint ARMOR = 115;
        public const uint SPOLIER = 51;
        public const uint EXHAUST = 67;
        public const uint FBUMPER = 55;
        public const uint RBUMPER = 59;
        public const uint SKIRTS = 63;
        public const uint HOOD = 79;
        public const uint ROOF = 91;
        public const uint GRILLE = 75;
        public const uint XENON = 139;
        public const uint SUSPENSION = 111;
        public const uint TRANSMISSION = 103;
        public const uint TURBO = 123;
        public const uint ENGINE = 95;
        //const uint BULLETPROOF1 = 195;
        public const uint BULLETPROOF2 = 210;
        public const uint CUSTOMTIRES = 155;
        public const uint CUSTOMREARTIRE = 159;
        public const uint BRAKES = 99;
        public static uint GaragePointer = 0;
        public static uint garageoffset = 0;
        public static bool connected = false;
        public static System.Windows.Forms.ListBox garageList;

        public static void setUp()
        {
            carhashes.Add(3078201489, "adder");
            carhashes.Add(1283517198, "airbus");
            carhashes.Add(1560980623, "airtug");
            carhashes.Add(1672195559, "akuma");
            carhashes.Add(1171614426, "ambulance");
            carhashes.Add(837858166, "annihilator");
            carhashes.Add(3087536137, "armytanker");
            carhashes.Add(2818520053, "armytrailer");
            carhashes.Add(2657817814, "armytrailer2");
            carhashes.Add(2485144969, "asea");
            carhashes.Add(2487343317, "asea2");
            carhashes.Add(2391954683, "asterope");
            carhashes.Add(2154536131, "bagger");
            carhashes.Add(3895125590, "baletrailer");
            carhashes.Add(3486135912, "baller");
            carhashes.Add(142944341, "baller2");
            carhashes.Add(3253274834, "banshee");
            carhashes.Add(3471458123, "barracks");
            carhashes.Add(1074326203, "barracks2");
            carhashes.Add(4180675781, "bati");
            carhashes.Add(3403504941, "bati2");
            carhashes.Add(2053223216, "benson");
            carhashes.Add(1126868326, "bfinjection");
            carhashes.Add(850991848, "biff");
            carhashes.Add(4278019151, "bison");
            carhashes.Add(2072156101, "bison2");
            carhashes.Add(1739845664, "bison3");
            carhashes.Add(850565707, "bjxl");
            carhashes.Add(2166734073, "blazer");
            carhashes.Add(4246935337, "blazer2");
            carhashes.Add(3025077634, "blazer3");
            carhashes.Add(4143991942, "blimp");
            carhashes.Add(3950024287, "blista");
            carhashes.Add(1131912276, "bmx");
            carhashes.Add(524108981, "boattrailer");
            carhashes.Add(1069929536, "bobcatxl");
            carhashes.Add(2859047862, "bodhi2");
            carhashes.Add(2307837162, "boxville");
            carhashes.Add(4061868990, "boxville2");
            carhashes.Add(121658888, "boxville3");
            carhashes.Add(3612755468, "buccaneer");
            carhashes.Add(3990165190, "buffalo");
            carhashes.Add(736902334, "buffalo2");
            carhashes.Add(1886712733, "bulldozer");
            carhashes.Add(2598821281, "bullet");
            carhashes.Add(2948279460, "burrito");
            carhashes.Add(3387490166, "burrito2");
            carhashes.Add(2551651283, "burrito3");
            carhashes.Add(893081117, "burrito4");
            carhashes.Add(1132262048, "burrito5");
            carhashes.Add(3581397346, "bus");
            carhashes.Add(788747387, "buzzard");
            carhashes.Add(745926877, "buzzard2");
            carhashes.Add(3334677549, "cablecar");
            carhashes.Add(1147287684, "caddy");
            carhashes.Add(3757070668, "caddy2");
            carhashes.Add(1876516712, "camper");
            carhashes.Add(2072687711, "carbonizzare");
            carhashes.Add(11251904, "carbonrs");
            carhashes.Add(4244420235, "cargobob");
            carhashes.Add(1621617168, "cargobob2");
            carhashes.Add(1394036463, "cargobob3");
            carhashes.Add(368211810, "cargoplane");
            carhashes.Add(2006918058, "cavalcade");
            carhashes.Add(3505073125, "cavalcade2");
            carhashes.Add(2983812512, "cheetah");
            carhashes.Add(2222034228, "coach");
            carhashes.Add(330661258, "cogcabrio");
            carhashes.Add(3249425686, "comet2");
            carhashes.Add(108773431, "coquette");
            carhashes.Add(448402357, "cruiser");
            carhashes.Add(321739290, "crusader");
            carhashes.Add(3650256867, "cuban800");
            carhashes.Add(3288047904, "cutter");
            carhashes.Add(2006142190, "daemon");
            carhashes.Add(3164157193, "dilettante");
            carhashes.Add(1682114128, "dilettante2");
            carhashes.Add(1033245328, "dinghy");
            carhashes.Add(276773164, "dinghy2");
            carhashes.Add(1770332643, "dloader");
            carhashes.Add(2154757102, "docktrailer");
            carhashes.Add(3410276810, "docktug");
            carhashes.Add(80636076, "dominator");
            carhashes.Add(2623969160, "double");
            carhashes.Add(1177543287, "dubsta");
            carhashes.Add(3900892662, "dubsta2");
            carhashes.Add(2164484578, "dump");
            carhashes.Add(2633113103, "dune");
            carhashes.Add(534258863, "dune2");
            carhashes.Add(970356638, "duster");
            carhashes.Add(3728579874, "elegy2");
            carhashes.Add(3609690755, "emperor");
            carhashes.Add(2411965148, "emperor2");
            carhashes.Add(3053254478, "emperor3");
            carhashes.Add(3003014393, "entityxf");
            carhashes.Add(4289813342, "exemplar");
            carhashes.Add(3703357000, "f620");
            carhashes.Add(55628203, "faggio2");
            carhashes.Add(1127131465, "fbi");
            carhashes.Add(2647026068, "fbi2");
            carhashes.Add(3903372712, "felon");
            carhashes.Add(4205676014, "felon2");
            carhashes.Add(2299640309, "feltzer2");
            carhashes.Add(1938952078, "firetruk");
            carhashes.Add(3458454463, "fixter");
            carhashes.Add(1353720154, "flatbed");
            carhashes.Add(1491375716, "forklift");
            carhashes.Add(3157435195, "fq2");
            carhashes.Add(1030400667, "freight");
            carhashes.Add(184361638, "freightcar");
            carhashes.Add(920453016, "freightcont1");
            carhashes.Add(240201337, "freightcont2");
            carhashes.Add(642617954, "freightgrain");
            carhashes.Add(3517691494, "freighttrailer");
            carhashes.Add(744705981, "frogger");
            carhashes.Add(1949211328, "frogger2");
            carhashes.Add(1909141499, "fugitive");
            carhashes.Add(499169875, "fusilade");
            carhashes.Add(2016857647, "futo");
            carhashes.Add(2494797253, "gauntlet");
            carhashes.Add(2549763894, "gburrito");
            carhashes.Add(1019737494, "graintrailer");
            carhashes.Add(2519238556, "granger");
            carhashes.Add(2751205197, "gresley");
            carhashes.Add(884422927, "habanero");
            carhashes.Add(444583674, "handler");
            carhashes.Add(1518533038, "hauler");
            carhashes.Add(301427732, "hexer");
            carhashes.Add(37348240, "hotknife");
            carhashes.Add(418536135, "infernus");
            carhashes.Add(3005245074, "ingot");
            carhashes.Add(886934177, "intruder");
            carhashes.Add(3117103977, "issi2");
            carhashes.Add(3670438162, "jackal");
            carhashes.Add(1051415893, "jb700");
            carhashes.Add(1058115860, "jet");
            carhashes.Add(861409633, "jetmax");
            carhashes.Add(4174679674, "journey");
            carhashes.Add(544021352, "khamelion");
            carhashes.Add(1269098716, "landstalker");
            carhashes.Add(3013282534, "lazer");
            carhashes.Add(469291905, "lguard");
            carhashes.Add(621481054, "luxor");
            carhashes.Add(2548391185, "mammatus");
            carhashes.Add(2170765704, "manana");
            carhashes.Add(3251507587, "marquis");
            carhashes.Add(2634305738, "maverick");
            carhashes.Add(914654722, "mesa");
            carhashes.Add(3546958660, "mesa2");
            carhashes.Add(2230595153, "mesa3");
            carhashes.Add(868868440, "metrotrain");
            carhashes.Add(3984502180, "minivan");
            carhashes.Add(3510150843, "mixer");
            carhashes.Add(475220373, "mixer2");
            carhashes.Add(3861591579, "monroe");
            carhashes.Add(1783355638, "mower");
            carhashes.Add(904750859, "mule");
            carhashes.Add(3244501995, "mule2");
            carhashes.Add(3660088182, "nemesis");
            carhashes.Add(1032823388, "ninef");
            carhashes.Add(2833484545, "ninef2");
            carhashes.Add(1348744438, "oracle");
            carhashes.Add(3783366066, "oracle2");
            carhashes.Add(569305213, "packer");
            carhashes.Add(3486509883, "patriot");
            carhashes.Add(2287941233, "pbus");
            carhashes.Add(3385765638, "pcj");
            carhashes.Add(3917501776, "penumbra");
            carhashes.Add(1830407356, "peyote");
            carhashes.Add(2157618379, "phantom");
            carhashes.Add(2199527893, "phoenix");
            carhashes.Add(1507916787, "picador");
            carhashes.Add(2046537925, "police");
            carhashes.Add(2667966721, "police2");
            carhashes.Add(1912215274, "police3");
            carhashes.Add(2321795001, "police4");
            carhashes.Add(4260343491, "policeb");
            carhashes.Add(2758042359, "policeold1");
            carhashes.Add(2515846680, "policeold2");
            carhashes.Add(456714581, "policet");
            carhashes.Add(353883353, "polmav");
            carhashes.Add(4175309224, "pony");
            carhashes.Add(943752001, "pony2");
            carhashes.Add(2112052861, "pounder");
            carhashes.Add(2844316578, "prairie");
            carhashes.Add(741586030, "pranger");
            carhashes.Add(3806844075, "predator");
            carhashes.Add(2411098011, "premier");
            carhashes.Add(3144368207, "primo");
            carhashes.Add(356391690, "proptrailer");
            carhashes.Add(2643899483, "radi");
            carhashes.Add(390902130, "raketrailer");
            carhashes.Add(1645267888, "rancherxl");
            carhashes.Add(1933662059, "rancherxl2");
            carhashes.Add(2360515092, "rapidgt");
            carhashes.Add(1737773231, "rapidgt2");
            carhashes.Add(3627815886, "ratloader");
            carhashes.Add(3087195462, "rebel");
            carhashes.Add(2249373259, "rebel2");
            carhashes.Add(4280472072, "regina");
            carhashes.Add(3196165219, "rentalbus");
            carhashes.Add(782665360, "rhino");
            carhashes.Add(3089277354, "riot");
            carhashes.Add(3448987385, "ripley");
            carhashes.Add(2136773105, "rocoto");
            carhashes.Add(627094268, "romero");
            carhashes.Add(2589662668, "rubble");
            carhashes.Add(3401388520, "ruffian");
            carhashes.Add(4067225593, "ruiner");
            carhashes.Add(1162065741, "rumpo");
            carhashes.Add(2518351607, "rumpo2");
            carhashes.Add(2609945748, "sabregt");
            carhashes.Add(3695398481, "sadler");
            carhashes.Add(734217681, "sadler2");
            carhashes.Add(788045382, "sanchez");
            carhashes.Add(2841686334, "sanchez2");
            carhashes.Add(3105951696, "sandking");
            carhashes.Add(989381445, "sandking2");
            carhashes.Add(3039514899, "schafter2");
            carhashes.Add(3548084598, "schwarzer");
            carhashes.Add(4108429845, "scorcher");
            carhashes.Add(2594165727, "scrap");
            carhashes.Add(3264692260, "seashark");
            carhashes.Add(3678636260, "seashark2");
            carhashes.Add(1221512915, "seminole");
            carhashes.Add(1349725314, "sentinel");
            carhashes.Add(873639469, "sentinel2");
            carhashes.Add(1337041428, "serrano");
            carhashes.Add(3080461301, "shamal");
            carhashes.Add(2611638396, "sheriff");
            carhashes.Add(1922257928, "sheriff2");
            carhashes.Add(1044954915, "skylift");
            carhashes.Add(3484649228, "speedo");
            carhashes.Add(728614474, "speedo2");
            carhashes.Add(400514754, "squalo");
            carhashes.Add(2817386317, "stanier");
            carhashes.Add(1545842587, "stinger");
            carhashes.Add(2196019706, "stingergt");
            carhashes.Add(1747439474, "stockade");
            carhashes.Add(4080511798, "stockade3");
            carhashes.Add(1723137093, "stratum");
            carhashes.Add(2333339779, "stretch");
            carhashes.Add(2172210288, "stunt");
            carhashes.Add(771711535, "submersible");
            carhashes.Add(970598228, "sultan");
            carhashes.Add(4012021193, "suntrap");
            carhashes.Add(1123216662, "superd");
            carhashes.Add(384071873, "surano");
            carhashes.Add(699456151, "surfer");
            carhashes.Add(2983726598, "surfer2");
            carhashes.Add(2400073108, "surge");
            carhashes.Add(1951180813, "taco");
            carhashes.Add(3286105550, "tailgater");
            carhashes.Add(3564062519, "tanker");
            carhashes.Add(586013744, "tankercar");
            carhashes.Add(3338918751, "taxi");
            carhashes.Add(48339065, "tiptruck");
            carhashes.Add(3347205726, "tiptruck2");
            carhashes.Add(1981688531, "titan");
            carhashes.Add(464687292, "tornado");
            carhashes.Add(1531094468, "tornado2");
            carhashes.Add(1762279763, "tornado3");
            carhashes.Add(2261744861, "tornado4");
            carhashes.Add(1941029835, "tourbus");
            carhashes.Add(2971866336, "towtruck");
            carhashes.Add(3852654278, "towtruck2");
            carhashes.Add(2078290630, "tr2");
            carhashes.Add(1784254509, "tr3");
            carhashes.Add(2091594960, "tr4");
            carhashes.Add(1641462412, "tractor");
            carhashes.Add(2218488798, "tractor2");
            carhashes.Add(1445631933, "tractor3");
            carhashes.Add(2016027501, "trailerlogs");
            carhashes.Add(3417488910, "trailers");
            carhashes.Add(2715434129, "trailers2");
            carhashes.Add(2236089197, "trailers3");
            carhashes.Add(712162987, "trailersmall");
            carhashes.Add(1917016601, "trash");
            carhashes.Add(2942498482, "trflat");
            carhashes.Add(1127861609, "tribike");
            carhashes.Add(3061159916, "tribike2");
            carhashes.Add(3894672200, "tribike3");
            carhashes.Add(290013743, "tropic");
            carhashes.Add(2524324030, "tvtrailer");
            carhashes.Add(516990260, "utillitruck");
            carhashes.Add(887537515, "utillitruck2");
            carhashes.Add(2132890591, "utillitruck3");
            carhashes.Add(338562499, "vacca");
            carhashes.Add(4154065143, "vader");
            carhashes.Add(2621610858, "velum");
            carhashes.Add(3469130167, "vigero");
            carhashes.Add(2672523198, "voltic");
            carhashes.Add(523724515, "voodoo2");
            carhashes.Add(1777363799, "washington");
            carhashes.Add(65402552, "youga");
            carhashes.Add(3172678083, "zion");
            carhashes.Add(3101863448, "zion2");
            carhashes.Add(758895617, "ztype");
            carhashes.Add(3945366167, "bifta");
            carhashes.Add(92612664, "kalahari");
            carhashes.Add(1488164764, "paradise");
            carhashes.Add(231083307, "speeder");
            carhashes.Add(117401876, "btype");
            carhashes.Add(2997294755, "jester");
            carhashes.Add(408192225, "turismor");
            carhashes.Add(767087018, "alpha");
            carhashes.Add(1341619767, "vestra");
            carhashes.Add(4152024626, "massacro");
            carhashes.Add(2891838741, "zentorno");
            carhashes.Add(486987393, "huntley");
            carhashes.Add(1836027715, "thrust");
            carhashes.Add(841808271, "rhapsody");
            carhashes.Add(1373123368, "warrener");
            carhashes.Add(3089165662, "blade");
            carhashes.Add(75131841, "glendale");
            carhashes.Add(3863274624, "panto");
            carhashes.Add(3057713523, "dubsta3");
            carhashes.Add(1078682497, "pigalle");
            carhashes.Add(3449006043, "monster");
            carhashes.Add(743478836, "sovereign");
            carhashes.Add(1824333165, "besra");
            carhashes.Add(165154707, "miljet");
            carhashes.Add(1011753235, "coquette2");
            carhashes.Add(3955379698, "swift");
            carhashes.Add(1265391242, "hakuchou");
            carhashes.Add(2076032661, "furoregt");
            carhashes.Add(4135840458, "innovat");
            carhashes.Add(0xBE0E6126, "jester2");
            carhashes.Add(0xDA5819A3, "massacro2");
            carhashes.Add(0xDCE1D9F7, "ratloader2");
            carhashes.Add(0x2B7F9DE3, "slamvan");
            carhashes.Add(2242229361, "mule3");
            carhashes.Add(1077420264, "velum2");
            carhashes.Add(1956216962, "tanker2");
            carhashes.Add(941800958, "casco");
            carhashes.Add(444171386, "boxville4");
            carhashes.Add(970385471, "hydra");
            carhashes.Add(2434067162, "insurgent");
            carhashes.Add(2071877360, "insurgent2");
            carhashes.Add(296357396, "gburrito2");
            carhashes.Add(2198148358, "technical");
            carhashes.Add(509498602, "dinghy3");
            carhashes.Add(4212341271, "savage");
            carhashes.Add(1753414259, "enduro");
            carhashes.Add(2186977100, "guardian");
            carhashes.Add(640818791, "lectro");
            carhashes.Add(2922118804, "kuruma");
            carhashes.Add(410882957, "kuruma2");
            carhashes.Add(3039269212, "trash2");
            carhashes.Add(630371791, "barracks3");
            carhashes.Add(2694714877, "valkyrie");
            carhashes.Add(833469436, "slamvan2");
            carhashes.Add(0x4019CB4C, "swift2");
            carhashes.Add(0xB79F589E, "luxor2");
            carhashes.Add(0xA29D6D10, "feltzer3");
            carhashes.Add(0x767164D6, "osiris");
            carhashes.Add(0xE2504942, "virgo");
            carhashes.Add(0x5E4327C8, "windsor");
            carhashes.Add(0x14d69010, "chino");
            carhashes.Add(0x2ec385fe, "coquette3");
            carhashes.Add(0x6322b39a, "t20");
            carhashes.Add(0xaf599f01, "vindicator");
            carhashes.Add(0x3fd5aa2f, "toro");
            carhashes.Add(0xa7ce1bc5, "brawler");
        }

        public static string hash2car(uint car)
        {
            if (carhashes.ContainsKey(car))
                return carhashes[car];
            else
                return "unknown";
        }

        public static uint car2hash(string carname)
        {
            if (carhashes.ContainsValue(carname))
                return carhashes.First(x => x.Value == carname).Key;
            else
                return 0;
        }

        public static void setlicene(string licensetext, uint carnumber)
        {
            if (connected && updating == false)
            {
                byte[] lplate = Encoding.ASCII.GetBytes(licensetext);
                PS3.SetMemory(garageoffset + (carnumber * GARAGEITEMLENGTH) + LICENSE, lplate);
            }
        }

        public static void changecar(uint carnumber, uint garagepos)
        {
            byte[] cartyp = BitConverter.GetBytes(carnumber);
            Array.Reverse(cartyp);
            PS3.SetMemory(garageoffset + CARTYPEOFFSET + (garagepos * GARAGEITEMLENGTH), new Byte[] { 0x00, 0x00, 0x00, 0x00 });
            System.Threading.Thread.Sleep(100);
            PS3.SetMemory(garageoffset + CARTYPEOFFSET + (garagepos * GARAGEITEMLENGTH), cartyp);
            LoadGarage(garageoffset);
        }

        public static void applycarchange(uint selcar)
        {
            byte[] Cartype = PS3.GetBytes(garageoffset + CARTYPEOFFSET + (selcar * GARAGEITEMLENGTH), 4);
            PS3.SetMemory(garageoffset + CARTYPEOFFSET + (selcar * GARAGEITEMLENGTH), new Byte[] { 0x00, 0x00, 0x00, 0x00 });
            System.Threading.Thread.Sleep(50);
            PS3.SetMemory(garageoffset + CARTYPEOFFSET + (selcar * GARAGEITEMLENGTH), Cartype);

        }

        public static string PadBoth(string source, int length)
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft).PadRight(length);

        }

        public static void start(System.Windows.Forms.ListBox garageListP)
        {
            byte[] GarageLocation = PS3.GetBytes(GaragePointer, 4);
            Array.Reverse(GarageLocation);
            garageoffset = BitConverter.ToUInt32(GarageLocation, 0);
            garageList = garageListP;
            LoadGarage(garageoffset);
        }

        public static void LoadGarage(uint garage)
        {
            int tempindex = garageList.SelectedIndex;
            garageList.Items.Clear();
            uint i = 0;
            for (i = 0; i < 39; i++)
            {
                byte[] Cartype = PS3.GetBytes(garage + GarageEditor.CARTYPEOFFSET + (i * GarageEditor.GARAGEITEMLENGTH), 4);
                Array.Reverse(Cartype);
                garageList.Items.Add(GarageEditor.hash2car(BitConverter.ToUInt32(Cartype, 0)));


            }

            if (tempindex == -1)
                garageList.SelectedIndex = 0;
            else
                garageList.SelectedIndex = tempindex;
        } 
    }
}
