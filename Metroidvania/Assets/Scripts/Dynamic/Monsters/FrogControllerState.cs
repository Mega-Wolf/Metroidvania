using UnityEngine;
using WolfBT;

public class FrogControllerState : ControllerState {

    public static int CAST_TIME = 50;

    public static int PUKE_TIME = 20;
    public static int END_IDLE = 50;
    public static int POST_IDLE = 0;

    #region [MemberFields]

    [SerializeField] private GameObject prePuke;
    [SerializeField] private Transform f_pukePosition;

    #endregion

    #region [FinalVariables]

    private BehaviourTree f_behaviourTree;

    #endregion

    #region [Init]

    private void Start() {
        if (f_behaviourTree != null) {
            return;
        }

        f_behaviourTree = new BehaviourTree(
            new Sequence(
                new ActionGroup(Actions.PlayAnimation(f_controller.Animator, "Idle")),
                new TimerState(CAST_TIME),
                new ActionGroup(Actions.PlayAnimation(f_controller.Animator, "Puke")),
                new TimerState(PUKE_TIME),
                new ActionGroup(() => {
                    GameObject go = Instantiate(prePuke, f_pukePosition.position, Quaternion.identity);
                    go.transform.GetChild(0).GetComponent<Spit>().Shoot();
                }),
                new TimerState(END_IDLE),

                
                new TimerState(POST_IDLE)
            )
        );
    }

    #endregion

    #region [Override]

    public override bool EnterOnCondition() { return true; }

    public override void LogicalEnter() {
        if (f_behaviourTree == null) {
            Start();
        }
        f_behaviourTree.Enter();
    }

    public override void EffectualEnter() { }

    public override bool HandleFixedUpdate() {
        f_controller.Velocity = Vector2.zero;
        f_behaviourTree.FixedUpdate(1);

        return true;
    }

    public override void Abort() { /* never reached */ }

    #endregion
}