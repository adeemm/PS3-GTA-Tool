using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace PS3_GTA_Tool
{
    static class Functions
    {
        public static PS3API PS3 = Form1.PS3;

        public static void add_armour_to_ped(int PedID, int armor)
        {
            RPC.Call(Addresses.Natives.add_armour_to_ped, PedID, armor);
        }

        public static void add_explosion(float[] coords, int type, float radius, float shake)
        {
            RPC.Call(Addresses.Natives.add_explosion, coords, type, radius, 1, 0, shake);
        }

        public static void add_owned_explosion(int killer, float[] coords, int type, float radius, float shake)
        {
            RPC.Call(Addresses.Natives.add_owned_explosion, killer, coords, type, radius, 1, 0, shake);
        }

        public static void clear_all_ped_props(int PedID)
        {
            RPC.Call(Addresses.Natives.clear_all_ped_props, PedID);
        }

        public static void clear_ped_blood_damage(int PedID)
        {
            RPC.Call(Addresses.Natives.clear_ped_blood_damage, PedID);
        }

        public static void clear_ped_tasks_immediately(int PedID)
        {
            RPC.Call(Addresses.Natives.clear_ped_tasks_immediately, PedID);
        }

        public static void clear_timecycle_modifier()
        {
            RPC.Call(Addresses.Natives.clear_timecycle_modifier);
        }

        public static void create_ambient_pickup(int pickupHash, float[] coords, int Amount)
        {
            RPC.Call(Addresses.Natives.create_ambient_pickup, pickupHash, coords, 0, Amount, 1, 0, 1);
        }

        public static void create_object(uint hash, float[] coords)
        {
            RPC.Call(Addresses.Natives.create_object, hash, coords, 1, 1, 0);
        }

        public static void do_screen_fade_in(int time)
        {
            RPC.Call(Addresses.Natives.do_screen_fade_in, time);
        }

        public static void do_screen_fade_out(int time)
        {
            RPC.Call(Addresses.Natives.do_screen_fade_out, time);
        }

        public static void freeze_entity_position(int entity, int status)
        {
            RPC.Call(Addresses.Natives.freeze_entity_position, entity, status);
        }

        public static int get_closest_vehicle(float[] coords, float radius)
        {
            return RPC.Call(Addresses.Natives.get_closest_vehicle, coords, radius, 0, 0);
        }

        public static float[] get_entity_coords(int Entity)
        {
            float[] numArray = new float[3];
            RPC.Call(Addresses.Natives.get_entity_coords, Addresses.Other.entityXCoord, Entity);
            numArray[0] = PS3.Extension.ReadFloat(Addresses.Other.entityXCoord);
            numArray[1] = PS3.Extension.ReadFloat(Addresses.Other.entityYCoord);
            numArray[2] = PS3.Extension.ReadFloat(Addresses.Other.entityZCoord);
            return numArray;
        }

        public static int get_entity_max_health(int entity)
        {
            return RPC.Call(Addresses.Natives.get_entity_max_health, entity);
        }

        public static int get_player_name(int PedID)
        {
            return RPC.Call(Addresses.Natives.get_player_name, PedID);
        }

        public static int get_player_ped(int PedID)
        {
            return RPC.Call(Addresses.Natives.get_player_ped, PedID);
        }

        public static int get_player_wanted_level(int PlayerID)
        {
            return RPC.Call(Addresses.Natives.get_player_wanted_level, PlayerID);
        }

        public static int get_players_last_vehicle(int Player)
        {
            return RPC.Call(Addresses.Natives.get_players_last_vehicle, Player);
        }

        public static int get_vehicle_ped_is_in(int PedID)
        {
            return RPC.Call(Addresses.Natives.get_vehicle_ped_is_in, PedID, new object[0]);
        }

        public static uint GetHash(string input) // Custom Hash Generator
        { // CLL 0xfa638911
            byte[] stingbytes = Encoding.UTF8.GetBytes(input.ToLower());
            uint num1 = 0U;
            for (int i = 0; i < stingbytes.Length; i++)
            {
                uint num2 = num1 + (uint)stingbytes[i];
                uint num3 = num2 + (num2 << 10);
                num1 = num3 ^ num3 >> 6;
            }
            uint num4 = num1 + (num1 << 3);
            uint num5 = num4 ^ num4 >> 11;
            return num5 + (num5 << 15);
        }

        public static void Give_All_Weapons_Custom(int PedID)
        {
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.KNIFE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.NIGHTSTICK, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.HAMMER, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.BAT, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.GOLFCLUB, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.CROWBAR, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.PISTOL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.COMBATPISTOL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.APPISTOL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.PISTOL50, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.MICROSMG, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.SMG, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.ASSAULTSMG, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.ASSAULTRIFLE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.CARBINERIFLE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.ADVANCEDRIFLE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.MG, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.COMBATMG, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.PUMPSHOTGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.SAWNOFFSHOTGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.ASSAULTSHOTGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.BULLPUPSHOTGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.STUNGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.SNIPERRIFLE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.HEAVYSNIPER, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.GRENADELAUNCHER, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.RPG, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.MINIGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.GRENADE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.STICKYBOMB, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.SMOKEGRENADE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.BZGAS, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.MOLOTOV, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.FIREEXTINGUISHER, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.PETROLCAN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.BALL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.FLARE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.BOTTLE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.GUSENBERG, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.SPECIALCARBINE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.HEAVYPISTOL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.SNSPISTOL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.BULLPUPRIFLE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.DAGGER, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.VINTAGEPISTOL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.FIREWORK, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.MUSKET, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.HEAVYSHOTGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.MARKSMANRIFLE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.HOMINGLAUNCHER, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.PROXMINE, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.SNOWBALL, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.FLAREGUN, 9999, 0, 1);
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Addresses.Hashes.Weapons.COMBATPDW, 9999, 0, 1);
        }

        public static void give_achievement_to_player(int achievement)
        {
            RPC.Call(Addresses.Natives.give_achievement_to_player, achievement);
        }

        public static void give_weapon_to_ped(int PedID, uint Weapon)
        {
            RPC.Call(Addresses.Natives.give_weapon_to_ped, PedID, Weapon, 9999, 0, 1);
        }

        public static int has_achievement_been_passed(int achievement)
        {
            return RPC.Call(Addresses.Natives.has_achievement_been_passed, achievement);
        }

        public static int is_control_pressed(uint button)
        {
            return RPC.Call(Addresses.Natives.is_control_pressed, 0, button);
        }

        public static int is_ped_in_any_vehicle(int PedID)
        {
            return RPC.Call(Addresses.Natives.is_ped_in_any_vehicle, PedID, 0);
        }

        public static int is_player_free_aiming(int PlayerID)
        {
            return RPC.Call(Addresses.Natives.is_player_free_aiming, PlayerID);
        }

        public static int is_player_targetting_anything(int PlayerID)
        {
            return RPC.Call(Addresses.Natives.is_player_targetting_anything, PlayerID);
        }

        public static int is_vehicle_seat_free(int VehID, int seat)
        {
            return RPC.Call(Addresses.Natives.is_vehicle_seat_free, VehID, seat);
        }

        public static void network_earn_from_rockstar(int amount)
        {
            RPC.Call(Addresses.Natives.network_earn_from_rockstar, amount);
        }

        public static void network_request_control_of_entity(int Entity)
        {
            RPC.Call(Addresses.Natives.network_request_control_of_entity, Entity);
        }

        public static int player_id()
        {
            return RPC.Call(Addresses.Natives.player_id);
        }

        public static int player_ped_id()
        {
            return RPC.Call(Addresses.Natives.player_ped_id, new object[0]);
        }

        public static void remove_all_ped_weapons(int PedID)
        {
            RPC.Call(Addresses.Natives.remove_all_ped_weapons, PedID, 1);
        }

        public static void remove_ipl(string ipl)
        {
            RPC.Call(Addresses.Natives.remove_ipl, ipl);
        }

        public static void request_ipl(string objectname)
        {
            RPC.Call(Addresses.Natives.request_ipl, objectname);
        }

        public static void set_enable_handcuffs(int PedID, int status)
        {
            RPC.Call(Addresses.Natives.set_enable_handcuffs, PedID, status);
        }

        public static void set_entity_can_be_damaged(int Entity, int status)
        {
            RPC.Call(Addresses.Natives.set_entity_can_be_damaged, Entity, 0);
        }

        public static void set_entity_collision(int Entity, int status)
        {
            RPC.Call(Addresses.Natives.set_entity_collision, Entity, status, 0);
        }

        public static void set_entity_coords(int Entity, float[] coords)
        {
            RPC.Call(Addresses.Natives.set_entity_coords, Entity, coords, 1, 0, 0, 1);
        }

        public static void set_entity_health(int entity, int health)
        {
            RPC.Call(Addresses.Natives.set_entity_health, entity, health);
        }

        public static void set_entity_proofs(int entity, int unk_1, int unk_2, int unk_3, int unk_4, int unk_5, int unk_6, int unk_7, int unk_8)
        {
            RPC.Call(Addresses.Natives.set_entity_proofs, entity, unk_1, unk_2, unk_3, unk_4, unk_5, unk_6, unk_7, unk_8);
        }

        public static void set_entity_visible(int Entity, bool Visible)
        {
            if (Visible)
            {
                RPC.Call(Addresses.Natives.set_entity_visible, Entity, 1);
            }
            else
            {
                RPC.Call(Addresses.Natives.set_entity_visible, Entity, 0);
            }
        }

        public static void set_nightvision(bool Vision)
        {
            if (Vision)
            {
                RPC.Call(Addresses.Natives.set_nightvision, 1);
            }
            else
            {
                RPC.Call(Addresses.Natives.set_nightvision, 0);
            }
        }

        public static void set_override_weather(string type)
        {
            RPC.Call(Addresses.Natives.set_override_weather, type);
        }

        public static void set_ped_can_be_knocked_off_vehicle(int Ped, int unk)
        {
            RPC.Call(Addresses.Natives.set_ped_can_be_knocked_off_vehicle, Ped, unk);
        }

        public static void set_ped_can_ragdoll(int PedID, int status)
        {
            RPC.Call(Addresses.Natives.set_ped_can_ragdoll, PedID, status);
        }

        public static void set_ped_into_vehicle(int PedID, int VehID, int seat)
        {
            RPC.Call(Addresses.Natives.set_ped_into_vehicle, PedID, VehID, seat);
        }

        public static void set_ped_prop_index(int PedID, int cat, int index, int color)
        {
            RPC.Call(Addresses.Natives.set_ped_prop_index, PedID, cat, index, color, 0);
        }

        public static void set_ped_random_component_variation(int PedID)
        {
            RPC.Call(Addresses.Natives.set_ped_random_component_variation, PedID, 0);
        }

        public static void set_ped_random_props(int PedID)
        {
            RPC.Call(Addresses.Natives.set_ped_random_props, PedID);
        }

        public static void set_ped_to_ragdoll(int PedID, int xForce, int yForce, int zForce)
        {
            RPC.Call(Addresses.Natives.set_ped_to_ragdoll, PedID, xForce, yForce, zForce, 1, 1, 0);
        }

        public static void set_player_wanted_level(int PlayerID, int stars)
        {
            RPC.Call(Addresses.Natives.set_player_wanted_level, PlayerID, stars, 0);
        }

        public static void set_player_wanted_level_now(int PlayerID, int stars)
        {
            RPC.Call(Addresses.Natives.set_player_wanted_level_now, PlayerID, stars);
        }

        public static void set_player_weapon_damage_modifier(int Player, float amount)
        {
            RPC.Call(Addresses.Natives.set_player_weapon_damage_modifier, Player, amount);
        }

        public static void set_seethrough(bool enable)
        {
            if (enable)
            {
                RPC.Call(Addresses.Natives.set_seethrough, 1);
            }
            else
            {
                RPC.Call(Addresses.Natives.set_seethrough, 0);
            }
        }

        public static void set_timecycle_modifier(string timecycle)
        {
            RPC.Call(Addresses.Natives.set_timecycle_modifier, timecycle);
        }

        public static void set_time_scale(float scale)
        {
            RPC.Call(Addresses.Natives.set_time_scale, scale);
        }

        public static void set_vehicle_can_be_visibly_damaged(int VehID, int status)
        {
            RPC.Call(Addresses.Natives.set_vehicle_can_be_visibly_damaged, VehID, status);
        }

        public static void set_vehicle_can_break(int VehID, int status)
        {
            RPC.Call(Addresses.Natives.set_vehicle_can_break, VehID, status);
        }

        public static void set_vehicle_colours(int VehID, int Color1, int Color2)
        {
            RPC.Call(Addresses.Natives.set_vehicle_colours, VehID, Color1, Color2);
        }

        public static void set_vehicle_custom_primary_colour(int VehID, int red, int green, int blue)
        {
            RPC.Call(Addresses.Natives.set_vehicle_custom_primary_colour, VehID, red, green, blue);
        }

        public static void set_vehicle_custom_secondary_colour(int VehID, int red, int green, int blue)
        {
            RPC.Call(Addresses.Natives.set_vehicle_custom_secondary_colour, VehID, red, green, blue);
        }

        public static void set_vehicle_dirt_level(int VehID, int dirt)
        {
            RPC.Call(Addresses.Natives.set_vehicle_dirt_level, VehID, dirt);
        }

        public static void set_vehicle_door_open(int VehID, int door)
        {
            RPC.Call(Addresses.Natives.set_vehicle_door_open, VehID, door, 1, 1);
        }

        public static void set_vehicle_door_shut(int VehID, int door)
        {
            RPC.Call(Addresses.Natives.set_vehicle_door_shut, VehID, door, 1, 1);
        }

        public static void set_vehicle_engine_health(int VehID, float health)
        {
            RPC.Call(Addresses.Natives.set_vehicle_engine_health, VehID, health);
        }

        public static void set_vehicle_extra_colours(int VehID, int pearl, int wheel)
        {
            RPC.Call(Addresses.Natives.set_vehicle_extra_colours, VehID, pearl, wheel);
        }

        public static void set_vehicle_fixed(int VehID)
        {
            RPC.Call(Addresses.Natives.set_vehicle_fixed, VehID);
        }

        public static void set_vehicle_forward_speed(int VehID, float speed)
        {
            RPC.Call(Addresses.Natives.set_vehicle_forward_speed, VehID, speed);
        }

        public static void set_vehicle_gravity(int VehID, int gravity)
        {
            RPC.Call(Addresses.Natives.set_vehicle_gravity, VehID, gravity);
        }

        public static void set_vehicle_indicator_lights(int VehID, int left, int right)
        {
            RPC.Call(Addresses.Natives.set_vehicle_indicator_lights, VehID, left, right);
        }

        public static void set_vehicle_light_multiplier(int VehID, float multiplier)
        {
            RPC.Call(Addresses.Natives.set_vehicle_light_multiplier, VehID, multiplier);
        }

        public static void set_vehicle_mod(int VehID, int type, int style)
        {
            RPC.Call(Addresses.Natives.set_vehicle_mod, VehID, type, style);
        }

        public static void set_vehicle_mod_kit(int VehID, int kit)
        {
            RPC.Call(Addresses.Natives.set_vehicle_mod_kit, VehID, kit);
        }

        public static void set_vehicle_number_plate_index(int VehID, int type)
        {
            RPC.Call(Addresses.Natives.set_vehicle_number_plate_text_index, VehID, type);
        }

        public static void set_vehicle_petrol_tank_health(int VehID, float health)
        {
            RPC.Call(Addresses.Natives.set_vehicle_petrol_tank_health, VehID, health);
        }

        public static void set_vehicle_number_plate_text(int VehID, string text)
        {
            RPC.Call(Addresses.Natives.set_vehicle_number_plate_text, VehID, text);
        }

        public static void set_vehicle_reduce_grip(int VehID, int status)
        {
            RPC.Call(Addresses.Natives.set_vehicle_reduce_grip, VehID, status);
        }

        public static void set_vehicle_strong(int VehID, int status)
        {
            RPC.Call(Addresses.Natives.set_vehicle_strong, VehID, status);
        }

        public static void set_vehicle_undriveable(int VehID, int status)
        {
            RPC.Call(Addresses.Natives.set_vehicle_undriveable, VehID, status);
        }

        public static void set_vehicle_wheel_type(int VehID, int type)
        {
            RPC.Call(Addresses.Natives.set_vehicle_wheel_type, VehID, type);
        }

        public static void set_vehicle_window_tint(int VehID, int value)
        {
            RPC.Call(Addresses.Natives.set_vehicle_window_tint, VehID, value);
        }

        public static void set_weather_type_now(string type)
        {
            RPC.Call(Addresses.Natives.set_weather_type_now, type);
        }

        public static void shake_gameplay_cam(string type, float amount)
        {
            RPC.Call(Addresses.Natives.shake_gameplay_cam, type, amount);
        }

        public static void skip_radio_forward()
        {
            RPC.Call(Addresses.Natives.skip_radio_forward);
        }

        public static void start_entity_fire(int entity)
        {
            RPC.Call(Addresses.Natives.start_entity_fire, entity);
        }

        public static int stat_get_bool(uint statHash)
        {
            return RPC.Call(Addresses.Natives.stat_get_bool, statHash, 1, 1);
        }

        public static int stat_get_int(uint statHash)
        {
            return RPC.Call(Addresses.Natives.stat_get_int, 1, -1);
        }

        public static void stat_set_bool(uint statHash, int value)
        {
            RPC.Call(Addresses.Natives.stat_set_bool, statHash, value, 1);
        }

        public static void stat_set_float(uint statHash, float value)
        {
            RPC.Call(Addresses.Natives.stat_set_float, statHash, value, 1);
        }

        public static void stat_set_int(uint statHash, int value)
        {
            RPC.Call(Addresses.Natives.stat_set_int, statHash, value, 1);
        }

        public static void stat_set_string(uint statHash, string value)
        {
            RPC.Call(Addresses.Natives.stat_set_string, statHash, value, 1);
        }

        public static void ReadXML(string dir, NSComboBox list)
        {
            XmlTextReader reader = new XmlTextReader(Directory.GetCurrentDirectory() + dir);

            while(reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Place":
                            string attribute = reader["Name"];
                            if (attribute != null) list.Items.Add(attribute);
                            break;
                    }
                }
            }
        }

        public static void TeleportToXMLPlace(string dir, string location, NSComboBox list)
        {
            Functions.do_screen_fade_out(400);

            XmlTextReader reader = new XmlTextReader(Directory.GetCurrentDirectory() + dir);

            while (reader.Read())
            {
                if(reader.IsStartElement())
                {
                    if(reader.Name == "Place")
                    {
                        string attribute = reader["Name"];
                        if (attribute != null && attribute == location)
                        {
                            XmlDocument xmlDocument = new XmlDocument();
                            xmlDocument.Load(Directory.GetCurrentDirectory() + dir);
                            XmlNode node = xmlDocument.ReadNode(reader);

                            string[] scoords = node.InnerText.Split('\n');
                            float[] fcoods = new float[3]; 
                            int num = 0;
                            foreach (string item in scoords)
                            {
                                if (item.Any(char.IsDigit))
                                {
                                    fcoods[num] = float.Parse(item);
                                    num++;
                                }
                            }

                            int PedID = Functions.player_ped_id();
                            int VehicleStatus = Functions.is_ped_in_any_vehicle(PedID);
                            System.Threading.Thread.Sleep(1000);

                            if (VehicleStatus == 1) Functions.set_entity_coords(Functions.get_vehicle_ped_is_in(PedID), fcoods);
                            else Functions.set_entity_coords(PedID, fcoods);

                            System.Threading.Thread.Sleep(5250);
                            Functions.do_screen_fade_in(400);
                        }
                    }
                }
            }
        }

        public static void ReadCustomMap(string dir, NSComboBox list)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + dir);

            foreach (string file in files)
            {
                string name = Between(file, @"MapMods\", ".map");
                list.Items.Add(name);
            }
        }

        public static void LoadCustomMap(string dir, string map, NSComboBox list)
        {
    //        Functions.do_screen_fade_out(400);

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + dir);
            string foundFile = "";
            bool found = false;

            foreach (string file in files)
            {
                string name = Between(file, @"MapMods\", ".map");
                if (name == map)
                {
                    foundFile = file;
                    found = true;
                    break;
                }
            }

            string[] lines = File.ReadAllLines(foundFile);

            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                int hash = Convert.ToInt32(split[0]);
                float[] coords = { float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]) -1f};
                float[] rot = { float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6]) };

                int mapO = Hook.Call(Addresses.Natives.create_object, hash, coords, 1, 1, 0);
                Hook.Call(Addresses.Natives.set_entity_rotation, mapO, rot, 2, 1);
                Hook.Call(Addresses.Natives.freeze_entity_position, mapO, 1);
            }

            string first = lines[0];
            string[] firsts = first.Split(' ');
            float[] fcoords = { float.Parse(firsts[1]), float.Parse(firsts[2]), float.Parse(firsts[3]) };

            MessageBox.Show("Custom Map Loaded! Press OK To Teleport!\n(All Credits Go To Original Map Creators)", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            int PedID = Functions.player_ped_id();
            int VehicleStatus = Functions.is_ped_in_any_vehicle(PedID);

            if (VehicleStatus == 1) Functions.set_entity_coords(Functions.get_vehicle_ped_is_in(PedID), fcoords);
            else Functions.set_entity_coords(PedID, fcoords);

  //          Functions.do_screen_fade_in(400);
        }

        public static string Between(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        public static void LoadNativesWeb(string site)
        {
            string raw = new System.Net.WebClient().DownloadString(site);

            string[] lines = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                var names = typeof(Addresses.Natives).GetFields().Select(field => field.Name).ToArray();

                string combine = line.Replace(" = ", String.Empty);

                foreach (string name in names)
                {

                    if (combine.ToLower().Contains(name.ToLower()))
                    {
                        string a = combine.ToLower().Substring(name.Length);

                        if (a.Contains("_"))
                        {

                        }

                        else
                        {
                            string address = combine.Substring(name.Length);

                            var type = typeof(Addresses.Natives);
                            var field = type.GetField(name);
                            field.SetValue(null, System.Convert.ToUInt32(address, 16));
                        }
                    }

                    if (combine.Contains("[") || combine.Contains("]"))
                    {
                        string ver1 = combine.Replace("[", null);
                        string ver2 = ver1.Replace("]", null);
                        Updater.nativesSaved = ver2;
                    }
                }
            }
        }

        public static void LoadNativesFile(string dir)
        {
            try
            {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + dir);

                string raw = sr.ReadToEnd();

                sr.Close();

                string[] lines = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    var names = typeof(Addresses.Natives).GetFields().Select(field => field.Name).ToArray();

                    string combine = line.Replace(" = ", String.Empty);

                    foreach (string name in names)
                    {

                        if (combine.ToLower().Contains(name.ToLower()))
                        {
                            string a = combine.ToLower().Substring(name.Length);

                            if (a.Contains("_"))
                            {

                            }

                            else
                            {
                                string address = combine.Substring(name.Length);

                                var type = typeof(Addresses.Natives);
                                var field = type.GetField(name);

                                if (field.Name == name)
                                {
                                    field.SetValue(null, System.Convert.ToUInt32(address, 16));
                                }
                            }
                        }

                        if (combine.Contains("[") || combine.Contains("]"))
                        {
                            string ver1 = combine.Replace("[", null);
                            string ver2 = ver1.Replace("]", null);
                            Updater.nativesSaved = ver2;
                        }
                    }
                }
            }
            catch (Exception) { }
            
        }

        public static void LoadRTMWeb(string site)
        {
            string raw = new System.Net.WebClient().DownloadString(site);

            string[] lines = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                var names = typeof(Addresses.RTM).GetFields().Select(field => field.Name).ToArray();

                string combine = line.Replace(" = ", String.Empty);

                foreach (string name in names)
                {

                    if (combine.ToLower().Contains(name.ToLower()))
                    {
                        string address = combine.Substring(name.Length);
                        var type = typeof(Addresses.RTM);
                        var field = type.GetField(name);
                        field.SetValue(null, System.Convert.ToUInt32(address, 16));
                    }

                    if (combine.Contains("[") || combine.Contains("]"))
                    {
                        string ver1 = combine.Replace("[", null);
                        string ver2 = ver1.Replace("]", null);
                        Updater.rtmSaved = ver2;
                    }
                }
            }
        }

        public static void LoadRTMFile(string dir)
        {

            try
            {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + dir);

                string raw = sr.ReadToEnd();

                sr.Close();

                string[] lines = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    var names = typeof(Addresses.RTM).GetFields().Select(field => field.Name).ToArray();

                    string combine = line.Replace(" = ", String.Empty);

                    foreach (string name in names)
                    {

                        if (combine.ToLower().Contains(name.ToLower()))
                        {
                            string address = combine.Substring(name.Length);
                            var type = typeof(Addresses.RTM);
                            var field = type.GetField(name);
                            field.SetValue(null, System.Convert.ToUInt32(address, 16));
                        }

                        if (combine.Contains("[") || combine.Contains("]"))
                        {
                            string ver1 = combine.Replace("[", null);
                            string ver2 = ver1.Replace("]", null);
                            Updater.rtmSaved = ver2;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public static void LoadRPCWeb(string site)
        {
            string raw = new System.Net.WebClient().DownloadString(site);

            string[] lines = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                var names = typeof(RPC).GetFields().Select(field => field.Name).ToArray();

                string combine = line.Replace(" = ", String.Empty);

                foreach (string name in names)
                {

                    if (combine.ToLower().Contains(name.ToLower()))
                    {
                        string address = combine.Substring(name.Length);
                        var type = typeof(RPC);
                        var field = type.GetField(name);
                        field.SetValue(null, System.Convert.ToUInt32(address, 16));
                    }

                    if (combine.Contains("[") || combine.Contains("]"))
                    {
                        string ver1 = combine.Replace("[", null);
                        string ver2 = ver1.Replace("]", null);
                        Updater.rpcSaved = ver2;
                    }
                }
            }
        }

        public static void LoadRPCFile(string dir)
        {
            try
            {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + dir);

                string raw = sr.ReadToEnd();

                sr.Close();

                string[] lines = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    var names = typeof(RPC).GetFields().Select(field => field.Name).ToArray();

                    string combine = line.Replace(" = ", String.Empty);

                    foreach (string name in names)
                    {

                        if (combine.ToLower().Contains(name.ToLower()))
                        {
                            string address = combine.Substring(name.Length);
                            var type = typeof(RPC);
                            var field = type.GetField(name);
                            field.SetValue(null, System.Convert.ToUInt32(address, 16));
                        }

                        if (combine.Contains("[") || combine.Contains("]"))
                        {
                            string ver1 = combine.Replace("[", null);
                            string ver2 = ver1.Replace("]", null);
                            Updater.rpcSaved = ver2;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public static ulong RPToRank(int Level)
        {
            ulong[] level1_99 = { 0, 0, 800, 2100, 3800, 6100, 9500, 12500, 16000, 19800, 24000,
            28500, 33400, 38700, 44200, 50200, 56400, 63000, 69900, 77100, 84700,
            92500, 100700, 109200, 118000, 127100, 136500, 146200, 156200, 166500, 177100,
            188000, 199200, 210700, 222400, 234500, 246800, 259400, 272300, 285500, 299000,
            312700, 326800, 341000, 355600, 370500, 385600, 401000, 416600, 432600, 448800,
            465200, 482000, 499000, 516300, 533800, 551600, 569600, 588000, 606500, 625400,
            644500, 663800, 683400, 703300, 723400, 743800, 764500, 785400, 806500, 827900,
            849600, 871500, 893600, 916000, 938700, 961600, 984700, 1008100, 1031800, 1055700,
            1079800, 1104200, 1128800, 1153700, 1178800, 1204200, 1229800, 1255600, 1281700, 1308100,
            1334600, 1361400, 1388500, 1415800, 1443300, 1471100, 1499100, 1527300, 1555800 };

            int count = 0;
            List<ulong> lTable = new List<ulong>();
            for (ulong i = 1, c = 100; i < c; i++)
            {
                lTable.Add(level1_99[i]);
                count++;
                if (count == 10)
                {
                    count = 0;
                }
            }
            ulong acc = 0;
            for (ulong i = 1, c = 8000 - 98; i < c; i++)
            {
                acc += i;
                lTable.Add((1555800 + 28500 * i + acc * 50));
                count++;
                if (count == 10)
                {
                    count = 0;
                }
            }
            return lTable[Level - 1];
        }
    }
}
