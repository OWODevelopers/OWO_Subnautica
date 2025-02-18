using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using static VFXParticlesPool;

namespace OWO_Subnautica
{
    [BepInPlugin("org.bepinex.plugins.OWO_Subnautica", "OWO_Subnautica", "0.0.2")]
    public class Plugin : BaseUnityPlugin
    {
#pragma warning disable CS0109 // Remove unnecessary warning
        internal static new ManualLogSource Log;
#pragma warning restore CS0109
        public static OWOSkin owoSkin;
        public static ConfigEntry<bool> swimmingEffects;
        public static bool playerHasSpawned = false;
        public static List<TechType> oneShotTypes = new List<TechType>();

        private void Awake()
        {
            Log = Logger;
            Logger.LogMessage("OWO_Subnautica plugin is loaded!");
            owoSkin = new OWOSkin();

            var harmony = new Harmony("owo.patch.subnautica");
            harmony.PatchAll();
        }

        public static bool CantFeel()
        {
            return owoSkin.suitDisabled || !playerHasSpawned;
        }

        #region Actions

        [HarmonyPatch(typeof(Inventory), "OnAddItem")]
        public class OnPickupItem
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;
                
                owoSkin.Feel("Pickup Item", 2);
            }
        }

        [HarmonyPatch(typeof(Player), "LateUpdate")]
        public class OnSwimming
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {                
                if (__instance.IsAlive() && __instance.movementSpeed > .1) playerHasSpawned = true;

                if (CantFeel()) return;

                if (__instance.isUnderwaterForSwimming.value && __instance.movementSpeed > 0)
                {
                    //owoSkin.LOG($"Swimming speed: {__instance.movementSpeed}");
                    owoSkin.StartSwimming();
                    owoSkin.seaGlideEquipped = __instance.motorMode == Player.MotorMode.Seaglide;
                    int delay = (int)((30 - Math.Truncate(__instance.movementSpeed * 10)) * 100);
                    owoSkin.SwimmingDelay = (delay > 0) ? (delay < 3000) ? delay : 3000 : 1000;

                    float intensity = (float)(Math.Truncate(__instance.movementSpeed * 10) / 30);
                    owoSkin.SwimmingIntensity = intensity;
                }
                else
                {
                    owoSkin.StopSwimming();
                }
            }
        }

        [HarmonyPatch(typeof(PrecursorTeleporter), "OnPlayerCinematicModeEnd")]
        public class OnTeleportingStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.StartTeleport();
            }
        }

        [HarmonyPatch(typeof(Player), "CompleteTeleportation")]
        public class OnTeleportingStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.StopTeleport();
            }
        }

        [HarmonyPatch(typeof(Player), "UpdateIsUnderwater")]
        public class OnEnterExitWater
        {
            public static bool isUnderwaterForSwimming = false;

            [HarmonyPrefix]
            public static void Prefix(Player __instance)
            {
                if (CantFeel()) return;

                isUnderwaterForSwimming = __instance.isUnderwaterForSwimming.value;
            }

            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                if (CantFeel()) return;

                if (__instance.isUnderwaterForSwimming.value != isUnderwaterForSwimming)
                {
                    owoSkin.Feel((!isUnderwaterForSwimming) ? "Enter Water" : "Exit Water", 2);
                }

                isUnderwaterForSwimming = __instance.isUnderwaterForSwimming.value;
            }
        }

        #endregion

        #region Equipments

        [HarmonyPatch(typeof(Knife), "IsValidTarget")]
        public class OnKnifeAttack
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (CantFeel() || !__result) return;

                owoSkin.Feel("Knife Attack", 3);
            }
        }

        [HarmonyPatch(typeof(StasisRifle), "Fire")]
        public class OnStatisAttack
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.Feel("Recoil R", 3);
            }
        }

        [HarmonyPatch(typeof(ScannerTool), "OnRightHandDown")]
        public class Onscanning
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (CantFeel() || !__result) return;

                owoSkin.Feel("Scanning", 2);
            }
        }
        [HarmonyPatch(typeof(AirBladder), "OnRightHandDown")]
        public class OnAirBladder
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (CantFeel() || !__result) return;

                owoSkin.Feel("Air Bladder", 3);
            }
        }
        
        [HarmonyPatch(typeof(Welder), "OnToolUseAnim")]
        public class OnToolUseAnim
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.StartDrilling(true);
            }
        }


        //[HarmonyPatch(typeof(Welder), "Weld")]
        //public class Welder
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix()
        //    {
        //        if (CantFeel()) return;

        //        owoSkin.StartDrilling(true);
        //    }
        //}

        [HarmonyPatch(typeof(Welder), "StopWeldingFX")]
        public class StopWeldingFX
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.StopDrilling(true);
            }
        }

        [HarmonyPatch(typeof(LaserCutter), "StartLaserCuttingFX")]
        public class LaserCutterStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.StartDrilling(true);
            }
        }

        [HarmonyPatch(typeof(LaserCutter), "StopLaserCuttingFX")]
        public class LaserCutterStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.StopDrilling(true);
            }
        }

        [HarmonyPatch(typeof(BuilderTool), "OnRightHandDown")]
        public class OnBuilderTool
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (owoSkin.suitDisabled || !__result) return;

                owoSkin.Feel("Builder", 2);
            }
        }

        [HarmonyPatch(typeof(Constructor), "OnRightHandDown")]
        public class OnConstructor
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (CantFeel() || !__result) return;

                owoSkin.Feel("Constructor", 2);
            }
        }
        [HarmonyPatch(typeof(PropulsionCannon), "GrabObject")]
        public class OnPropulsionCannon
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.Feel("Grab Object",2);
            }
        }

        [HarmonyPatch(typeof(PropulsionCannon), "OnShoot")]
        public class OnPropulsionCannonShoot
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.Feel("Cannon Shot",3);
            }
        }

        [HarmonyPatch(typeof(StasisRifle), "OnRightHandDown")]
        public class OnStatisCharging
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (CantFeel() || !__result) return;

                owoSkin.Feel("Statis Charging", 2);
            }
        }

        #endregion

        #region Vehicles

        [HarmonyPatch(typeof(CyclopsExternalDamageManager), "OnTakeDamage")]
        public class OnCyclopDamage
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.Feel("Vehicle Impact",3);
            }
        }

        [HarmonyPatch(typeof(CyclopsEngineChangeState), "OnClick")]
        public class OnCyclopEngineStart
        {
            [HarmonyPrefix]
            public static void Prefix(CyclopsEngineChangeState __instance)
            {
                if (CantFeel()) return;

                if (!__instance.motorMode.engineOn)
                {
                    owoSkin.Feel("Jump Landing", 2);
                    //Plugin.owoSkin.Feel("LandAfterJump", true, 1f, 4f);
                }
            }
        }

        [HarmonyPatch(typeof(Vehicle), "PlaySplashSound")]
        public class OnSplash
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.Feel("Splash", 1);
            }
        }

        [HarmonyPatch(typeof(uGUI_SeamothHUD), "Update")]
        public class OnDamageVehicle
        {
            public static int health;

            [HarmonyPrefix]
            public static void Prefix(uGUI_SeamothHUD __instance)
            {
                if (CantFeel()) return;

                health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
            }

            [HarmonyPostfix]
            public static void Postfix(uGUI_SeamothHUD __instance)
            {
                if (CantFeel()) return;

                if (health > Traverse.Create(__instance).Field("lastHealth").GetValue<int>())
                {
                    owoSkin.Feel("Vehicle Impact", 3);

                    health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_ExosuitHUD), "Update")]
        public class OnDamageExosuit
        {
            public static int health;

            [HarmonyPrefix]
            public static void Prefix(uGUI_ExosuitHUD __instance)
            {
                if (CantFeel()) return;

                health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
            }

            [HarmonyPostfix]
            public static void Postfix(uGUI_ExosuitHUD __instance)
            {
                if (CantFeel()) return;
                var exo = Player.main.GetVehicle() as Exosuit;

                if (health > Traverse.Create(__instance).Field("lastHealth").GetValue<int>())
                {
                    owoSkin.Feel("Exosuit Impact", 3);

                    health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
                }
            }
        }

        [HarmonyPatch(typeof(Exosuit), "OnLand")]
        public class OnExosuitLanding
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (CantFeel() || !__instance.playerFullyEntered) return;
                owoSkin.Feel("Exosuit Landing", 2);
            }
        }

        [HarmonyPatch(typeof(Exosuit), "ApplyJumpForce")]
        public class OnExosuitJumping
        {
            static bool grounded = false;

            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance)
            {
                if (CantFeel()) return;

                if (grounded != Traverse.Create(__instance).Field("onGround").GetValue<bool>())
                {
                    owoSkin.Feel("Exosuit Pre Jump", 2);
                }

                grounded = Traverse.Create(__instance).Field("onGround").GetValue<bool>();
            }

            public static void Postfix(Exosuit __instance) {
                if (grounded != Traverse.Create(__instance).Field("onGround").GetValue<bool>())
                {
                    owoSkin.Feel("Exosuit Jump", 2);
                }

                grounded = Traverse.Create(__instance).Field("onGround").GetValue<bool>();
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotLeftDown")]
        public class OnExosuitArmDown
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (CantFeel()) return;

                if (__instance.mainAnimator.GetBool("use_tool_left"))
                {
                    TechType type = Traverse.Create(__instance).Field("currentLeftArmType").GetValue<TechType>();
                    if (oneShotTypes.Contains(type))
                    {
                        owoSkin.Feel("Recoil L",3);
                    }
                    if (type == TechType.ExosuitDrillArmModule)
                    {
                        owoSkin.StartDrilling(false);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotLeftUp")]
        public class OnExosuitArmUp
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (CantFeel()) return;

                TechType type = Traverse.Create(__instance).Field("currentLeftArmType").GetValue<TechType>();

                if (type == TechType.ExosuitDrillArmModule)
                {
                    owoSkin.StopDrilling(false);
                }
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotRightDown")]
        public class OnExosuitArmDownRight
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (CantFeel()) return;

                if (__instance.mainAnimator.GetBool("use_tool_right"))
                {
                    TechType type = Traverse.Create(__instance).Field("currentRightArmType").GetValue<TechType>();

                    if (oneShotTypes.Contains(type))
                    {
                        owoSkin.Feel("Recoil R", 3);
                    }

                    if (type == TechType.ExosuitDrillArmModule)
                    {
                        owoSkin.StartDrilling(true);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotRightUp")]
        public class OnExosuitArmUpRight
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (CantFeel()) return;

                TechType type = Traverse.Create(__instance).Field("currentRightArmType").GetValue<TechType>();

                if (type == TechType.ExosuitDrillArmModule)
                {
                    owoSkin.StopDrilling(true);
                }
            }
        }

        #endregion

        #region Health

        [HarmonyPatch(typeof(Player), "OnKill")]
        public class OnDeath
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.StopAllHapticFeedback();
                owoSkin.Feel("Death", 4);
                playerHasSpawned = false;
            }
        }

        [HarmonyPatch(typeof(Player), "OnTakeDamage")]
        public class OnTakeDamage
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;

                owoSkin.Feel("Impact", 3);
            }
        }

        [HarmonyPatch(typeof(uGUI_OxygenBar), "OnPulse")]
        public class OnLowOxygen
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_OxygenBar __instance)
            {
                if (CantFeel()) return;

                float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                if (pulseDelay >= 2 || !Utils.GetLocalPlayerComp().isUnderwaterForSwimming.value)
                {
                    owoSkin.StopLowOxygen();
                    return;
                }
                if (pulseDelay < 2)
                {
                    owoSkin.StartLowOxygen();
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_WaterBar), "OnPulse")]
        public class OnLowWaterStart
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_WaterBar __instance)
            {
                if (CantFeel()) return;

                float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                if (pulseDelay <= 0.85f)
                {
                    owoSkin.StartLowWater();
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_WaterBar), "OnDrink")]
        public class OnLowWaterStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                playerHasSpawned = true;
                if (CantFeel()) return;

                owoSkin.StopLowWater();
                owoSkin.Feel("Eating", 2);
            }
        }

        [HarmonyPatch(typeof(uGUI_FoodBar), "OnPulse")]
        public class OnLowFoodStart
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_FoodBar __instance)
            {
                if (CantFeel()) return;

                float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                if (pulseDelay <= 0.85f)
                {
                    owoSkin.StartLowFood();
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_FoodBar), "OnEat")]
        public class OnLowFoodStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                playerHasSpawned = true;
                if (CantFeel()) return;

                owoSkin.StopLowFood();
                owoSkin.Feel("Eating", 2);
            }
        }

        [HarmonyPatch(typeof(uGUI_HealthBar), "OnPulse")]
        public class OnLowHealthStart
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_HealthBar __instance)
            {
                if (CantFeel()) return;

                float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                if (pulseDelay <= 1.85f)
                {
                    owoSkin.StartHeartBeat();
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_HealthBar), "OnHealDamage")]
        public class OnLowHealthStop
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_HealthBar __instance)
            {
                if (CantFeel()) return;

                owoSkin.StopHeartBeat();
                owoSkin.Feel("Heal", 1);
            }
        }

        #endregion

        [HarmonyPatch(typeof(Player), "OnDestroy")]
        public class OnPlayerDestroy
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;
                owoSkin.StopAllHapticFeedback();
                playerHasSpawned = false;
            }
        }

        [HarmonyPatch(typeof(Player), "OnDisable")]
        public class OnPlayerDisabled
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (CantFeel()) return;
                owoSkin.StopAllHapticFeedback();
                playerHasSpawned = false;
            }
        }
    }
}
