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

    #region [PrivateVariables]

    private float m_health;
    private bool m_living = true;
    private int m_deadFrames;

    #endregion

    #region [Init]

    protected override void Awake() {
        base.Awake();
        m_health = f_maxHealth;
        f_health.Init(f_maxHealth, f_weight, this);
        f_health.Add(this);
    }

    private void OnEnable() {
        f_health.Visibility = true;
    }

    public virtual void Start() {}

    private void OnDisable() {
        f_health.Visibility = false;
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

    public virtual void HandleDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        if (m_living && healthAfter <= 0) {
            Consts.Instance.Player.Energy += f_loot.DropEnergy;
            m_living = false;

            Damage[] damages = GetComponentsInChildren<Damage>();
            foreach (Damage damage in damages) {
                damage.enabled = false;
            }

            Animator.enabled = false;
            f_spriteRenderer.color = Color.black;
            return;
        }

        if (!f_noImpact) {
            ReactOnImpact(hitNormal);
            if (hitNormal.sqrMagnitude > 0) {
                float angle = Vector2.Angle(hitNormal, transform.right);
                Backwards = angle < 90 ? true : false;
            }
        }

    }

    protected override void Move() {
        if (GroundMovement != null) {
            Grounded = GroundMovement.TryStickToGround(false);
        }

        if (!Grounded) {
            Movement.AirMove();
        }
        base.Move();
    }

    #endregion

}