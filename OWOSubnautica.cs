using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace OWO_Subnautica
{
    [BepInPlugin("org.bepinex.plugins.OWO_Subnautica", "OWO_Subnautica", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
#pragma warning disable CS0109 // Remove unnecessary warning
        internal static new ManualLogSource Log;
#pragma warning restore CS0109
        //public static TactsuitVR tactsuitVr;
        public static bool startedHeart = false;
        public static float currentHealth = 0;
        public static bool inventoryOpened = false;
        public static long buttonPressTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        public static bool playerHasSpawned = false;

        private void Awake()
        {
            // Make my own logger so it can be accessed from the Tactsuit class
            Log = base.Logger;
            // Plugin startup logic
            Logger.LogMessage("OWO Integration plugin is loaded!");
            //tactsuitVr = new TactsuitVR();
            // one startup heartbeat so you know the vest works correctly
            //tactsuitVr.PlaybackHaptics("HeartBeat");
            // patch all functions
            var harmony = new Harmony("owo.patch.subnautica");
            harmony.PatchAll();
        }
    }
    }
