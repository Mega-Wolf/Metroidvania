using UnityEngine;
using System.Collections.Generic;

public class TouchDamage : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField]
    private Collider2D f_collider;

    [SerializeField]
    private Collider2D f_collider2;

    [SerializeField]
    private EDamageReceiver f_eDamageReceiver;

    [SerializeField]
    private int f_damage;

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        List<Collider2D> colliderList = DamageHelper.ContactList;
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)f_eDamageReceiver);
        cf.useLayerMask = true;

        f_collider.OverlapCollider(cf, colliderList);

        //TODO; this is ugly and unnecessary if the other one is enabled
        if (colliderList.Count == 0) {
            f_collider2.OverlapCollider(cf, colliderList);
        }

        for (int i = 0; i < colliderList.Count; ++i) {
            Health health = colliderList[i].GetComponent<Health>();
            if (health) {
                //TODO; this should still bump the toucher backwards a bit
                //TODO; Also, I would still actually want the damaged HashSet since otherwise I kill other creatures very fast
                health.TakeDamage(f_damage, Vector2.zero);
            }
        }
    }

    #endregion

}