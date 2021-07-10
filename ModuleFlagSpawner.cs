using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagAsCargoItem
{
    public class ModuleFlagSpawner : PartModule
    {
        public override void OnAwake()
        {
            base.OnAwake();
        }

        [KSPEvent(guiName = "#flagascargoitem_spawnflag", guiActiveEditor = false, guiActive = true, externalToEVAOnly = true, guiActiveUnfocused = true, groupName = "craftParts", groupDisplayName = "#oretotanks_groupname")]
        public void SpawnFlag()
        {
            var inventory = part.Modules.OfType<ModuleInventoryPart>().SingleOrDefault();
            if (inventory == null)
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#flagascargoitem_cannotspawnflag_noinventory"), 2.0f, ScreenMessageStyle.UPPER_CENTER);
                return;
            }

            if (!inventory.CheckPartStorage("evaFlag"))
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#flagascargoitem_cannotspawnflag_full"), 2.0f, ScreenMessageStyle.UPPER_CENTER);
                return;
            }
            inventory.StoreCargoPartAtSlot("evaFlag", -1);
        }
    }
}
