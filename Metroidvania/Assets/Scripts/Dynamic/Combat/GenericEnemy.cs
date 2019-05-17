using UnityEngine;

public class GenericEnemy : MonoBehaviour, IDamagable {

    #region [MemberFields]

    //TODO all that stuff in SO

    [SerializeField]
    private int f_maxHealth;

    [SerializeField]
    private float f_weight;

    [SerializeField]
    private LootDrop f_loot;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private Health f_health;

    [SerializeField, Autohook]
    private Controller f_controller;

    #endregion

    #region [PrivateVariables]

    private float m_health;

    #endregion

    #region [Init]

    private void Awake() {
        m_health = f_maxHealth;

        f_health.Init(f_maxHealth, f_weight);
    }

    #endregion

    #region [Override]

    public void TakeDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        f_controller.ReactOnImpact(hitNormal);
    }

    #endregion

}