using UnityEngine;

public class GenericEnemy : Controller, IDamagable {

    #region [MemberFields]

    //TODO all that stuff in SO

    [SerializeField]
    private bool f_noImpact;

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

    #endregion

    #region [PrivateVariables]

    private float m_health;

    #endregion

    #region [Init]

    protected override void Awake() {
        base.Awake();
        m_health = f_maxHealth;
    }

    protected virtual void Start() {
        f_health.Init(f_maxHealth, f_weight);
        f_health.Add(this);
    }

    #endregion

    // trigger hit with someone will happen with just adding a hit to this

    #region [Override]

    public void TakeDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        if (healthAfter == 0) {
            Consts.Instance.Player.Energy += f_loot.DropEnergy;
            return;
        }

        if (!f_noImpact) {
            ReactOnImpact(hitNormal);
        }

    }

    #endregion

}