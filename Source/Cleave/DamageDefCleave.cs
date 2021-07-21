using Verse;

namespace MeleePlease
{
    public class DamageDefCleave : DamageDef
    {
        public float armorPenetration = 0f;
        public DamageDef cleaveDamage = null;
        public float cleaveFactor = 0.7f; //Damage factor for the cleave attack
        public int cleaveTargets = 0; //Number of bonus targets
    }
}