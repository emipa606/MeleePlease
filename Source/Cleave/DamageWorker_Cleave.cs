using RimWorld;
using Verse;

namespace MeleePlease;

public class DamageWorker_Cleave : DamageWorker_AddInjury
{
    private DamageDefCleave Def => def as DamageDefCleave;


    /// CALCULATION NOTES:
    /// Cleave calculation is determined by a few factors.
    /// 0) Only pursue these calculations if 0 is set as target flag.
    /// 1) Pawns with weapons will use the mass to decide
    /// how many adjacent targets are hit with the cleave attack.
    /// 2) Pawns without weapons will use their body size.
    /// 3) Otherwise, only 1 additional attack.
    protected virtual int NumToCleave(Thing t)
    {
        if (Def.cleaveTargets != 0)
        {
            return Def.cleaveTargets;
        }

        if (t is not Pawn p)
        {
            return 1;
        }

        if (p.equipment?.Primary is { } w)
        {
            return (int)w.GetStatValue(StatDefOf.Mass);
        }

        return (int)p.BodySize;
    }

    public override DamageResult Apply(DamageInfo dinfo, Thing victim)
    {
        if (dinfo is { InstantPermanentInjury: false, Instigator: not null })
        {
            const float maxDist = 4;
            var cleaveAttacks = NumToCleave(dinfo.Instigator);
            if (victim?.PositionHeld != default(IntVec3))
            {
                for (var i = 0; i < 8; i++)
                {
                    if (victim == null)
                    {
                        continue;
                    }

                    var c = victim.PositionHeld + GenAdj.AdjacentCells[i];
                    if (cleaveAttacks <= 0 ||
                        !((dinfo.Instigator.Position - c).LengthHorizontalSquared < maxDist))
                    {
                        continue;
                    }

                    var pawnsInCell = c.GetThingList(victim.Map).FindAll(x =>
                        x is Pawn && x != dinfo.Instigator && x.Faction != dinfo.Instigator.Faction);
                    for (var k = 0; cleaveAttacks > 0 && k < pawnsInCell.Count; k++)
                    {
                        --cleaveAttacks;
                        var p = (Pawn)pawnsInCell[k];
                        p.TakeDamage(new DamageInfo(Def.cleaveDamage,
                            (int)(dinfo.Amount * Def.cleaveFactor), Def.armorPenetration, -1,
                            dinfo.Instigator));
                    }
                }
            }
        }

        var result = base.Apply(dinfo, victim);
        return result;
    }
}