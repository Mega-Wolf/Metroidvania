using UnityEngine;

public class GenericEnemy : Controller, IDamagable {

    #region [MemberFields]

    //TODO all that stuff in SO

    [SerializeField]
    private int f_maxHealth;

    [SerializeField]
    private float f_weight;

    [SerializeField]
    private LootDrop f_loot;

    [SerializeField]
    private Damage[] f_allAttacks;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private Health f_health;

    [SerializeField, Autohook]
    private DummyState f_dummyState;

    #endregion

    #region [PrivateVariables]

    private float m_health;

    #endregion

    #region [Init]

    private void Awake() {
        m_health = f_maxHealth;

        SetStartState(f_dummyState);
    }

    private void Start() {
        f_health.Init(f_maxHealth, f_weight);
        f_health.Add(this);
    }

    #endregion

    // trigger hit with someone will happen with just adding a hit to this

    #region [Override]

    public void TakeDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        ReactOnImpact(hitNormal);
    }

    #endregion

}