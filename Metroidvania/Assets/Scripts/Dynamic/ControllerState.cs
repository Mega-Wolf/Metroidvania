using System.Collections.Generic;

public abstract class ControllerState {

    #region [FinalVariables]

    private List<ControllerState> f_states = new List<ControllerState>();

    protected Controller f_controller;

    #endregion

    #region [PrivateVariables]

    protected ControllerState m_stateBefore;

    #endregion

    #region [Properties]

    public List<ControllerState> FutureStates { get { return f_states; } }

    #endregion

    #region [Constructors]

    public ControllerState(Controller player) {
        f_controller = player;
    }

    #endregion

    #region [PublicMethods]

    /// <summary>
    /// This adds a state to the possible future states of this one
    /// Those states are check every frame to enter them
    /// </summary>
    /// <param name="label">This label gives the state a name which can then be referenced to be called</param>
    /// <param name="state">The possible future state</param>
    public void AddTransitionGoal(string label, ControllerState state) {
        f_states.Add(state);
    }

    /// <summary>
    /// This function is checked to see if this state shall be entered
    /// If true then it considered itself as started
    /// IMPORTANT: Enter is NOT called in that case
    /// </summary>
    /// <returns></returns>
    public bool EnterOnCondition(ControllerState stateBefore) {
        m_stateBefore = stateBefore;
        return EnterOnCondition();
    }

    protected abstract bool EnterOnCondition();

    public abstract void Enter();

    /// <summary>
    /// This function is called every frame while the state is active
    /// </summary>
    public abstract void HandleFixedUpdate();

    //public abstract void Leave();

    #endregion

    
#if UNITY_EDITOR

    public virtual void OnDrawGizmos() { }

#endif

}