using UnityEngine;

public class DummyState : ControllerState {
    
    public override void EffectualEnter() {
        
    }

    public override bool EnterOnCondition() {
        return false;
    }

    public override bool HandleFixedUpdate() {
        f_controller.Velocity = Vector2.zero;
        return true;
    }

    public override void LogicalEnter() {
        
    }

    public override void Abort() { }
}