using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour, IDamager {

    public static float ACCURACY = 0f;
    public static float SPEED_FACTOR = 1f;

    #region [MemberFields]

    [SerializeField] private float f_speed;
    [SerializeField] private float f_b;
    [SerializeField] private int f_damageValue;
    [SerializeField] private int f_cooldownExplosionFrames;
    [SerializeField] private int f_explodeFrameCount;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook] private Damage f_damage;
    [SerializeField, Autohook] private Animator f_aniamtor;

    #endregion

    #region [PrivateVariables]

    private float m_a;
    private float m_b;
    private bool m_right;
    private bool m_shallMove = true;
    private int m_explodeCooldown;
    private Vector3 m_originalScale;

    #endregion

    #region [Init]

    private void OnValidate() {
        if (f_damage) {
            f_damage.Init(EDamageReceiver.Player | EDamageReceiver.Default, -1, this, false, true);
        }
    }

    private void Start() {
        OnValidate();
        f_damage.ExecuteHit(f_damageValue, Vector2.zero);
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        if (m_shallMove) {
            float newX = transform.localPosition.x + SPEED_FACTOR * f_speed / 50f * (m_right ? 1 : -1);
            transform.localPosition = new Vector3(newX, newX * (m_a * newX + m_b), transform.localPosition.z);
        } else {
            --m_explodeCooldown;
            if (m_explodeCooldown == 0) {
                f_aniamtor.enabled = true;
                f_aniamtor.Play("Explode");
            }
            if (m_explodeCooldown > 0) {
                Vector2 pseudoGoal = Vector3.one * (0.15f * Mathf.Sin(5 * 2 * Mathf.PI * (m_explodeCooldown / (float)f_cooldownExplosionFrames)) + 1);
                Vector2 goal = pseudoGoal * m_originalScale;
                transform.localScale = new Vector3(goal.x, goal.y, m_originalScale.z);
            }
            if (m_explodeCooldown == -f_explodeFrameCount * 5) {
                Destroy(transform.gameObject);
            }
        }
    }

    #endregion

    #region [PublicMethods]

    public void Shoot() {
        Vector2 playerPos = transform.InverseTransformPoint(Consts.Instance.Player.transform.position + Vector3.up / 2f);

        m_b = f_b;

        if (playerPos.x < 0) {
            m_b = -m_b;
            playerPos.x = Mathf.Min(-1, playerPos.x + ACCURACY);
        } else {
            playerPos.x = Mathf.Max(1, playerPos.x - ACCURACY);
        }

        if (SceneLoader.Instance == null && Mathf.Abs(playerPos.x) < 0.05f) {
            m_a = -1;
        } else {
            m_a = (playerPos.y - m_b * playerPos.x) / (playerPos.x * playerPos.x);
            if (m_a >= 0) {
                m_a = -1;
            }
        }

        if (SceneLoader.Instance == null && Medianight.Instance == null) {
            m_a = Mathf.Min(m_a, -0.2f);
        }

        m_a = Mathf.Max(m_a, -1.5f);

        m_right = playerPos.x > 0;
    }

    #endregion

    #region [Override]

    public void Damaged(IDamageTaker damageTaker) {

        //f_damage.Abort();
        m_shallMove = false;

        if (damageTaker == null) {
            // since I don't have the contact point anymore; i just create it again
            List<Collider2D> colliders = DamageHelper.ContactList;
            // This is not generally correct

            Transform oldParent = transform.parent;

            transform.localRotation = Quaternion.identity;

            float newX = transform.localPosition.x - SPEED_FACTOR * f_speed / 50f * (m_right ? 1 : -1) * 0.5f;
            transform.localPosition = new Vector3(newX, newX * (m_a * newX + m_b), transform.localPosition.z);

            transform.position = transform.position + Vector3.back * 3;

            transform.SetParent(colliders[0].transform, true);

            Destroy(oldParent.gameObject);

            m_originalScale = transform.localScale;
            m_explodeCooldown = f_cooldownExplosionFrames;
            StartCoroutine(SetEnabled());

            // when hitting in a bad angle, this might be false, so I set it to true again

        } else {
            if (transform.parent.name.StartsWith("Spit")) {
                Destroy(transform.parent.gameObject);
            } else {
                Destroy(transform.gameObject);
            }

        }
    }

    private IEnumerator SetEnabled() {
        yield return new WaitForFixedUpdate();
        enabled = true;
    }

    #endregion

}