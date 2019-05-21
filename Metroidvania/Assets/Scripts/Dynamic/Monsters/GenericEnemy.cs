using UnityEngine;

public class GenericEnemy : Controller, IDamagable {

    private const int DEAD_FRAMES = 100;

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

    [SerializeField, Autohook]
    private SpriteRenderer f_spriteRenderer;

    #endregion

    #region [PrivateVariables]

    private float m_health;
    private bool m_living = true;
    private int m_deadFrames;

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

    #region [Updates]

    protected override void FixedUpdate() {
        if (m_living) {
            base.FixedUpdate();
        } else {
            if (m_deadFrames == DEAD_FRAMES) {
                Destroy(gameObject);
            }

            ++m_deadFrames;
        }
    }

    #endregion

    // trigger hit with someone will happen with just adding a hit to this

    #region [Override]

    public void TakeDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        if (healthAfter == 0) {
            Consts.Instance.Player.Energy += f_loot.DropEnergy;
            m_living = false;
            f_health.gameObject.SetActive(false);
            Animator.enabled = false;
            f_spriteRenderer.color = Color.black;
            return;
        }

        if (!f_noImpact) {
            ReactOnImpact(hitNormal);
        }

    }

    #endregion

}