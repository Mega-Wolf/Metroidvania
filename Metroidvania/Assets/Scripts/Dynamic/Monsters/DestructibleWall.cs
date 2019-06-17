using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamageTaker {

    #region [MemberFields]

    [SerializeField] private MonoBehaviour[] f_blocked;

    #endregion

    #region [Init]

    private void Start() {
        for (int i = 0; i < f_blocked.Length; ++i) {
            f_blocked[i].enabled = false;
        }
    }

    #endregion

    #region [Override]

    public Controller Controller { get { return null; } }

    public void TakeDamage(int amount, Vector2 hitNormal) {
        Destroy(gameObject);
        for (int i = 0; i < f_blocked.Length; ++i) {
            f_blocked[i].enabled = true;
        }
    }

    #endregion

}