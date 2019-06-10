using WolfBT;
using UnityEngine;
using System;

public class Feather : MonoBehaviour {

    // #region [Consts]

    // that would require to pass FloatObjects instead of floats as frame lengths
    // [SerializeField] private FeatherSO FeatherSO;

    // #endregion

    #region [MemberFields]

    [SerializeField] private AnimationCurve f_animationCurve;
    [SerializeField] private AnimationCurve f_rotationCurve;

    #endregion

    #region [FinalVariables]

    private BehaviourTree f_bt;
    private Func<Vector2> f_goalFunc;

    private Transform f_originalParent;

    #endregion

    #region [PrivateVariables]

    private Vector2 m_goal;

    #endregion

    #region [Init]

    private void Awake() {

        f_goalFunc = () => {
            return m_goal;
        };

        f_originalParent = transform.parent;

        FeatherFight ff = transform.parent.GetComponent<FeatherFight>();

        Vector2 returnPosition = transform.localPosition;

        f_bt = new BehaviourTree(
            new Sequence(new BTState[] {
                new ActionGroup(() => transform.SetParent(null)),
                new RotateTowardsTransform(transform.GetChild(0), Consts.Instance.Player.transform, 20),
                new MoveTowardsValue(transform, f_goalFunc, 10 / 50f),
                new TimerState(new Rotate(transform.GetChild(0), f_rotationCurve, 360 * 2.5f), 25),
                new RotateTowardsTransform(transform.GetChild(0), f_originalParent, 20),
                new Parallel(
                    BTStateReturn.True,
                    new Loop(new ActionGroup(Actions.LookAt(transform.GetChild(0), f_originalParent))),
                    new MoveTowardsValue(transform, () => (Vector2)f_originalParent.transform.position + returnPosition, 10 / 50f)
                ),
                new RotateTowardsValue(transform.GetChild(0), 0, 20),
                new ActionGroup(
                    () => ff.AvailableFeathers.Add(this),
                    () => transform.SetParent(f_originalParent)
                )
            })
        );
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        BTStateReturn ret = f_bt.FixedUpdate(1);
        if (ret != BTStateReturn.Running) {
            enabled = false;
        }
    }

    #endregion

    #region [PublicMethods]

    public void Shoot(Vector2 goal) {
        m_goal = goal;

        enabled = true;
        f_bt.Enter();
    }

    #endregion

}