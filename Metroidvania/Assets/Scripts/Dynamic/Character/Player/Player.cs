using UnityEngine;

public class Player : Controller, IDamagable {

    #region [MemberFields]

    [SerializeField]
    private PlayerHittingSide f_hittingSide;

    [SerializeField]
    private PlayerHittingUp f_hittingUp;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private PlayerAir f_air;

    [SerializeField, Autohook]
    private PlayerGrounded f_grounded;

    [SerializeField, Autohook]
    private PlayerHittingDown f_hittingDown;

    [SerializeField, Autohook]
    private Health f_health;

    #endregion

    #region [Init]

    private void Awake() {
        f_air.AddTransitionGoal("Grounded", f_grounded);
        f_grounded.AddTransitionGoal("Air", f_air);

        f_air.AddTransitionGoal("CharHittingDown", f_hittingDown, true);
        f_air.AddTransitionGoal("CharHittingUp", f_hittingUp, true);
        f_air.AddTransitionGoal("CharHittingSide", f_hittingSide, true);

        f_grounded.AddTransitionGoal("CharHittingUp", f_hittingUp, true);
        f_grounded.AddTransitionGoal("CharHittingSide", f_hittingSide, true);

        SetStartState(f_grounded);
    }

    private void Start() {
        f_health.Add(this);
        f_health.Init(Consts.Instance.PlayerSO.HEALTH, 0);
    }

    #endregion

    #region [Override]

    public void TakeDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        //TODO
    }

    #endregion

}