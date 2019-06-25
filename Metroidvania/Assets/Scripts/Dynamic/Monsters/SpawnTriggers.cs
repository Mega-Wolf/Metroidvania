using UnityEngine;

public class SpawnTriggers : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private bool f_respawn = true;

    [SerializeField] private Transform f_out;
    [SerializeField] private Transform f_in;

    [SerializeField] private int f_extendFrames;
    [SerializeField] private int f_cooldownTime;

    [SerializeField] private GameObject preObject;

    [SerializeField] private Collider2D f_nearCollider;
    [SerializeField] private Collider2D f_farCollider;

    #endregion

    #region [PrivateVariables]

    private bool m_outwards = false;
    private GenericEnemy m_enemy;
    private int m_currentExtendFrames = 0;

    private int m_currentCooldown;

    #endregion

    #region [Init]

    private void Awake() {
        if (!f_respawn) {
            m_enemy = Instantiate(preObject, transform).GetComponent<GenericEnemy>();
        }
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        if (!m_enemy) {
            Destroy(gameObject);
        }

        if (m_currentCooldown > 0) {
            --m_currentCooldown;
        }

        if (m_currentCooldown == 0) {
            f_nearCollider.OverlapCollider(DamageHelper.PlayerFilter, DamageHelper.ContactList);
            if (DamageHelper.ContactList.Count > 0) {
                if (m_outwards) {
                    m_currentCooldown = f_cooldownTime;
                }
                m_outwards = false;
            } else {
                f_farCollider.OverlapCollider(DamageHelper.PlayerFilter, DamageHelper.ContactList);
                if (DamageHelper.ContactList.Count > 0) {
                    if (!m_outwards) {
                        m_currentCooldown = f_cooldownTime;
                    }
                    m_outwards = true;
                } else {
                    if (m_outwards) {
                        m_currentCooldown = f_cooldownTime;
                    }
                    m_outwards = false;
                }
            }
        }

        if (m_outwards) {
            m_currentExtendFrames = Mathf.Min(f_extendFrames, m_currentExtendFrames + 1);
            if (m_currentExtendFrames == 1) {
                if (!m_enemy) {
                    m_enemy = Instantiate(preObject, transform).GetComponent<GenericEnemy>();
                    m_enemy.enabled = false;
                }
                m_enemy.Orient();
            }
            if (m_currentExtendFrames == f_extendFrames - 1) {
                m_enemy.enabled = true;
                m_enemy.Start();
            }
            // if (m_currentExtendFrames == f_extendFrames && !f_respawn) {
            //     m_enemy.transform.parent = null;
            //     Destroy(gameObject);
            // }
        } else {
            if (m_enemy != null && m_enemy.Health.Value == 0) {
                Destroy(m_enemy.gameObject);
                m_currentCooldown = 5 * f_cooldownTime;
            } else {
                m_currentExtendFrames = Mathf.Max(0, m_currentExtendFrames - 1);
                if (m_currentExtendFrames == f_extendFrames - 1) {
                    if (m_enemy) {
                        m_enemy.enabled = false;
                    }
                }
            }

        }

        if (m_enemy) {
            m_enemy.transform.position = Vector3.Lerp(f_in.position, f_out.position, m_currentExtendFrames / (float)f_extendFrames);
        }

    }

    #endregion

}