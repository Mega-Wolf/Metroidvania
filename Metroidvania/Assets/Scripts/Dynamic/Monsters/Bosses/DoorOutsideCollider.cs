using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorOutsideCollider : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField, Autohook(AutohookAttribute.AutohookMode.AlsoParent)] private Door f_door;
    [SerializeField, Autohook] private Collider2D f_collider;
    [SerializeField, Autohook(AutohookAttribute.AutohookMode.AllParents)] private BossFightRhino f_bossFightRhino;

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        List<Collider2D> colliderList = new List<Collider2D>();
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)EDamageReceiver.Enemy);
        cf.useLayerMask = true;

        f_collider.OverlapCollider(cf, colliderList);

        foreach (Collider2D coll in colliderList) {
            // if (coll.GetComponent<Health>().Controller.gameObject.transform.localScale.x < 1.5f) {
            //     return;
            // }
            Controller controller = coll.GetComponent<Health>().Controller;
            controller.gameObject.SetActive(false);
            f_bossFightRhino.SetNextStep(f_door, ((RhinoBoss)controller));

        }
    }

    #endregion

}