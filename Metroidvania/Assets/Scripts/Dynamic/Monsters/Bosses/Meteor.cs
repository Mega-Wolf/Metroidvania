using UnityEngine;

public class Meteor : MonoBehaviour {

    public static float SPEED = 10f;

    #region [MemberFields]

    [SerializeField, Autohook] private Damage f_damage;

    #endregion

    #region [Init]

    private void Start() {
        f_damage.Init(EDamageReceiver.Player | EDamageReceiver.Environment | EDamageReceiver.Default, -1, null, true, true);
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        transform.position = transform.position + Vector3.down * SPEED / 50;
    }

    #endregion

}