using WolfBT;
using UnityEngine;
using System;

public class Feather : MonoBehaviour {

    public static float SPEED = 10f;
    public static int CAST_TIME = 20;
    public static float ACCURACY = 0;

    // #region [Consts]

    // that would require to pass FloatObjects instead of floats as frame lengths
    // [SerializeField] private FeatherSO FeatherSO;

    // #endregion

    #region [MemberFields]

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
            new Parallel(BTStateReturn.True,
                new Mapper(
                    new ActionGroup(() => {
                        if (!f_originalParent) Destroy(gameObject);
                    }),
                    false,
                    false),
                new Sequence(new BTState[] {
                    //new TimerState(new Rotate(transform.GetChild(0), f_rotationCurve, 360 * 3), 20),
                    new TimerState(new Rotate(transform.GetChild(0), f_rotationCurve, 360 * 3), CAST_TIME),
                    new ActionGroup(() => {
                        transform.SetParent(null);

                    Vector2 modifiedPlayerPos = Consts.Instance.Player.transform.position;

                    if (transform.position.x > Consts.Instance.Player.transform.position.x) {
                        modifiedPlayerPos.x += ACCURACY;
                    } else {
                        modifiedPlayerPos.x -= ACCURACY;
                    }

                    Vector2 dif = modifiedPlayerPos - (Vector2) transform.position;
                    float wholeYDif = -6 - transform.position.y;



                    float factor = wholeYDif / dif.y;

                    m_goal = (Vector2)transform.position + dif * factor;

                    //m_goal = new Vector2(Consts.Instance.Player.transform.position.x, -6);
                    }),
                    new RotateTowardsTransform(transform.GetChild(0), Consts.Instance.Player.transform, 20),
                    new MoveTowardsValue(transform, f_goalFunc, SPEED / 50f),
                    new TimerState(new Rotate(transform.GetChild(0), f_rotationCurve, 360 * 2.5f), CAST_TIME),
                    new RotateTowardsTransform(transform.GetChild(0), f_originalParent, 20),
                    new Parallel(
                        BTStateReturn.True,
                        new Loop(new ActionGroup(Actions.LookAt(transform.GetChild(0), f_originalParent))),
                        new MoveTowardsValue(transform, () => (Vector2)f_originalParent.transform.position + returnPosition, SPEED / 50f)
                    ),
                    new RotateTowardsValue(transform.GetChild(0), 0, 20),
                    new ActionGroup(
                        () => ff.AvailableFeathers.Add(this),
                        () => {
                            if (f_originalParent == null) {
                                Destroy(gameObject);
                            } else {
                                transform.SetParent(f_originalParent);
                            }
                        }
                    )
                })
        ));
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
        //m_goal = goal;
        //TODO; this is now very hard coded
        m_goal = new Vector2(goal.x, -6f);

        enabled = true;
        f_bt.Enter();
    }

    #endregion

}