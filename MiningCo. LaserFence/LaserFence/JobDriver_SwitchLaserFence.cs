﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using RimWorld;
//using RimWorld.Planet;


namespace LaserFence
{
    /// <summary>
    /// Order a pawn to go and switch the laser fences of a pylon.
    /// </summary>
    public class JobDriver_SwitchLaserFence : JobDriver
    {
        public TargetIndex pylonIndex = TargetIndex.A;

        public override bool TryMakePreToilReservations()
        {
            return this.pawn.Reserve(this.TargetA, this.job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(pylonIndex, PathEndMode.InteractionCell);

            yield return Toils_General.Wait(30);

            Toil switchLaserFenceToil = new Toil()
            {
                initAction = () =>
                {
                    (this.TargetThingA as Building_LaserFencePylon).SwitchLaserFence();
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return switchLaserFenceToil;

            yield return Toils_Reserve.Release(pylonIndex);
        }
    }
}
