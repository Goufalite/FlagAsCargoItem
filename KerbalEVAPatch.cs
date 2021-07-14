using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using KSP.Localization;

namespace FlagAsCargoItem
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class KerbalEVAPatch : MonoBehaviour
    {
        internal static Harmony harmony = new Harmony("org.goufastyle.goufamods.FlagAsCargoItem");

        public const string EVA_FLAG = "evaFlag";

        static bool patched = false;
        private void Awake()
        {
            Harmony.DEBUG = false;
            if (!patched)
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
                patched = true;
            }
        }
    }

    [HarmonyPatch(typeof(KerbalEVA),"CanPlantFlag")]
    class PatchCanPlantFlag
    {
        static void Prefix(KerbalEVA __instance)
        {
            int totalflags = __instance.ModuleInventoryPartReference.TotalAmountOfPartStored(KerbalEVAPatch.EVA_FLAG);
            __instance.flagItems = totalflags;
        }
    }

    [HarmonyPatch(typeof(KerbalEVA), "PlantFlag")]
    class PatchPlantFlag
    {
        static void Prefix(KerbalEVA __instance)
        {
            __instance.ModuleInventoryPartReference.RemoveNPartsFromInventory(KerbalEVAPatch.EVA_FLAG, 1);
            GameEvents.onModuleInventoryChanged.Fire(__instance.ModuleInventoryPartReference);
        }
    }

    [HarmonyPatch(typeof(FlagSite), "PickUp")]
    class PatchPickUpFlag
    {
        static bool Prefix(FlagSite __instance)
        {
            KerbalEVA myKerbal = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>();
            if (myKerbal == null)
            {
                //in case of tracking station
                return true;
            }
            ModuleInventoryPart inventory = myKerbal.ModuleInventoryPartReference;
            if (inventory == null)
            {
                // for some reason the kerbal doesn't have an inventory... continue as normal
                return true;
            }
            
            if (!CheatOptions.IgnoreKerbalInventoryLimits)
            {

                if (inventory.HasMassLimit && inventory.massCapacity + PartMass(KerbalEVAPatch.EVA_FLAG) > inventory.massLimit + 0.00001f)
                {
                    ScreenMessages.PostScreenMessage(Localizer.Format("#flagascargoitem_cannotstoreflag_mass"), 2.0f, ScreenMessageStyle.UPPER_CENTER);
                    return false;
                }
                if (inventory.HasPackedVolumeLimit && inventory.volumeCapacity + PartVolume(KerbalEVAPatch.EVA_FLAG) > inventory.packedVolumeLimit + 0.00001f)
                {
                    ScreenMessages.PostScreenMessage(Localizer.Format("#flagascargoitem_cannotstoreflag_volume"), 2.0f, ScreenMessageStyle.UPPER_CENTER);
                    return false;
                }
            }

            bool succes = RealStoreCargoPartAtSlot(myKerbal.ModuleInventoryPartReference, KerbalEVAPatch.EVA_FLAG, -1);
            if (!succes)
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#flagascargoitem_cannotstoreflag_full"), 2.0f, ScreenMessageStyle.UPPER_CENTER);
                return false;
            }
            return true;
        }

        #region helpers
        private static bool RealStoreCargoPartAtSlot(ModuleInventoryPart inventory, string partName, int inventorySlot)
        {
            if (String.IsNullOrEmpty(partName))
            {
                return false;
            }
            if (inventory == null)
            {
                return false;
            }
            int availableInventorySlot = -1;
            if (inventorySlot == -1)
            {
                // find first available inventory slot
                for (int i = 0; i < inventory.InventorySlots; i++)
                {
                    if (!inventory.storedParts.ContainsKey(i))
                    {
                        availableInventorySlot = i;
                        break;
                    }
                    StoredPart myStoredPart = inventory.storedParts[i];
                    if (myStoredPart != null && myStoredPart.IsEmpty)
                    {
                        availableInventorySlot = i;
                        break;
                    }
                    if (myStoredPart != null && myStoredPart.partName.Equals(partName) && myStoredPart.CanStack && myStoredPart.stackCapacity > myStoredPart.quantity)
                    {
                        availableInventorySlot = i;
                        break;
                    }
                }
            }
            else
            {
                if (inventory.storedParts.ContainsKey(inventorySlot) && inventory.storedParts[inventorySlot].IsFull)
                {
                    return false;
                }
                availableInventorySlot = inventorySlot;
            }
            if (availableInventorySlot == -1)
            {
                return false;
            }
            try
            {
                if (!inventory.storedParts.ContainsKey(availableInventorySlot) || !inventory.storedParts[availableInventorySlot].CanStack)
                {
                    return inventory.StoreCargoPartAtSlot(partName, availableInventorySlot);
                }
                else
                {
                    return inventory.UpdateStackAmountAtSlot(availableInventorySlot, inventory.storedParts[availableInventorySlot].quantity + 1);
                }

            }
            catch
            {
                return false;
            }

        }

        private static float PartMass(string partName)
        {
            AvailablePart availablePart = PartLoader.getPartInfoByName(partName);
            if (availablePart == null)
            {
                throw new Exception("Unknown part " + partName);
            }
            float mass = 0.0f;
            bool getvalue = availablePart.partConfig.TryGetValue("mass", ref mass);
            if (!getvalue)
            {
                throw new Exception("No mass found " + partName);
            }
            return mass;
        }

        private static float PartVolume(string partName)
        {
            AvailablePart availablePart = PartLoader.getPartInfoByName(partName);
            if (availablePart == null)
            {
                throw new Exception("Unknown part " + partName);
            }
            float volume = 0.0f;
            bool getvalue = false;
            foreach (ConfigNode module in availablePart.partConfig.GetNodes())
            {
                string name = "";
                module.TryGetValue("name", ref name);
                if (!name.Equals("ModuleCargoPart"))
                {
                    continue;
                }
                getvalue = module.TryGetValue("packedVolume", ref volume);
                break;

            }

            if (!getvalue)
            {
                throw new Exception("No volume found " + partName);
            }
            return volume;
        }
        #endregion
    }
}
