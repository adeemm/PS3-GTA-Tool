# PS3-GTA-Tool
A RPC/RTM PS3 mod tool for GTA V

This was my first ever C# project (created ~2014), so please excuse the messy / ugly code.

This tool hooks into in-game functions (that run every frame) and modifies them in memory so that they call a custom function written into empty memory space. This function is RPC (Remote Procedure Calling) that checks for input to tell it which in-game function to execute (such as set_visibility() or set_health()). 

RTM, on the other hand, is real-time-modification of the memory on the PS3. RTM is used to write the RPC to memory and can also be used to overwrite or modify in-game functions (if you know their locations in memory). With this, you can re-write the set_health() function to always return a high integer for your character's health instead of subtracting damage from it.

I also included an auto-update function (before I abandoned development of the tool), so users could continue to enjoy it after the game is updated and function locations change. It simply looks for certain memory patterns and performs some math to get to the new memory address after a game update.

## Partial Feature List
* Cheating Reports
* City Blackout Toggle
* Clean / Wash / Heal Player
* Custom Headlight Multiplier
* Custom Notifications
* Cutscene Player (430 Total)
* Dead-Eye Mod (Slow Motion Aiming)
* Drop 500K
* External XML Teleport File Support (For Preset Teleport Locations)
* First Person Cam
* Fix / Clean / Godmode Vehicle
* Force Ragdoll Player
* Freeze Vehicle
* Full Ped Model Spawner
* Full Vehicle Spawner (352 Total)
* Garage Vehicle Editor
* Get Current Coordinates
* Godmode
* Instant Wanted Level Changer (Get / Set)
* Instantly Kill Player
* Load Map Mods
* Low Vehicle Gravity
* Map Mods (Including Heist Aircraft Carrier)
* Night Vision
* Object Spawner (Custom Input Or Preset Models)
* Open / Close Selected Door
* Play Animations
* Player Minimap Blip Options (Scale & Sprite)
* Player Model Changer
* Player Visibility Options
* Police Options
* Portable Radio
* PS3 Power Options
* Rain Money
* Screen FX
* Selected RGB Color For Car (Primary / Secondary)
* Send Money
* Set Car Upgrades (Armor, Breaks, Engine, Horn... etc)
* Set Custom Timescale
* Set License Plate Text / Type
* Set Total Rank / XP (Primary / Secondary)
* Set Vehicle On Fire
* Set Vehicle Speed
* Set Weather Type
* Skip Current Radio Station
* Talking Player List
* Teleport Closest Vehicle To
* Teleport Inside Closest Vehicle
* Teleport Last Vehicle To Me
* Teleport To Custom Coordinates
* Teleport To Player
* Teleport To Waypoint (With Custom Z Coordinate)
* Thermal Vision
* Toggle Vehicle Undriveable
* Unlimited Ammo
* Unlock All Trophies
