using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField, Autohook] private SpriteRenderer f_spriteRenderer;
    [SerializeField, Autohook(AutohookAttribute.AutohookMode.AlsoParent)] private Door f_door;
    [SerializeField, Autohook] private Collider2D f_nearCollider;

    #endregion

    #region [Updates]

    private void Update() {
        List<Collider2D> colliderList = DamageHelper.ContactList;
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)EDamageReceiver.Enemy);
        cf.useLayerMask = true;

        f_nearCollider.OverlapCollider(cf, colliderList);

        foreach (Collider2D collider in colliderList) {
            //if (collider.transform.parent.parent.localScale.x >= 1.5) {
                f_spriteRenderer.enabled = true;
                f_door.SetEnabled(true);
                return;
            //}
        }

        f_spriteRenderer.enabled = false;
        f_door.SetEnabled(false);

        // if (f_nearCollider.OverlapCollider(cf, colliderList) > 0) {
        //     f_spriteRenderer.enabled = true;
        //     f_door.SetEnabled(true);
        // } else {
        //     f_spriteRenderer.enabled = false;
        //     f_door.SetEnabled(false);
        // }
    }



    #endregion

    // #region [Physics]

    // private void OnTriggerEnter2D(Collider2D other) {
    //     f_spriteRenderer.enabled = true;
    //     f_door.SetEnabled(true);
    // }

    // private void OnTriggerExit2D(Collider2D other) {
    //     f_spriteRenderer.enabled = false;
    //     f_door.SetEnabled(false);
    // }

    // #endregion

}