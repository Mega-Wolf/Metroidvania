using System.Collections.Generic;
using UnityEngine;

public class MaskSetter : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField, Autohook] private Collider2D f_collider;

    private HashSet<Collider2D> f_colliders = new HashSet<Collider2D>();

    #endregion

    #region [Updates]

    private void FixedUpdate() {

        List<Collider2D> colliderList = DamageHelper.ContactList;
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)EDamageReceiver.Enemy);
        cf.useLayerMask = true;
        f_collider.OverlapCollider(cf, colliderList);

        foreach (Collider2D c in colliderList) {
            if (f_colliders.Add(c)) {
                Controller controller = c.GetComponent<Health>().Controller;

                if (controller is Player) {
                    continue;
                }

                if (controller.transform.localScale.x >= 1.5f) {
                    continue;
                }

                controller.GroundMovement.SetGroundMask(new string[] { "Default", "MonsterTransparent" });

            }
        }



    }

    #endregion

}