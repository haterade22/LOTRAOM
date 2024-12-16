BANNER COLOR PERSISTENCE with BannerPaste - Version 1.4.6

Built for Bannerlord version 1.2.0

By request I've extended the built in BannerPaste functionality into
this latest builds of BannerColorPersistence. Originally created by
gooboon, BannerPaste will allow you to CTRL+V/CTRL+C paste/copy a
bannercode in your banner editor window. Please check out the original
BannerPaste mod at
https://www.nexusmods.com/mountandblade2bannerlord/mods/1003
to show your appreciation!

BannerColorPersistence will resolve the many color issues plaguing
complex custom banners. It will prevent all automatic color changes to
your clan bannerincluding those caused by joining a kingdom and on
save/load. It also prevents your clothing and armor from changing to
kingdom colors when you join a kingdom. These fixes prevent color
changes to everyone in your clan.
Configuration options are available to prevent any NPC color
changes as well as an experimental tweak that may potentially allow your
banner to change smoothly to your kingdom colors without breaking the
icons.
In addition, BannerColorPersistence now contains a fix for a bug that
was introduced in 1.6.0 Bannerlord code that causes a crash whenever you
have a banner with more than one icon layer and attempt to open the trade
or inventory screen. Also included is logic to automatically pick a
unique secondary color for new banners, resolving issues of vassals
ending up with monocolor banners.

REQUIREMENTS
This mod now requires the use of the Harmony mod, rather than packaging
the latest dll within it. Please visit
https://www.nexusmods.com/mountandblade2bannerlord/mods/2006
and download the latest version of Harmony that works for 1.1.0
(v2.2.2.145 as of this publishing)

USAGE
Ensure the mod folder is extracted to your Bannerlord Modules folder.
Ensure Harmony mod is installed properly.
Load order should be Harmony at the very top of the list and then
BannerColorPersistence below the standard game modules (Native, Sandbox,
SandboxCore, StoryMode, CustomBattle).
The mod prevents your banner from having its colors changed when loading
a save - however, it does not FIX already broken banners. If your banner
is already broken, please re-paste the original code into the banner
editor interface. From then on, it should remain fine.
By default, this mod prevents all banner color changes. This means that
when you change kingdoms, your banner will not change color to match
that kingdom's colors.

CONFIGURATION
Three configuration options are available if you're looking for
something a little different.

enableBannerColorPersistence: defaults to true. Set to false if you do
not want the banner color persistence logic to be run. This will allow
only the BannerPaste functionality and basic crash fixes to function.
preventNPCBannerColorChanges: defaults to false, will prevent all color
changes for every NPC, not just your own clan.
allowColorChangeOnIconsMatchingBackgroundColor: defaults to false. This
one may work well for you or it may not. It's completely dependent upon
your specific banner. Only useful if your banner has icons with colors
that match EXACTLY with your background colors in order to block out
portions of other icons.