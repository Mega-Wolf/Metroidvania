using System.Collections.Generic;
using UnityEngine;
using static GroundMovementRaycast;

public class Charge : ControllerState, IDamager {

    #region [Consts]

    [SerializeField]
    private IntSO FRAMES_ATTENTION;

    [SerializeField]
    private IntSO CHARGE_DAMAGE;

    [SerializeField]
    private FloatSO CHARGE_SPEED;

    [SerializeField]
    private IntSO CHARGE_COOLTIME;

    private Color COLOR_NORMAL = Color.yellow;
    private Color COLOR_RAGE = Color.red;

    #endregion

    #region [MemberFields]

    [SerializeField]
    private bool m_stopAtEdges = true;

    [SerializeField] private bool f_runOver;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook] private SpriteRenderer f_exclamationMark;

    [SerializeField]
    private EDamageReceiver f_damageReceiver;

    [SerializeField]
    private Collider2D f_chargeRange;

    [SerializeField]
    private Collider2D f_chargeHit;

    [SerializeField, Autohook]
    private Damage f_damage;

    #endregion

    #region [PrivateVariables]

    private int m_seeingEnemy = 0;

    private List<ControllerState> m_futureStatesHelper;
    private List<ControllerState> m_emptyFutureDummy = new List<ControllerState>();

    private int m_cooldown;

    #endregion

    private void OnValidate() {
        f_damage.Init(f_damageReceiver, 0, this);
    }

    #region [Override]

    public override bool EnterOnCondition() {
        // The charge state gets entered when there is something visible in the look collider of the monster

        List<Collider2D> colliderList = DamageHelper.ContactList;

        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)f_damageReceiver);
        cf.useLayerMask = true;
        int amount = f_chargeRange.OverlapCollider(cf, colliderList);

        if (amount > 0) {
            ++m_seeingEnemy;
            if (m_seeingEnemy == 1) {
                f_exclamationMark.enabled = true;
            }
        } else {
            if (m_seeingEnemy > 0) {
                --m_seeingEnemy;
                if (m_seeingEnemy == 0) {
                    f_exclamationMark.enabled = false;
                }
            }
        }

        f_exclamationMark.color = Color.Lerp(COLOR_NORMAL, COLOR_RAGE, m_seeingEnemy / (float)FRAMES_ATTENTION);

        if (m_seeingEnemy == FRAMES_ATTENTION) {
            f_exclamationMark.enabled = false;
        }

        return m_seeingEnemy == FRAMES_ATTENTION;
    }

    public override void LogicalEnter() {
        f_damage.Init(f_damageReceiver, 0, this);

        m_cooldown = -1;

        m_futureStatesHelper = f_futureStates;
        f_futureStates = m_emptyFutureDummy;

        //f_controller.Grounded = true;
        f_controller.Backwards = false;

        f_damage.ExecuteHit(CHARGE_DAMAGE, /*f_controller.LookDirection */ f_controller.Velocity);

        m_seeingEnemy = 0;
    }

    public override void EffectualEnter() {
        f_controller.Animator.Play("Charge");
    }

    public override bool HandleFixedUpdate() {
        //TODO; this is such a hack
        if (InputManager.Instance.IgnoreInput) {
            return true;
        }

        if (!f_controller.Grounded) {
            return true;
        }

        if (m_cooldown == -1) {
            if (f_controller.Backwards) {
                f_controller.Backwards = false;
                // if (f_controller.Velocity.x != 0) {
                //     m_walkingRight = 
                // }
            }

            if (Mathf.Abs(f_controller.Velocity.x) < 0.1f) {
                Cancel();
            }

            //TODO; does not care about transform
            if (f_controller.Velocity.x < -0.1f) {
                f_controller.GroundMovement.MovingRight = false;
            } else if (f_controller.Velocity.x > 0.1f) {
                f_controller.GroundMovement.MovingRight = true;
            }

            f_controller.Velocity = Vector2.zero;

            GroundTouch gt = f_controller.GroundMovement.Move((f_controller.GroundMovement.MovingRight ? 1 : -1) * CHARGE_SPEED);

            if (m_stopAtEdges) {
                int airDirection = GroundMovementRaycast.AirDirection(gt);
                if (airDirection != 0) {
                    Cancel();
                }
            }
        } else {
            if (m_cooldown == CHARGE_COOLTIME) {
                f_futureStates = m_futureStatesHelper;
                m_cooldown = -1;
            }

            ++m_cooldown;
        }

        return true;
    }

    public void Damaged(IDamageTaker health) {
        if (!f_runOver) {
            f_controller.GroundMovement.MovingRight = !f_controller.GroundMovement.MovingRight;
            f_controller.Velocity = new Vector2(-f_controller.Velocity.x, f_controller.Velocity.y);
            f_damage.Abort();
            f_futureStates = m_futureStatesHelper;
        }
    }

    public override void Abort() {
        //f_controller.Velocity = Vector2.zero;
    }

    #endregion

    #region [PrivateMethods]

    private void Cancel() {
        f_controller.Velocity = Vector2.zero;
        m_cooldown = 0;
        f_damage.Abort();
        f_controller.Animator.Play("Smash");
    }

    #endregion

}