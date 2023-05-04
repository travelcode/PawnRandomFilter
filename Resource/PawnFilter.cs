using RimWorld;
using System.Collections;
using System.Reflection;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    internal class PawnFilter
    {
        private Page_ConfigureStartingPawns? startingPage;
        private readonly FilterSettings settings = FilterSettings.Instance;
        private static PawnFilter? instance;
        private bool running = false;
        private int totalTimes;
        private int curTimes;
        private PawnFilter()
        {
            totalTimes = settings.MaxTimes;
        }
        public static PawnFilter Instance => instance ??= new PawnFilter();

        public bool Running => running;
        public void RestPawn(Page_ConfigureStartingPawns __instance)
        {
            startingPage = __instance;
            Pawn pawn = GetCurPawn();
            pawn = settings.RsetPawn(pawn);
            SetCurPawn(pawn);
            running = false;
            totalTimes = 0;

        }
        public void DoFilter(Page_ConfigureStartingPawns __instance)
        {
            if (settings.FilterMode == FilterMode.ChosenOne)
            {
                RestPawn(__instance);
                return;
            }
            startingPage = __instance;
            Initialize();
            Current.Root.StartCoroutine(FilterRandomizePawn());
        }

        private void Initialize()
        {
            if (!running)
            {
                totalTimes = settings.MaxTimes;
                curTimes = 0;
                running = true;
            }
        }
        public void Stop(StopRandomReason reason)
        {
            Current.Root.StopCoroutine("FilterRandomizePawn");
            running = false;
            DialogFliterMessage alert = new DialogFliterMessage
            {
                Text = GetResonText(reason)
            };
            Find.WindowStack.Add(alert);
        }

        private string GetResonText(StopRandomReason reason)
        {
            string key = "StopRandomReason." + reason.ToString();
            string text = key.Translate();
            string maxTimes = settings.MaxTimes.ToString();
            text = text.Replace("{maxTimes}", maxTimes);
            text = text.Replace("{curTimes}", curTimes.ToString());
            return text;
        }

        private IEnumerator FilterRandomizePawn()
        {
            for (int i = 0; i < totalTimes; i++)
            {
                curTimes = i + 1;
                if (!running)
                {
                    //this.Stop(StopRandomReason.None);
                    yield break;
                }
                Pawn? pawn = RandomizeCurPawn();
                if (pawn == null)
                {
                    Stop(StopRandomReason.PawnNull);
                    yield break;
                }
                if (MatchCondition(pawn))
                {
                    Stop(StopRandomReason.Find);
                    yield return true;
                }
                yield return false;
            }
            Stop(StopRandomReason.MaxTimes);
            yield break;
        }
        private bool MatchCondition(Pawn pawn)
        {
            return settings.Match(pawn);
        }
        private Pawn? RandomizeCurPawn()
        {
            Pawn? curPawn = null;
            if (TutorSystem.AllowAction("RandomizePawn"))
            {
                int num = 0;
                curPawn = GetCurPawn();
                do
                {
                    SpouseRelationUtility.Notify_PawnRegenerated(curPawn);
                    curPawn = StartingPawnUtility.RandomizeInPlace(curPawn);
                    num++;
                }
                while (num <= 20 && !StartingPawnUtility.WorkTypeRequirementsSatisfied());
                TutorSystem.Notify_Event("RandomizePawn");
                SetCurPawn(curPawn);
            }
            return curPawn;
        }
        private Pawn GetCurPawn()
        {
            FieldInfo curPawnFieldInfo = GetCurPawnFileldInfo();
            return (Pawn)curPawnFieldInfo.GetValue(startingPage);
        }

        private static FieldInfo GetCurPawnFileldInfo()
        {
            FieldInfo fieldInfo = typeof(Page_ConfigureStartingPawns).GetField("curPawn", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo curPawnFieldInfo = fieldInfo;
            return curPawnFieldInfo;
        }   

        private void SetCurPawn(Pawn curPawn)
        {
            FieldInfo curPanwFiledInfo = GetCurPawnFileldInfo();
            curPanwFiledInfo.SetValue(startingPage, curPawn);
        }

    }
}
