using UnityEngine;
using WolfBT;

public class Spit : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private float f_speed;
    [SerializeField] private float f_b;

    #endregion

    #region [PrivateVariables]

    private float m_a;
    private float m_b;
    private bool m_right;

    #endregion

    #region [Init]

    private void Start() {
        // f_behaviourTree = new BehaviourTree(
        //     //Only collide with statics since normal damage will be covered by Damage
        //     new Sequence(
        //         new Collide(f_collider, contactFilter),
        //         new ActionGroup(
        //             () => {
        //                 for (int i = 0; i < DamageHelper.ContactList.Count; ++i) {

        //                 }
        //             }
        //         )
        //     )

        // );

        //f_behaviourTree.Enter();
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        float newX = transform.localPosition.x + f_speed / 50f * (m_right ? 1 : -1);
        transform.localPosition = new Vector3(newX, newX * (m_a * newX + m_b) , transform.localPosition.z);
    }

    #endregion

    #region [PublicMethods]

    public void Shoot() {
        Vector2 playerPos = transform.InverseTransformPoint(Consts.Instance.Player.transform.position + Vector3.up / 2f);
        m_b = f_b;

        if (playerPos.x < 0) {
            m_b = -m_b;
        }

        if (Mathf.Abs(playerPos.x) < 0.05f) {
            m_a = -1;
        } else {
            m_a = (playerPos.y - m_b * playerPos.x) / (playerPos.x * playerPos.x);
            if (m_a >= 0) {
                m_a = -1;
            }
        }

        m_right = playerPos.x > 0;
    }

    #endregion

}