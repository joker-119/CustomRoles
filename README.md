# CustomRoles
==============
## Description
This plugin makes use of Exiled.CustomRoles to add new playable roles into the game. These roles have custom inventories, health, objectives and abilities.

**All information contained below assumes DEFAULT config values. Keep in mind that just about everything is configurable, the below is meant as an explination of what everything does by default.**

### Role List
Below is a table of all the current custom roles, followed by a breif description of them. Many of them rely on special abilities also added by this plugin, refer to the list of abilities for more details about what each does.

RoleName | RoleID | Abilities | Spawn Type | Description
:---: | :---: | :---: | :---: | :------
Ballistic Zombie | 1 | Martyrdom | Randomly when revived by SCP-049 | A zombie that explodes upon death. The explosion is the same as a frag grenade going off, including the damage amount. 
Berserker Zombie | 2 | SpeedOnKill, HealthOnKill | Randomly when revived by SCP-049, | A zombie that starts with less health than normal, but each time they get a kill they become faster and heal for a small amount.
Charger Zombie | 3 | Charge | Randomly when revived by SCP-049 | A slightly tankier than normal zombie, that is able to periodically charge in a straight line.
Demolitionist | 4 | None | During NTF Respawn waves | A NTF Specialist that spawns with a Grenade Launcher, 2 C4 charges, 4 HE Grenades and a radio. *
Dwarf | 5 | None | Immediately when a round begins | A normal human player, who does not consume stamina while sprinting, but are slightly smaller than normal humans. By default, players keep this role when they die, meaning they keep it for the entire round.
Dwarf Zombie | 6 | None | Randomly when revived by SCP-049 | A zombie who spawns with slightly less health than normal, but is smaller and faster than normal.
Medic | 7 | HealingMist | During NTF Respawn waves | A NTF Specialist who spawns with a Medigun, Tranqgun, EMP Grenade, Medkit, Adrenaline, Painkillers and Lt. Keycard. Can use their ability to heal nearby allies. *
Medic Zombie | 8 | HealingMist, MoveSpeedReduction | Randomly when revived by SCP-049 | A zombie version of the Medic, but without an inventory. Deals reduced damage. Reduced movement speed.
PD Zombie | 9 | None | Randomly when revived by SCP-049 | A zombie that takes significantly less ballistic damage, but if extremely vulnerable to flash grenades. Has a small chance to teleport players to the Pocket Dimension when attacking.
Chaos Phantom | 10 | ActiveCamo | Immediately when a round begins | A Chaos Conscript that takes the place of one of the facility guards when the round starts. They spawn with a Sniper Rifle, Implosion Grenade, EMP Grenade, SCP-127, Medkit, Adrenaline and Chaos Insurgency Device. Can periodically become completely invisible. *
Plague Zombie | 11 | Projectile, MoveSpeedReduction | Randomly when revived by SCP-049 | A slow zombie that does very little melee damage. Projectiles they shoot do not deal immediate damage like normal, instead they will poison any humans hit. Humans who die wile poisoned will turn into an instance of SCP-049-2.
SCP-575 | 12 | Blackout | Immediately when a round begins | See the 575 section below.
Juggernaut Zombie | 13 | ReactiveHume, MoveSpeedReduction | Randomly when revived by SCP-049 | A very tanky zombie capable of living through enourmous amounts of damage.


### Ability List
Below is a table of all the current abilities, followed by a description of them. 

AbilityName | Type | Duration | Cooldown | Description
:---: | :---: | :---: | :---: | :------
ActiveCamo | Active | 30s | 90s | Turns the user invisible when used. While invisible they may interact with objects, use items, and throw grenades without breaking their invisibility, however shooting a gun or starting to charge a micro will break their invisibility early.
Charge | Active | N/A | 45s | Causes the player to charge forward in a straight line very quickly. Upon colliding with a solid object, or another player, the user of the ability is ensnared for 5 seconds. If they collided with another player, that player is also ensnared for the same duration, and take 15 damage (if the player was not moving at the time, this value is doubled). If instead they collide with a non-player object (such as a wall/door), the user takes 15 damage instead.
HealingMist | Active | 15s | 3min | Every 1 second for the duration, all friendly players within 12ft of the user will be healed for 6 health. When the duration expires, any allies within that range are also given 45 AHP. Neither the heal, nor the AHP are applied to the user of this ability, however they can be affected by another player's use of this ability.
HealOnKill | Passive | Up to 10s | N/A | Heals the player for 2.5 health every second for 10 seconds after they kill another player. Taking damage while this effect is active will cancel the remaining duration. (If the heal over time option is disabled, all of the healing is done instantly, and damage taken will not prevent it)
Martyrdom | Passive | N/A | N/A | When the player dies, an HE grenade will explode on their corpse.
MoveSpeedReduction | Passive | N/A | N/A | Slows the player's movement speed by 30%.
Projectile | Active | N/A | 35s | Launches a projectile in the direction the player is facing. Upon reaching it's destination, it will explode like a grenade.
ReactiveHume | Passive | N/A | N/A | A special type of Hume shield that degenerates over time, instead of regenerating. When the player takes damage, the hume will gain 75% of the raw damage before any reductions are applied. Damage taken by the player is reduced based on how full the hume is, up to 80% reduction when the hume is full. The damage reduction calculation is done **PRIOR** to the hume gaining value from the attack.
SpeedOnKill | Passive | 5s | N/A | When the player kills another player, they will gain an intensity level of SCP-207 for the duration. This effect can stack multiple times, up to the configured limit. Getting kills while at the intensity limit will NOT refresh the duration.
