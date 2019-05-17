using UnityEngine;
using NaughtyAttributes.Editor;
using System.Collections.Generic;

public class Hit : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField, Autohook]
    private Collider2D f_collider;

    #endregion

    #region [PublicMethods]

    public void ExecuteHit(int amount, EDamageReceiver eDamageReceiver) {
        List<ContactPoint2D> contactList = DamageHelper.ContactList;
        contactList.Clear();
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)eDamageReceiver);
        Physics2D.GetContacts(f_collider, cf, contactList);
        for (int i = 0; i < contactList.Count; ++i) {
            contactList[i].collider.GetComponent<Health>().TakeDamage(amount, contactList[i].normal);
        }
    }

    #endregion

}