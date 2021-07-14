# Flag As Cargo Item
Mod for Kerbal Space Program changing flags to cargo items.

Plant flag.

Receive funds.

No more!

When I first saw [the 1.11 trailer](https://youtu.be/D3WXW7kwCyI?t=27) I thought to myself "Finally, no more infinite flags!" but I was wrong... 
I then [asked on reddit](https://www.reddit.com/r/KerbalSpaceProgram/comments/ka1izn/with_111_do_you_think_flags_couldshould_be_finite/) if people would like to see flags as a cargo item and with 86 voters I had a score of... perfect 50/50! 
I also asked if somebody knew a mod capable of doing this, nobody answered. Therefore…

**Introducing Flag As Cargo Item!**

Flags are now stackable cargo items! **Don’t forget to store them in your ship** in order to plant this glorious 1 meter stick with cloth.

(The description comes from redditor [Anameonreddit](https://www.reddit.com/r/KerbalSpaceProgram/comments/ka1izn/with_111_do_you_think_flags_couldshould_be_finite/gf9ntet?utm_source=share&utm_medium=web2x&context=3) which commented my poll)

You can even craft them with ore with my other mod [Ore To Parts](https://forum.kerbalspaceprogram.com/index.php?/topic/203705-11121121ore-to-parts-craft-parts-with-ore-duplicate-parts/)!

## Dependencies

* [Harmony](https://github.com/KSPModdingLibs/HarmonyKSP/releases) (hard dependency) - install separately  

## Instructions 

Remove previous versions. Unzip into the GameData folder of your KSP installation, your folder should look like `GameData/FlagAsCargoItem`

## Mods Supported

* [Ore To Parts](https://forum.kerbalspaceprogram.com/index.php?/topic/203705-11121121ore-to-parts-craft-parts-with-ore-duplicate-parts/) - an embedded MM script allows Ore To Parts to craft flags!

## Localization

English, French

CC-BY-SA 4.0

Github : https://github.com/Goufalite/FlagAsCargoItem

Issues : https://github.com/Goufalite/FlagAsCargoItem/issues

Latest releases for 1.12.1+ (**Warning: NOT compatible with 1.11.2**) : https://github.com/Goufalite/FlagAsCargoItem/releases

Release for 1.11.2+ : https://github.com/Goufalite/FlagAsCargoItem/releases/tag/v0.1.0.0

Please provide a KSP.log file in order for me to help you. Also please tell if you have the expansions installed.

## Known issues!

* If you have an engineer trying to "construct" the flag, you’ll see a dull cylinder smashing explosively on the ground! => The 1.12 changelog says *Decreased a height error check from 0.2m to 0.1m, in order to support smaller parts, and preventing them to sink into the ground.*. Although the flag is 0.7m this might be the cause. Sorry.

## Changelog

### 0.2.0.0
* WARNING NOT COMPATIBLE WITH 1.11.2
* Added inventory mass/volume limit cheat introduced in 1.12

### 0.1.0.0
* Initial release.
* Added a new part: the A1 deployable flag as a cargo item.
* Flags weigh 5kg, occupy 2L and can stack in 10.
* If a kerbal has a full inventory he/she cannot pick up the flag (but the tracking station can destroy it)
