using UnityEngine;

public class DifficultMeteorSpawn : MonoBehaviour {

    #region [MemberFields]

    /* [SerializeField]*/
    private int f_offset = 10;
    /* [SerializeField]*/
    private int f_amount = -1;

    public static float ACCURACY = 0;

    [SerializeField] private GameObject preMeteor;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook] BoxCollider2D f_collider;

    #endregion

    #region [PrivateVariables]

    private int m_currentFrame;

    #endregion

    #region [Init]

    private void OnEnable() {
        m_currentFrame = 0;
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        ++m_currentFrame;

        if (m_currentFrame == f_offset * f_amount) {
            enabled = false;
        }

        float x = Consts.Instance.Player.transform.position.x;

        if (m_currentFrame % f_offset == 0) {

            if (m_currentFrame % (10 * f_offset) == 0) {
                x += ACCURACY * (Random.value < 0.5f ? 1 : -1);
            } else {
                while (Mathf.Abs(x - Consts.Instance.Player.transform.position.x) <= 1) {
                    x = Random.Range(f_collider.bounds.min.x, f_collider.bounds.max.x);
                }
            }

            GameObject go = Instantiate(preMeteor, new Vector3(x, transform.position.y, 1), Quaternion.identity, transform);
        }
    }

    #endregion


}