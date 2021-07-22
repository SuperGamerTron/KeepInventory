using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeepInventory
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class KeepInventory : BaseUnityPlugin
    {
        public const string
            GUID = "KeepInventory",
            NAME = "KeepInventory",
            VERSION = "1.0.1";

        public static List<int> playersWithMod = new List<int>();
        public static bool hasMod = Directory.GetFiles(Directory.GetCurrentDirectory(), "KeepInventory.dll", SearchOption.AllDirectories).FirstOrDefault() != default;

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(MuckPatch));
            Logger.LogMessage("Loaded KeepInventory");
        }
    }
}
