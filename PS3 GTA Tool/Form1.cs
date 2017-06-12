using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using PS3Lib;

namespace PS3_GTA_Tool
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Functions.ReadXML(@"\Resources\Teleports.xml", rpcTeleportSelectedLocationBox);
            Functions.ReadCustomMap(@"\Resources\MapMods", customMapModList);
            GarageEditor.setUp();
            var list = GarageEditor.carhashes.Values.ToList();
            list.Sort();
            foreach (var key in list)
                garageCarSelect.Items.Add(key);
        }

        public static PS3API PS3 = new PS3API();

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void ccapiSelect_CheckedChanged(object sender)
        {
            PS3.ChangeAPI(SelectAPI.ControlConsole);
        }

        private void tmapiSelect_CheckedChanged(object sender)
        {
            PS3.ChangeAPI(SelectAPI.TargetManager);
        }

        private void colorTimer_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int a = rand.Next(0, 255);
            int r = rand.Next(0, 255);
            int g = rand.Next(0, 255);
            int b = rand.Next(0, 255);
            nameLabel.ForeColor = Color.FromArgb(a, r, g, b);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (blesSelect.Checked == false && blusSelect.Checked == false && bljmSelect.Checked == false)
            {
                MessageBox.Show("Select A Game Region First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connectLabel.Text != "Connected")
                {
                    if (PS3.ConnectTarget())
                    {
                        MessageBox.Show("Connected", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PS3.CCAPI.Notify(CCAPI.NotifyIcon.FRIEND, "Connected Sucessfully!");
                        connectLabel.Text = "Connected";
                        connectLabel.ForeColor = System.Drawing.Color.Green;
                    }
                }

                else
                {
                    MessageBox.Show("Already Connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            PS3.DisconnectTarget();
            MessageBox.Show("PS3 Disconnected", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connectLabel.ForeColor = System.Drawing.Color.Red;
            connectLabel.Text = "Not Connected";
            attachLabel.ForeColor = System.Drawing.Color.Red;
            attachLabel.Text = "Not Attached";
            rpcLabel.ForeColor = System.Drawing.Color.Red;
            rpcLabel.Text = "RPC Disabled";
            colorTimer.Stop();
            welcomeLabel.Value1 = "";
            nameLabel.Text = "";
        }

        private void attachButton_Click(object sender, EventArgs e)
        {
            if (attachLabel.Text != "Attached")
            {
                if (PS3.AttachProcess())
                {
                    MessageBox.Show("Attached", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.FRIEND, "Attached Sucessfully!");

                    attachLabel.Text = "Attached";
                    attachLabel.ForeColor = System.Drawing.Color.Green;

                    GarageEditor.connected = true;
                    GarageEditor.start(garageList);

                    if (PS3.Extension.ReadUInt32(RPC.rpcEnableOffset) == RPC.rpcEnableValue || Hook.IsEnable())
                    {
                        rpcLabel.Text = "RPC Enabled";
                        rpcLabel.ForeColor = System.Drawing.Color.Green;

                        Thread.Sleep(50);
                        int id = Functions.player_id();
                        string name = get_player_name(id).ToString();
                        colorTimer.Start();
                        welcomeLabel.Value1 = "Welcome:";
                        nameLabel.Text = name;
                    }
                }

                else
                {
                    MessageBox.Show("Failed to Attach", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Already Attached", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void shutdownButton_Click(object sender, EventArgs e)
        {
            if (connectLabel.Text == "Connected")
            {
                if (ccapiSelect.Checked)
                {
                    PS3.CCAPI.ShutDown(CCAPI.RebootFlags.ShutDown);
                    MessageBox.Show("PS3 Shutdown", "Shutdown", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    PS3.TMAPI.PowerOff(false);
                    MessageBox.Show("PS3 Shutdown", "Shutdown", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Connect First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void softBootButton_Click(object sender, EventArgs e)
        {
            if (connectLabel.Text == "Connected")
            {
                if (ccapiSelect.Checked)
                {
                    PS3.CCAPI.ShutDown(CCAPI.RebootFlags.SoftReboot);
                    MessageBox.Show("PS3 Rebooted", "Soft Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    PS3.TMAPI.PowerOff(false);
                    PS3.TMAPI.PowerOn(0);
                    MessageBox.Show("PS3 Rebooted", "Soft Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Connect First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void hardBootButton_Click(object sender, EventArgs e)
        {
            if (connectLabel.Text == "Connected")
            {
                if (ccapiSelect.Checked)
                {
                    PS3.CCAPI.ShutDown(CCAPI.RebootFlags.HardReboot);
                    MessageBox.Show("PS3 Rebooted", "Hard Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    PS3.TMAPI.PowerOff(true);
                    PS3.TMAPI.PowerOn(0);
                }
            }
            else
            {
                MessageBox.Show("Connect First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rtmToggleGod_CheckedChanged_1(object sender)
        {
            if (rtmToggleGod.Checked == true)
            {
                PS3.SetMemory(Addresses.RTM.godModeOffset, Addresses.RTM.toggleGodOn);
            }
            else
            {
                PS3.SetMemory(Addresses.RTM.godModeOffset, Addresses.RTM.toggleGodOff);
            }
        }

        private void rtmToggleAmmo_CheckedChanged_1(object sender)
        {
            if (rtmToggleAmmo.Checked == true)
            {
                PS3.SetMemory(Addresses.RTM.ammoOffset1, Addresses.RTM.toggleAmmoOn1);

                PS3.SetMemory(Addresses.RTM.ammoOffset2, Addresses.RTM.toggleAmmoOn2);
            }
            else
            {
                PS3.SetMemory(Addresses.RTM.ammoOffset1, Addresses.RTM.toggleAmmoOff1);

                PS3.SetMemory(Addresses.RTM.ammoOffset2, Addresses.RTM.toggleAmmoOff2);
            }
        }

        //CUSTOM NOTIFICATION
        private void customNotifText_TextChanged(object sender, EventArgs e)
        {

        }

        private void sendNotifButton_Click(object sender, EventArgs e)
        {
            if (connectLabel.Text == "Connected")
            {
                if (notifIconSelect.Text == "ARROW")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.ARROW, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "ARROWRIGHT")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.ARROWRIGHT, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "CAUTION")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.CAUTION, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "DIALOG")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.DIALOG, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "DIALOGSHADOW")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.DIALOGSHADOW, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "FINGER")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.FINGER, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "FRIEND")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.FRIEND, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "GRAB")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.GRAB, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "HAND")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.HAND, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "INFO")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.INFO, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "PEN")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.PEN, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "POINTER")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.POINTER, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "PROGRESS")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.PROGRESS, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "SLIDER")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.SLIDER, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "TEXT")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.TEXT, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "WRONGWAY")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.WRONGWAY, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "TROPHY1")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY1, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "TROPHY2")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY2, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "TROPHY3")
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY3, customNotifText.Text.ToString());
                }
                else if (notifIconSelect.Text == "TROPHY4")
	            {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY4, customNotifText.Text.ToString());
	            }

            }
            else
            {
                MessageBox.Show("Connect First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void notifIconSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        // RPC Stuff
        private void rpcEnableButton_Click(object sender, EventArgs e)
        {
            if (connectLabel.Text == "Connected")
            {
                if (attachLabel.Text == "Attached")
                {
                    if (rpcLabel.Text != "RPC Enabled")
                    {
                        RPC.Enable();
                        MessageBox.Show("RPC Enabled!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rpcLabel.Text = "RPC Enabled";
                        rpcLabel.ForeColor = System.Drawing.Color.Green;

                        int id = Functions.player_id();
                        string name = get_player_name(id).ToString();
                        colorTimer.Start();
                        welcomeLabel.Value1 = "Welcome:";
                        nameLabel.Text = name;
                    }
                    else if(Hook.IsEnable())
                    {
                        MessageBox.Show("Script Hook (Better RPC) Already Enabled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (PS3.Extension.ReadUInt32(RPC.rpcEnableOffset) == RPC.rpcEnableValue)
                    {
                        MessageBox.Show("Old RPC Already Enabled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Attach First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Connect First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpcToggleVisibility_CheckedChanged_1(object sender)
        {
            if (rpcToggleVisibility.Checked)
            {
                int PedID = Functions.player_ped_id();
                Functions.set_entity_visible(PedID, true);
            }
            else
            {
                int PedID = Functions.player_ped_id();
                Functions.set_entity_visible(PedID, false);
            }
        }

        private void rpcToggleNightVision_CheckedChanged_1(object sender)
        {
            if (rpcToggleNightVision.Checked)
            {
                Functions.set_nightvision(true);
            }
            else
            {
                Functions.set_nightvision(false);
            }
        }

        private void rpcToggleHeatVision_CheckedChanged_1(object sender)
        {
            if (rpcToggleHeatVision.Checked)
            {
                Functions.set_seethrough(true);
            }
            else
            {
                Functions.set_seethrough(false);
            }
        }


        private void rpcButtonCarFix_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            Thread.Sleep(10);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                int VehID = Functions.get_vehicle_ped_is_in(PedID);
                Thread.Sleep(20);
                Functions.set_vehicle_fixed(VehID);
            }
        }

        private void rpcButtonCleanCar_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            Thread.Sleep(10);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                int VehID = Functions.get_vehicle_ped_is_in(PedID);
                Thread.Sleep(20);
                Functions.set_vehicle_dirt_level(VehID, 0);
            }
        }

        private void rpcSetVehicleUndriveable_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                if (rpcSetVehicleUndriveable.Checked)
                {
                    Functions.set_vehicle_undriveable(VehID, 1);
                }
                else
                {
                    Functions.set_vehicle_undriveable(VehID, 0);
                }
            }
        }

        private void rpcCarFire_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_engine_health(VehID, -1.0f);
                Functions.set_vehicle_petrol_tank_health(VehID, -1.0f);
                Functions.set_entity_health(VehID, -1);
                Functions.start_entity_fire(VehID);
            }
        }

        private void rpcFreezeVehicle_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                if (rpcFreezeVehicle.Checked)
                {
                    Functions.freeze_entity_position(VehID, 1);
                }
                else
                {
                    Functions.freeze_entity_position(VehID, 0);
                }
            }
        }

        private void rpcLowVehGravity_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                if (rpcLowVehGravity.Checked)
                {
                    Functions.set_vehicle_gravity(VehID, 0);
                }
                else
                {
                    Functions.set_vehicle_gravity(VehID, 1);
                }
            }
        }

        private void rpcVehDrift_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                if (rpcVehDrift.Checked)
                {
                    Functions.set_vehicle_reduce_grip(VehID, 1);
                }
                else
                {
                    Functions.set_vehicle_reduce_grip(VehID, 0);
                }
            }
        }

        private void rpcOpenDoor_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_door_open(VehID, rpcDoorSelector.SelectedIndex);
            }
        }

        private void rpcCloseDoor_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_door_shut(VehID, rpcDoorSelector.SelectedIndex);
            }
        }

        private void rpcSetVehicleSpeed_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float speed = rpcVehicleSpeed.Value;

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                int VehID = Functions.get_vehicle_ped_is_in(PedID);
                Functions.set_vehicle_forward_speed(VehID, speed);
            }
        }

        private void rpcVehicleSpeed_Scroll(object sender)
        {
            rpcCarSpeedLabel.Value1 = "Speed = " + rpcVehicleSpeed.Value.ToString();
        }

        private void rpcSetPlayerClean_Click_1(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            Functions.clear_ped_blood_damage(PedID);
        }

        private void rpcSendMoney_Click(object sender, EventArgs e)
        {
            try
            {
                int amount = int.Parse(rpcSendMoneyAmount.Text);
                Functions.network_earn_from_rockstar(amount);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("System Format Exception", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpcSpawnObject_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords = Functions.get_entity_coords(PedID);
            uint hash = Functions.GetHash(rpcObjectSpawnName.Text);
            Functions.create_object(hash, coords);
        }

        private void rpcPresetObjectSpawn_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords = Functions.get_entity_coords(PedID);
            if (rpcObjectSpawnPreset.SelectedIndex == 0)
            {
                uint hash = Functions.GetHash("p_spinning_anus_s");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 1)
            {
                uint hash = Functions.GetHash("prop_Ld_ferris_wheel");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 2)
            {
                uint hash = Functions.GetHash("p_tram_crash_s");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 3)
            {
                uint hash = Functions.GetHash("p_oil_slick_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 4)
            {
                uint hash = Functions.GetHash("p_ld_soc_ball_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 5)
            {
                uint hash = Functions.GetHash("p_parachute1_s");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 6)
            {
                uint hash = Functions.GetHash("p_cablecar_s");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 7)
            {
                uint hash = Functions.GetHash("prop_beach_fire");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 8)
            {
                uint hash = Functions.GetHash("prop_windmill_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 9)
            {
                uint hash = Functions.GetHash("prop_lev_des_barge_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 10)
            {
                uint hash = Functions.GetHash("prop_sculpt_fix");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 11)
            {
                uint hash = Functions.GetHash("p_v_43_safe_s");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 12)
            {
                uint hash = Functions.GetHash("prop_air_bigradar");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 13)
            {
                uint hash = Functions.GetHash("prop_weed_pallet");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 14)
            {
                uint hash = Functions.GetHash("prop_large_gold");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 15)
            {
                uint hash = Functions.GetHash("Prop_weed_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 16)
            {
                uint hash = Functions.GetHash("prop_roller_car_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 17)
            {
                uint hash = Functions.GetHash("prop_water_corpse_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 18)
            {
                uint hash = Functions.GetHash("prop_dummy_01");
                Functions.create_object(hash, coords);
            }
            else if (rpcObjectSpawnPreset.SelectedIndex == 19)
            {
                uint hash = Functions.GetHash("prop_atm_01");
                Functions.create_object(hash, coords);
            }
        }

        private void rpcSendRP_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                uint hash = Functions.GetHash("MP0_CHAR_XP_FM");
                if (rpcSendRPAmmount.Text.Length > 0)
                {
                    int iRank = 0;
                    if (int.TryParse(rpcSendRPAmmount.Text, out iRank))
                    {
                        if (iRank > 0 && iRank <= 8000)
                        {
                            Functions.stat_set_int(hash, Convert.ToInt32(Functions.RPToRank(iRank)));
                        }
                        else
                        {
                            MessageBox.Show("Rank must be between 0 and 8000!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (rpcStatPreSecondary.Checked)
            {
                uint hash = Functions.GetHash("MP1_CHAR_XP_FM");
                if (rpcSendRPAmmount.Text.Length > 0)
                {
                    int iRank = 0;
                    if (int.TryParse(rpcSendRPAmmount.Text, out iRank))
                    {
                        if (iRank > 0 && iRank <= 8000)
                        {
                            Functions.stat_set_int(hash, Convert.ToInt32(Functions.RPToRank(iRank)));
                        }
                        else
                        {
                            MessageBox.Show("Rank must be between 0 and 8000!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
                MessageBox.Show("Select Either Primary Or Secondary Character First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void rpcToggleRagdoll_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();

            if (rpcToggleRagdoll.Checked)
            {
                Functions.set_ped_can_ragdoll(PedID, 1);
            }
            else
            {
                Functions.set_ped_can_ragdoll(PedID, 0);
            }
        }


        //Clients
        private void rpcClientRefresh_Click(object sender, EventArgs e)
        {
            this.Clients.Items.Clear();

            this.rpcExplosionKiller.Items.Clear();

            this.hookCombatPlayerList.Items.Clear();

            for (int i = 0; i < 0x10; i++)
            {
                this.Clients.Items.Add(get_player_name(i));

                this.rpcExplosionKiller.Items.Add(get_player_name(i));

                this.hookCombatPlayerList.Items.Add(get_player_name(i));

                if (Clients.Items.Contains(""))
                {
                    this.Clients.Items.Remove("");
                    this.Clients.Items.Add("--- Unavailable ---");
                }

                if (rpcExplosionKiller.Items.Contains(""))
                {
                    this.rpcExplosionKiller.Items.Remove("");
                    this.rpcExplosionKiller.Items.Add("--- Unavailable ---");
                }

                if (hookCombatPlayerList.Items.Contains(""))
                {
                    this.hookCombatPlayerList.Items.Remove("");
                    this.hookCombatPlayerList.Items.Add("--- Unavailable ---");
                }
            }

            this.rpcExplosionKiller.Items.Add("World / Default");
        }

        public static string get_player_name(int id)
        {
            uint offset = (uint)Functions.get_player_name(id);
            byte[] nameBytes = PS3.GetBytes(offset, 16);
            byte[] bytes = Enumerable.ToArray<byte>(Enumerable.TakeWhile<byte>((IEnumerable<byte>)nameBytes, (Func<byte, int, bool>)((v, index) => Enumerable.Any<byte>(Enumerable.Skip<byte>((IEnumerable<byte>)nameBytes, index), (Func<byte, bool>)(w => (int)w != 0)))));
            return !(Encoding.ASCII.GetString(bytes) != "") ? "--- Unavailable ---" : ((int)bytes[0] < 65 || (int)bytes[0] > 122 ? "" : Encoding.ASCII.GetString(bytes));
        }

        private void rpcClientMoneyRain_Click(object sender, EventArgs e)
        {
            float[] Position = new float[3];
            float[] pos = new float[3];

            int PedID = Functions.get_player_ped(Clients.SelectedIndex);

            Position = Functions.get_entity_coords(PedID);

            for (int i = 0; i < 20; i++)
            {
                Random RandomInt = new Random();
                int randomx = RandomInt.Next(-500, 500);
                int randomy = RandomInt.Next(-500, 500);

                pos = Position;
                pos[0] += (float)randomx / 100;
                pos[1] += (float)randomy / 100;
                pos[2] += 3;
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, pos, 2000);
            }
        }

        private void rpcClientPickupDrop_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            float[] pos = Functions.get_entity_coords(PedID);

            if (rpcClientsPickupSelect.Text == "PICKUP_MONEY_CASE")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_CASE, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_MONEY_DEP_BAG")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_DEP_BAG, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_MONEY_MED_BAG")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_MED_BAG, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_MONEY_PAPER_BAG")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_MONEY_PURSE")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PURSE, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_MONEY_SECURITY_CASE")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_SECURITY_CASE, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_MONEY_WALLET")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_WALLET, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_ARMOUR_STANDARD")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_ARMOUR_STANDARD, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_HEALTH_STANDARD")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_HEALTH_STANDARD, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_HEALTH_SNACK")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_HEALTH_SNACK, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_PORTABLE_PACKAGE")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_PORTABLE_PACKAGE, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_PARACHUTE")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_PARACHUTE, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_AMMO_BULLET_MP")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_AMMO_BULLET_MP, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_AMMO_MISSILE_MP")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_AMMO_MISSILE_MP, pos, 2000);
            }
            else if (rpcClientsPickupSelect.Text == "PICKUP_VEHICLE_HEALTH_STANDARD")
            {
                Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_VEHICLE_HEALTH_STANDARD, pos, 2000);
            }
        }

        private void rpcClientPortToPlayer_Click(object sender, EventArgs e)
        {
            int MyPed = Functions.player_ped_id();
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            float[] coords = Functions.get_entity_coords(PedID);
            Functions.do_screen_fade_out(400);
            if (Functions.is_ped_in_any_vehicle(MyPed) == 1)
            {
                int VehID = Functions.get_vehicle_ped_is_in(MyPed);
                Functions.set_entity_coords(VehID, coords);
            }
            else
            {
                Functions.set_entity_coords(MyPed, coords);
            }
            Functions.do_screen_fade_in(400);
        }

        private void rpcClientPortInCar_Click(object sender, EventArgs e)
        {
            int MyPed = Functions.player_ped_id();
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            for (int i = -1; i < 7; i++)
            {
                if (Functions.is_vehicle_seat_free(VehID, i) == 1)
                {
                    Functions.set_ped_into_vehicle(MyPed, VehID, i);
                    break;
                }
            }
        }

        private void rpcClientKickCar_Click_1(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            Functions.clear_ped_tasks_immediately(PedID);
        }

        private void rpcClientSendExplosion_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            float[] coords = Functions.get_entity_coords(PedID);

            if (rpcExplosionKiller.Text == "World / Default")
            {
                Functions.add_explosion(coords, rpcClientExplosionSelector.SelectedIndex, 5f, 5f);
            }
            else
            {
                int killer = Functions.get_player_ped(rpcExplosionKiller.SelectedIndex);
                Functions.add_owned_explosion(killer, coords, rpcClientExplosionSelector.SelectedIndex, 5f, 5f);
            }
        }

        private void rpcClientAllWeapons_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            Functions.Give_All_Weapons_Custom(PedID);
        }
        private void rpcClientTakeWeapons_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            Functions.remove_all_ped_weapons(PedID);
        }

        private void rpcClientGiveWeapon_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);

            if (rpcClientSelectWeapon.Text == "KNIFE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.KNIFE);
            }
            else if (rpcClientSelectWeapon.Text == "STINGER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.STINGER);
            }
            else if (rpcClientSelectWeapon.Text == "DIGISCANNER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.DIGISCANNER);
            }
            else if (rpcClientSelectWeapon.Text == "BRIEFCASE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BRIEFCASE);
            }
            else if (rpcClientSelectWeapon.Text == "BRIEFCASE_02")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BRIEFCASE_02);
            }
            else if (rpcClientSelectWeapon.Text == "ANIMAL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.ANIMAL);
            }
            else if (rpcClientSelectWeapon.Text == "COUGAR")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.COUGAR);
            }
            else if (rpcClientSelectWeapon.Text == "UNARMED")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.UNARMED);
            }
            else if (rpcClientSelectWeapon.Text == "NIGHTSTICK")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.NIGHTSTICK);
            }
            else if (rpcClientSelectWeapon.Text == "HAMMER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.HAMMER);
            }
            else if (rpcClientSelectWeapon.Text == "BAT")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BAT);
            }
            else if (rpcClientSelectWeapon.Text == "GOLFCLUB")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.GOLFCLUB);
            }
            else if (rpcClientSelectWeapon.Text == "CROWBAR")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.CROWBAR);
            }
            else if (rpcClientSelectWeapon.Text == "PISTOL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.PISTOL);
            }
            else if (rpcClientSelectWeapon.Text == "COMBATPISTOL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.COMBATPISTOL);
            }
            else if (rpcClientSelectWeapon.Text == "APPISTOL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.APPISTOL);
            }
            else if (rpcClientSelectWeapon.Text == "PISTOL50")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.PISTOL50);
            }
            else if (rpcClientSelectWeapon.Text == "MICROSMG")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.MICROSMG);
            }
            else if (rpcClientSelectWeapon.Text == "SMG")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.SMG);
            }
            else if (rpcClientSelectWeapon.Text == "ASSAULTSMG")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.ASSAULTSMG);
            }
            else if (rpcClientSelectWeapon.Text == "ASSAULTRIFLE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.ASSAULTRIFLE);
            }
            else if (rpcClientSelectWeapon.Text == "CARBINERIFLE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.CARBINERIFLE);
            }
            else if (rpcClientSelectWeapon.Text == "ADVANCEDRIFLE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.ADVANCEDRIFLE);
            }
            else if (rpcClientSelectWeapon.Text == "MG")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.MG);
            }
            else if (rpcClientSelectWeapon.Text == "COMBATMG")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.COMBATMG);
            }
            else if (rpcClientSelectWeapon.Text == "PUMPSHOTGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.PUMPSHOTGUN);
            }
            else if (rpcClientSelectWeapon.Text == "SAWNOFFSHOTGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.SAWNOFFSHOTGUN);
            }
            else if (rpcClientSelectWeapon.Text == "ASSAULTSHOTGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.ASSAULTSHOTGUN);
            }
            else if (rpcClientSelectWeapon.Text == "BULLPUPSHOTGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BULLPUPSHOTGUN);
            }
            else if (rpcClientSelectWeapon.Text == "STUNGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.STUNGUN);
            }
            else if (rpcClientSelectWeapon.Text == "SNIPERRIFLE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.SNIPERRIFLE);
            }
            else if (rpcClientSelectWeapon.Text == "HEAVYSNIPER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.HEAVYSNIPER);
            }
            else if (rpcClientSelectWeapon.Text == "GRENADELAUNCHER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.GRENADELAUNCHER);
            }
            else if (rpcClientSelectWeapon.Text == "GRENADELAUNCHER_SMOKE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.GRENADELAUNCHER_SMOKE);
            }
            else if (rpcClientSelectWeapon.Text == "RPG")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.RPG);
            }
            else if (rpcClientSelectWeapon.Text == "MINIGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.MINIGUN);
            }
            else if (rpcClientSelectWeapon.Text == "GRENADE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.GRENADE);
            }
            else if (rpcClientSelectWeapon.Text == "STICKYBOMB")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.STICKYBOMB);
            }
            else if (rpcClientSelectWeapon.Text == "SMOKEGRENADE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.SMOKEGRENADE);
            }
            else if (rpcClientSelectWeapon.Text == "BZGAS")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BZGAS);
            }
            else if (rpcClientSelectWeapon.Text == "MOLOTOV")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.MOLOTOV);
            }
            else if (rpcClientSelectWeapon.Text == "FIREEXTINGUISHER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.FIREEXTINGUISHER);
            }
            else if (rpcClientSelectWeapon.Text == "PETROLCAN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.PETROLCAN);
            }
            else if (rpcClientSelectWeapon.Text == "BALL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BALL);
            }
            else if (rpcClientSelectWeapon.Text == "FLARE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.FLARE);
            }
            else if (rpcClientSelectWeapon.Text == "BOTTLE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BOTTLE);
            }
            else if (rpcClientSelectWeapon.Text == "GUSENBERG")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.GUSENBERG);
            }
            else if (rpcClientSelectWeapon.Text == "SPECIALCARBINE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.SPECIALCARBINE);
            }
            else if (rpcClientSelectWeapon.Text == "HEAVYPISTOL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.HEAVYPISTOL);
            }
            else if (rpcClientSelectWeapon.Text == "SNSPISTOL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.SNSPISTOL);
            }
            else if (rpcClientSelectWeapon.Text == "BULLPUPRIFLE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.BULLPUPRIFLE);
            }
            else if (rpcClientSelectWeapon.Text == "DAGGER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.DAGGER);
            }
            else if (rpcClientSelectWeapon.Text == "VINTAGEPISTOL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.VINTAGEPISTOL);
            }
            else if (rpcClientSelectWeapon.Text == "FIREWORK")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.FIREWORK);
            }
            else if (rpcClientSelectWeapon.Text == "MUSKET")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.MUSKET);
            }
            else if (rpcClientSelectWeapon.Text == "MARKSMANRIFLE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.MARKSMANRIFLE);
            }
            else if (rpcClientSelectWeapon.Text == "HEAVYSHOTGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.HEAVYSHOTGUN);
            }
            else if (rpcClientSelectWeapon.Text == "HOMINGLAUNCHER")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.HOMINGLAUNCHER);
            }
            else if (rpcClientSelectWeapon.Text == "PROXMINE")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.PROXMINE);
            }
            else if (rpcClientSelectWeapon.Text == "SNOWBALL")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.SNOWBALL);
            }
            else if (rpcClientSelectWeapon.Text == "FLAREGUN")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.FLAREGUN);
            }
            else if (rpcClientSelectWeapon.Text == "COMBATPDW")
            {
                Functions.give_weapon_to_ped(PedID, Addresses.Hashes.Weapons.COMBATPDW);
            }
        }



        //Car Mods
        private void rpcCustomCarSetEngine_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarEngine.SelectedIndex == 0)
                {
                    Functions.set_vehicle_mod(VehID, 11, -1);
                }
                else if (rpcCustomCarEngine.SelectedIndex == 1)
                {
                    Functions.set_vehicle_mod(VehID, 11, 0);
                }
                else if (rpcCustomCarEngine.SelectedIndex == 2)
                {
                    Functions.set_vehicle_mod(VehID, 11, 1);
                }
                else if (rpcCustomCarEngine.SelectedIndex == 3)
                {
                    Functions.set_vehicle_mod(VehID, 11, 2);
                }
                else if (rpcCustomCarEngine.SelectedIndex == 4)
                {
                    Functions.set_vehicle_mod(VehID, 11, 3);
                }

            }
        }

        private void rpcCustomCarSetBreak_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarBreak.SelectedIndex == 0)
                {
                    Functions.set_vehicle_mod(VehID, 12, -1);
                }
                else if (rpcCustomCarBreak.SelectedIndex == 1)
                {
                    Functions.set_vehicle_mod(VehID, 12, 0);
                }
                else if (rpcCustomCarBreak.SelectedIndex == 2)
                {
                    Functions.set_vehicle_mod(VehID, 12, 1);
                }
                else if (rpcCustomCarBreak.SelectedIndex == 3)
                {
                    Functions.set_vehicle_mod(VehID, 12, 2);
                }

            }
        }

        private void rpcCustomCarSetSuspension_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarSuspension.SelectedIndex == 0)
                {
                    Functions.set_vehicle_mod(VehID, 15, -1);
                }
                else if (rpcCustomCarSuspension.SelectedIndex == 1)
                {
                    Functions.set_vehicle_mod(VehID, 15, 0);
                }
                else if (rpcCustomCarSuspension.SelectedIndex == 2)
                {
                    Functions.set_vehicle_mod(VehID, 15, 1);
                }
                else if (rpcCustomCarSuspension.SelectedIndex == 3)
                {
                    Functions.set_vehicle_mod(VehID, 15, 2);
                }
                else if (rpcCustomCarSuspension.SelectedIndex == 4)
                {
                    Functions.set_vehicle_mod(VehID, 15, 3);
                }
            }
        }

        private void rpcCustomCarSetTransmission_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarTransmission.SelectedIndex == 0)
                {
                    Functions.set_vehicle_mod(VehID, 13, -1);
                }
                else if (rpcCustomCarTransmission.SelectedIndex == 1)
                {
                    Functions.set_vehicle_mod(VehID, 13, 0);
                }
                else if (rpcCustomCarTransmission.SelectedIndex == 2)
                {
                    Functions.set_vehicle_mod(VehID, 13, 1);
                }
                else if (rpcCustomCarTransmission.SelectedIndex == 3)
                {
                    Functions.set_vehicle_mod(VehID, 13, 2);
                }
            }
        }

        private void rpcCustomCarSetTurbo_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 18, rpcCustomCarTurbo.SelectedIndex);
            }
        }

        private void rpcCustomCarSetArmor_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarArmor.SelectedIndex == 0)
                {
                    Functions.set_vehicle_mod(VehID, 16, -1);
                }
                else if (rpcCustomCarArmor.SelectedIndex == 1)
                {
                    Functions.set_vehicle_mod(VehID, 16, 0);
                }
                else if (rpcCustomCarArmor.SelectedIndex == 2)
                {
                    Functions.set_vehicle_mod(VehID, 16, 1);
                }
                else if (rpcCustomCarArmor.SelectedIndex == 3)
                {
                    Functions.set_vehicle_mod(VehID, 16, 2);
                }
                else if (rpcCustomCarArmor.SelectedIndex == 4)
                {
                    Functions.set_vehicle_mod(VehID, 16, 3);
                }
                else if (rpcCustomCarArmor.SelectedIndex == 5)
                {
                    Functions.set_vehicle_mod(VehID, 16, 4);
                }
            }
        }

        private void rpcCustomCarSetHorn_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 14, rpcCustomCarHorn.SelectedIndex);
            }
        }

        private void rpcCustomCarPlate_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            string text = rpcCustomCarPlateText.Text;
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_number_plate_text(VehID, text);
            }
        }

        private void rpcCustomCarSetPlateType_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_number_plate_index(VehID, rpcCustomCarPlateType.SelectedIndex);
            }
        }

        private void rpcCustomCarWindowSet_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarWindowSelect.SelectedIndex > 3)
                {
                    Functions.set_vehicle_window_tint(VehID, rpcCustomCarWindowSelect.SelectedIndex + 1);
                }
                else
                {
                    Functions.set_vehicle_window_tint(VehID, rpcCustomCarWindowSelect.SelectedIndex);
                }
            }
        }

        private void rpcCustomCarPrimaryRGB_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarRGB.ShowDialog() == DialogResult.OK)
                {
                    int red = rpcCustomCarRGB.Color.R;
                    int green = rpcCustomCarRGB.Color.G;
                    int blue = rpcCustomCarRGB.Color.B;
                    Functions.set_vehicle_custom_primary_colour(VehID, red, green, blue);
                }
            }
        }

        private void rpcCustomCarRandomColor_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Random rand = new Random();
                int red = rand.Next(0, 255);
                int r = rand.Next(0, 255);
                int green = rand.Next(0, 255);
                int g = rand.Next(0, 255);
                int blue = rand.Next(0, 255);
                int b = rand.Next(0, 255);
                Functions.set_vehicle_custom_primary_colour(VehID, red, green, blue);
                Functions.set_vehicle_custom_secondary_colour(VehID, r, g, b);
            }
        }

        private void rpcCustomCarWheelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarWheelType.SelectedIndex < 5)
                {
                    Functions.set_vehicle_wheel_type(VehID, rpcCustomCarWheelType.SelectedIndex);
                }
                else
                {
                    Functions.set_vehicle_wheel_type(VehID, rpcCustomCarWheelType.SelectedIndex + 1);
                }
            }
            if (rpcCustomCarWheelType.SelectedIndex == 0)
            {
                rpcCustomCarWheel.Items.Clear();
                rpcCustomCarWheel.Items.Add("Inferno");
                rpcCustomCarWheel.Items.Add("Deep Five");
                rpcCustomCarWheel.Items.Add("Lozspeed Mk.V");
                rpcCustomCarWheel.Items.Add("Diamond Cut");
                rpcCustomCarWheel.Items.Add("Chrono");
                rpcCustomCarWheel.Items.Add("Feroci RR");
                rpcCustomCarWheel.Items.Add("FiftyNine");
                rpcCustomCarWheel.Items.Add("Mercie");
                rpcCustomCarWheel.Items.Add("Synthetic Z");
                rpcCustomCarWheel.Items.Add("Organic Type 0");
                rpcCustomCarWheel.Items.Add("Endo v.1");
                rpcCustomCarWheel.Items.Add("GT One");
                rpcCustomCarWheel.Items.Add("Duper 7");
                rpcCustomCarWheel.Items.Add("Uzer");
                rpcCustomCarWheel.Items.Add("GroundRide");
                rpcCustomCarWheel.Items.Add("S Racer");
                rpcCustomCarWheel.Items.Add("Venum");
                rpcCustomCarWheel.Items.Add("Cosmo");
                rpcCustomCarWheel.Items.Add("Dash VIP");
                rpcCustomCarWheel.Items.Add("Ice Kid");
                rpcCustomCarWheel.Items.Add("Ruff Weld");
                rpcCustomCarWheel.Items.Add("Wangan Master");
                rpcCustomCarWheel.Items.Add("Super Five");
                rpcCustomCarWheel.Items.Add("Endo v.2");
                rpcCustomCarWheel.Items.Add("Split Six");
            }
            if (rpcCustomCarWheelType.SelectedIndex == 1)
            {
                rpcCustomCarWheel.Items.Clear();
                rpcCustomCarWheel.Items.Add("Classic Five");
                rpcCustomCarWheel.Items.Add("Dukes");
                rpcCustomCarWheel.Items.Add("Muscle Freak");
                rpcCustomCarWheel.Items.Add("Kracka");
                rpcCustomCarWheel.Items.Add("Azreal");
                rpcCustomCarWheel.Items.Add("Mecha");
                rpcCustomCarWheel.Items.Add("Black Top");
                rpcCustomCarWheel.Items.Add("Drag SPL");
                rpcCustomCarWheel.Items.Add("Revolver");
                rpcCustomCarWheel.Items.Add("Classic Rod");
                rpcCustomCarWheel.Items.Add("Fairlie");
                rpcCustomCarWheel.Items.Add("Spooner");
                rpcCustomCarWheel.Items.Add("Five Star");
                rpcCustomCarWheel.Items.Add("Old School");
                rpcCustomCarWheel.Items.Add("El Jefe");
                rpcCustomCarWheel.Items.Add("Dodman");
                rpcCustomCarWheel.Items.Add("Six Gun");
                rpcCustomCarWheel.Items.Add("Mercenary");
            }
            if (rpcCustomCarWheelType.SelectedIndex == 2)
            {
                rpcCustomCarWheel.Items.Clear();
                rpcCustomCarWheel.Items.Add("Flare");
                rpcCustomCarWheel.Items.Add("Wired");
                rpcCustomCarWheel.Items.Add("Triple Golds");
                rpcCustomCarWheel.Items.Add("Big Worm");
                rpcCustomCarWheel.Items.Add("Seven Fives");
                rpcCustomCarWheel.Items.Add("Split Six");
                rpcCustomCarWheel.Items.Add("Fresh Mesh");
                rpcCustomCarWheel.Items.Add("Lead Sled");
                rpcCustomCarWheel.Items.Add("Turbine");
                rpcCustomCarWheel.Items.Add("Super Fin");
                rpcCustomCarWheel.Items.Add("Classic Rod");
                rpcCustomCarWheel.Items.Add("Dollar");
                rpcCustomCarWheel.Items.Add("Dukes");
                rpcCustomCarWheel.Items.Add("Low Five");
                rpcCustomCarWheel.Items.Add("Gooch");
            }
            if (rpcCustomCarWheelType.SelectedIndex == 3)
            {
                rpcCustomCarWheel.Items.Clear();
                rpcCustomCarWheel.Items.Add("VIP");
                rpcCustomCarWheel.Items.Add("Benefactor");
                rpcCustomCarWheel.Items.Add("Cosmo");
                rpcCustomCarWheel.Items.Add("Bippu");
                rpcCustomCarWheel.Items.Add("Royal Six");
                rpcCustomCarWheel.Items.Add("Fagorme");
                rpcCustomCarWheel.Items.Add("Deluxe");
                rpcCustomCarWheel.Items.Add("Iced Out");
                rpcCustomCarWheel.Items.Add("Cognoscenti");
                rpcCustomCarWheel.Items.Add("LozSpeed Ten");
                rpcCustomCarWheel.Items.Add("Supernova");
                rpcCustomCarWheel.Items.Add("Obey RS");
                rpcCustomCarWheel.Items.Add("LozSpeed Baller");
                rpcCustomCarWheel.Items.Add("Extravaganzo");
                rpcCustomCarWheel.Items.Add("Split Six");
                rpcCustomCarWheel.Items.Add("Empowered");
                rpcCustomCarWheel.Items.Add("Sunrise");
                rpcCustomCarWheel.Items.Add("Dash VIP");
                rpcCustomCarWheel.Items.Add("Cutter");
            }
            if (rpcCustomCarWheelType.SelectedIndex == 4)
            {
                rpcCustomCarWheel.Items.Clear();
                rpcCustomCarWheel.Items.Add("Raider");
                rpcCustomCarWheel.Items.Add("Mudslinger");
                rpcCustomCarWheel.Items.Add("Nevis");
                rpcCustomCarWheel.Items.Add("Cairngorm");
                rpcCustomCarWheel.Items.Add("Amazon");
                rpcCustomCarWheel.Items.Add("Challenger");
                rpcCustomCarWheel.Items.Add("Dune Basher");
                rpcCustomCarWheel.Items.Add("Five Star");
                rpcCustomCarWheel.Items.Add("Rock Crawler");
                rpcCustomCarWheel.Items.Add("Mil Spec Steelie");
            }
            if (rpcCustomCarWheelType.SelectedIndex == 5)
            {
                rpcCustomCarWheel.Items.Clear();
                rpcCustomCarWheel.Items.Add("Cosmo");
                rpcCustomCarWheel.Items.Add("Super Mesh");
                rpcCustomCarWheel.Items.Add("Outsider");
                rpcCustomCarWheel.Items.Add("Rollas");
                rpcCustomCarWheel.Items.Add("Driftmeister");
                rpcCustomCarWheel.Items.Add("Slicer");
                rpcCustomCarWheel.Items.Add("El Quatro");
                rpcCustomCarWheel.Items.Add("Dubbed");
                rpcCustomCarWheel.Items.Add("Five Star");
                rpcCustomCarWheel.Items.Add("Slideways");
                rpcCustomCarWheel.Items.Add("Apex");
                rpcCustomCarWheel.Items.Add("Stanced EG");
                rpcCustomCarWheel.Items.Add("Countersteer");
                rpcCustomCarWheel.Items.Add("Endo v.1");
                rpcCustomCarWheel.Items.Add("Endo v.2 Dish");
                rpcCustomCarWheel.Items.Add("Gruppe Z");
                rpcCustomCarWheel.Items.Add("Choku-Dori");
                rpcCustomCarWheel.Items.Add("Chicane");
                rpcCustomCarWheel.Items.Add("Saisoku");
                rpcCustomCarWheel.Items.Add("Dished Eight");
                rpcCustomCarWheel.Items.Add("Fujiwara");
                rpcCustomCarWheel.Items.Add("Zokusha");
                rpcCustomCarWheel.Items.Add("Battle VIII");
                rpcCustomCarWheel.Items.Add("Rally Master");
            }
            if (rpcCustomCarWheelType.SelectedIndex == 6)
            {
                rpcCustomCarWheel.Items.Clear();
                rpcCustomCarWheel.Items.Add("Shadow");
                rpcCustomCarWheel.Items.Add("Hypher");
                rpcCustomCarWheel.Items.Add("Blade");
                rpcCustomCarWheel.Items.Add("Diamond");
                rpcCustomCarWheel.Items.Add("Supa Gee");
                rpcCustomCarWheel.Items.Add("Chromatic Z");
                rpcCustomCarWheel.Items.Add("Mercie Ch.Lip");
                rpcCustomCarWheel.Items.Add("Obey RS");
                rpcCustomCarWheel.Items.Add("GT Chrome");
                rpcCustomCarWheel.Items.Add("Cheetah RR");
                rpcCustomCarWheel.Items.Add("Solar");
                rpcCustomCarWheel.Items.Add("Split Ten");
                rpcCustomCarWheel.Items.Add("Dash VIP");
                rpcCustomCarWheel.Items.Add("LozSpeed Ten");
                rpcCustomCarWheel.Items.Add("Carbon Inferno");
                rpcCustomCarWheel.Items.Add("Carbon Shadow");
                rpcCustomCarWheel.Items.Add("Carbonic Z");
                rpcCustomCarWheel.Items.Add("Carbon Solar");
                rpcCustomCarWheel.Items.Add("Cheetah Carbon R");
                rpcCustomCarWheel.Items.Add("Carbon S Racer");
            }
        }

        private void rpcCustomCarWheel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 23, rpcCustomCarWheel.SelectedIndex);
            }
        }




        //Stats
        private void rpcStatPreLevels_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "script_increase_stam"), 100);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "script_increase_strn"), 100);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "script_increase_lung"), 100);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "script_increase_driv"), 100);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "script_increase_fly"), 100);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "script_increase_sho"), 100);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "script_increase_stl"), 100);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "script_increase_stam"), 100);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "script_increase_strn"), 100);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "script_increase_lung"), 100);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "script_increase_driv"), 100);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "script_increase_fly"), 100);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "script_increase_sho"), 100);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "script_increase_stl"), 100);
            }
        }

        private void rpcStatPreWeapon_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "PISTOL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CMBTPISTOL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "PISTOL50_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "APPISTOL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "MICROSMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "SMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ASLTSMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ASLTRIFLE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CRBNRIFLE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADVRIFLE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "MG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CMBTMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ASLTMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "PUMP_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "SAWNOFF_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "BULLPUP_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ASLTSHTGN_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "SNIPERRFL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "HVYSNIPER_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "GRNLAUNCH_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "RPG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "MINIGUNS_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "GRENADE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "SMKGRENADE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "STKYBMB_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "MOLOTOV_ENEMY_KILLS"), 600);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "PISTOL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CMBTPISTOL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "PISTOL50_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "APPISTOL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "MICROSMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "SMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ASLTSMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ASLTRIFLE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CRBNRIFLE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADVRIFLE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "MG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CMBTMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ASLTMG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "PUMP_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "SAWNOFF_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "BULLPUP_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ASLTSHTGN_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "SNIPERRFL_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "HVYSNIPER_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "GRNLAUNCH_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "RPG_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "MINIGUNS_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "GRENADE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "SMKGRENADE_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "STKYBMB_ENEMY_KILLS"), 600);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "MOLOTOV_ENEMY_KILLS"), 600);
            }
        }

        private void rpcStatPreSmoke_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CIGARETTES_BOUGHT"), 9999);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CIGARETTES_BOUGHT"), 9999);
            }
        }

        private void rpcStatPreSnack_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "NO_BOUGHT_YUM_SNACKS"), 9999);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "NO_BOUGHT_HEALTH_SNACKS"), 9999);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "NO_BOUGHT_EPIC_SNACKS"), 9999);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "NO_BOUGHT_YUM_SNACKS"), 9999);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "NO_BOUGHT_HEALTH_SNACKS"), 9999);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "NO_BOUGHT_EPIC_SNACKS"), 9999);
            }
        }

        private void rpcStatPreAward_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_100_KILLS_PISTOL"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_100_KILLS_SNIPER"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_50_KILLS_GRENADES"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_100_KILLS_SHOTGUN"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_100_KILLS_SMG"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_50_KILLS_ROCKETLAUNCH"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_25_KILLS_STICKYBOMBS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_20_KILLS_MELEE"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_100_HEADSHOTS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_50_VEHICLES_BLOWNUP"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_VEHICLES_JACKEDR"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_ENEMYDRIVEBYKILLS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_COPS_KILLED"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_BUY_EVERY_GUN"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_HOLD_UP_SHOPS"), 20);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_LAPDANCES"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_DRIVE_ALL_COP_CARS"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_CARS_EXPORTED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_SECURITY_CARS_ROBBED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_5STAR_WANTED_AVOIDANCE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_VEHICLE_JUMP" + "_OVER_40M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_VEHICLE_JUMP0" + "_OVER_40M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_VEHICLE_JUMP1" + "_OVER_40M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_RACES_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_CARS_EXPORTED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_NO_ARMWRESTLING_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_WIN_AT_DARTS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_CAR_BOMBS_ENEMY_KILLS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_KILLS_ASSAULT_RIFLE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_KILLS_MACHINEGUN"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_CARS_EXPORTED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_NO_HAIRCUTS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_PARACHUTE_JUMP" + "S_50M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_PARACHUTE_JUMP0" + "S_50M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_PARACHUTE_JUMP1" + "S_50M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_PARACHUTE_JUMP" + "S_20M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_PARACHUTE_JUMP0" + "S_20M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_PARACHUTE_JUMP1" + "S_20M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_GOLF_HOLE_IN_1"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_GOLF_BIRDIES"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_GOLF_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_SHOOTRANG_TG_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_SHOOTRANG_RT_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_SHOOTRANG_CT_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_SHOOTRANG_GRAN_WON"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_TENNIS_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_TENNIS_ACE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_TENNIS_5_SET_WINS"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_TENNIS_STASETWIN"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_GTA_RACES_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_RACES_FASTEST_LAP"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_RACE_LAST_FIRST"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_DM_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_TDM_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_TDM_MVP"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_DM_KILLSTREAK"), 100);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_DM_TOTALKILLS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_DM_3KILLSAMEGUY"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_DM_STOLENKILL"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMATTGANGHQ"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMBASEJMP" + ""), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMBASEJMP0" + ""), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMBASEJMP1" + ""), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMHORDWAVESSURVIVE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMBBETWIN"), 500000);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMCRATEDROPS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM6DARTCHKOUT"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMWINEVERYGAMEMODE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMP" + "ICKUPDLCCRATE1ST"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMP0" + "ICKUPDLCCRATE1ST"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMP1" + "ICKUPDLCCRATE1ST"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMWINALLRACEMODES"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMRACEWORLDRECHOLDER"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMRALLYWONDRIVE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMRALLYWONNAV"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM50DIFITEMSCLOTHES"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMFULLYMODDEDCAR"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMWINCUSTOMRACE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM50DIFFERENTDM"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM50DIFFERENTRACES"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMMOSTKILLSGANGHIDE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMMOSTKILLSSURVIVE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMSHOOTDOWNCOPHELI"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMKILLCHEATER"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMKILL3ANDWINGTARACE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMTATTOOALLBODYPARTS"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMWINRACETOPOINTS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMKILLBOUNTY"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMOVERALLKILLS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMWINSEARACE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMREVENGEKILLSDM"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMKILLSTREAKSDM"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMFURTHESTWHEELIE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMMOSTFLIPSINONEVEHICLE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMMOSTSPINSINONEVEHICLE"), 50);
            }
            else if (rpcStatPreSecondary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_100_KILLS_PISTOL"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_100_KILLS_SNIPER"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_50_KILLS_GRENADES"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_100_KILLS_SHOTGUN"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_100_KILLS_SMG"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_50_KILLS_ROCKETLAUNCH"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_25_KILLS_STICKYBOMBS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_20_KILLS_MELEE"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_100_HEADSHOTS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_50_VEHICLES_BLOWNUP"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_VEHICLES_JACKEDR"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_ENEMYDRIVEBYKILLS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_COPS_KILLED"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_BUY_EVERY_GUN"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_HOLD_UP_SHOPS"), 20);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_LAPDANCES"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_DRIVE_ALL_COP_CARS"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_CARS_EXPORTED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_SECURITY_CARS_ROBBED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_5STAR_WANTED_AVOIDANCE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_VEHICLE_JUMP" + "_OVER_40M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_VEHICLE_JUMP0" + "_OVER_40M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_VEHICLE_JUMP1" + "_OVER_40M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_RACES_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_CARS_EXPORTED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_NO_ARMWRESTLING_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_WIN_AT_DARTS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_CAR_BOMBS_ENEMY_KILLS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_KILLS_ASSAULT_RIFLE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_KILLS_MACHINEGUN"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_CARS_EXPORTED"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_NO_HAIRCUTS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_PARACHUTE_JUMP" + "S_50M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_PARACHUTE_JUMP0" + "S_50M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_PARACHUTE_JUMP1" + "S_50M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_PARACHUTE_JUMP" + "S_20M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_PARACHUTE_JUMP0" + "S_20M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_PARACHUTE_JUMP1" + "S_20M"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_GOLF_HOLE_IN_1"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_GOLF_BIRDIES"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_GOLF_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_SHOOTRANG_TG_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_SHOOTRANG_RT_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_SHOOTRANG_CT_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_SHOOTRANG_GRAN_WON"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_TENNIS_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_TENNIS_ACE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_TENNIS_5_SET_WINS"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_TENNIS_STASETWIN"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_GTA_RACES_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_RACES_FASTEST_LAP"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_RACE_LAST_FIRST"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_DM_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_TDM_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_TDM_MVP"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_DM_KILLSTREAK"), 100);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_DM_TOTALKILLS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_DM_3KILLSAMEGUY"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_DM_STOLENKILL"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMATTGANGHQ"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMBASEJMP" + ""), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMBASEJMP0" + ""), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMBASEJMP1" + ""), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMHORDWAVESSURVIVE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMBBETWIN"), 500000);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMCRATEDROPS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM6DARTCHKOUT"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMWINEVERYGAMEMODE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMP" + "ICKUPDLCCRATE1ST"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMP0" + "ICKUPDLCCRATE1ST"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMP1" + "ICKUPDLCCRATE1ST"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMWINALLRACEMODES"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMRACEWORLDRECHOLDER"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMRALLYWONDRIVE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMRALLYWONNAV"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM50DIFITEMSCLOTHES"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMFULLYMODDEDCAR"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMWINCUSTOMRACE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM50DIFFERENTDM"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM50DIFFERENTRACES"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMMOSTKILLSGANGHIDE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMMOSTKILLSSURVIVE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMSHOOTDOWNCOPHELI"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMKILLCHEATER"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMKILL3ANDWINGTARACE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMTATTOOALLBODYPARTS"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMWINRACETOPOINTS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMKILLBOUNTY"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMOVERALLKILLS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMWINSEARACE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMREVENGEKILLSDM"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMKILLSTREAKSDM"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMFURTHESTWHEELIE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMMOSTFLIPSINONEVEHICLE"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMMOSTSPINSINONEVEHICLE"), 50);
            }
        }

        private void rpcStatCustomSet_Click(object sender, EventArgs e)
        {

            if (rpcStatCustomPrimary.Checked)
            {
                uint statHash = Functions.GetHash("MP0_" + rpcStatCustomName.Text);

                if (rpcStatCustomSelect.SelectedIndex == 0)
                {
                    try
                    {
                        int value = int.Parse(rpcStatCustomValue.Text);
                        Functions.stat_set_bool(statHash, value);
                    }
                    catch (System.FormatException)
                    {
                        Functions.stat_set_bool(statHash, Convert.ToInt32(rpcStatCustomValue.Text));
                    }
                }

                if (rpcStatCustomSelect.SelectedIndex == 1)
                {
                    try
                    {
                        Functions.stat_set_float(statHash, float.Parse(rpcStatCustomValue.Text));
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Format Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (rpcStatCustomSelect.SelectedIndex == 2)
                {
                    try
                    {
                        Functions.stat_set_int(statHash, int.Parse(rpcStatCustomValue.Text));
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Format Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (rpcStatCustomSelect.SelectedIndex == 3)
                {
                    try
                    {
                        Functions.stat_set_string(statHash, rpcStatCustomValue.Text);
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Format Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            else
            {
                uint statHash = Functions.GetHash("MP1_" + rpcStatCustomName.Text);

                if (rpcStatCustomSelect.SelectedIndex == 0)
                {
                    try
                    {
                        int value = int.Parse(rpcStatCustomValue.Text);
                        Functions.stat_set_bool(statHash, value);
                    }
                    catch (System.FormatException)
                    {
                        Functions.stat_set_bool(statHash, Convert.ToInt32(rpcStatCustomValue.Text));
                    }
                }

                if (rpcStatCustomSelect.SelectedIndex == 1)
                {
                    try
                    {
                        Functions.stat_set_float(statHash, float.Parse(rpcStatCustomValue.Text));
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Format Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (rpcStatCustomSelect.SelectedIndex == 2)
                {
                    try
                    {
                        Functions.stat_set_int(statHash, int.Parse(rpcStatCustomValue.Text));
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Format Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (rpcStatCustomSelect.SelectedIndex == 3)
                {
                    try
                    {
                        Functions.stat_set_string(statHash, rpcStatCustomValue.Text);
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Format Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        //Teleports
        private void rpcTeleportButton_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords = { float.Parse(rpcTeleportX.Text), float.Parse(rpcTeleportY.Text), float.Parse(rpcTeleportZ.Text) };
            Functions.do_screen_fade_out(400);
            Thread.Sleep(1000);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                int VehID = Functions.get_vehicle_ped_is_in(PedID);
                Functions.set_entity_coords(VehID, coords);
            }
            else
            {
                Functions.set_entity_coords(PedID, coords);
            }
            Thread.Sleep(2500);
            Functions.do_screen_fade_in(400);
        }

        private void rpcTeleportRefreshCurrent_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords = Functions.get_entity_coords(PedID);
            xCurrentLabel.Value1 = "X: " + coords[0].ToString();
            yCurrentLabel.Value1 = "Y: " + coords[1].ToString();
            zCurrentLabel.Value1 = "Z: " + coords[2].ToString();
        }

        private void rpcTeleportSelectedButton_Click(object sender, EventArgs e)
        {
            Functions.TeleportToXMLPlace(@"\Resources\Teleports.xml", rpcTeleportSelectedLocationBox.Text, rpcTeleportSelectedLocationBox);
        }

        private void rpcWaypointPort_Click(object sender, EventArgs e)
        {
            byte[] waypointX = PS3.CCAPI.GetBytes(Addresses.RTM.waypointX, 0x04);
            Array.Reverse(waypointX);
            float x = BitConverter.ToSingle(waypointX, 0);
            byte[] waypointY = PS3.CCAPI.GetBytes(Addresses.RTM.waypointY, 0x04);
            Array.Reverse(waypointY);
            float y = BitConverter.ToSingle(waypointY, 0);
            Functions.do_screen_fade_out(400);

            if (rpcWaypointZ.Text.Length == 0)
            {
                int PedID = Functions.player_ped_id();
                float[] currentCoords = Functions.get_entity_coords(PedID);
                float[] coords = { x, y, currentCoords[2] };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else
            {
                int PedID = Functions.player_ped_id();
                float[] currentCoords = Functions.get_entity_coords(PedID);
                float[] coords = { x, y, Convert.ToSingle(rpcWaypointZ.Text) };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }
            Functions.do_screen_fade_in(400);
        }

        private void rpcHealPlayer_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int max = Functions.get_entity_max_health(PedID);
            Functions.set_entity_health(PedID, max);
        }

        private void rpcGiveArmor_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            Functions.add_armour_to_ped(PedID, 100);
        }

        private void rpcLeftLightOn_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            Functions.set_vehicle_indicator_lights(VehID, 1, 1);
        }

        private void rpcLightOff_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            Functions.set_vehicle_indicator_lights(VehID, 1, 0);
            Functions.set_vehicle_indicator_lights(VehID, 0, 0);
        }

        private void rpcRightLightOn_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            Functions.set_vehicle_indicator_lights(VehID, 0, 1);
        }

        private void rpcWeatherSend_Click(object sender, EventArgs e)
        {
            Functions.set_weather_type_now(rpcWeatherSelector.Text);
            Functions.set_override_weather(rpcWeatherSelector.Text);
        }

        private void rpcWeatherRandom_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            var weather = new List<string> { "CLEAR", "EXTRASUNNY", "CLOUDS", "SMOG", "FOGGY", "OVERCAST", "RAIN", "THUNDER", "CLEARING", "NEUTRAL", "SNOW", "BLIZZARD", "SNOWLIGHT" };
            int index = rand.Next(weather.Count);
            var name = weather[index];
            weather.RemoveAt(index);
            Functions.set_override_weather(name);
        }

        private void rpcSetTimeMod_Click(object sender, EventArgs e)
        {
            float scale = float.Parse(timeSelect.Value.ToString());
            Functions.set_time_scale(scale);
        }

        private void rpcUnlockAllTrophies_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 60; i++)
            {
                if (Functions.has_achievement_been_passed(i) == 0)
                {
                    Functions.give_achievement_to_player(i);
                    Thread.Sleep(5000);
                }
            }
        }

        private void rpcClientArmor_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            Functions.add_armour_to_ped(PedID, 100);
        }

        private void rpcGetWanted_Click(object sender, EventArgs e)
        {
            int PlayerID = Functions.player_id();
            int stars = Functions.get_player_wanted_level(PlayerID);
            rpcWantedLevel.Value = stars;
        }

        private void rpcSetWanted_Click(object sender, EventArgs e)
        {
            int PlayerID = Functions.player_id();
            int stars = rpcWantedLevel.Value;
            Functions.set_player_wanted_level(PlayerID, stars);
            Functions.set_player_wanted_level_now(PlayerID, stars);
        }

        private void rpcPortCloseVehicle_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords =Functions.get_entity_coords(PedID);
            int Vehicle = Functions.get_closest_vehicle(coords, 500f);
            Functions.set_ped_into_vehicle(PedID, Vehicle, -1);
        }

        private void rpcCarGodmode_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
	        {
                Functions.set_vehicle_strong(VehID, 1);
                Functions.set_entity_proofs(VehID, 1, 1, 1, 1, 1, 1, 1, 1);
                Functions.set_entity_can_be_damaged(VehID, 0);
                Functions.set_vehicle_can_be_visibly_damaged(VehID, 0);
                Functions.set_vehicle_can_break(VehID, 0);
	        }
        }

        private void rpcCustomCarSecondRGB_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                if (rpcCustomCarRGB.ShowDialog() == DialogResult.OK)
                {
                    int red = rpcCustomCarRGB.Color.R;
                    int green = rpcCustomCarRGB.Color.G;
                    int blue = rpcCustomCarRGB.Color.B;
                    Functions.set_vehicle_custom_secondary_colour(VehID, red, green, blue);
                }
            }
        }

        private void rpcCustomCarSetHeadlights_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 22, rpcCustomCarHeadlight.SelectedIndex);
            }
        }

        private void rpcToggleDeadEye_CheckedChanged(object sender)
        {
            if (rpcToggleDeadEye.Checked)
            {
                deadEyeTimer.Start();
            }
            else
            {
                deadEyeTimer.Stop();
                Functions.set_time_scale(1f);
            }
        }

        private void deadEyeTimer_Tick(object sender, EventArgs e)
        {
            int PlayerID = Functions.player_id();

            if (Functions.is_player_free_aiming(PlayerID) == 1)
            {
                Functions.set_time_scale(0.1f);
            }

            else if (Functions.is_player_targetting_anything(PlayerID) == 1)
            {
                Functions.set_time_scale(0.1f);
            }

            else
            {
                Functions.set_time_scale(1f);
            }
        }

        private void rpcPortVehToMe_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords = Functions.get_entity_coords(PedID);
            int VehID = Functions.get_closest_vehicle(coords, 500f);
            Functions.network_request_control_of_entity(VehID);
            Functions.set_entity_coords(VehID, coords);
            Functions.set_ped_into_vehicle(PedID, VehID, 0);
            Functions.set_ped_into_vehicle(PedID, VehID, -1);
        }

        private void rpcButtonDropFix_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords = Functions.get_entity_coords(PedID);
            Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_VEHICLE_HEALTH_STANDARD, coords, 2000);
        }

        private void findModelOffset_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please Wait While The Tool Searches...", "Please Wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModelChanger.find();
            if (ModelChanger.found)
            {
                MessageBox.Show("Offset Found", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Offset Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void setCustomModel_Click(object sender, EventArgs e)
        {
            if (!ModelChanger.found)
            {
                MessageBox.Show("Find Offset First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ModelChanger.changeModel(modelSelector.Text);
            }
        }

        private void modelKillPlayer_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            Functions.set_entity_health(PedID, 0);
        }

        private void vehPortToMe_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            int MyPed = Functions.player_ped_id();
            float[] coords = Functions.get_entity_coords(MyPed);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                for (; ; )
                {
                    Hook.Call(Addresses.Natives.network_request_control_of_entity, VehID);
                    if (Hook.Call(Addresses.Natives.network_has_control_of_entity, VehID) == 1)
                        break;
                }
                Functions.set_entity_coords(VehID, coords);
            }
        }

        private void vehClientSpeed_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                for (; ; )
                {
                    Hook.Call(Addresses.Natives.network_request_control_of_entity, VehID);
                    if (Hook.Call(Addresses.Natives.network_has_control_of_entity, VehID) == 1)
                        break;
                }
                Functions.set_vehicle_forward_speed(VehID, vehControlSpeed.Value);
            }
        }

        private void vehControlSpeed_Scroll(object sender)
        {
            vehControlSpeedLabel.Value1 = "Speed = " + vehControlSpeed.Value.ToString();
        }

        private void vehControlFire_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.network_request_control_of_entity(VehID);
                for (; ; )
                {
                    Hook.Call(Addresses.Natives.network_request_control_of_entity, VehID);
                    if (Hook.Call(Addresses.Natives.network_has_control_of_entity, VehID) == 1)
                        break;
                }
                Functions.set_vehicle_engine_health(VehID, -1.0f);
                Functions.set_vehicle_petrol_tank_health(VehID, -1.0f);
                Functions.set_entity_health(VehID, -1);
                Functions.start_entity_fire(VehID);
            }
        }

        private void vehControlFreeze_CheckedChanged(object sender)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                if (vehControlFreeze.Checked)
                {
                    Functions.network_request_control_of_entity(VehID);
                    for (; ; )
                    {
                        Hook.Call(Addresses.Natives.network_request_control_of_entity, VehID);
                        if (Hook.Call(Addresses.Natives.network_has_control_of_entity, VehID) == 1)
                            break;
                    }
                    Functions.freeze_entity_position(VehID, 1);
                }
                else
                {
                    Functions.network_request_control_of_entity(VehID);
                    for (; ; )
                    {
                        Hook.Call(Addresses.Natives.network_request_control_of_entity, VehID);
                        if (Hook.Call(Addresses.Natives.network_has_control_of_entity, VehID) == 1)
                            break;
                    }
                    Functions.freeze_entity_position(VehID, 0);
                }
            }
        }

        private void rpcClientKill_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            Functions.set_entity_health(PedID, 0);
        }

        private void rpcClientDropMoney_Click(object sender, EventArgs e)
        {
            if (rpcClientMoneySelect.SelectedIndex > -1)
            {
                if (rpcClientMoneySelect.SelectedIndex == 0)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 1000);
                }
                else if (rpcClientMoneySelect.SelectedIndex == 1)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                }
                else if (rpcClientMoneySelect.SelectedIndex == 2)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 2; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                    }
                    Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 1000);
                }
                else if (rpcClientMoneySelect.SelectedIndex == 3)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 5; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                    }
                }
                else if (rpcClientMoneySelect.SelectedIndex == 4)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 10; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                        Thread.Sleep(10);
                    }
                }
                else if (rpcClientMoneySelect.SelectedIndex == 5)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 12; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                        Thread.Sleep(10);
                    }
                    Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 1000);
                }
                else if (rpcClientMoneySelect.SelectedIndex == 6)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 25; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                        Thread.Sleep(10);
                    }
                }
                else if (rpcClientMoneySelect.SelectedIndex == 7)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 50; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                        Thread.Sleep(10);
                    }
                }
                else if (rpcClientMoneySelect.SelectedIndex == 8)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 250; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                        Thread.Sleep(20);
                    }
                }
                else if (rpcClientMoneySelect.SelectedIndex == 9)
                {
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);
                    for (int i = 0; i < 500; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                        Thread.Sleep(20);
                    }
                }
            }
            else
            {
                for (int i = 0; i < rpcClientCustomMoney.Text.Length; i++)
                {
                    if (rpcClientCustomMoney.Text.Contains(","))
                    {
                        int index = rpcClientCustomMoney.Text.IndexOf(",");
                        rpcClientCustomMoney.Text.Remove(index);
                    }
                }

                int entered;

                if (int.TryParse(rpcClientCustomMoney.Text, out entered))
                {
                    int times = entered / 2000;
                    int remain = entered % 2000;
                    int PedID = Functions.get_player_ped(Clients.SelectedIndex);
                    float[] coords = Functions.get_entity_coords(PedID);

                    for (int i = 0; i < times; i++)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, 2000);
                        Thread.Sleep(10);
                    }

                    if (remain > 0)
                    {
                        Functions.create_ambient_pickup(Addresses.Hashes.Pickups.PICKUP_MONEY_PAPER_BAG, coords, remain);
                    }
                }
                else
                {
                    MessageBox.Show("Please Only Enter Numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rpcClientCustomMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
            else
            {
                MessageBox.Show("Please Only Enter Numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
            }
        }

        private void resetCheating_Click(object sender, EventArgs e)
        {
            Functions.stat_set_int(Functions.GetHash("MPPLY_GRIEFING"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_VC_ANNOYINGME"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_VC_HATE"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_OFFENSIVE_LANGUAGE"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_BAD_CREW_NAME"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_BAD_CREW_MOTTO"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_BAD_CREW_STATUS"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_BAD_CREW_EMBLEM"), 0);
            Functions.stat_set_int(Functions.GetHash("MPPLY_EXPLOITS"), 0);
        }

        private void garageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GarageEditor.updating = true;
            byte[] bLicense = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.LICENSE, 8);
            string sLicense = Encoding.ASCII.GetString(bLicense);
            garagePlateText.Text = sLicense;

            //get other attributes
            byte[] bEngine = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.ENGINE, 1);
            //MessageBox.Show(bEngine[0].ToString());
            if (bEngine[0] < 5)
                garageEngine.SelectedIndex = bEngine[0];

            byte[] bSuspension = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.SUSPENSION, 1);
            if (bSuspension[0] < 5)
                garageSuspension.SelectedIndex = bSuspension[0];

            byte[] bTrans = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.TRANSMISSION, 1);
            if (bTrans[0] < 4)
                garageTrans.SelectedIndex = bTrans[0];

            byte[] bBrakes = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.BRAKES, 1);
            if (bBrakes[0] < 4)
                garageBrakes.SelectedIndex = bBrakes[0];

            byte[] bArmor = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.ARMOR, 1);
            if (bArmor[0] < 6)
                garageArmor.SelectedIndex = bArmor[0];

            byte[] bWindow = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.WINDOW, 1);
            if (bWindow[0] < 7)
                garageWindow.SelectedIndex = bWindow[0];

            byte[] bHorn = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.HORN, 1);
            if (bHorn[0] < 32)
                garageHorn.SelectedIndex = bHorn[0];

            byte[] bWheelCat = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.RIMCLASS, 1);
            if (bWheelCat[0] < 8)
                garageWheelCategory.SelectedIndex = bWheelCat[0];

            byte[] bWheel = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.RIMS, 1);
            int MaxWheel = 0;
            switch (bWheel[0])
            {
                case 0:
                    MaxWheel = 25;
                    break;
                case 1:
                    MaxWheel = 18;
                    break;
                case 2:
                    MaxWheel = 15;
                    break;
                case 3:
                    MaxWheel = 19;
                    break;
                case 4:
                    MaxWheel = 9;
                    break;
                case 5:
                    MaxWheel = 24;
                    break;
                case 6:
                    MaxWheel = 12;
                    break;
                case 7:
                    MaxWheel = 20;
                    break;
            }

            if (bWheel[0] < MaxWheel)
                garageWheelType.SelectedIndex = bWheel[0];

            byte[] bRimcolor = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.RIMCOLOR, 1);
            if (bRimcolor[0] < 161)
                garageWheelColor.SelectedIndex = bRimcolor[0];

            byte[] bLcolor = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.LICENSECOLOR, 1);
            if (bLcolor[0] < 6)
                garagePlateType.SelectedIndex = bLcolor[0];

            byte[] bPcolor = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.PRIM_COLOR, 1);
            if (bPcolor[0] < 161)
                garageColorPrime.SelectedIndex = bPcolor[0];

            byte[] bScolor = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.SEC_COLOR, 1);
            if (bScolor[0] < 161)
                garageColorSecond.SelectedIndex = bScolor[0];

            byte[] bXcolor = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.PEARL_COLOR, 1);
            if (bXcolor[0] < 161)
                garageColorPearl.SelectedIndex = bXcolor[0];

            byte[] bTurbo = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.TURBO, 1);
            if (bTurbo[0] == 1)
                garageTurbo.Checked = true;
            else
                garageTurbo.Checked = false;

            byte[] bInsurance = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.INSURANCE, 1);
            if (bInsurance[0] == 4)
                garageInsurance.Checked = true;
            else
                garageInsurance.Checked = false;

            byte[] bXenon = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.XENON, 1);
            if (bXenon[0] == 1)
                garageXenon.Checked = true;
            else
                garageXenon.Checked = false;

            byte[] bProof = PS3.GetBytes(GarageEditor.garageoffset + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH) + GarageEditor.BULLETPROOF, 1);
            if (bProof[0] == 1)
                garageBullet.Checked = true;
            else
                garageBullet.Checked = false;

            GarageEditor.updating = false;
        }

        private void garageCarSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                GarageEditor.changecar(GarageEditor.car2hash(garageCarSelect.Text), (uint)garageList.SelectedIndex);
            }
        }

        private void garageSuspension_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Susp = { Convert.ToByte(garageSuspension.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.SUSPENSION + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Susp);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garagePatriot_CheckedChanged(object sender)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                if (garagePatriot.Checked == true)
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_R + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x00 });
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_G + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x00 });
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_B + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x00 });
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_ENABLED + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x01 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
                else
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_R + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x17 });
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_G + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x17 });
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_B + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x17 });
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TIRESMOKE_ENABLED + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x01 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
            }
        }

        private void garageTurbo_CheckedChanged(object sender)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                if (garageTurbo.Checked == true)
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TURBO + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x01 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
                else
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TURBO + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x00 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
            }
        }

        private void garageBullet_CheckedChanged(object sender)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                if (garageBullet.Checked == true)
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.BULLETPROOF + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x01 });
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.BULLETPROOF2 + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x02 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
                else
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.BULLETPROOF + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x00 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
            }
        }

        private void garageWheelColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Rimcolor = { Convert.ToByte(garageWheelColor.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.RIMCOLOR + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Rimcolor);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageWheelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Wheeltype = { Convert.ToByte(garageWheelType.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.RIMS + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Wheeltype);
                Byte[] Wheelclass = { Convert.ToByte(garageWheelCategory.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.RIMCLASS + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Wheelclass);
                if (garageWheelCategory.SelectedIndex == 6)
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.REARRIM + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Wheeltype);

                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        public void FillWheels(uint category)
        {
            garageWheelType.Items.Clear();
            switch (category)
            {
                case 0:
                    garageWheelType.Items.Add("Default");
                    garageWheelType.Items.Add("Inferno");
                    garageWheelType.Items.Add("Deep Fice");
                    garageWheelType.Items.Add("Lozspeed Mk V");
                    garageWheelType.Items.Add("Diamond Cut");
                    garageWheelType.Items.Add("Chrono");
                    garageWheelType.Items.Add("Feroci RR");
                    garageWheelType.Items.Add("FiftyNine");
                    garageWheelType.Items.Add("Mercie");
                    garageWheelType.Items.Add("SYnthetic Z");
                    garageWheelType.Items.Add("Organic Type");
                    garageWheelType.Items.Add("Endo v.1");
                    garageWheelType.Items.Add("GT One");
                    garageWheelType.Items.Add("Duper 7");
                    garageWheelType.Items.Add("Uzer");
                    garageWheelType.Items.Add("GroundRide");
                    garageWheelType.Items.Add("S Racer");
                    garageWheelType.Items.Add("Venum");
                    garageWheelType.Items.Add("Cosmo");
                    garageWheelType.Items.Add("Dash VIP");
                    garageWheelType.Items.Add("Ice Kid");
                    garageWheelType.Items.Add("Ruff Weld");
                    garageWheelType.Items.Add("Wangan Master");
                    garageWheelType.Items.Add("Super Five");
                    garageWheelType.Items.Add("Endo v.2");
                    garageWheelType.Items.Add("Split Six");
                    break;
                case 1:
                    garageWheelType.Items.Add("Default");
                    garageWheelType.Items.Add("Classic Five");
                    garageWheelType.Items.Add("Dukes");
                    garageWheelType.Items.Add("Muscle Freak");
                    garageWheelType.Items.Add("Kracka");
                    garageWheelType.Items.Add("Azreal");
                    garageWheelType.Items.Add("Mecha");
                    garageWheelType.Items.Add("Black Top");
                    garageWheelType.Items.Add("Drag SPL");
                    garageWheelType.Items.Add("Revolver");
                    garageWheelType.Items.Add("Classic Rod");
                    garageWheelType.Items.Add("Fairlie");
                    garageWheelType.Items.Add("Spooner");
                    garageWheelType.Items.Add("Five Star");
                    garageWheelType.Items.Add("Old School");
                    garageWheelType.Items.Add("El Jefe");
                    garageWheelType.Items.Add("Dodman");
                    garageWheelType.Items.Add("Six Gun");
                    garageWheelType.Items.Add("Mercenary");
                    break;
                case 2:
                    garageWheelType.Items.Add("Default");
                    garageWheelType.Items.Add("Flare");
                    garageWheelType.Items.Add("Wired");
                    garageWheelType.Items.Add("Triple Golds");
                    garageWheelType.Items.Add("Big Worm");
                    garageWheelType.Items.Add("Seven Fives");
                    garageWheelType.Items.Add("Split Six");
                    garageWheelType.Items.Add("Frech Mesh");
                    garageWheelType.Items.Add("Lead Sled");
                    garageWheelType.Items.Add("Turbine");
                    garageWheelType.Items.Add("Super Fin");
                    garageWheelType.Items.Add("Classic Rod");
                    garageWheelType.Items.Add("Dollar");
                    garageWheelType.Items.Add("Dukes");
                    garageWheelType.Items.Add("Low Five");
                    garageWheelType.Items.Add("Gooch");
                    break;
                case 3:
                    garageWheelType.Items.Add("Default");
                    garageWheelType.Items.Add("VIP");
                    garageWheelType.Items.Add("Benefactor");
                    garageWheelType.Items.Add("Cosmo");
                    garageWheelType.Items.Add("Bippu");
                    garageWheelType.Items.Add("Royal Six");
                    garageWheelType.Items.Add("Fagorme");
                    garageWheelType.Items.Add("Deluxe");
                    garageWheelType.Items.Add("Iced Out");
                    garageWheelType.Items.Add("Cignoscenti");
                    garageWheelType.Items.Add("LowSpeed Ten");
                    garageWheelType.Items.Add("SuperNova");
                    garageWheelType.Items.Add("Obey RS");
                    garageWheelType.Items.Add("LozSpeed Baller");
                    garageWheelType.Items.Add("Extravaganzo");
                    garageWheelType.Items.Add("Split Six");
                    garageWheelType.Items.Add("Empowered");
                    garageWheelType.Items.Add("Sunrise");
                    garageWheelType.Items.Add("Dash VIP");
                    garageWheelType.Items.Add("Cutter");
                    break;
                case 4:
                    garageWheelType.Items.Add("Raider");
                    garageWheelType.Items.Add("Mugslinger");
                    garageWheelType.Items.Add("Nevis");
                    garageWheelType.Items.Add("Cairngorm");
                    garageWheelType.Items.Add("Amazon");
                    garageWheelType.Items.Add("Challenger");
                    garageWheelType.Items.Add("Dune Basher");
                    garageWheelType.Items.Add("Five Star");
                    garageWheelType.Items.Add("Rock Crawler");
                    garageWheelType.Items.Add("Mil Spec Steelie");
                    break;
                case 5:
                    garageWheelType.Items.Add("Default");
                    garageWheelType.Items.Add("Cosmo");
                    garageWheelType.Items.Add("Super Mesh");
                    garageWheelType.Items.Add("Outsider");
                    garageWheelType.Items.Add("Rollas");
                    garageWheelType.Items.Add("Driftmeister");
                    garageWheelType.Items.Add("Slicer");
                    garageWheelType.Items.Add("El Quatro");
                    garageWheelType.Items.Add("Dubbed");
                    garageWheelType.Items.Add("Five Star");
                    garageWheelType.Items.Add("Slideways");
                    garageWheelType.Items.Add("Apex");
                    garageWheelType.Items.Add("Stanced EG");
                    garageWheelType.Items.Add("Countersteer");
                    garageWheelType.Items.Add("Endo v.1 ");
                    garageWheelType.Items.Add("Endo v.2 Dish");
                    garageWheelType.Items.Add("Gruppe Z");
                    garageWheelType.Items.Add("Choku-Dori");
                    garageWheelType.Items.Add("Chicane");
                    garageWheelType.Items.Add("Saisoku");
                    garageWheelType.Items.Add("Dushed Eight");
                    garageWheelType.Items.Add("Fujiwara");
                    garageWheelType.Items.Add("Zokusha");
                    garageWheelType.Items.Add("Battle VIII");
                    garageWheelType.Items.Add("Rally Master");
                    break;
                case 6:
                    garageWheelType.Items.Add("Stock Wheels");
                    garageWheelType.Items.Add("Speedway");
                    garageWheelType.Items.Add("Street Special");
                    garageWheelType.Items.Add("Racer");
                    garageWheelType.Items.Add("Track Star");
                    garageWheelType.Items.Add("Overlord");
                    garageWheelType.Items.Add("Trident");
                    garageWheelType.Items.Add("Triple Threat");
                    garageWheelType.Items.Add("Stilleto");
                    garageWheelType.Items.Add("Wire");
                    garageWheelType.Items.Add("Bobber");
                    garageWheelType.Items.Add("Solidus");
                    garageWheelType.Items.Add("Ice Shield");
                    break;
                case 7:
                    garageWheelType.Items.Add("Default");
                    garageWheelType.Items.Add("Shadow");
                    garageWheelType.Items.Add("Hypher");
                    garageWheelType.Items.Add("Blade");
                    garageWheelType.Items.Add("Diamond");
                    garageWheelType.Items.Add("Supa Gee");
                    garageWheelType.Items.Add("Chromatic Z");
                    garageWheelType.Items.Add("Mercie Ch.Lip");
                    garageWheelType.Items.Add("Obey RS");
                    garageWheelType.Items.Add("GT Chrome");
                    garageWheelType.Items.Add("Cheetah RR");
                    garageWheelType.Items.Add("Solar");
                    garageWheelType.Items.Add("Split Ten");
                    garageWheelType.Items.Add("Dash VIP");
                    garageWheelType.Items.Add("LozSpeed Ten");
                    garageWheelType.Items.Add("Carbon Inferno");
                    garageWheelType.Items.Add("Carbon Shadow");
                    garageWheelType.Items.Add("Carbonic Z");
                    garageWheelType.Items.Add("Carbon Solar");
                    garageWheelType.Items.Add("Cheetah Carbon");
                    garageWheelType.Items.Add("Carbon S Racer");
                    break;
                default:
                    return;
            }
        }

        private void garageWheelCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillWheels((uint)garageWheelCategory.SelectedIndex);
        }

        private void garageColorPrime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] pColor = { Convert.ToByte(garageColorPrime.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.PRIM_COLOR + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), pColor);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageColorSecond_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] pColor = { Convert.ToByte(garageColorSecond.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.SEC_COLOR + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), pColor);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageColorPearl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] pColor = { Convert.ToByte(garageColorPearl.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.PEARL_COLOR + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), pColor);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garagePlateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Lcolor = { Convert.ToByte(garagePlateType.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.LICENSECOLOR + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Lcolor);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Engine = { Convert.ToByte(garageEngine.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.ENGINE + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Engine);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageTrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Trans = { Convert.ToByte(garageTrans.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.TRANSMISSION + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Trans);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageBrakes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Brake = { Convert.ToByte(garageBrakes.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.BRAKES + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Brake);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageXenon_CheckedChanged(object sender)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                if (garageXenon.Checked == true)
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.XENON + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x01 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
                else
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.XENON + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x00 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
            }
        }

        private void garageInsurance_CheckedChanged(object sender)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                if (garageInsurance.Checked == true)
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.INSURANCE + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x04 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
                else
                {
                    PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.INSURANCE + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), new byte[] { 0x00 });
                    GarageEditor.applycarchange((uint)garageList.SelectedIndex);
                }
            }
        }

        private void garageArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Armor = { Convert.ToByte(garageArmor.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.ARMOR + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Armor);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Window = { Convert.ToByte(garageWindow.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.WINDOW + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Window);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garageHorn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                Byte[] Horn = { Convert.ToByte(garageHorn.SelectedIndex) };
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.HORN + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), Horn);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void garagePlateText_TextChanged(object sender, EventArgs e)
        {
            string sNew;
            if (GarageEditor.connected && GarageEditor.updating == false)
            {
                string sLicense = garagePlateText.Text;
                if (sLicense.Length < 8)
                    sNew = GarageEditor.PadBoth(sLicense, 8);
                else
                    sNew = sLicense;


                byte[] lplate = Encoding.ASCII.GetBytes(sNew);
                PS3.SetMemory(GarageEditor.garageoffset + GarageEditor.LICENSE + ((uint)garageList.SelectedIndex * GarageEditor.GARAGEITEMLENGTH), lplate);
                GarageEditor.applycarchange((uint)garageList.SelectedIndex);
            }
        }

        private void nsButton3_Click(object sender, EventArgs e)
        {
            garageList.TopIndex = 27;
        }

        private void nsButton4_Click(object sender, EventArgs e)
        {
            garageList.TopIndex = 13;
        }

        private void nsButton2_Click(object sender, EventArgs e)
        {
            garageList.TopIndex = 0;
        }

        private void rtmToggleCops_CheckedChanged(object sender)
        {
            if (rtmToggleCops.Checked)
            {
                PS3.SetMemory(Addresses.RTM.copsOffset, Addresses.RTM.toggleCopsOff);
            }
            else
            {
                PS3.SetMemory(Addresses.RTM.copsOffset, Addresses.RTM.toggleCopsOff);
            }
        }

        private void rpcLoadCustomIPL_Click(object sender, EventArgs e)
        {
            Functions.request_ipl(customIPL.Text);
            MessageBox.Show("Custom IPL Loaded", "Loaded");
        }

        private void rpcRemoveCustomIPL_Click(object sender, EventArgs e)
        {
            Functions.remove_ipl(customIPL.Text);
            MessageBox.Show("Custom IPL Removed", "Removed");
        }

        private void mapLoad_Click(object sender, EventArgs e)
        {
            if (mapLoadSelect.SelectedIndex == 0)
            {
                Functions.request_ipl("prologue01");
                Functions.request_ipl("Prologue01c");
                Functions.request_ipl("Prologue01d");
                Functions.request_ipl("Prologue01e");
                Functions.request_ipl("Prologue01f");
                Functions.request_ipl("Prologue01g");
                Functions.request_ipl("prologue01h");
                Functions.request_ipl("prologue01i");
                Functions.request_ipl("prologue01j");
                Functions.request_ipl("prologue01k");
                Functions.request_ipl("prologue01z");
                Functions.request_ipl("prologue02");
                Functions.request_ipl("prologue03");
                Functions.request_ipl("prologue03b");
                Functions.request_ipl("prologue03_grv_cov");
                Functions.request_ipl("prologue03_grv_dug");
                Functions.request_ipl("prologue03_grv_fun");
                Functions.request_ipl("prologue04");
                Functions.request_ipl("prologue04b");
                Functions.request_ipl("prologue04_cover");
                Functions.request_ipl("prologue05");
                Functions.request_ipl("prologue05b");
                Functions.request_ipl("prologue06");
                Functions.request_ipl("prologue06b");
                Functions.request_ipl("prologue06_int");
                Functions.request_ipl("prologuerd");
                Functions.request_ipl("prologuerdb");
                Functions.request_ipl("prologue_DistantLights");
                Functions.request_ipl("prologue_grv_torch");
                Functions.request_ipl("prologue_m2_door");
                Functions.request_ipl("prologue_LODLights");
                Functions.request_ipl("DES_ProTree_start");
                Functions.request_ipl("DES_ProTree_start_lod");
                MessageBox.Show("North Yankton Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 1)
            {
                Functions.request_ipl("ufo");
                Functions.request_ipl("ufo_eye");
                MessageBox.Show("UFO Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 2)
            {
                Functions.request_ipl("farmint");
                Functions.request_ipl("farm_props");
                MessageBox.Show("Farm Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 3)
            {
                Functions.request_ipl("smBoat");
                MessageBox.Show("Porn Yacht Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 4)
            {
                Functions.request_ipl("carshowroom_boarded");
                Functions.request_ipl("csr_afterMissionB");
                Functions.request_ipl("shr_int");
                MessageBox.Show("Simeon's Shop Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 5)
            {
                Functions.request_ipl("id2_14_during1");
                MessageBox.Show("Lester's Factory Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 6)
            {
                Functions.request_ipl("FINBANK");
                MessageBox.Show("Final Bank Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 7)
            {
                Functions.request_ipl("Coroner_Int_on");
                MessageBox.Show("Morgue Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 8)
            {
                Functions.request_ipl("CS1_02_cf_onmission1");
                Functions.request_ipl("CS1_02_cf_onmission2");
                Functions.request_ipl("CS1_02_cf_onmission3");
                Functions.request_ipl("CS1_02_cf_onmission4");
                MessageBox.Show("Cluckin Bell Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 9)
            {
                Functions.request_ipl("post_hiest_unload");
                MessageBox.Show("Jewelry Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 10)
            {
                Functions.request_ipl("cargoship");
                MessageBox.Show("Cargo Ship Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 11)
            {
                Functions.request_ipl("FIBlobby");
                MessageBox.Show("FIB Lobby Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 12)
            {
                Functions.request_ipl("sunkcargoship");
                Functions.request_ipl("SUNK_SHIP_FIRE");
                MessageBox.Show("Sunken Cargo Ship Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 13)
            {
                Functions.request_ipl("hei_carrier");
                Functions.request_ipl("Hei_carrier");
                Functions.request_ipl("hei_carrier_int1");
                Functions.request_ipl("hei_carrier_int2");
                Functions.request_ipl("hei_carrier_int3");
                Functions.request_ipl("hei_carrier_int4");
                Functions.request_ipl("hei_carrier_int5");
                Functions.request_ipl("hei_carrier_int6");
                Functions.request_ipl("hei_carrier_DistantLights");
                Functions.request_ipl("hei_carrier_LODLights");
                MessageBox.Show("Heist Aircraft Carrier Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 14)
            {
                Functions.request_ipl("hei_yacht_heist");
                Functions.request_ipl("hei_yacht_heist_Bar");
                Functions.request_ipl("hei_yacht_heist_Bedrm");
                Functions.request_ipl("hei_yacht_heist_Bridge");
                Functions.request_ipl("hei_yacht_heist_DistantLights");
                Functions.request_ipl("hei_yacht_heist_enginrm");
                Functions.request_ipl("hei_yacht_heist_LODLights");
                Functions.request_ipl("hei_yacht_heist_Lounge");
                MessageBox.Show("Heist Yacht Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 15)
            {
                Functions.request_ipl("facelobbyfake");
                Functions.request_ipl("facelobby");
                MessageBox.Show("LifeInvader Lobby Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 16)
            {
                Functions.request_ipl("Plane_crash_trench");
                uint hash = Functions.GetHash("prop_shamal_crash");
                float [] coords = new float[] { 2808.387f, 4795.484f, 47.2104f };
                Functions.create_object(hash, coords);
                MessageBox.Show("Plane Crash Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 17)
            {
                Functions.request_ipl("redCarpet");
                MessageBox.Show("Red Carpet Event Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 18)
            {
                Functions.remove_ipl("RC12B_Default");
                Functions.remove_ipl("RC12B_Fixed");
                Functions.request_ipl("RC12B_HospitalInterior");
                MessageBox.Show("Destroyed Hospital Interior Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 19)
            {
                Functions.remove_ipl("gasstation_ipl_group1");
                Functions.request_ipl("gasparticle_grp2");
                Functions.request_ipl("gasstation_ipl_group2");
                MessageBox.Show("Destroyed Gas Station Loaded", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rpcSelectedMapRemove_Click(object sender, EventArgs e)
        {
            if (mapLoadSelect.SelectedIndex == 0)
            {
                Functions.remove_ipl("prologue01");
                Functions.remove_ipl("Prologue01c");
                Functions.remove_ipl("Prologue01d");
                Functions.remove_ipl("Prologue01e");
                Functions.remove_ipl("Prologue01f");
                Functions.remove_ipl("Prologue01g");
                Functions.remove_ipl("prologue01h");
                Functions.remove_ipl("prologue01i");
                Functions.remove_ipl("prologue01j");
                Functions.remove_ipl("prologue01k");
                Functions.remove_ipl("prologue01z");
                Functions.remove_ipl("prologue02");
                Functions.remove_ipl("prologue03");
                Functions.remove_ipl("prologue03b");
                Functions.remove_ipl("prologue03_grv_cov");
                Functions.remove_ipl("prologue03_grv_dug");
                Functions.remove_ipl("prologue03_grv_fun");
                Functions.remove_ipl("prologue04");
                Functions.remove_ipl("prologue04b");
                Functions.remove_ipl("prologue04_cover");
                Functions.remove_ipl("prologue05");
                Functions.remove_ipl("prologue05b");
                Functions.remove_ipl("prologue06");
                Functions.remove_ipl("prologue06b");
                Functions.remove_ipl("prologue06_int");
                Functions.remove_ipl("prologuerd");
                Functions.remove_ipl("prologuerdb");
                Functions.remove_ipl("prologue_DistantLights");
                Functions.remove_ipl("prologue_grv_torch");
                Functions.remove_ipl("prologue_m2_door");
                Functions.remove_ipl("prologue_LODLights");
                Functions.remove_ipl("DES_ProTree_start");
                Functions.remove_ipl("DES_ProTree_start_lod");
                MessageBox.Show("North Yankton Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 1)
            {
                Functions.remove_ipl("ufo");
                Functions.remove_ipl("ufo_eye");
                MessageBox.Show("UFO Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 2)
            {
                Functions.remove_ipl("farmint");
                Functions.remove_ipl("farm_props");
                MessageBox.Show("Farm Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 3)
            {
                Functions.remove_ipl("smBoat");
                MessageBox.Show("Porn Yacht Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 4)
            {
                Functions.remove_ipl("carshowroom_boarded");
                Functions.remove_ipl("csr_afterMissionB");
                Functions.remove_ipl("shr_int");
                MessageBox.Show("Simeon's Shop Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 5)
            {
                Functions.remove_ipl("id2_14_during1");
                MessageBox.Show("Lester's Factory Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 6)
            {
                Functions.remove_ipl("FINBANK");
                MessageBox.Show("Final Bank Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 7)
            {
                Functions.remove_ipl("Coroner_Int_on");
                MessageBox.Show("Morgue Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 8)
            {
                Functions.remove_ipl("CS1_02_cf_onmission1");
                Functions.remove_ipl("CS1_02_cf_onmission2");
                Functions.remove_ipl("CS1_02_cf_onmission3");
                Functions.remove_ipl("CS1_02_cf_onmission4");
                MessageBox.Show("Cluckin Bell Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 9)
            {
                Functions.remove_ipl("post_hiest_unload");
                MessageBox.Show("Jewelry Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 10)
            {
                Functions.remove_ipl("cargoship");
                MessageBox.Show("Cargo Ship Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 11)
            {
                Functions.remove_ipl("FIBlobby");
                MessageBox.Show("FIB Lobby Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 12)
            {
                Functions.remove_ipl("sunkcargoship");
                Functions.remove_ipl("SUNK_SHIP_FIRE");
                MessageBox.Show("Sunken Cargo Ship Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 13)
            {
                Functions.remove_ipl("hei_carrier");
                Functions.remove_ipl("Hei_carrier");
                Functions.remove_ipl("hei_carrier_int1");
                Functions.remove_ipl("hei_carrier_int2");
                Functions.remove_ipl("hei_carrier_int3");
                Functions.remove_ipl("hei_carrier_int4");
                Functions.remove_ipl("hei_carrier_int5");
                Functions.remove_ipl("hei_carrier_int6");
                Functions.remove_ipl("hei_carrier_DistantLights");
                Functions.remove_ipl("hei_carrier_LODLights");
                MessageBox.Show("Heist Aircraft Carrier Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 14)
            {
                Functions.remove_ipl("hei_yacht_heist");
                Functions.remove_ipl("hei_yacht_heist_Bar");
                Functions.remove_ipl("hei_yacht_heist_Bedrm");
                Functions.remove_ipl("hei_yacht_heist_Bridge");
                Functions.remove_ipl("hei_yacht_heist_DistantLights");
                Functions.remove_ipl("hei_yacht_heist_enginrm");
                Functions.remove_ipl("hei_yacht_heist_LODLights");
                Functions.remove_ipl("hei_yacht_heist_Lounge");
                MessageBox.Show("Heist Yacht Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 15)
            {
                Functions.remove_ipl("facelobbyfake");
                Functions.remove_ipl("facelobby");
                MessageBox.Show("LifeInvader Lobby Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 16)
            {
                Functions.remove_ipl("Plane_crash_trench");
                MessageBox.Show("Plane Crash Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 17)
            {
                Functions.remove_ipl("redCarpet");
                MessageBox.Show("Red Carpet Event Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 18)
            {
                Functions.request_ipl("RC12B_Default");
                Functions.request_ipl("RC12B_Fixed");
                MessageBox.Show("Destroyed Hospital Interior Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (mapLoadSelect.SelectedIndex == 19)
            {
                Functions.remove_ipl("gasparticle_grp2");
                Thread.Sleep(100);
                Functions.remove_ipl("gasstation_ipl_group2");
                Functions.request_ipl("gasstation_ipl_group1");
                MessageBox.Show("Destroyed Gas Station Removed", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }  

        private void mapTP_Click(object sender, EventArgs e)
        {
            Functions.do_screen_fade_out(400);
            Thread.Sleep(1000);

            if (mapTPSelect.SelectedIndex == 0)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 3595.39673F, -4893.727F, 115.838394F };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 1)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 5310.1090F, -5210.5060F, 83.5216F };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 2)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 5302.84000F, -5185.84000F, 84.14000F };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 3)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -2051.99463F, 3237.05835F, 1456.97021F };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 4)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 2490.47729F, 3774.84351F, 2414.035F };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 5)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 2439.892f, 4969.99268f, 51.5648232f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 6)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -2045.533f, -1030.97668f, 11.9807472f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 7)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -57.5950623f, -1093.24524f, 26.4223347f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 8)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -32.12754f, -1090.2085f, 26.4222355f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 9)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -31.08326f, -1104.67712f, 26.4223347f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 10)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 712.7159f, -962.905457f, 30.3953228f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 11)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 2.69689322f, -667.0166f, 16.1306286f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 12)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 6.194215f, -660.759338f, 33.4501877f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 13)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 243.7397f, -1375.94031f, 39.534317f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 14)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -72.68752f, 6253.72656f, 31.08991f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 15)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -624.77124f, -233.189011f, 38.0570145f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 16)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -181.3641f, -2360.863f, 9.319064f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 17)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 111.2632f, -751.7224f, 45.7574f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 18)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 3069.330f, -4704.220f, 15.043f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 19)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 3102.963f, -4819.75f, 7.029271f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 20)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 3067.615f, -4692.494f, 6.077293f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 21)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 3087.66f, -4698.28f, 36.79125f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 22)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 3083.134f, -4687.491f, 27.252f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 23)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -2045.8f, -1031.2f, 11.9f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 24)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -1047.9f, -233.0f, 39.0f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 25)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 2814.7f, 4758.5f, 50.0f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 26)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 293.6717f, 180.8342f, 104.295f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 27)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 302.4858f, -587.7629f, 43.30757f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 28)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { 502.6115f, 5596.5225f, 795.9439f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            else if (mapTPSelect.SelectedIndex == 29)
            {
                int PedID = Functions.player_ped_id();
                float[] coords = { -102.2483f, 6410.027f, 31.48674f };

                if (Functions.is_ped_in_any_vehicle(PedID) == 1)
                {
                    int VehID = Functions.get_vehicle_ped_is_in(PedID);
                    Functions.set_entity_coords(VehID, coords);
                }
                else
                {
                    Functions.set_entity_coords(PedID, coords);
                }
            }

            Thread.Sleep(2500);
            Functions.do_screen_fade_in(400);
        }

        private void blesSelect_CheckedChanged(object sender)
        {
            if (blesSelect.Checked)
            {
                GarageEditor.GaragePointer = 0x1E60290;
                Functions.LoadNativesFile(@"\Resources\BLES-Natives.txt");
                Functions.LoadRTMFile(@"\Resources\BLES-RTM.txt");
                Functions.LoadRPCFile(@"\Resources\BLES-RPC.txt");
                this.updateLabel.Value1 = "Saved Update:  " + Updater.rpcSaved;
                MessageBox.Show("Region Loaded", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void blusSelect_CheckedChanged(object sender)
        {
            if (blusSelect.Checked)
            {
                Functions.LoadNativesFile(@"\Resources\BLUS-Natives.txt");
                Functions.LoadRTMFile(@"\Resources\BLUS-RTM.txt");
                Functions.LoadRPCFile(@"\Resources\BLUS-RPC.txt");
                this.updateLabel.Value1 = "Saved Update:  " + Updater.rpcSaved;
                MessageBox.Show("Region Loaded", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bljmSelect_CheckedChanged(object sender)
        {
            if (bljmSelect.Checked)
            {
                Functions.LoadNativesFile(@"\Resources\BLJM-Natives.txt");
                Functions.LoadRTMFile(@"\Resources\BLJM-RTM.txt");
                Functions.LoadRPCFile(@"\Resources\BLJM-RPC.txt");
                this.updateLabel.Value1 = "Saved Update:  " + Updater.rpcSaved;
                MessageBox.Show("Region Loaded", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rpcRainbow_CheckedChanged(object sender)
        {
            if (rpcRainbow.Checked)
            {
                int PedID = Functions.player_ped_id();
                int VehID = Functions.get_vehicle_ped_is_in(PedID);

                rainbowVehTimer.Tag = VehID;
                rainbowVehTimer.Start();
            }
            else
            {
                rainbowVehTimer.Stop();
            }
        }

        private void rainbowVehTimer_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int r1 = rand.Next(0, 255);
            int g1 = rand.Next(0, 255);
            int b1 = rand.Next(0, 255);
            int r2 = rand.Next(0, 255);
            int g2 = rand.Next(0, 255);
            int b2 = rand.Next(0, 255);

            int Veh = (int)rainbowVehTimer.Tag;

            Functions.set_vehicle_custom_primary_colour(Veh, r1, g1, b1);
            Functions.set_vehicle_custom_secondary_colour(Veh, r2, g2, b2);
        }

        private void rpcToggleDrunk_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();

            if (rpcToggleDrunk.Checked)
            {
                Functions.shake_gameplay_cam("DRUNK_SHAKE", 5f);
            }
            else
            {
                Functions.shake_gameplay_cam("DRUNK_SHAKE", 0f);
            }
        }

        private void statLSC100_Click(object sender, EventArgs e)
        {
            string ch = "MP0";

            if (rpcStatPreSecondary.Checked)
            {
                ch = "MP1";
            }

            string[] Stat_Names = { "RACES_WON", "NUMBER_SLIPSTREAMS_IN_RACE", "NUMBER_TURBO_STARTS_IN_RACE", "AWD_FMWINSEARACE", "AWD_FMWINAIRRACE", "AWD_FM_RACES_FASTEST_LAP", "USJS_FOUND", "USJS_COMPLETED", "AWD_FMRALLYWONDRIVE" };
            int[] Values = { 50, 110, 90, 10, 1, 50, 50, 50, 10, 120 };

            for (int t = 0; t <= Stat_Names.Length - 1; t++)
            {
                Functions.stat_set_int(Functions.GetHash(ch + "_" + Stat_Names[t]), Values[t]);
            }
            Functions.stat_set_int(Functions.GetHash(ch + "_" + "MPPLY_TIMES_RACE_BEST_LAP"), Values[9]);
            Functions.stat_set_int(Functions.GetHash("MPPLY_TIMES_RACE_BEST_LAP"), Values[9]);
        }

        private void openAllDoors_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            for (int i = 0; i < 8; i++)
            {
                Functions.set_vehicle_door_open(VehID, i);
            }
        }

        private void closeAllDoors_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);

            for (int i = 0; i < 8; i++)
            {
                Functions.set_vehicle_door_shut(VehID, i);
            }
        }

        private void skipRadioForward_Click(object sender, EventArgs e)
        {
            Functions.skip_radio_forward();
        }

        private void cheatsFind_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please Wait While The Tool Searches...", "Please Wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SPCheats.find();

            if (SPCheats.found)
            {
                MessageBox.Show("Offset Found", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Offset Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cheatSpawnCustomVeh_Click(object sender, EventArgs e)
        {
            byte[] activate = { 0x02 };
            uint hash = Functions.GetHash(cheatVeh.Text);
            byte[] P1 = BitConverter.GetBytes(hash);
            Array.Reverse(P1);
            if (!SPCheats.found)
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                PS3.SetMemory(SPCheats.CheatOffset + 0x51, P1);
                PS3.SetMemory(SPCheats.CheatOffset, activate);
            }
        }

        private void cheatCarSpawnFromList_Click(object sender, EventArgs e)
        {
            byte[] activate = { 0x02 };
            uint hash = Functions.GetHash(cheatCarList.Text);
            byte[] P1 = BitConverter.GetBytes(hash);
            Array.Reverse(P1);
            if (!SPCheats.found)
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                PS3.SetMemory(SPCheats.CheatOffset + 0x51, P1);
                PS3.SetMemory(SPCheats.CheatOffset, activate);
            }
        }

        private void firstPersonVeh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please Wait While The Tool Searches...", "Please Wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PS3.GetMemory(Addresses.Other.VehicleStuff.ModsStartOffset, Addresses.Other.VehicleStuff.ModsLongFoundsBytes);
            uint OffsetPlace = SPCheats.ContainsSequence(Addresses.Other.VehicleStuff.ModsLongFoundsBytes, Addresses.Other.VehicleStuff.ModsBytes, Addresses.Other.VehicleStuff.ModsStartOffset);
            if (OffsetPlace == 0u)
            {
                Addresses.Other.VehicleStuff.HandlingOffset = 0x0;
                MessageBox.Show("Offset Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                uint realoffset = OffsetPlace;
                Addresses.Other.VehicleStuff.ModsOffset = realoffset;
                string OffsetString = Convert.ToString(realoffset, 16);
                Addresses.Other.VehicleStuff.fpFound = true;
                MessageBox.Show("Offset Found", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            uint ModsOffset1 = Addresses.Other.VehicleStuff.ModsOffset + 0x18;

            for (int i = 0; i <= 297; i++)
            {
                uint j = Convert.ToUInt32(i);
                Addresses.Other.VehicleStuff.ModsOffsetUsed[i] = (j * 0x270) + ModsOffset1;
            }
        }

        private void firstPersonVehActivate_Click(object sender, EventArgs e)
        {
            if (!Addresses.Other.VehicleStuff.fpFound)
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                byte[] bytes = { 0x4E, 0xBF, 0xC8, 0x4A };
                Addresses.Other.VehicleStuff.ModsOffsetUsedCamera = Addresses.Other.VehicleStuff.ModsOffsetUsed[0] + 0xA4;
                for (int i = 0; i <= 297; i++)
                {
                    uint j = Convert.ToUInt32(i);
                    PS3.SetMemory(Addresses.Other.VehicleStuff.ModsOffsetUsedCamera + (j * 0x270), bytes);
                }
            }
        }

        private void skyFallCheat_Click(object sender, EventArgs e)
        {
            if (SPCheats.found)
            {
                byte[] E14 = new byte[] { 0x02 };
                PS3.SetMemory(SPCheats.CheatOffset + 0x48, E14);
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rechargeCheat_Click(object sender, EventArgs e)
        {
            if (SPCheats.found)
            {
                byte[] E14 = new byte[] { 0x04 };
                PS3.SetMemory(SPCheats.CheatOffset + 0x20, E14);
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void parachuteCheat_Click(object sender, EventArgs e)
        {
            if (SPCheats.found)
            {
                byte[] E14 = new byte[] { 0x04 };
                PS3.SetMemory(SPCheats.CheatOffset + 0x2C, E14);
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void weaponCheat_Click(object sender, EventArgs e)
        {
            if (SPCheats.found)
            {
                byte[] E14 = new byte[] { 0x04 };
                PS3.SetMemory(SPCheats.CheatOffset + 0x14, E14);
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void invincibleCheat_Click(object sender, EventArgs e)
        {
            if (SPCheats.found)
            {
                byte[] E14 = new byte[] { 0x04 };
                PS3.SetMemory(SPCheats.CheatOffset + 0x40, E14);
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void weatherCheat_Click(object sender, EventArgs e)
        {
            if (SPCheats.found)
            {
                byte[] E14 = new byte[] { 0x04 };
                PS3.SetMemory(SPCheats.CheatOffset + 0x18, E14);
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void superJumpCheat_CheckedChanged(object sender)
        {
            if (SPCheats.found)
            {
                if (superJumpCheat.Checked)
                {
                    byte[] E14 = new byte[] { 0x04 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x4, E14);
                }
                else
                {
                    byte[] E14 = new byte[] { 0x09 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x4, E14);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void moonGravityCheat_CheckedChanged(object sender)
        {
            if (SPCheats.found)
            {
                if (moonGravityCheat.Checked)
                {
                    byte[] E14 = new byte[] { 0x04 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x3C, E14);
                }
                else
                {
                    byte[] E14 = new byte[] { 0x09 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x3C, E14);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void boomMeleeCheat_CheckedChanged(object sender)
        {
            if (SPCheats.found)
            {
                if (boomMeleeCheat.Checked)
                {
                    byte[] E14 = new byte[] { 0x04 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x38, E14);
                }
                else
                {
                    byte[] E14 = new byte[] { 0x09 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x38, E14);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void boomBulletCheat_CheckedChanged(object sender)
        {
            if (SPCheats.found)
            {
                if (boomBulletCheat.Checked)
                {
                    byte[] E14 = new byte[] { 0x04 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x30, E14);
                }
                else
                {
                    byte[] E14 = new byte[] { 0x09 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x30, E14);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void flameBulletCheat_CheckedChanged(object sender)
        {
            if (SPCheats.found)
            {
                if (flameBulletCheat.Checked)
                {
                    byte[] E14 = new byte[] { 0x04 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x34, E14);
                }
                else
                {
                    byte[] E14 = new byte[] { 0x09 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x34, E14);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void carSlideCheat_CheckedChanged(object sender)
        {
            if (SPCheats.found)
            {
                if (carSlideCheat.Checked)
                {
                    byte[] E14 = new byte[] { 0x04 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x8, E14);
                }
                else
                {
                    byte[] E14 = new byte[] { 0x09 };
                    PS3.SetMemory(SPCheats.CheatOffset + 0x8, E14);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clientMaxUpgrade_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 11, 3);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 12, 2);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 15, 3);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 13, 2);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 18, 1);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 16, 4);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_colours(VehID, 120, 120);
            }
        }

        private void paintSelectedColour_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                if (pearlColor.SelectedIndex == -1 && wheelColor.SelectedIndex == -1)
                {
                    Functions.set_vehicle_mod_kit(VehID, 0);
                    RPC.Call(Addresses.Natives.clear_vehicle_custom_primary_colour, VehID);
                    RPC.Call(Addresses.Natives.clear_vehicle_custom_secondary_colour, VehID);
                    Functions.set_vehicle_colours(VehID, carColours.SelectedIndex, carColours2.SelectedIndex);
                }
                else if (pearlColor.SelectedIndex != -1 && wheelColor.SelectedIndex != -1)
                {
                    Functions.set_vehicle_mod_kit(VehID, 0);
                    RPC.Call(Addresses.Natives.clear_vehicle_custom_primary_colour, VehID);
                    RPC.Call(Addresses.Natives.clear_vehicle_custom_secondary_colour, VehID);
                    Functions.set_vehicle_colours(VehID, carColours.SelectedIndex, carColours2.SelectedIndex);
                    Functions.set_vehicle_extra_colours(VehID, pearlColor.SelectedIndex, wheelColor.SelectedIndex);
                }
                else if (pearlColor.SelectedIndex == -1 && wheelColor.SelectedIndex != -1)
                {
                    MessageBox.Show("To change wheel color, you must also select a pearlescent", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (pearlColor.SelectedIndex != -1 && wheelColor.SelectedIndex == -1)
                {
                    MessageBox.Show("To change pearlescent color, you must also select a wheel color", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clientDowngrade_Click(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (Functions.is_ped_in_any_vehicle(PedID) == 1)
            {
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 11, -1);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 12, -1);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 15, -1);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 13, -1);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 18, 0);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_mod(VehID, 16, -1);
                Functions.set_vehicle_mod_kit(VehID, 0);
                Functions.set_vehicle_custom_primary_colour(VehID, 255, 20, 147);
                Functions.set_vehicle_custom_secondary_colour(VehID, 255, 20, 147);
            }
        }

        private void weaponOffsetFind_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please Wait While The Tool Searches...", "Please Wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (Weapons.find())
            {
                MessageBox.Show("Offset Found", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Offset Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void weaponProjType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Weapons.found)
            {
                if (weaponProjType.Text == "Normal")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponProjType.Text == "Rocket")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.RpgOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Stinger")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.StingerOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Grenade")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.GrenadeOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Sticky Bomb")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.StickyOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Smoke Grenade")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.SgrenadeOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "BZGas")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.BzgasOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Molotov")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.MolotovOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Ball")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.BallOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Flare")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.FlareOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Green Laser")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.PlaserOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Tank Rounds")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.TankOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Grenade Launcher")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.GlauncherOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
                else if (weaponProjType.Text == "Smoke Grenade Launcher")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x04 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                    byte[] A1 = PS3.GetBytes(Weapons.SglauncherOffset + 0x38, 4);
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x38, A1);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void weaponSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Weapons.found)
            {
                if (weaponSelect.Text == "WEAPON_KNIFE") { Weapons.WeaponsOffsetUsed = Weapons.KnifeOffset; }
                else if (weaponSelect.Text == "WEAPON_NIGHTSTICK") { Weapons.WeaponsOffsetUsed = Weapons.NightOffset; }
                else if (weaponSelect.Text == "WEAPON_HAMMER") { Weapons.WeaponsOffsetUsed = Weapons.HammerOffset; }
                else if (weaponSelect.Text == "WEAPON_BAT") { Weapons.WeaponsOffsetUsed = Weapons.BatOffset; }
                else if (weaponSelect.Text == "WEAPON_GOLFCLUB") { Weapons.WeaponsOffsetUsed = Weapons.GolfOffset; }
                else if (weaponSelect.Text == "WEAPON_CROWBAR") { Weapons.WeaponsOffsetUsed = Weapons.CrowbarOffset; }
                else if (weaponSelect.Text == "WEAPON_PISTOL") { Weapons.WeaponsOffsetUsed = Weapons.PistolOffset; }
                else if (weaponSelect.Text == "WEAPON_COMBATPISTOL") { Weapons.WeaponsOffsetUsed = Weapons.Pistol50Offset; }
                else if (weaponSelect.Text == "WEAPON_APPISTOL") { Weapons.WeaponsOffsetUsed = Weapons.AppistolOffset; }
                else if (weaponSelect.Text == "WEAPON_PISTOL50") { Weapons.WeaponsOffsetUsed = Weapons.Pistol50Offset + 0x2880; }
                else if (weaponSelect.Text == "WEAPON_MICROSMG") { Weapons.WeaponsOffsetUsed = Weapons.MsmgOffset; }
                else if (weaponSelect.Text == "WEAPON_SMG") { Weapons.WeaponsOffsetUsed = Weapons.SmgOffset; }
                else if (weaponSelect.Text == "WEAPON_ASSAULTSMG") { Weapons.WeaponsOffsetUsed = Weapons.AsmgOffset; }
                else if (weaponSelect.Text == "WEAPON_ASSAULTRIFLE") { Weapons.WeaponsOffsetUsed = Weapons.ArifleOffset; }
                else if (weaponSelect.Text == "WEAPON_CARBINERIFLE") { Weapons.WeaponsOffsetUsed = Weapons.CrifleOffset; }
                else if (weaponSelect.Text == "WEAPON_ADVANCEDRIFLE") { Weapons.WeaponsOffsetUsed = Weapons.AdrifleOffset; }
                else if (weaponSelect.Text == "WEAPON_MG") { Weapons.WeaponsOffsetUsed = Weapons.MgOffset; }
                else if (weaponSelect.Text == "WEAPON_COMBATMG") { Weapons.WeaponsOffsetUsed = Weapons.CmgOffset; }
                else if (weaponSelect.Text == "WEAPON_PUMPSHOTGUN") { Weapons.WeaponsOffsetUsed = Weapons.PshotOffset; }
                else if (weaponSelect.Text == "WEAPON_SAWNOFFSHOTGUN") { Weapons.WeaponsOffsetUsed = Weapons.SshotOffset; }
                else if (weaponSelect.Text == "WEAPON_ASSAULTSHOTGUN") { Weapons.WeaponsOffsetUsed = Weapons.AshotOffset; }
                else if (weaponSelect.Text == "WEAPON_BULLPUPSHOTGUN") { Weapons.WeaponsOffsetUsed = Weapons.BshotOffset; }
                else if (weaponSelect.Text == "WEAPON_STUNGUN") { Weapons.WeaponsOffsetUsed = Weapons.StunOffset; }
                else if (weaponSelect.Text == "WEAPON_SNIPERRIFLE") { Weapons.WeaponsOffsetUsed = Weapons.SniperOffset; }
                else if (weaponSelect.Text == "WEAPON_HEAVYSNIPER") { Weapons.WeaponsOffsetUsed = Weapons.HsniperOffset; }
                else if (weaponSelect.Text == "WEAPON_REMOTESNIPER") { Weapons.WeaponsOffsetUsed = Weapons.RsniperOffset; }
                else if (weaponSelect.Text == "WEAPON_GRENADELAUNCHER") { Weapons.WeaponsOffsetUsed = Weapons.GlauncherOffset; }
                else if (weaponSelect.Text == "WEAPON_GRENADELAUNCHER_SMOKE") { Weapons.WeaponsOffsetUsed = Weapons.SglauncherOffset; }
                else if (weaponSelect.Text == "WEAPON_RPG") { Weapons.WeaponsOffsetUsed = Weapons.RpgOffset; }
                else if (weaponSelect.Text == "WEAPON_PASSENGER_ROCKET") { Weapons.WeaponsOffsetUsed = Weapons.ProcketOffset; }
                else if (weaponSelect.Text == "WEAPON_AIRSTRIKE_ROCKET") { Weapons.WeaponsOffsetUsed = Weapons.ArocketOffset; }
                else if (weaponSelect.Text == "WEAPON_STINGER") { Weapons.WeaponsOffsetUsed = Weapons.StingerOffset; }
                else if (weaponSelect.Text == "WEAPON_MINIGUN") { Weapons.WeaponsOffsetUsed = Weapons.MinigunOffset; }
                else if (weaponSelect.Text == "WEAPON_GRENADE") { Weapons.WeaponsOffsetUsed = Weapons.GrenadeOffset; }
                else if (weaponSelect.Text == "WEAPON_STICKYBOMB") { Weapons.WeaponsOffsetUsed = Weapons.StickyOffset; }
                else if (weaponSelect.Text == "WEAPON_SMOKEGRENADE") { Weapons.WeaponsOffsetUsed = Weapons.SgrenadeOffset; }
                else if (weaponSelect.Text == "WEAPON_BZGAS") { Weapons.WeaponsOffsetUsed = Weapons.BzgasOffset; }
                else if (weaponSelect.Text == "WEAPON_MOLOTOV") { Weapons.WeaponsOffsetUsed = Weapons.MolotovOffset; }
                else if (weaponSelect.Text == "WEAPON_FIREEXTINGUISHER") { Weapons.WeaponsOffsetUsed = Weapons.FireextOffset; }
                else if (weaponSelect.Text == "WEAPON_PETROLCAN") { Weapons.WeaponsOffsetUsed = Weapons.PetrolOffset; }
                else if (weaponSelect.Text == "WEAPON_DIGISCANNER") { Weapons.WeaponsOffsetUsed = Weapons.DigiOffset; }
                else if (weaponSelect.Text == "WEAPON_BRIEFCASE") { Weapons.WeaponsOffsetUsed = Weapons.BriefOffset; }
                else if (weaponSelect.Text == "WEAPON_BRIEFCASE_02") { Weapons.WeaponsOffsetUsed = Weapons.Brief2Offset; }
                else if (weaponSelect.Text == "WEAPON_BALL") { Weapons.WeaponsOffsetUsed = Weapons.BallOffset; }
                else if (weaponSelect.Text == "WEAPON_FLARE") { Weapons.WeaponsOffsetUsed = Weapons.FlareOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_TANK") { Weapons.WeaponsOffsetUsed = Weapons.TankOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_SPACE_ROCKET") { Weapons.WeaponsOffsetUsed = Weapons.SpaceOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_PLANE_ROCKET") { Weapons.WeaponsOffsetUsed = Weapons.PlaneOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_PLAYER_LASER") { Weapons.WeaponsOffsetUsed = Weapons.PlaserOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_PLAYER_BULLET") { Weapons.WeaponsOffsetUsed = Weapons.PbulletOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_PLAYER_BUZZARD") { Weapons.WeaponsOffsetUsed = Weapons.PbuzzardOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_PLAYER_HUNTER") { Weapons.WeaponsOffsetUsed = Weapons.PhunterOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_PLAYER_LAZER") { Weapons.WeaponsOffsetUsed = Weapons.PlazerOffset; }
                else if (weaponSelect.Text == "VEHICLE_WEAPON_ENEMY_LASER") { Weapons.WeaponsOffsetUsed = Weapons.ElaserOffset; }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void autoUpdateAddresses_Click(object sender, EventArgs e)
        {
            if (connectLabel.Text != "Connected" || attachLabel.Text != "Attached")
            {
                MessageBox.Show("Connect & Attach Before Trying To Update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                ToolUpdate ToolUpdate = new ToolUpdate();
                MessageBox.Show("Wait while the tool updates RPC, native addresses, and RTM offsets.\nIt will take a little while...", "Please Wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ToolUpdate.Show();

                if (blesSelect.Checked)
                {
                    Updater.UpdateRPC(Directory.GetCurrentDirectory() + @"\Resources\BLES-RPC.txt");
                    ToolUpdate.dumpLabel.Text = Updater.result;
                    ToolUpdate.dumpLabel.ForeColor = Updater.resColor;
                    ToolUpdate.dumpLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.rpcLabel.Text = "Working . . .";
                    ToolUpdate.rpcLabel.Text = Updater.result;
                    ToolUpdate.rpcLabel.ForeColor = Updater.resColor;
                    ToolUpdate.rpcLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.rtmLabel.Text = "Working . . .";
                    Updater.UpdateRTM(Directory.GetCurrentDirectory() + @"\Resources\BLES-RTM.txt");
                    ToolUpdate.rtmLabel.Text = Updater.result;
                    ToolUpdate.rtmLabel.ForeColor = Updater.resColor;
                    ToolUpdate.rtmLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.nativeLabel.Text = "Working . . .";
                    Updater.UpdateNatives(Directory.GetCurrentDirectory() + @"\Resources\BLES-Natives.txt");
                    ToolUpdate.nativeLabel.Text = Updater.result;
                    ToolUpdate.nativeLabel.ForeColor = Updater.resColor;
                    ToolUpdate.nativeLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    Updater.result = "Complete!";
                    Updater.resColor = System.Drawing.Color.Green;
                    this.updateLabel.Value1 = "Saved Update:  " + Updater.rpcSaved;
                    MessageBox.Show("To Load New Update, Restart The Tool", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (blusSelect.Checked)
                {
                    Updater.UpdateRPC(Directory.GetCurrentDirectory() + @"\Resources\BLUS-RPC.txt");
                    ToolUpdate.dumpLabel.Text = Updater.result;
                    ToolUpdate.dumpLabel.ForeColor = Updater.resColor;
                    ToolUpdate.dumpLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.rpcLabel.Text = "Working . . .";
                    ToolUpdate.rpcLabel.Text = Updater.result;
                    ToolUpdate.rpcLabel.ForeColor = Updater.resColor;
                    ToolUpdate.rpcLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.rtmLabel.Text = "Working . . .";
                    Updater.UpdateRTM(Directory.GetCurrentDirectory() + @"\Resources\BLUS-RTM.txt");
                    ToolUpdate.rtmLabel.Text = Updater.result;
                    ToolUpdate.rtmLabel.ForeColor = Updater.resColor;
                    ToolUpdate.rtmLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.nativeLabel.Text = "Working . . .";
                    Updater.UpdateNatives(Directory.GetCurrentDirectory() + @"\Resources\BLUS-Natives.txt");
                    ToolUpdate.nativeLabel.Text = Updater.result;
                    ToolUpdate.nativeLabel.ForeColor = Updater.resColor;
                    ToolUpdate.nativeLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    Updater.result = "Complete!";
                    Updater.resColor = System.Drawing.Color.Green;
                    this.updateLabel.Value1 = "Saved Update:  " + Updater.rpcSaved;
                    MessageBox.Show("To Load New Update, Restart The Tool", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (bljmSelect.Checked)
                {
                    Updater.UpdateRPC(Directory.GetCurrentDirectory() + @"\Resources\BLJM-RPC.txt");
                    ToolUpdate.dumpLabel.Text = Updater.result;
                    ToolUpdate.dumpLabel.ForeColor = Updater.resColor;
                    ToolUpdate.dumpLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.rpcLabel.Text = "Working . . .";
                    ToolUpdate.rpcLabel.Text = Updater.result;
                    ToolUpdate.rpcLabel.ForeColor = Updater.resColor;
                    ToolUpdate.rpcLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.rtmLabel.Text = "Working . . .";
                    Updater.UpdateRTM(Directory.GetCurrentDirectory() + @"\Resources\BLJM-RTM.txt");
                    ToolUpdate.rtmLabel.Text = Updater.result;
                    ToolUpdate.rtmLabel.ForeColor = Updater.resColor;
                    ToolUpdate.rtmLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    ToolUpdate.nativeLabel.Text = "Working . . .";
                    Updater.UpdateNatives(Directory.GetCurrentDirectory() + @"\Resources\BLJM-Natives.txt");
                    ToolUpdate.nativeLabel.Text = Updater.result;
                    ToolUpdate.nativeLabel.ForeColor = Updater.resColor;
                    ToolUpdate.nativeLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    Updater.result = "Complete!";
                    Updater.resColor = System.Drawing.Color.Green;
                    this.updateLabel.Value1 = "Saved Update:  " + Updater.rpcSaved;
                    MessageBox.Show("To Load New Update, Restart The Tool", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void weaponImpact_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Weapons.found)
            {
                if (weaponImpact.Text == "Normal")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponImpact.Text == "Water")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0x00, 0x00, 0x00, 0x0D };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponImpact.Text == "Flame")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0x00, 0x00, 0x00, 0x0C };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponImpact.Text == "Explosion")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0x00, 0x00, 0x00, 0x10 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponImpact.Text == "Smoke")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0x00, 0x00, 0x00, 0x13 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponImpact.Text == "BZGas")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0x00, 0x00, 0x00, 0x14 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponImpact.Text == "Flare")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0x00, 0x00, 0x00, 0x16 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
                else if (weaponImpact.Text == "Molotov")
                {
                    byte[] S14 = new byte[] { 0x05 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x13, S14);
                    byte[] S15 = new byte[] { 0x00, 0x00, 0x00, 0x03 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x14, S15);
                    byte[] S16 = new byte[] { 0x02 };
                    PS3.SetMemory(Weapons.WeaponsOffsetUsed + 0x2F, S16);
                }
            }
            else
            {
                MessageBox.Show("Find Offset First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpcLastVehTP_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            int Player = Functions.player_id();
            int VehID = Functions.get_players_last_vehicle(Player);
            Functions.set_ped_into_vehicle(PedID, VehID, -1);
        }

        private void rpcLastVehTPToMe_Click(object sender, EventArgs e)
        {
            int PedID = Functions.player_ped_id();
            float[] coords = Functions.get_entity_coords(PedID);
            int Player = Functions.player_id();
            int VehID = Functions.get_players_last_vehicle(Player);
            Functions.network_request_control_of_entity(VehID);
            Functions.set_entity_coords(VehID, coords);
        }

        private void rpcVisionClear_Click(object sender, EventArgs e)
        {
            Functions.clear_timecycle_modifier();
        }

        private void rpcVisionSet_Click(object sender, EventArgs e)
        {
            Functions.set_timecycle_modifier(rpcVisionSelect.Text);
        }

        private void rpcVehCollision_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();
            int VehID = Functions.get_vehicle_ped_is_in(PedID);
            if (rpcVehCollision.Checked)
            {
                Functions.set_entity_collision(VehID, 1);
            }
            else
            {
                Functions.set_entity_collision(VehID, 0);
            }
        }

        private void rpcForceRagdoll_Click(object sender, EventArgs e)
        {
            Functions.set_ped_to_ragdoll(Functions.player_ped_id(), 1000, 1000, 0);
        }

        private void rpcEarthquakeShake_CheckedChanged(object sender)
        {
            if (rpcEarthquakeShake.Checked)
            {
                Functions.shake_gameplay_cam("ROAD_VIBRATION_SHAKE", 10f);
            }
            else
            {
                Functions.shake_gameplay_cam("ROAD_VIBRATION_SHAKE", 0f);
            }
        }

        private void rpcDrugShake_CheckedChanged(object sender)
        {
            if (rpcDrugShake.Checked)
            {
                Functions.shake_gameplay_cam("FAMILY5_DRUG_TRIP_SHAKE", 2f);
            }
            else
            {
                Functions.shake_gameplay_cam("FAMILY5_DRUG_TRIP_SHAKE", 0f);
            }
        }

        private void rpcGiveRandProps_Click(object sender, EventArgs e)
        {
            Functions.set_ped_random_props(Functions.player_ped_id());
        }

        private void rpcClearProps_Click(object sender, EventArgs e)
        {
            Functions.clear_all_ped_props(Functions.player_ped_id());
        }

        private void nsButton5_Click_1(object sender, EventArgs e)
        {
            Functions.set_ped_prop_index(Functions.player_ped_id(), (int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value);
        }

        private void rpcFallOffBike_CheckedChanged(object sender)
        {
            int PedID = Functions.player_ped_id();
            if (rpcFallOffBike.Checked)
            {
                Functions.set_ped_can_be_knocked_off_vehicle(PedID, 0);
            }
            else
            {
                Functions.set_ped_can_be_knocked_off_vehicle(PedID, 0);
                Functions.set_ped_can_be_knocked_off_vehicle(PedID, 1);
                Functions.set_ped_can_be_knocked_off_vehicle(PedID, 4);
            }
        }

        private void rpcSetHeadlight_Click(object sender, EventArgs e)
        {
            int VehID = Functions.get_vehicle_ped_is_in(Functions.player_ped_id());
            Functions.set_vehicle_light_multiplier(VehID, (float)rpcHeadlightMultiplier.Value);
        }

        private void rpcHeadlightMultiplier_Scroll(object sender)
        {
            rpcHeadlightLabel.Value1 = "Multiplier = " + rpcHeadlightMultiplier.Value.ToString();
        }

        private void hookRadio_CheckedChanged(object sender)
        {
            Hook.Call(Addresses.Natives.set_mobile_radio_enabled_during_gameplay, Convert.ToInt32(hookRadio.Checked));
        }

        private void hookModelSet_Click(object sender, EventArgs e)
        {
            int player = Hook.Call(Addresses.Natives.player_id);
            uint model = Functions.GetHash(hookModelList.Text);
            Hook.Call(Addresses.Natives.request_model, model);
            for (; ;)
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, model) == 1)
                    break;
            }
            Hook.Call(Addresses.Natives.set_player_model, player, model);
            Hook.Call(Addresses.Natives.set_model_as_no_longer_needed, model);
        }

        private void hookRandomVariation_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            Hook.Call(Addresses.Natives.set_ped_random_component_variation, PedID, 0);
        }

        private void hookStopMoving_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            Hook.Call(Addresses.Natives.clear_ped_tasks_immediately, PedID);
        }

        private void hookStartScenario_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            Hook.Call(Addresses.Natives.task_start_scenario_in_place, PedID, hookScenarioList.Text, 0, 1);
        }

        private void hookStartFX_Click(object sender, EventArgs e)
        {
            Hook.Call(Addresses.Natives.unk_0x1D980479, hookFXList.Text, 0, 0);
        }

        private void nsButton7_Click(object sender, EventArgs e)
        {
            Hook.Call(Addresses.Natives.unk_0x6BB5CDA, hookFXList.Text);
        }

        private void hookSpawnVeh_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            uint hash = Functions.GetHash(hookVehList.Text);
            Hook.Call(Addresses.Natives.request_model, hash);
            for (; ; )
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, hash) == 1)
                    break;
            }
            float[] coords = new float[3];
            Hook.Call(Addresses.Natives.get_entity_coords, Addresses.Other.entityXCoord, PedID);
            coords[0] = PS3.Extension.ReadFloat(Addresses.Other.entityXCoord);
            coords[1] = PS3.Extension.ReadFloat(Addresses.Other.entityYCoord);
            coords[2] = PS3.Extension.ReadFloat(Addresses.Other.entityZCoord);
            int veh = Hook.Call(Addresses.Natives.create_vehicle, hash, coords, 0, 1, 0);
            Hook.Call(Addresses.Natives.set_ped_into_vehicle, PedID, veh, -1);
        }

        private void hookStartCutscene_Click(object sender, EventArgs e)
        {
            Hook.Call(Addresses.Natives.request_cutscene, hookCutsceneList.Text, 8);
            for(; ; )
            {
                if (Hook.Call(Addresses.Natives.has_cutscene_loaded) == 1)
                    break;
            }
            Hook.Call(Addresses.Natives.start_cutscene, hookCutsceneList.Text);
        }

        private void hookStopCutscene_Click(object sender, EventArgs e)
        {
            Hook.Call(Addresses.Natives.stop_cutscene_immediately);
        }

        int cam;

       /* private void hookFirstPersonCam_CheckedChanged(object sender)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            float[] coords = { 0.0f, 0.15f, 0.02f };

            if (hookFirstPersonCam.Checked)
            {
                cam = Hook.Call(Addresses.Natives.create_cam, "DEFAULT_SCRIPTED_CAMERA", 1);
                Hook.Call(Addresses.Natives.attach_cam_to_ped_bone, cam, PedID, 31086, coords, 1);
                Hook.Call(Addresses.Natives.set_cam_active, cam, 1);
                Hook.Call(Addresses.Natives.render_script_cams, 1, 0, 3000, 1, 0);
            }
            else
            {
                Hook.Call(Addresses.Natives.set_cam_active, cam, 0);
                Hook.Call(Addresses.Natives.render_script_cams, 0, 0, 3000, 1, 0);
                Hook.Call(Addresses.Natives.destroy_cam, cam, 0);
            }
        } */

        private void hookSpawnPed_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            uint hash = Functions.GetHash(hookPedList.Text);
            Hook.Call(Addresses.Natives.request_model, hash);
            for (; ; )
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, hash) == 1)
                    break;
            }
            float[] coords = new float[3];
            Hook.Call(Addresses.Natives.get_entity_coords, Addresses.Other.entityXCoord, PedID);
            coords[0] = PS3.Extension.ReadFloat(Addresses.Other.entityXCoord);
            coords[1] = PS3.Extension.ReadFloat(Addresses.Other.entityYCoord);
            coords[2] = PS3.Extension.ReadFloat(Addresses.Other.entityZCoord);
            int ped = Hook.Call(Addresses.Natives.create_ped, 26, hash, coords, 0, 1, 0);
            Hook.Call(Addresses.Natives.set_model_as_no_longer_needed, hash);
            hookSpawnedPeds.Items.Add(hookPedList.Text + " = " + ped.ToString());
        }

        private void hookToggle40KLoop_CheckedChanged(object sender)
        {
            int PedID = Hook.Call(Addresses.Natives.get_player_ped, Clients.SelectedIndex);
            uint hash = 0x113FD533;
            Hook.Call(Addresses.Natives.request_model, hash);
            for (; ; )
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, hash) == 1)
                    break;
            }

            PS3.Extension.WriteUInt32(0x10010000, Addresses.Natives.get_entity_coords);
            PS3.Extension.WriteInt32(0x10010004, PedID);
            PS3.Extension.WriteUInt32(0x10010008, 0x10031000);
            Hook.LoopToggle(hookToggle40KLoop.Checked);
            Hook.Loop(Addresses.Natives.create_ambient_pickup, 0xCE6FDD6B, 0x10031000, 0, 40000, 0x113FD533, 0, 1);
        }

        private void enableScriptHook_Click(object sender, EventArgs e)
        {
            if (rpcLabel.Text != "RPC Enabled")
            {
                if (!Hook.IsEnable())
                {
                    Hook.Enable(Addresses.Natives.is_player_online);
                    rpcLabel.Text = "RPC Enabled";
                    rpcLabel.ForeColor = System.Drawing.Color.Green;

                    int id = Functions.player_id();
                    string name = get_player_name(id).ToString();
                    colorTimer.Start();
                    welcomeLabel.Value1 = "Welcome:";
                    nameLabel.Text = name;

                    MessageBox.Show("Script Hook Enabled", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Already Hooked", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (PS3.Extension.ReadUInt32(RPC.rpcEnableOffset) == RPC.rpcEnableValue)
            {
                MessageBox.Show("Old RPC Already Enabled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void hookBlackout_CheckedChanged(object sender)
        {
            Hook.Call(Addresses.Natives.unk_0xAA2A0EAF, Convert.ToInt32(hookBlackout.Checked));
        }

        private void hookHideRadar_CheckedChanged(object sender)
        {
            Hook.Call(Addresses.Natives.display_radar, Convert.ToInt32(!hookHideRadar.Checked));
        }

        private void hookMakePedGuard_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            int Me = Hook.Call(Addresses.Natives.player_ped_id);

            int group = Hook.Call(Addresses.Natives.get_player_group, Me);
            Hook.Call(Addresses.Natives.set_ped_as_group_leader, Me, group);
            Hook.Call(Addresses.Natives.set_ped_as_group_member, id, group);
            Hook.Call(Addresses.Natives.set_ped_never_leaves_group, id, group);
            Hook.Call(Addresses.Natives.set_ped_combat_ability, id, 100);
            Hook.Call(Addresses.Natives.set_ped_can_switch_weapon, id, 1);
            int blip = Hook.Call(Addresses.Natives.add_blip_for_entity, id);
            Hook.Call(Addresses.Natives.set_blip_as_friendly, blip);
            Hook.Call(Addresses.Natives.set_blip_scale, blip, 1.0f);
            Hook.Call(Addresses.Natives.set_blip_colour, blip, 2);
        }

        private void hookPortMeToPed_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            int Me = Hook.Call(Addresses.Natives.player_ped_id);

            float[] coords = new float[3];
            Hook.Call(Addresses.Natives.get_entity_coords, Addresses.Other.entityXCoord, id);
            coords[0] = PS3.Extension.ReadFloat(Addresses.Other.entityXCoord);
            coords[1] = PS3.Extension.ReadFloat(Addresses.Other.entityYCoord);
            coords[2] = PS3.Extension.ReadFloat(Addresses.Other.entityZCoord);
            Hook.Call(Addresses.Natives.set_entity_coords, Me, coords, 1, 0, 0, 1);
        }

        private void hookPortPedToMe_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            int Me = Hook.Call(Addresses.Natives.player_ped_id);

            float[] coords = new float[3];
            Hook.Call(Addresses.Natives.get_entity_coords, Addresses.Other.entityXCoord, Me);
            coords[0] = PS3.Extension.ReadFloat(Addresses.Other.entityXCoord);
            coords[1] = PS3.Extension.ReadFloat(Addresses.Other.entityYCoord);
            coords[2] = PS3.Extension.ReadFloat(Addresses.Other.entityZCoord);
            Hook.Call(Addresses.Natives.set_entity_coords, id, coords, 1, 0, 0, 1);
        }

        private void hookGivePedWeapon_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            uint hash = Functions.GetHash("WEAPON_" + hookPedWeaponList.Text);
            Hook.Call(Addresses.Natives.give_weapon_to_ped, id, hash, 9999, 0, 1);
        }

        private void hookRemovePedWeapons_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            Hook.Call(Addresses.Natives.remove_all_ped_weapons, id, 1);
        }

        private void hookGodPed_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            Hook.Call(Addresses.Natives.set_entity_invincible, id, 1);
        }

        private void hookKillPed_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            Hook.Call(Addresses.Natives.set_entity_health, id, 0);
            hookSpawnedPeds.Items.RemoveAt(hookSpawnedPeds.SelectedIndex);
        }

        private void hookCombatPlayer_Click(object sender, EventArgs e)
        {
            string[] ped = hookSpawnedPeds.Text.Split('=');
            int id = Convert.ToInt32(ped[1].Trim());
            int player = Hook.Call(Addresses.Natives.get_player_ped, hookCombatPlayerList.SelectedIndex);
            Hook.Call(Addresses.Natives.task_combat_ped, id, player, 0, 0x10);
        }

        private void hookToggleFreeze_CheckedChanged(object sender)
        {
            if (hookToggleFreeze.Checked)
                freezeTimer.Start();
            else
                freezeTimer.Stop();
        }

        private void hookCustomVariation_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            Hook.Call(Addresses.Natives.set_ped_component_variation, PedID, componentID.SelectedIndex, (int)drawableID.Value, (int)textureID.Value, 0);
        }

        private void hook2kLoop_CheckedChanged(object sender)
        {
            int PedID = Hook.Call(Addresses.Natives.get_player_ped, Clients.SelectedIndex);
            PS3.Extension.WriteUInt32(0x10010000, Addresses.Natives.get_entity_coords);
            PS3.Extension.WriteInt32(0x10010004, PedID);
            PS3.Extension.WriteUInt32(0x10010008, 0x10031000);
            Hook.LoopToggle(hook2kLoop.Checked);
            Hook.Loop(Addresses.Natives.create_ambient_pickup, Addresses.Hashes.Pickups.PICKUP_MONEY_VARIABLE, 0x10031000, 0, 2000, 1, 0, 1);
        }

        private void hookAnimalFreezeFix_Click(object sender, EventArgs e)
        {
            PS3.SetMemory(Addresses.RTM.animalFreezeFix1, Addresses.RTM.toggleAnimalFix1);
            PS3.SetMemory(Addresses.RTM.animalFreezeFix2, Addresses.RTM.toggleAnimalFix2);
            PS3.SetMemory(Addresses.RTM.animalFreezeFix3, Addresses.RTM.toggleAnimalFix3);
        }

        private void clientRefreshTalking_Click(object sender, EventArgs e)
        {
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 0) == 1)
                client1.Value1 = get_player_name(0);
            else
                client1.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 1) == 1)
                client2.Value1 = get_player_name(1);
            else
                client2.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 2) == 1)
                client3.Value1 = get_player_name(2);
            else
                client3.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 3) == 1)
                client4.Value1 = get_player_name(3);
            else
                client4.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 4) == 1)
                client5.Value1 = get_player_name(4);
            else
                client5.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 5) == 1)
                client6.Value1 = get_player_name(5);
            else
                client6.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 6) == 1)
                client7.Value1 = get_player_name(6);
            else
                client7.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 7) == 1)
                client8.Value1 = get_player_name(7);
            else
                client8.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 8) == 1)
                client9.Value1 = get_player_name(8);
            else
                client9.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 9) == 1)
                client10.Value1 = get_player_name(9);
            else
                client10.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 10) == 1)
                client11.Value1 = get_player_name(10);
            else
                client11.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 11) == 1)
                client12.Value1 = get_player_name(11);
            else
                client12.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 12) == 1)
                client13.Value1 = get_player_name(12);
            else
                client13.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 13) == 1)
                client14.Value1 = get_player_name(13);
            else
                client14.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 14) == 1)
                client15.Value1 = get_player_name(14);
            else
                client15.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 15) == 1)
                client16.Value1 = get_player_name(15);
            else
                client16.Value1 = "";
        }

        private void clientTalkAuto_CheckedChanged(object sender)
        {
            if (clientTalkAuto.Checked)
            {
                clientTalkingAutoRefresh.Start();
            }
            else
            {
                clientTalkingAutoRefresh.Stop();
            }
        }

        private void clientTalkingAutoRefresh_Tick(object sender, EventArgs e)
        {
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 0) == 1)
                client1.Value1 = get_player_name(0);
            else
                client1.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 1) == 1)
                client2.Value1 = get_player_name(1);
            else
                client2.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 2) == 1)
                client3.Value1 = get_player_name(2);
            else
                client3.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 3) == 1)
                client4.Value1 = get_player_name(3);
            else
                client4.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 4) == 1)
                client5.Value1 = get_player_name(4);
            else
                client5.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 5) == 1)
                client6.Value1 = get_player_name(5);
            else
                client6.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 6) == 1)
                client7.Value1 = get_player_name(6);
            else
                client7.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 7) == 1)
                client8.Value1 = get_player_name(7);
            else
                client8.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 8) == 1)
                client9.Value1 = get_player_name(8);
            else
                client9.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 9) == 1)
                client10.Value1 = get_player_name(9);
            else
                client10.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 10) == 1)
                client11.Value1 = get_player_name(10);
            else
                client11.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 11) == 1)
                client12.Value1 = get_player_name(11);
            else
                client12.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 12) == 1)
                client13.Value1 = get_player_name(12);
            else
                client13.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 13) == 1)
                client14.Value1 = get_player_name(13);
            else
                client14.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 14) == 1)
                client15.Value1 = get_player_name(14);
            else
                client15.Value1 = "";
            if (RPC.Call(Addresses.Natives.network_is_player_talking, 15) == 1)
                client16.Value1 = get_player_name(15);
            else
                client16.Value1 = "";
        }

        private void randomThunder_Click(object sender, EventArgs e)
        {
            Hook.Call(Addresses.Natives.unk_0xDF38165E);
        }

        private void toggleOneHitKill_CheckedChanged(object sender)
        {
            int Player = Functions.player_id();

            if (toggleOneHitKill.Checked)
                Functions.set_player_weapon_damage_modifier(Player, 700f);
            else
                Functions.set_player_weapon_damage_modifier(Player, 1f);
        }

   /*     private void toggleForcefield_CheckedChanged(object sender)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            Hook.Call(Addresses.Natives.set_entity_invincible, PedID, Convert.ToInt32(toggleForcefield.Checked));
            Thread.Sleep(1000);

            if (toggleForcefield.Checked)
                forcefield.Start();
            else
                forcefield.Stop();
        } */

        private void forcefield_Tick(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            float[] coords = Functions.get_entity_coords(PedID);
            Hook.Call(Addresses.Natives.add_explosion, coords, 26, float.MaxValue, 0, 1, 0.0f);
        }

        private void unlockHeistCars_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CHAR_FM_VEHICLE_1_UNLCK"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CHAR_FM_VEHICLE_2_UNLCK"), -1);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CHAR_FM_VEHICLE_1_UNLCK"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CHAR_FM_VEHICLE_2_UNLCK"), -1);
            }
        }

        private void unlockTattoos_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_DM_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_TDM_MVP"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FM_DM_TOTALKILLS"), 500);
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "AWD_FMATTGANGHQ"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMBBETWIN"), 50000);
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "AWD_FMWINEVERYGAMEMODE"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "AWD_FMRACEWORLDRECHOLDER"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "AWD_FMFULLYMODDEDCAR"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "AWD_FMMOSTKILLSSURVIVE"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "AWD_FMKILL3ANDWINGTARACE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMKILLBOUNTY"), 25);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_FMREVENGEKILLSDM"), 50);
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "AWD_FMKILLSTREAKSDM"), 1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_HOLD_UP_SHOPS"), 20);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_LAPDANCES"), 25);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_SECURITY_CARS_ROBBED"), 25);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_RACES_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "AWD_CAR_BOMBS_ENEMY_KILLS"), 25);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "PLAYER_HEADSHOTS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DB_PLAYER_KILLS"), 1000);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_DM_WINS"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_TDM_MVP"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FM_DM_TOTALKILLS"), 500);
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "AWD_FMATTGANGHQ"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMBBETWIN"), 50000);
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "AWD_FMWINEVERYGAMEMODE"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "AWD_FMRACEWORLDRECHOLDER"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "AWD_FMFULLYMODDEDCAR"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "AWD_FMMOSTKILLSSURVIVE"), 1);
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "AWD_FMKILL3ANDWINGTARACE"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMKILLBOUNTY"), 25);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_FMREVENGEKILLSDM"), 50);
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "AWD_FMKILLSTREAKSDM"), 1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_HOLD_UP_SHOPS"), 20);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_LAPDANCES"), 25);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_SECURITY_CARS_ROBBED"), 25);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_RACES_WON"), 50);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "AWD_CAR_BOMBS_ENEMY_KILLS"), 25);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "PLAYER_HEADSHOTS"), 500);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DB_PLAYER_KILLS"), 1000);
            }
        }

        private void remakePlayer_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_bool(Functions.GetHash("MP0_" + "FM_CHANGECHAR_ASKED"), 0);
            }
            else
            {
                Functions.stat_set_bool(Functions.GetHash("MP1_" + "FM_CHANGECHAR_ASKED"), 0);
            }
        }

        private void unlockAllHair_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_7"), -1);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_7"), -1);
            }
        }

        private void unlockClothes_Click(object sender, EventArgs e)
        {
            if (rpcStatPrePrimary.Checked)
            {
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_HAIR_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_OUTFIT"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_OUTFIT"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_JBIB_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_JBIB"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_JBIB_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_JBIB_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_JBIB_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_JBIB_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_JBIB_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_JBIB_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_LEGS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_LEGS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_FEET_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_FEET_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_8"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_9"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_PROPS_10"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_TEETH"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_TEETH_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_TEETH_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_TEETH"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_TEETH_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_TEETH_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_BERD_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_BERD_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_TORSO"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_TORSO"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_SPECIAL2_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_SPECIAL2_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_AVAILABLE_DECL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "CLTHS_ACQUIRED_DECL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_0"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_8"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_9"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_10"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_11"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_12"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_13"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_14"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_15"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_16"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_17"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_18"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_19"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_20"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_21"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_22"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_23"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_24"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_25"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_26"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_27"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_28"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_29"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_30"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_31"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_32"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_33"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_34"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_35"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_36"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_37"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_38"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_39"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "DLC_APPAREL_ACQUIRED_40"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_8"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_9"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_10"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_11"), -1);
                Functions.stat_set_int(Functions.GetHash("MP0_" + "ADMIN_CLOTHES_GV_BS_12"), -1);
            }
            else
            {
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_HAIR_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_OUTFIT"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_OUTFIT"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_JBIB_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_JBIB"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_JBIB_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_JBIB_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_JBIB_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_JBIB_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_JBIB_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_JBIB_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_LEGS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_LEGS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_FEET_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_FEET_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_8"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_9"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_PROPS_10"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_TEETH"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_TEETH_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_TEETH_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_TEETH"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_TEETH_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_TEETH_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_BERD_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_BERD_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_TORSO"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_TORSO"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_SPECIAL2_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_SPECIAL2_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_AVAILABLE_DECL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "CLTHS_ACQUIRED_DECL"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_0"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_8"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_9"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_10"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_11"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_12"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_13"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_14"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_15"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_16"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_17"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_18"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_19"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_20"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_21"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_22"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_23"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_24"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_25"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_26"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_27"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_28"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_29"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_30"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_31"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_32"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_33"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_34"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_35"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_36"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_37"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_38"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_39"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "DLC_APPAREL_ACQUIRED_40"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_1"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_2"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_3"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_4"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_5"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_6"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_7"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_8"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_9"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_10"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_11"), -1);
                Functions.stat_set_int(Functions.GetHash("MP1_" + "ADMIN_CLOTHES_GV_BS_12"), -1);
            }
        }

        private void phoneCar_Click(object sender, EventArgs e)
        {
            Functions.stat_set_int(Functions.GetHash("MPPLY_VEHICLE_ID_ADMIN_WEB"), 117401876);
        }

        private void blipSprite_SelectedIndexChanged(object sender, EventArgs e)
        {
            int blip = Hook.Call(Addresses.Natives.get_main_player_blip_id);
            string sprite = Regex.Match(blipSprite.Text, @"\(([^)]*)\)").Groups[1].Value;
            Hook.Call(Addresses.Natives.set_blip_sprite, blip, int.Parse(sprite));
        }

        private void blipScale_Scroll(object sender)
        {
            int blip = Hook.Call(Addresses.Natives.get_main_player_blip_id);
            scaleLabel.Value1 = "(" + blipScale.Value.ToString() + ")";
            Hook.Call(Addresses.Natives.set_blip_scale, blip, (float)blipScale.Value);
        }

        private void freezeTimer_Tick(object sender, EventArgs e)
        {
            int PedID = Functions.get_player_ped(Clients.SelectedIndex);
            Functions.clear_ped_tasks_immediately(PedID);
        }

        private void playAmbSpeech_Click(object sender, EventArgs e)
        {
            int ped = Functions.player_ped_id();

            if (ambSpeechName.SelectedIndex == -1 || ambSpeechParam.SelectedIndex == -1)
                MessageBox.Show("Select a speech name and parameter before playing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else
                Hook.Call(Addresses.Natives.unk_0x5C57B85D, ped, ambSpeechName.Text, ambSpeechParam.Text);
        }

        private void playPolRep_Click(object sender, EventArgs e)
        {
            Hook.Call(Addresses.Natives.play_police_report, policeReport.Text, 0.0f);
        }

        private void sendNetText_Click(object sender, EventArgs e)
        {
            int ped = Functions.get_player_ped(Clients.SelectedIndex);
            Hook.Call(Addresses.Natives.network_send_text_message, textMessage.Text, ped);
        }

        private void rpcSuperRunSwim_CheckedChanged(object sender)
        {
            int player = Functions.player_id();
            if (rpcSuperRunSwim.Checked)
            {
                Hook.Call(Addresses.Natives.unk_0x825423C2, player, 1.49f);
                Hook.Call(Addresses.Natives.unk_0xB986FF47, player, 1.49f);
            }
            else
            {
                Hook.Call(Addresses.Natives.unk_0x825423C2, player, 1f);
                Hook.Call(Addresses.Natives.unk_0xB986FF47, player, 1f);
            }
        }

        private void waypointAirstrike_Click(object sender, EventArgs e)
        {
            Hook.Call(Addresses.Natives.request_weapon_asset, Addresses.Hashes.Weapons.AIRSTRIKE_ROCKET, 0);

            byte[] waypointX = PS3.CCAPI.GetBytes(Addresses.RTM.waypointX, 0x04);
            Array.Reverse(waypointX);
            float x = BitConverter.ToSingle(waypointX, 0);
            byte[] waypointY = PS3.CCAPI.GetBytes(Addresses.RTM.waypointY, 0x04);
            Array.Reverse(waypointY);
            float y = BitConverter.ToSingle(waypointY, 0);
            float[] currentCoords = Functions.get_entity_coords(Functions.player_ped_id());
            float[] coords = { x, y, currentCoords[2] };

            float[] coords2 = { coords[0], coords[1], coords[2] + 50f };
            float[] coords3 = { coords[0] + 15f, coords[1], coords[2] + 50f };
            float[] coords4 = { coords[0] - 15f, coords[1], coords[2] + 50f };
            float[] coords5 = { coords[0], coords[1] + 15f, coords[2] + 50f };
            float[] coords6 = { coords[0], coords[1] - 15f, coords[2] + 50f };
            Hook.Call(Addresses.Natives.shoot_single_bullet_between_coords, coords2, coords, 200, 1, Addresses.Hashes.Weapons.AIRSTRIKE_ROCKET, Functions.player_ped_id(), 1, 0, 3.21283686E+09f);
            Hook.Call(Addresses.Natives.shoot_single_bullet_between_coords, coords3, coords, 200, 1, Addresses.Hashes.Weapons.AIRSTRIKE_ROCKET, Functions.player_ped_id(), 1, 0, 3.21283686E+09f);
            Hook.Call(Addresses.Natives.shoot_single_bullet_between_coords, coords4, coords, 200, 1, Addresses.Hashes.Weapons.AIRSTRIKE_ROCKET, Functions.player_ped_id(), 1, 0, 3.21283686E+09f);
            Hook.Call(Addresses.Natives.shoot_single_bullet_between_coords, coords5, coords, 200, 1, Addresses.Hashes.Weapons.AIRSTRIKE_ROCKET, Functions.player_ped_id(), 1, 0, 3.21283686E+09f);
            Hook.Call(Addresses.Natives.shoot_single_bullet_between_coords, coords6, coords, 200, 1, Addresses.Hashes.Weapons.AIRSTRIKE_ROCKET, Functions.player_ped_id(), 1, 0, 3.21283686E+09f);
        }

        private void loadCustomMap_Click(object sender, EventArgs e)
        {
            Functions.LoadCustomMap(@"\Resources\MapMods", customMapModList.Text, customMapModList);
        }

        private void policeMinigame_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            int player = Functions.player_id();
            uint hash = Functions.GetHash("s_m_y_cop_01");
            Hook.Call(Addresses.Natives.request_model, hash);
            for (; ; )
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, hash) == 1)
                    break;
            }
            uint hash2 = Functions.GetHash("police");
            Hook.Call(Addresses.Natives.request_model, hash2);
            for (; ; )
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, hash2) == 1)
                    break;
            }
            float[] coords = new float[3];
            Hook.Call(Addresses.Natives.get_entity_coords, Addresses.Other.entityXCoord, PedID);
            coords[0] = PS3.Extension.ReadFloat(Addresses.Other.entityXCoord);
            coords[1] = PS3.Extension.ReadFloat(Addresses.Other.entityYCoord);
            coords[2] = PS3.Extension.ReadFloat(Addresses.Other.entityZCoord);

            int ped = Hook.Call(Addresses.Natives.create_ped, 26, hash, coords, 0, 1, 0);
            int veh = Hook.Call(Addresses.Natives.create_vehicle, hash2, coords, 0, 1, 0);
            Hook.Call(Addresses.Natives.set_ped_into_vehicle, ped, veh, -1);
            Hook.Call(Addresses.Natives.task_vehicle_drive_wander, ped, veh, 20f, 786603);
            Functions.give_weapon_to_ped(ped, Addresses.Hashes.Weapons.PISTOL50);
            Thread.Sleep(500);
            Hook.Call(Addresses.Natives.set_ped_into_vehicle, PedID, veh, 0);
        }

        private void swatToWaypoint_Click(object sender, EventArgs e)
        {
            int PedID = Hook.Call(Addresses.Natives.player_ped_id);
            uint hash = Functions.GetHash("s_m_y_swat_01");
            Hook.Call(Addresses.Natives.request_model, hash);
            for (; ; )
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, hash) == 1)
                    break;
            }
            uint hash2 = Functions.GetHash("riot");
            Hook.Call(Addresses.Natives.request_model, hash2);
            for (; ; )
            {
                if (Hook.Call(Addresses.Natives.has_model_loaded, hash2) == 1)
                    break;
            }
            float[] coords = new float[3];
            Hook.Call(Addresses.Natives.get_entity_coords, Addresses.Other.entityXCoord, PedID);
            coords[0] = PS3.Extension.ReadFloat(Addresses.Other.entityXCoord);
            coords[1] = PS3.Extension.ReadFloat(Addresses.Other.entityYCoord);
            coords[2] = PS3.Extension.ReadFloat(Addresses.Other.entityZCoord);

            byte[] waypointX = PS3.CCAPI.GetBytes(Addresses.RTM.waypointX, 0x04);
            Array.Reverse(waypointX);
            float x = BitConverter.ToSingle(waypointX, 0);
            byte[] waypointY = PS3.CCAPI.GetBytes(Addresses.RTM.waypointY, 0x04);
            Array.Reverse(waypointY);
            float y = BitConverter.ToSingle(waypointY, 0);
            float[] waypoint = { x, y, coords[2] };

            int ped = Hook.Call(Addresses.Natives.create_ped, 26, hash, coords, 0, 1, 0);
            int veh = Hook.Call(Addresses.Natives.create_vehicle, hash2, coords, 0, 1, 0);
            Hook.Call(0x450E6C, veh, 1); //SET_VEHICLE_SIREN
            Hook.Call(Addresses.Natives.set_ped_into_vehicle, ped, veh, -1);
            int a = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 0, 1, 0);
            int b = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 1, 1, 0); 
            int c = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 2, 1, 0); 
            int d = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 3, 1, 0); 
            int ee = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 4, 1, 0);
            int f = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 5, 1, 0); 
            int g = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 6, 1, 0); 
            int h = Hook.Call(Addresses.Natives.create_ped_inside_vehicle, veh, 27, hash, 7, 1, 0);
            Functions.give_weapon_to_ped(ped, Addresses.Hashes.Weapons.PISTOL50);
            Functions.give_weapon_to_ped(a, Addresses.Hashes.Weapons.PISTOL50);
            Functions.give_weapon_to_ped(b, Addresses.Hashes.Weapons.PISTOL50);
            Functions.give_weapon_to_ped(c, Addresses.Hashes.Weapons.PUMPSHOTGUN);
            Functions.give_weapon_to_ped(d, Addresses.Hashes.Weapons.PUMPSHOTGUN);
            Functions.give_weapon_to_ped(ee, Addresses.Hashes.Weapons.ADVANCEDRIFLE);
            Functions.give_weapon_to_ped(f, Addresses.Hashes.Weapons.ADVANCEDRIFLE);
            Functions.give_weapon_to_ped(g, Addresses.Hashes.Weapons.ADVANCEDRIFLE);
            Functions.give_weapon_to_ped(h, Addresses.Hashes.Weapons.ADVANCEDRIFLE);
            Hook.Call(Addresses.Natives.task_vehicle_drive_to_coord, ped, veh, waypoint, 40f, 1, hash2, 1, 0x2C0025, -1);
        }
    }
}