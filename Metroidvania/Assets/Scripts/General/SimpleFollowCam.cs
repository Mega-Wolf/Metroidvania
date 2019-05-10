using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowCam : MonoBehaviour {

    #region [MemberFields]

    [SerializeField]
    private Transform m_followed;

    #endregion

    #region [Updates]

    private void LateUpdate() {
        transform.position = m_followed.position - Vector3.forward * 10;
    }

    #endregion
}
