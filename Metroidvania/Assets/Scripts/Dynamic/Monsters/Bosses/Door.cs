using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private Transform f_spawnPoint;
    [SerializeField] private Vector2 f_startDirection;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook] private SpriteRenderer f_door;
    [SerializeField] private Collider2D f_farCollider;

    #endregion

    #region [Properties]

    public Transform SpawnPoint { get { return f_spawnPoint; } }
    public Vector2 StartDirection { get { return f_startDirection; } }

    public Color EnableColor { get; set; }
    public Color DisableColor { get; set; }

    #endregion

    #region [PublicMethods]

    public void SetEnabled(bool enabled) {
        f_door.color = enabled ? EnableColor : DisableColor;
    }

    public bool ContainsPlayer() {
        List<Collider2D> colliderList = DamageHelper.ContactList;
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask((int)EDamageReceiver.Player);
        cf.useLayerMask = true;
        return f_farCollider.OverlapCollider(cf, colliderList) > 0;
    }

    #endregion

}