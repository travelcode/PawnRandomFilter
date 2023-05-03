using RimWorld;
using System.Collections;
using System.Reflection;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    internal class PawnFilter
    {
        private Page_ConfigureStartingPawns? startingPage;
        private readonly FilterSettings settings = FilterSettings.Instance;
        private static PawnFilter? instance;
        private bool running = false;
        private int curTimes;
        private PawnFilter()
        {
            curTimes = settings.MaxTimes;
        }
        public static PawnFilter Instance => instance ??= new PawnFilter();

        public bool Running => this.running;
        public void RestPawn(Page_ConfigureStartingPawns __instance)
        {
            this.startingPage = __instance;
            Pawn pawn = this.GetCurPawn();
            pawn=settings.RsetPawn(pawn);
            this.SetCurPawn(pawn);
            this.running = false;
            this.curTimes = 0;

        }
        public void DoFilter(Page_ConfigureStartingPawns __instance)
        {
            if(settings.FilterMode==FilterMode.ChosenOne)
            {
                RestPawn(__instance);
                return;
            }
            this.startingPage = __instance;
            Initialize();
            Current.Root.StartCoroutine(FilterRandomizePawn());
        }

        private void Initialize()
        {
            if (!running)
            {
                this.curTimes = settings.MaxTimes;
                running = true;
            }
        }
        public void Stop()
        {
            this.curTimes = 0;
            Current.Root.StopCoroutine(FilterRandomizePawn());
            running = false;
        }
        private IEnumerator FilterRandomizePawn()
        {
            for (int i = 0; i < curTimes; i++)
            {
                if (!this.running)
                {
                    yield break;
                }
                Pawn? pawn = RandomizeCurPawn();
                if (pawn == null)
                {
                    this.Stop();
                    yield break;
                }
                if (MatchCondition(pawn))
                {
                    this.Stop();
                    yield return true;
                }
                yield return false;
            }
            this.Stop();
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
            return (Pawn)curPawnFieldInfo.GetValue(this.startingPage);
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
            curPanwFiledInfo.SetValue(this.startingPage, curPawn);
        }

    }
}
