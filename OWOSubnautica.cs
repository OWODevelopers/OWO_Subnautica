using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace OWO_Subnautica
{
    [BepInPlugin("org.bepinex.plugins.OWO_Subnautica", "OWO_Subnautica", "0.0.2")]
    public class Plugin : BaseUnityPlugin
    {
#pragma warning disable CS0109 // Remove unnecessary warning
        internal static new ManualLogSource Log;
#pragma warning restore CS0109
        public static OWOSkin owoSkin;
        //public static ConfigEntry<bool> swimmingEffects;
        //public static List<TechType> oneShotTypes = new List<TechType>();

        private void Awake()
        {
            Log = Logger;
            Logger.LogMessage("OWO_Subnautica plugin is loaded!");
            owoSkin = new OWOSkin();

            var harmony = new Harmony("owo.patch.subnautica");
            harmony.PatchAll();
        }
        //#region Actions

        //[HarmonyPatch(typeof(Inventory), "OnAddItem")]
        //public class OnPickupItem
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled)
        //        {
        //            return;
        //        }
        //        owoSkin.Feel("PickUpItem");
        //    }
        //}

        //[HarmonyPatch(typeof(Player), "LateUpdate")]
        //public class OnSwimming
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(Player __instance)
        //    {
        //        if (owoSkin.suitDisabled)
        //        {
        //            return;
        //        }
        //        if (__instance.isUnderwaterForSwimming.value)
        //        {
        //            //start thread
        //            owoSkin.StartSwimming();
        //            //is seaglide equipped ?
        //            owoSkin.seaGlideEquipped = __instance.motorMode == Player.MotorMode.Seaglide;
        //            //calculate delay and intensity
        //            int delay = (int)((30 - Math.Truncate(__instance.movementSpeed * 10)) * 100);
        //            owoSkin.SwimmingDelay =(delay > 0)?(delay < 3000)? delay : 3000: 1000;

        //            float intensity = (float)(Math.Truncate(__instance.movementSpeed * 10) / 30);
        //            owoSkin.SwimmingIntensity = intensity;
        //        }
        //        else
        //        {
        //            owoSkin.StopSwimming();
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(PrecursorTeleporter), "OnPlayerCinematicModeEnd")]
        //public class OnTeleportingStart
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StartTeleport();
        //    }
        //}

        //[HarmonyPatch(typeof(Player), "CompleteTeleportation")]
        //public class OnTeleportingStop
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StopTeleport();
        //    }
        //}

        //[HarmonyPatch(typeof(Player), "UpdateIsUnderwater")]
        //public class OnEnterExitWater
        //{
        //    public static bool isUnderwaterForSwimming = false;

        //    [HarmonyPrefix]
        //    public static void Prefix(Player __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        isUnderwaterForSwimming = __instance.isUnderwaterForSwimming.value;
        //    }

        //    [HarmonyPostfix]
        //    public static void Postfix(Player __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        if (__instance.isUnderwaterForSwimming.value != isUnderwaterForSwimming)
        //        {
        //            owoSkin.Feel((!isUnderwaterForSwimming) ? "Enter Water" : "Exit Water");
        //        }

        //        isUnderwaterForSwimming = __instance.isUnderwaterForSwimming.value;
        //    }
        //}

        //#endregion

        //#region Equipments

        //[HarmonyPatch(typeof(Knife), "IsValidTarget")]
        //public class OnKnifeAttack
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(bool __result)
        //    {
        //        if (owoSkin.suitDisabled || !__result)
        //        {
        //            return;
        //        }
        //        owoSkin.Feel("Knife Attack");
        //    }
        //}

        //[HarmonyPatch(typeof(StasisRifle), "Fire")]
        //public class OnStatisAttack
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled)
        //        {
        //            return;
        //        }
        //        owoSkin.Feel("Recoil R");
        //    }
        //}

        //[HarmonyPatch(typeof(ScannerTool), "OnRightHandDown")]
        //public class Onscanning
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(bool __result)
        //    {
        //        if (owoSkin.suitDisabled || !__result) return;

        //        owoSkin.Feel("Scanning");
        //    }
        //}
        //[HarmonyPatch(typeof(AirBladder), "OnRightHandDown")]
        //public class OnAirBladder
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(bool __result)
        //    {
        //        if (owoSkin.suitDisabled || !__result) return;

        //        owoSkin.Feel("Air Bladder");
        //    }
        //}

        //[HarmonyPatch(typeof(Welder), "Weld")]
        //public class Welder
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StartDrilling(true);
        //    }
        //}

        //[HarmonyPatch(typeof(Welder), "StopWeldingFX")]
        //public class StopWeldingFX
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StopDrilling(true);
        //    }
        //}

        //[HarmonyPatch(typeof(LaserCutter), "StartLaserCuttingFX")]
        //public class LaserCutterStart
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return; 

        //        owoSkin.StartDrilling(true);
        //    }
        //}

        //[HarmonyPatch(typeof(LaserCutter), "StopLaserCuttingFX")]
        //public class LaserCutterStop
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StopDrilling(true);
        //    }
        //}

        //[HarmonyPatch(typeof(BuilderTool), "OnRightHandDown")]
        //public class OnBuilderTool
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(bool __result)
        //    {
        //        if (owoSkin.suitDisabled || !__result) return;

        //        owoSkin.Feel("Builder");
        //    }
        //}

        //[HarmonyPatch(typeof(Constructor), "OnRightHandDown")]
        //public class OnConstructor
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(bool __result)
        //    {
        //        if (owoSkin.suitDisabled || !__result) return;

        //        owoSkin.Feel("Constructor");
        //    }
        //}
        //[HarmonyPatch(typeof(PropulsionCannon), "GrabObject")]
        //public class OnPropulsionCannon
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.Feel("Grab Object");
        //    }
        //}

        //[HarmonyPatch(typeof(PropulsionCannon), "OnShoot")]
        //public class OnPropulsionCannonShoot
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.Feel("Cannon Shot");
        //    }
        //}

        //[HarmonyPatch(typeof(StasisRifle), "OnRightHandDown")]
        //public class OnStatisCharging
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(bool __result)
        //    {
        //        if (owoSkin.suitDisabled || !__result) return;

        //        owoSkin.Feel("Statis Charging");
        //    }
        //}

        //#endregion

        //#region Vehicles

        //[HarmonyPatch(typeof(CyclopsExternalDamageManager), "OnTakeDamage")]
        //public class OnCyclopDamage
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.Feel("Vehicle Impact");
        //    }
        //}

        //[HarmonyPatch(typeof(CyclopsEngineChangeState), "OnClick")]
        //public class OnCyclopEngineStart
        //{
        //    [HarmonyPrefix]
        //    public static void Prefix(CyclopsEngineChangeState __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        if (!__instance.motorMode.engineOn)
        //        {
        //            owoSkin.Feel("LandAfterJump");
        //            //Plugin.owoSkin.Feel("LandAfterJump", true, 1f, 4f);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(Vehicle), "PlaySplashSound")]
        //public class OnSplash
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.Feel("Splash");
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_SeamothHUD), "Update")]
        //public class OnDamageVehicle
        //{
        //    public static int health;

        //    [HarmonyPrefix]
        //    public static void Prefix(uGUI_SeamothHUD __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
        //    }

        //    [HarmonyPostfix]
        //    public static void Postfix(uGUI_SeamothHUD __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        if (health > Traverse.Create(__instance).Field("lastHealth").GetValue<int>())
        //        {
        //            owoSkin.Feel("Vehicle Impact");

        //            health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_ExosuitHUD), "Update")]
        //public class OnDamageExosuit
        //{
        //    public static int health;

        //    [HarmonyPrefix]
        //    public static void Prefix(uGUI_ExosuitHUD __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
        //    }

        //    [HarmonyPostfix]
        //    public static void Postfix(uGUI_ExosuitHUD __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        if (health != Traverse.Create(__instance).Field("lastHealth").GetValue<int>())
        //        {
        //            owoSkin.Feel("Exosuit Impact");

        //            health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(Exosuit), "OnLand")]
        //public class OnExosuitLanding
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.Feel("Land After Jump");
        //    }
        //}

        //[HarmonyPatch(typeof(Exosuit), "ApplyJumpForce")]
        //public class OnExosuitJumping
        //{
        //    [HarmonyPrefix]
        //    public static void Prefix(Exosuit __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        bool grounded = Traverse.Create(__instance).Field("onGround").GetValue<bool>();
        //        double timeLastJumped = (double)(Traverse.Create(__instance).Field("timeLastJumped").GetValue<float>() + 1.0);

        //        Log.LogWarning("JUMP " + grounded + " " + timeLastJumped + " " + (double)Time.time);

        //        if (grounded && timeLastJumped <= (double)Time.time)
        //        {
        //            owoSkin.Feel("Land After Jump");
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(Exosuit), "SlotLeftDown")]
        //public class OnExosuitArmDown
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(Exosuit __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        if (__instance.mainAnimator.GetBool("use_tool_left"))
        //        {
        //            TechType type = Traverse.Create(__instance).Field("currentLeftArmType").GetValue<TechType>();
        //            if (oneShotTypes.Contains(type))
        //            {
        //                owoSkin.Feel("Recoil L");
        //            }
        //            if (type == TechType.ExosuitDrillArmModule)
        //            {
        //                owoSkin.StartDrilling(false);
        //            }
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(Exosuit), "SlotLeftUp")]
        //public class OnExosuitArmUp
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(Exosuit __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        TechType type = Traverse.Create(__instance).Field("currentLeftArmType").GetValue<TechType>();

        //        if (type == TechType.ExosuitDrillArmModule)
        //        {
        //            owoSkin.StopDrilling(false);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(Exosuit), "SlotRightDown")]
        //public class OnExosuitArmDownRight
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(Exosuit __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        if (__instance.mainAnimator.GetBool("use_tool_right"))
        //        {
        //            TechType type = Traverse.Create(__instance).Field("currentRightArmType").GetValue<TechType>();

        //            if (oneShotTypes.Contains(type))
        //            {
        //                owoSkin.Feel("Recoil R");
        //            }

        //            if (type == TechType.ExosuitDrillArmModule)
        //            {
        //                owoSkin.StartDrilling(true);
        //            }
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(Exosuit), "SlotRightUp")]
        //public class OnExosuitArmUpRight
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(Exosuit __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        TechType type = Traverse.Create(__instance).Field("currentRightArmType").GetValue<TechType>();

        //        if (type == TechType.ExosuitDrillArmModule)
        //        {
        //            owoSkin.StopDrilling(true);
        //        }
        //    }
        //}

        //#endregion

        //#region Health

        //[HarmonyPatch(typeof(Player), "OnKill")]
        //public class OnDeath
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StopAllHapticFeedback();
        //        owoSkin.Feel("Death", 4);
        //    }
        //}

        //[HarmonyPatch(typeof(Player), "OnTakeDamage")]
        //public class OnTakeDamage
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        //Plugin.owoSkin.Feel("Impact", true, 2f);
        //        owoSkin.Feel("impact");
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_OxygenBar), "OnPulse")]
        //public class OnLowOxygen
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(uGUI_OxygenBar __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
        //        if (pulseDelay >= 2 || !Utils.GetLocalPlayerComp().isUnderwaterForSwimming.value)
        //        {
        //            owoSkin.StopLowOxygen();
        //            return;
        //        }
        //        if (pulseDelay < 2)
        //        {
        //            owoSkin.StartLowOxygen();
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_WaterBar), "OnPulse")]
        //public class OnLowWaterStart
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(uGUI_WaterBar __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
        //        if (pulseDelay <= 0.85f)
        //        {
        //            owoSkin.StartLowWater();
        //            return;
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_WaterBar), "OnDrink")]
        //public class OnLowWaterStop
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StopLowWater();
        //        owoSkin.Feel("Eating");
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_FoodBar), "OnPulse")]
        //public class OnLowFoodStart
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(uGUI_FoodBar __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
        //        if (pulseDelay <= 0.85f)
        //        {
        //            owoSkin.StartLowFood();
        //            return;
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_FoodBar), "OnEat")]
        //public class OnLowFoodStop
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StopLowFood();
        //        owoSkin.Feel("Eating");
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_HealthBar), "OnPulse")]
        //public class OnLowHealthStart
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(uGUI_HealthBar __instance)
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
        //        if (pulseDelay <= 1.85f)
        //        {
        //            owoSkin.StartHeartBeat();
        //            return;
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(uGUI_HealthBar), "OnHealDamage")]
        //public class OnLowHealthStop
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;

        //        owoSkin.StopHeartBeat();
        //        owoSkin.Feel("Heal");
        //    }
        //}

        //#endregion

        //[HarmonyPatch(typeof(Player), "OnDestroy")]
        //public class OnPlayerDestroy
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;
        //        owoSkin.StopAllHapticFeedback();
        //    }
        //}

        //[HarmonyPatch(typeof(Player), "OnDisable")]
        //public class OnPlayerDisabled
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (owoSkin.suitDisabled) return;
        //        owoSkin.StopAllHapticFeedback();
        //    }
        //}
    }
}
