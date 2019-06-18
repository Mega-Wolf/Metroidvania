using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private int f_destructionTimer;

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        --f_destructionTimer;
        if (f_destructionTimer <= 0) {
            Destroy(gameObject);
        }
    }

    #endregion
}
