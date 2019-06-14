using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamageTaker {

    #region [Override]

    public void TakeDamage(int amount, Vector2 hitNormal) {
        Destroy(gameObject);
    }

    #endregion

}