using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace OWO_Subnautica
{
    [BepInPlugin("org.bepinex.plugins.OWO_Subnautica", "OWO_Subnautica", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
#pragma warning disable CS0109 // Remove unnecessary warning
        internal static new ManualLogSource Log;
#pragma warning restore CS0109
        public static OWOSkin owoSkin;
        public static bool startedHeart = false;
        public static float currentHealth = 0;
        public static bool inventoryOpened = false;
        public static long buttonPressTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        public static bool playerHasSpawned = false;

        private void Awake()
        {
            Log = base.Logger;
            Logger.LogMessage("OWO Integration plugin is loaded!");
            owoSkin = new OWOSkin();

            owoSkin.Feel("HeartBeat", 0);

            var harmony = new Harmony("owo.patch.subnautica");
            harmony.PatchAll();
        }
        #region Actions

        [HarmonyPatch(typeof(Inventory), "OnAddItem")]
        public class bhaptics_OnPickupItem
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.Feel("PickUpItem");
            }
        }

        [HarmonyPatch(typeof(Player), "LateUpdate")]
        public class bhaptics_OnSwimming
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                if (__instance.isUnderwaterForSwimming.value)
                {
                    //start thread
                    Plugin.owoSkin.StartSwimming();
                    //is seaglide equipped ?
                    //Plugin.owoSkin.seaGlideEquipped = __instance.motorMode == Player.MotorMode.Seaglide;
                    //calculate delay and intensity
                    int delay = (int)((30 - Math.Truncate(__instance.movementSpeed * 10)) * 100);
                    //Plugin.owoSkin.SwimmingDelay =(delay > 0)?(delay < 3000)? delay : 3000: 1000;

                    float intensity = (float)(Math.Truncate(__instance.movementSpeed * 10) / 30);
                    //Plugin.owoSkin.SwimmingIntensity = intensity;
                }
                else
                {
                    Plugin.owoSkin.StopSwimming();
                }
            }
        }

        [HarmonyPatch(typeof(PrecursorTeleporter), "OnPlayerCinematicModeEnd")]
        public class bhaptics_OnTeleportingStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StartTeleportation();
            }
        }

        [HarmonyPatch(typeof(Player), "CompleteTeleportation")]
        public class bhaptics_OnTeleportingStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StopTeleportation();
            }
        }

        [HarmonyPatch(typeof(Player), "UpdateIsUnderwater")]
        public class bhaptics_OnEnterExitWater
        {
            public static bool isUnderwaterForSwimming = false;

            [HarmonyPrefix]
            public static void Prefix(Player __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                isUnderwaterForSwimming = __instance.isUnderwaterForSwimming.value;
            }

            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                if (__instance.isUnderwaterForSwimming.value != isUnderwaterForSwimming)
                {
                    Plugin.owoSkin.Feel(
                        (!isUnderwaterForSwimming) ? "EnterWater_Vest" : "ExitWater_Vest");
                    Plugin.owoSkin.Feel(
                        (!isUnderwaterForSwimming) ? "EnterWater_Arms" : "ExitWater_Arms");
                    Plugin.owoSkin.Feel("waterVisor");
                }
                isUnderwaterForSwimming = __instance.isUnderwaterForSwimming.value;
            }
        }

        #endregion

        #region Equipments

        [HarmonyPatch(typeof(Knife), "IsValidTarget")]
        public class bhaptics_OnKnifeAttack
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (Plugin.owoSkin.suitDisabled || !__result)
                {
                    return;
                }
                Plugin.owoSkin.Feel("RecoilArm_R");
                Plugin.owoSkin.Feel("RecoilVest_R");
            }
        }

        [HarmonyPatch(typeof(StasisRifle), "Fire")]
        public class bhaptics_OnStatisAttack
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.Feel("RecoilArm_R");
                Plugin.owoSkin.Feel("RecoilVest_R");
            }
        }

        [HarmonyPatch(typeof(ScannerTool), "OnRightHandDown")]
        public class bhaptics_Onscanning
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (Plugin.owoSkin.suitDisabled || !__result)
                {
                    return;
                }
                Plugin.owoSkin.Feel("Scanning_Vest");
                Plugin.owoSkin.Feel("Scanning_Arm_R");
            }
        }
        [HarmonyPatch(typeof(AirBladder), "OnRightHandDown")]
        public class bhaptics_OnAirBladder
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (Plugin.owoSkin.suitDisabled || !__result)
                {
                    return;
                }
                Plugin.owoSkin.Feel("Scanning_Vest");
                Plugin.owoSkin.Feel("Scanning_Arm_R");
            }
        }

        [HarmonyPatch(typeof(Welder), "Weld")]
        public class bhaptics_Welder
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StartDrilling("right");
            }
        }

        [HarmonyPatch(typeof(Welder), "StopWeldingFX")]
        public class bhaptics_StopWeldingFX
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StopDrilling("right");
            }
        }

        [HarmonyPatch(typeof(LaserCutter), "StartLaserCuttingFX")]
        public class bhaptics_LaserCutterStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StartDrilling("right");
            }
        }

        [HarmonyPatch(typeof(LaserCutter), "StopLaserCuttingFX")]
        public class bhaptics_LaserCutterStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StopDrilling("right");
            }
        }

        [HarmonyPatch(typeof(BuilderTool), "OnRightHandDown")]
        public class bhaptics_OnBuilderTool
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (Plugin.owoSkin.suitDisabled || !__result)
                {
                    return;
                }
                Plugin.owoSkin.Feel("Scanning_Vest");
                Plugin.owoSkin.Feel("Scanning_Arm_R");
            }
        }

        [HarmonyPatch(typeof(Constructor), "OnRightHandDown")]
        public class bhaptics_OnConstructor
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (Plugin.owoSkin.suitDisabled || !__result)
                {
                    return;
                }
                Plugin.owoSkin.Feel("Scanning_Vest");
                Plugin.owoSkin.Feel("Scanning_Arm_R");
            }
        }
        [HarmonyPatch(typeof(PropulsionCannon), "GrabObject")]
        public class bhaptics_OnPropulsionCannon
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.Feel("Scanning_Vest");
                Plugin.owoSkin.Feel("Scanning_Arm_R");
            }
        }

        [HarmonyPatch(typeof(PropulsionCannon), "OnShoot")]
        public class bhaptics_OnPropulsionCannonShoot
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.Feel("RecoilArm_R");
                Plugin.owoSkin.Feel("RecoilVest_R");
            }
        }

        [HarmonyPatch(typeof(StasisRifle), "OnRightHandDown")]
        public class bhaptics_OnStatisCharging
        {
            [HarmonyPostfix]
            public static void Postfix(bool __result)
            {
                if (Plugin.owoSkin.suitDisabled || !__result)
                {
                    return;
                }
                Plugin.owoSkin.Feel("Scanning_Vest");
                Plugin.owoSkin.Feel("Scanning_Arm_R");
            }
        }

        #endregion

        #region Vehicles

        [HarmonyPatch(typeof(CyclopsExternalDamageManager), "OnTakeDamage")]
        public class bhaptics_OnCyclopDamage
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.Feel("VehicleImpact_Arms");
                Plugin.owoSkin.Feel("VehicleImpact_Vest");
                Plugin.owoSkin.Feel("impactVisor");
            }
        }

        [HarmonyPatch(typeof(CyclopsEngineChangeState), "OnClick")]
        public class bhaptics_OnCyclopEngineStart
        {
            [HarmonyPrefix]
            public static void Prefix(CyclopsEngineChangeState __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                if (!__instance.motorMode.engineOn)
                {
                    Plugin.owoSkin.Feel("LandAfterJump");
                    //Plugin.owoSkin.Feel("LandAfterJump", true, 1f, 4f);
                }
            }
        }

        [HarmonyPatch(typeof(Vehicle), "PlaySplashSound")]
        public class bhaptics_OnSplash
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.Feel("Splash_Arms");
                Plugin.owoSkin.Feel("Splash_Vest");
                Plugin.owoSkin.Feel("waterVisor");
            }
        }

        [HarmonyPatch(typeof(uGUI_SeamothHUD), "Update")]
        public class bhaptics_OnDamageVehicle
        {
            public static int health;

            [HarmonyPrefix]
            public static void Prefix(uGUI_SeamothHUD __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
            }

            [HarmonyPostfix]
            public static void Postfix(uGUI_SeamothHUD __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                if (health > Traverse.Create(__instance).Field("lastHealth").GetValue<int>())
                {
                    Plugin.owoSkin.Feel("VehicleImpact_Arms");
                    Plugin.owoSkin.Feel("VehicleImpact_Vest");
                    Plugin.owoSkin.Feel("impactVisor");
                    health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_ExosuitHUD), "Update")]
        public class bhaptics_OnDamageExosuit
        {
            public static int health;

            [HarmonyPrefix]
            public static void Prefix(uGUI_ExosuitHUD __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
            }

            [HarmonyPostfix]
            public static void Postfix(uGUI_ExosuitHUD __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                if (health != Traverse.Create(__instance).Field("lastHealth").GetValue<int>())
                {
                    Plugin.owoSkin.Feel("VehicleImpact_Arms");
                    Plugin.owoSkin.Feel("VehicleImpact_Vest");
                    Plugin.owoSkin.Feel("impactVisor");
                    health = Traverse.Create(__instance).Field("lastHealth").GetValue<int>();
                }
            }
        }

        [HarmonyPatch(typeof(Exosuit), "OnLand")]
        public class bhaptics_OnExosuitLanding
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.Feel("LandAfterJump");
            }
        }

        [HarmonyPatch(typeof(Exosuit), "ApplyJumpForce")]
        public class bhaptics_OnExosuitJumping
        {
            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                bool grounded = Traverse.Create(__instance).Field("onGround").GetValue<bool>();
                double timeLastJumped = (double)(Traverse.Create(__instance)
                    .Field("timeLastJumped").GetValue<float>()
                    + 1.0);
                Plugin.Log.LogWarning("JUMP " + grounded + " " + timeLastJumped + " " + (double)Time.time);
                if (grounded && timeLastJumped <= (double)Time.time)
                {
                    Plugin.owoSkin.Feel("LandAfterJump");
                }
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotLeftDown")]
        public class bhaptics_OnExosuitArmDown
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //if (__instance.mainAnimator.GetBool("use_tool_left"))
                //{
                //    TechType type = Traverse.Create(__instance).Field("currentLeftArmType")
                //        .GetValue<TechType>();
                //    if (Plugin.oneShotTypes.Contains(type))
                //    {
                //        Plugin.owoSkin.Feel("RecoilArm_L");
                //        Plugin.owoSkin.Feel("RecoilVest_L");
                //    }
                //    if (type == TechType.ExosuitDrillArmModule)
                //    {
                //        Plugin.owoSkin.StartDrilling("left");
                //    }
                //}
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotLeftUp")]
        public class bhaptics_OnExosuitArmUp
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                TechType type = Traverse.Create(__instance).Field("currentLeftArmType")
                       .GetValue<TechType>();
                if (type == TechType.ExosuitDrillArmModule)
                {
                    //Plugin.owoSkin.StopDrilling("left");
                }
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotRightDown")]
        public class bhaptics_OnExosuitArmDownRight
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //if (__instance.mainAnimator.GetBool("use_tool_right"))
                //{
                //    TechType type = Traverse.Create(__instance).Field("currentRightArmType")
                //        .GetValue<TechType>();
                //    if (Plugin.oneShotTypes.Contains(type))
                //    {
                //        Plugin.owoSkin.Feel("RecoilArm_R");
                //        Plugin.owoSkin.Feel("RecoilVest_R");
                //    }
                //    if (type == TechType.ExosuitDrillArmModule)
                //    {
                //        Plugin.owoSkin.StartDrilling("right");
                //    }
                //}
            }
        }

        [HarmonyPatch(typeof(Exosuit), "SlotRightUp")]
        public class bhaptics_OnExosuitArmUpRight
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                TechType type = Traverse.Create(__instance).Field("currentRightArmType")
                       .GetValue<TechType>();
                if (type == TechType.ExosuitDrillArmModule)
                {
                    //Plugin.owoSkin.StopDrilling("right");
                }
            }
        }

        #endregion

        #region Health

        [HarmonyPatch(typeof(Player), "OnKill")]
        public class bhaptics_OnDeath
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.StopAllHapticFeedback();
                Plugin.owoSkin.Feel("Death", 4);
            }
        }

        [HarmonyPatch(typeof(Player), "OnTakeDamage")]
        public class bhaptics_OnTakeDamage
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.Feel("Impact", true, 2f);
                Plugin.owoSkin.Feel("impact");
            }
        }

        [HarmonyPatch(typeof(uGUI_OxygenBar), "OnPulse")]
        public class bhaptics_OnLowOxygen
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_OxygenBar __instance)
            {
                //if (Plugin.owoSkin.suitDisabled)
                //{
                //    return;
                //}
                //float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                //if (pulseDelay >= 2 || !Utils.GetLocalPlayerComp().isUnderwaterForSwimming.value)
                //{
                //    Plugin.owoSkin.StopLowOxygen();
                //    return;
                //}
                //if (pulseDelay < 2)
                //{
                //    Plugin.owoSkin.StartLowOxygen();
                //}
            }
        }

        [HarmonyPatch(typeof(uGUI_WaterBar), "OnPulse")]
        public class bhaptics_OnLowWaterStart
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_WaterBar __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                if (pulseDelay <= 0.85f)
                {
                    //Plugin.owoSkin.StartLowWater();
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_WaterBar), "OnDrink")]
        public class bhaptics_OnLowWaterStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StopLowWater();
                Plugin.owoSkin.Feel("Eating");
            }
        }

        [HarmonyPatch(typeof(uGUI_FoodBar), "OnPulse")]
        public class bhaptics_OnLowFoodStart
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_FoodBar __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                if (pulseDelay <= 0.85f)
                {
                    //Plugin.owoSkin.StartLowFood();
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_FoodBar), "OnEat")]
        public class bhaptics_OnLowFoodStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                //Plugin.owoSkin.StopLowFood();
                Plugin.owoSkin.Feel("Eating");
            }
        }

        [HarmonyPatch(typeof(uGUI_HealthBar), "OnPulse")]
        public class bhaptics_OnLowHealthStart
        {
            [HarmonyPostfix]
            public static void Postfix(uGUI_HealthBar __instance)
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                float pulseDelay = Traverse.Create(__instance).Field("pulseDelay").GetValue<float>();
                if (pulseDelay <= 1.85f)
                {
                    Plugin.owoSkin.StartHeartBeat();
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_HealthBar), "OnHealDamage")]
        public class bhaptics_OnLowHealthStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.StopHeartBeat();
                Plugin.owoSkin.Feel("Heal");
            }
        }

        #endregion

        [HarmonyPatch(typeof(Player), "OnDestroy")]
        public class bhaptics_OnPlayerDestroy
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.StopAllHapticFeedback();
            }
        }

        [HarmonyPatch(typeof(Player), "OnDisable")]
        public class bhaptics_OnPlayerDisabled
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Plugin.owoSkin.suitDisabled)
                {
                    return;
                }
                Plugin.owoSkin.StopAllHapticFeedback();
            }
        }
    }
}
