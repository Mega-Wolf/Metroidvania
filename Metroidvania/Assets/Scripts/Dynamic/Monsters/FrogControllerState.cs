using UnityEngine;
using WolfBT;

public class FrogControllerState : ControllerState {

    #region [MemberFields]

    [SerializeField] private GameObject prePuke;
    [SerializeField] private Transform f_pukePosition;

    #endregion

    #region [FinalVariables]

    private BehaviourTree f_behaviourTree;

    #endregion

    #region [Init]

    private void Start() {
        f_behaviourTree = new BehaviourTree(
            new Sequence(
                new ActionGroup(Actions.PlayAnimation(f_controller.Animator, "Idle")),
                new TimerState(100),
                new ActionGroup(Actions.PlayAnimation(f_controller.Animator, "Puke")),
                new TimerState(10),
                new ActionGroup(() => {
                    GameObject go = Instantiate(prePuke, f_pukePosition.position, Quaternion.identity);
                    go.transform.GetChild(0).GetComponent<Spit>().Shoot();
                }),
                new TimerState(100)
            )
        );
    }

    #endregion

    #region [Override]

    public override bool EnterOnCondition() { return true; }

    public override void LogicalEnter() {
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