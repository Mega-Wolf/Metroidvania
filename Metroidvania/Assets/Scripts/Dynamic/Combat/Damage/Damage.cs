using UnityEngine;
using System.Collections.Generic;

public class Damage : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField, Autohook]
    private Collider2D f_collider;

    private int f_maxFrames;
    private IDamager f_damager;
    private EDamageReceiver f_eDamageReceiver;
    private bool f_selfDestroy;
    private bool f_reportEveryHit;

    private HashSet<Collider2D> f_hittedAlready = new HashSet<Collider2D>();

    #endregion

    #region [PrivateVariables]

    private int m_currentDuration;
    private int m_damage;
    private Vector2 m_direction;

    #endregion

    #region [Init]

    public void Init(EDamageReceiver eDamageReceiver, int frameLength, IDamager damager, bool selfDestroy = false, bool reportEveryHit = false) {
        f_eDamageReceiver = eDamageReceiver;
        f_maxFrames = frameLength;
        f_damager = damager;
        f_selfDestroy = selfDestroy;
        f_reportEveryHit = reportEveryHit;
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        List<Collider2D> colliderList = DamageHelper.ContactList;
        //colliderList.Clear();

        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)f_eDamageReceiver);
        cf.useLayerMask = true;
        f_collider.OverlapCollider(cf, colliderList);

        bool hittedOne = false;

        for (int i = 0; i < colliderList.Count; ++i) {
            if (f_hittedAlready.Add(colliderList[i])) {
                hittedOne = true;
                IDamageTaker health = colliderList[i].GetComponent<IDamageTaker>();
                if (health == null) {
                    health = colliderList[i].transform.parent.GetComponent<IDamageTaker>();
                }
                if (health != null) {
                    if (health.Controller == null || health.Controller.enabled) {
                        Vector2 dir = m_direction;
                        if (m_direction == Vector2.zero) {
                            //TODO a velocity - b velocity (or other way round)
                        }
                        health.TakeDamage(m_damage, dir);
                        f_damager?.Damaged(health);


                        Instantiate(Consts.Instance.PreHit, (colliderList[i].transform.position + f_collider.transform.position) / 2f, Quaternion.identity);
                    }
                } else if (f_reportEveryHit) {
                    f_damager?.Damaged(null);
                }
            }
        }

        if (f_selfDestroy && hittedOne) {
            Destroy(gameObject);
        }

        ++m_currentDuration;
        if (m_currentDuration == f_maxFrames) {
            Abort();
        }
    }

    #endregion

    #region [PublicMethods]

    public void UpdateHitDirection(Vector2 hitDirection) {
        m_direction = hitDirection;
    }

    public void ExecuteHit(int damage, Vector2 direction) {
        m_damage = damage;
        m_direction = direction;

        m_currentDuration = 0;
        enabled = true;
        f_hittedAlready.Clear();
    }

    public void Abort() {
        enabled = false;
    }

    #endregion

}