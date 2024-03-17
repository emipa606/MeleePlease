using Verse;

namespace MeleePlease;

public class DamageDefCleave : DamageDef
{
    public readonly float armorPenetration = 0f;
    public readonly DamageDef cleaveDamage = null;
    public readonly float cleaveFactor = 0.7f; //Damage factor for the cleave attack
    public readonly int cleaveTargets = 0; //Number of bonus targets
}