using UnityEngine;
/// <summary>
/// The Health component collects all these on the same GO and executes them all
/// Mind the order of them on the GO
/// </summary>
public interface IDamagable {
    
    void TakeDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal);

}