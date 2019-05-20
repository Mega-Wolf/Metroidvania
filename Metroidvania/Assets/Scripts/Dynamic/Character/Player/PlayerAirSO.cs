using UnityEngine;

public class PlayerAirSO : ConstScriptableObject {

    #region [MemberFields]

    [SerializeField] private int coyoteFrames = 5;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float jumpHalfDuration = 0.45f;

    #endregion

    #region [Properties]

    public int COYOTE_FRAMES { get { return coyoteFrames; } }
    public float JUMP_HEIGHT { get { return jumpHeight; } }
    public float JUMP_HALF_DURATION { get { return jumpHalfDuration; } }

    public float JUMP_SPEED { get { return 2 * JUMP_HEIGHT / JUMP_HALF_DURATION; } }
    public float MAX_FALL_SPEED { get { return JUMP_SPEED * 1.5f; } }
    public float ACCELERATION_Y { get { return JUMP_SPEED / JUMP_HALF_DURATION; } }

    #endregion
}