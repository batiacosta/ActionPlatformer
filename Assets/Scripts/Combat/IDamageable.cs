using UnityEngine;

namespace Combat
{
    public interface IDamageable: IHitable
    {
        void TakeDamage(Vector2 damageSourceDirection, int damageAmount, float knockbackThrust);
    }
}