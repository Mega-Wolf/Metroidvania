using System.Collections.Generic;

public abstract class ControllerState {

    #region [FinalVariables]

    private List<ControllerState> f_states = new List<ControllerState>();

    protected Controller f_controller;

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
    /// Those states are checked every frame to enter them
    /// </summary>
    /// <param name="label">This label gives the state a name which can then be referenced to be called</param>
    /// <param name="state">The possible future state</param>
    public void AddTransitionGoal(string label, ControllerState state) {
        f_states.Add(state);
    }

    /// <summary>
    /// This function is checked to see if this state shall be entered
    /// If true then it considered itself as started
    /// </summary>
    /// <returns>True if the condition was met; this is now  the new state</returns>
    public abstract bool EnterOnCondition();

    /// <summary>
    /// This will get triggered when the function is started
    /// It shall express what logically happens now that this state is entered
    /// </summary>
    public abstract void LogicalEnter();

    /// <summary>
    /// This is called everytime this ControllerState gets the focus (either by entering it or by leaving a stacked state above)
    /// </summary>
    public abstract void EffectualEnter();

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