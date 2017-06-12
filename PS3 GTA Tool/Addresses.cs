using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3_GTA_Tool
{

    class Addresses
    {
        public class Hashes
        {
            public class Pickups
            {
                public static int PICKUP_AMMO_BULLET_MP = 1426343849;
                public static int PICKUP_AMMO_MISSILE_MP = -107080240;
                public static int PICKUP_ARMOUR_STANDARD = 1274757841;
                public static int PICKUP_HEALTH_SNACK = 483577702;
                public static int PICKUP_HEALTH_STANDARD = -1888453608;
                public static int PICKUP_MONEY_CASE = -831529621;
                public static int PICKUP_MONEY_DEP_BAG = 545862290;
                public static int PICKUP_MONEY_MED_BAG = 341217064;
                public static int PICKUP_MONEY_PAPER_BAG = 1897726628;
                public static int PICKUP_MONEY_PURSE = 513448440;
                public static int PICKUP_MONEY_SECURITY_CASE = -562499202;
                public static int PICKUP_MONEY_WALLET = 1575005502;
                public static int PICKUP_MONEY_VARIABLE = -31919185;
                public static int PICKUP_PARACHUTE = 1735599485;
                public static int PICKUP_PORTABLE_PACKAGE = -2136239332;
                public static int PICKUP_VEHICLE_HEALTH_STANDARD = 160266735;
            }

            public class Weapons
            {
                public static uint ADVANCEDRIFLE = 0xAF113F99;
                public static uint AIRSTRIKE_ROCKET = 0x13579279;
                public static uint ANIMAL = 0xF9FBAEBE;
                public static uint APPISTOL = 0x22D8FE39;
                public static uint ASSAULTRIFLE = 0xBFEFFF6D;
                public static uint ASSAULTSHOTGUN = 0xE284C527;
                public static uint ASSAULTSMG = 0xEFE7E2DF;
                public static uint BALL = 0x23C9F95C;
                public static uint BAT = 0x958A4A8F;
                public static uint BOTTLE = 0xF9E6AA4B;
                public static uint BRIEFCASE = 0x88C78EB7;
                public static uint BRIEFCASE_02 = 0x01B79F17;
                public static uint BULLPUPRIFLE = 0x7F229F94;
                public static uint BULLPUPSHOTGUN = 0x9D61E50F;
                public static uint BZGAS = 0xA0973D5E;
                public static uint CARBINERIFLE = 0x83BF0278;
                public static uint COMBATMG = 0x7FD62962;
                public static uint COMBATPDW = 0x0A3D4D34;
                public static uint COMBATPISTOL = 0x5EF9FEC4;
                public static uint COUGAR = 0x08D4BE52;
                public static uint CROWBAR = 0x84BD7BFD;
                public static uint DAGGER = 0x92A27487;
                public static uint DIGISCANNER = 0xFDBADCED;
                public static uint FIREEXTINGUISHER = 0x060EC506;
                public static uint FIREWORK = 0x7F7497E5;
                public static uint FLARE = 0x497FACC3;
                public static uint FLAREGUN = 0x47757124;
                public static uint GOLFCLUB = 0x440E4788;
                public static uint GRENADE = 0x93E220BD;
                public static uint GRENADELAUNCHER = 0xA284510B;
                public static uint GRENADELAUNCHER_SMOKE = 0x4DD2DC56;
                public static uint GUSENBERG = 0x61012683;
                public static uint HAMMER = 0x4E875F73;
                public static uint HEAVYPISTOL = 0xD205520E;
                public static uint HEAVYSHOTGUN = 0x3AABBBAA;
                public static uint HEAVYSNIPER = 0x0C472FE2;
                public static uint HOMINGLAUNCHER = 0x63AB0442;
                public static uint KNIFE = 0x99B507EA;
                public static uint MARKSMANRIFLE = 0xC734385A;
                public static uint MG = 0x9D07F764;
                public static uint MICROSMG = 0x13532244;
                public static uint MINIGUN = 0x42BF8A85;
                public static uint MOLOTOV = 0x24B17070;
                public static uint MUSKET = 0xA89CB99E;
                public static uint NIGHTSTICK = 0x678B81B1;
                public static uint PETROLCAN = 0x34A67B97;
                public static uint PISTOL = 0x1B06D571;
                public static uint PISTOL50 = 0x99AEEB3B;
                public static uint PROXMINE = 0xAB564B93;
                public static uint PUMPSHOTGUN = 0x1D073A89;
                public static uint RPG = 0xB1CA77B1;
                public static uint SAWNOFFSHOTGUN = 0x7846A318;
                public static uint SMG = 0x2BE6766B;
                public static uint SMOKEGRENADE = 0xFDBC8A50;
                public static uint SNIPERRIFLE = 0x05FC3C11;
                public static uint SNOWBALL = 0x787F0BB;
                public static uint SNSPISTOL = 0xBFD21232;
                public static uint SPECIALCARBINE = 0xC0A3098D;
                public static uint STICKYBOMB = 0x2C3731D9;
                public static uint STINGER = 0x687652CE;
                public static uint STUNGUN = 0x3656C8C1;
                public static uint UNARMED = 0xA2719263;
                public static uint VINTAGEPISTOL = 0x083839C4;
            }
        }

        public class Natives
        {
            public static uint add_armour_to_ped;
            public static uint add_blip_for_entity;
            public static uint add_explosion;
            public static uint add_owned_explosion;
            public static uint attach_cam_to_ped_bone;
            public static uint clear_all_ped_props;
            public static uint clear_ped_blood_damage;
            public static uint clear_ped_tasks_immediately;
            public static uint clear_player_wanted_level;
            public static uint clear_timecycle_modifier;
            public static uint clear_vehicle_custom_primary_colour;
            public static uint clear_vehicle_custom_secondary_colour;
            public static uint create_ambient_pickup;
            public static uint create_cam;
            public static uint create_object;
            public static uint create_ped;
            public static uint create_ped_inside_vehicle;
            public static uint create_vehicle;
            public static uint destroy_cam;
            public static uint display_radar;
            public static uint do_screen_fade_in;
            public static uint do_screen_fade_out;
            public static uint freeze_entity_position;
            public static uint get_closest_vehicle;
            public static uint get_entity_coords;
            public static uint get_entity_max_health;
            public static uint get_gameplay_cam_rot;
            public static uint get_main_player_blip_id;
            public static uint get_player_group;
            public static uint get_player_name;
            public static uint get_player_ped;
            public static uint get_player_wanted_level;
            public static uint get_players_last_vehicle;
            public static uint get_vehicle_ped_is_in;
            public static uint give_achievement_to_player;
            public static uint give_weapon_to_ped;
            public static uint has_achievement_been_passed;
            public static uint has_model_loaded;
            public static uint has_cutscene_loaded;
            public static uint is_control_pressed;
            public static uint is_ped_in_any_vehicle;
            public static uint is_player_free_aiming;
            public static uint is_player_online;
            public static uint is_player_targetting_anything;
            public static uint is_vehicle_seat_free;
            public static uint network_earn_from_rockstar;
            public static uint network_request_control_of_entity;
            public static uint network_has_control_of_entity;
            public static uint network_is_player_talking;
            public static uint network_send_text_message;
            public static uint play_police_report;
            public static uint player_id;
            public static uint player_ped_id;
            public static uint remove_all_ped_weapons;
            public static uint remove_ipl;
            public static uint render_script_cams;
            public static uint request_cutscene;
            public static uint request_ipl;
            public static uint request_model;
            public static uint request_weapon_asset;
            public static uint set_blip_as_friendly;
            public static uint set_blip_colour;
            public static uint set_blip_scale;
            public static uint set_blip_sprite;
            public static uint set_cam_active;
            public static uint set_cam_rot;
            public static uint set_enable_handcuffs;
            public static uint set_entity_can_be_damaged;
            public static uint set_entity_collision;
            public static uint set_entity_coords;
            public static uint set_entity_health;
            public static uint set_entity_invincible;
            public static uint set_entity_proofs;
            public static uint set_entity_rotation;
            public static uint set_entity_visible;
            public static uint set_max_wanted_level;
            public static uint set_mobile_radio_enabled_during_gameplay;
            public static uint set_model_as_no_longer_needed;
            public static uint set_nightvision;
            public static uint set_override_weather;
            public static uint set_ped_as_group_leader;
            public static uint set_ped_as_group_member;
            public static uint set_ped_can_be_knocked_off_vehicle;
            public static uint set_ped_can_ragdoll;
            public static uint set_ped_can_switch_weapon;
            public static uint set_ped_combat_ability;
            public static uint set_ped_component_variation;
            public static uint set_ped_into_vehicle;
            public static uint set_ped_never_leaves_group;
            public static uint set_ped_prop_index;
            public static uint set_ped_random_component_variation;
            public static uint set_ped_random_props;
            public static uint set_ped_to_ragdoll;
            public static uint set_player_invincible;
            public static uint set_player_model;
            public static uint set_player_wanted_level;
            public static uint set_player_wanted_level_now;
            public static uint set_player_weapon_damage_modifier;
            public static uint set_seethrough;
            public static uint set_timecycle_modifier;
            public static uint set_time_scale;
            public static uint set_vehicle_can_be_visibly_damaged;
            public static uint set_vehicle_can_break;
            public static uint set_vehicle_colours;
            public static uint set_vehicle_custom_primary_colour;
            public static uint set_vehicle_custom_secondary_colour;
            public static uint set_vehicle_dirt_level;
            public static uint set_vehicle_door_open;
            public static uint set_vehicle_door_shut;
            public static uint set_vehicle_engine_health;
            public static uint set_vehicle_extra_colours;
            public static uint set_vehicle_fixed;
            public static uint set_vehicle_forward_speed;
            public static uint set_vehicle_gravity;
            public static uint set_vehicle_indicator_lights;
            public static uint set_vehicle_light_multiplier;
            public static uint set_vehicle_mod;
            public static uint set_vehicle_mod_kit;
            public static uint set_vehicle_number_plate_text;
            public static uint set_vehicle_number_plate_text_index;
            public static uint set_vehicle_petrol_tank_health;
            public static uint set_vehicle_reduce_grip;
            public static uint set_vehicle_strong;
            public static uint set_vehicle_undriveable;
            public static uint set_vehicle_wheel_type;
            public static uint set_vehicle_window_tint;
            public static uint set_weather_type_now;
            public static uint shake_gameplay_cam;
            public static uint shoot_single_bullet_between_coords;
            public static uint skip_radio_forward;
            public static uint start_cutscene;
            public static uint start_entity_fire;
            public static uint stat_get_bool;
            public static uint stat_get_int;
            public static uint stat_set_bool;
            public static uint stat_set_float;
            public static uint stat_set_int;
            public static uint stat_set_string;
            public static uint stop_cutscene_immediately;
            public static uint task_combat_ped;
            public static uint task_start_scenario_in_place;
            public static uint task_vehicle_drive_to_coord;
            public static uint task_vehicle_drive_wander;
            public static uint unk_0x1D980479;
            public static uint unk_0x5C57B85D;
            public static uint unk_0x6BB5CDA;
            public static uint unk_0x825423C2;
            public static uint unk_0xAA2A0EAF;
            public static uint unk_0xB986FF47;
            public static uint unk_0xDF38165E;
        }

        public class Other
        {
            public static uint entityXCoord = 0x10030000;
            public static uint entityYCoord = 0x10030004;
            public static uint entityZCoord = 0x10030008;

            public class VehicleStuff
            {
                public static byte[] ModsLongFoundsBytes = new byte[10000000];
                public static byte[] ModsBytes = new byte[] { 0x4E, 0x49, 0x4E, 0x45, 0x46, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4F, 0x42, 0x45, 0x59 };
                public static uint HandlingOffset = 0;
                public static uint ModsOffset = 0;
                public static uint ModsStartOffset = 0x40000000;
                public static bool fpFound = false;
                public static uint ModsOffsetUsedCamera = 0;
                public static uint[] ModsOffsetUsed = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
        }

        public class RTM
        {
            public static uint ammoOffset1;
            public static byte[] toggleAmmoOff1 = new byte[] { 0x63, 0xfd, 0x00, 0x00 };
            public static byte[] toggleAmmoOn1 = new byte[] { 0x3b, 0xa0, 0x03, 0xe7 };

            public static uint ammoOffset2;
            public static byte[] toggleAmmoOff2 = new byte[] { 0x60, 0xa4, 0x00, 0x00 };
            public static byte[] toggleAmmoOn2 = new byte[] { 0x38, 0xe0, 0x00, 0x63 };

            public static uint animalFreezeFix1;
            public static uint animalFreezeFix2;
            public static uint animalFreezeFix3;
            public static byte[] toggleAnimalFix1 = new byte[] { 0x2C, 0x05, 0x02, 0x2B };
            public static byte[] toggleAnimalFix2 = new byte[] { 0x2C, 0x04, 0x02, 0x2B };
            public static byte[] toggleAnimalFix3 = new byte[] { 0x60, 0x00, 0x00, 0x00 };

            public static uint copsOffset;
            public static byte[] toggleCopsOff = new byte[] { 0x39, 0x60, 0x00, 0x00 };
            public static byte[] toggleCopsNormal = new byte[] { 0x81, 0x7D, 0x00, 0x04 };

            public static uint garagePointer;

            public static uint godModeOffset;
            public static byte[] toggleGodOff = new byte[] { 0xFC, 0x01, 0x10, 0x00, 0x41, 0x80, 0x01, 0x14 };
            public static byte[] toggleGodOn = new byte[] { 0x38, 0x60, 0x7f, 0xff, 0xb0, 0x7f, 0x00, 0xb4 };

            public static uint waypointX;
            public static uint waypointY;
        }
    }
}
