﻿using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TimeLord.Patches
{
    internal sealed class DefaultMobilePartyFoodConsumptionModelPatch : Patch
    {
        internal DefaultMobilePartyFoodConsumptionModelPatch()
            : base(Type.Postfix, TargetMethod, PatchMethod, HarmonyLib.Priority.Last) { }

        private static readonly Reflect.Method<DefaultMobilePartyFoodConsumptionModel> TargetMethod = new("CalculateDailyFoodConsumptionf");
        private static readonly Reflect.Method<DefaultMobilePartyFoodConsumptionModelPatch> PatchMethod = new(nameof(CalculateDailyFoodConsumptionf));
        private static readonly TextObject Explanation = new($"[{Main.DisplayName}] Time Multiplier");

        private static void CalculateDailyFoodConsumptionf(MobileParty party, ref ExplainedNumber __result)
        {
            try
            {
                _ = (party);

                if (!Main.Settings!.EnableFoodTweaks)
                {
                    return;
                }

                var offset = (__result.ResultNumber / Main.Settings.TimeMultiplier) - __result.ResultNumber;

                if (!Util.NearEqual(offset, 0f, 1e-2f))
                {
                    __result.Add(offset, Explanation);
                }
            }
            catch (System.Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace); Debug.WriteDebugLineOnScreen(e.ToString());  Debug.SetCrashReportCustomString(e.Message); Debug.SetCrashReportCustomStack(e.StackTrace); 
            }
        }
    }
}