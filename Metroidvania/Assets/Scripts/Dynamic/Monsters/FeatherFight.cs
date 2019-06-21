using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class FeatherFight : ControllerState {

    #region [MemberFields]

    [SerializeField] private BoxCollider2D f_rect;
    [SerializeField] private float f_speed;
    [SerializeField] private int f_spawnFrames = 10;

    [SerializeField] private int f_shootBreakFrames = 200;

    [SerializeField] private GameObject preFeather;

    #endregion

    #region [FinalVariables]

    private List<Feather> f_availableFeathers = new List<Feather>();

    #endregion

    #region [PrivateVariables]

    private int m_spawnedFeathers = 0;
    private int m_featherNum = 1;

    private int m_currentFrames = 0;
    private int m_shootingFrames = 0;

    #endregion

    #region [Properties]

    public List<Feather> AvailableFeathers { get { return f_availableFeathers; } }

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        return true;
    }

    public override void LogicalEnter() {
        SetNewFlyGoal();
        SpawnFeathers();
        f_controller.AirMovement.SetSmooth(false);
    }

    public override void EffectualEnter() {
        f_controller.Animator.Play("Fly");
    }

    public override bool HandleFixedUpdate() {

        // Movement part
        {
            ++m_currentFrames;
            if (((Vector2)f_controller.transform.position - f_controller.AirMovement.Goal).sqrMagnitude < (0.35f + 0.005f * m_currentFrames) * (0.35f + 0.005f * m_currentFrames) || f_controller.Velocity.sqrMagnitude < 0.1f * 0.1f) {
                // near to goal or almost not moving (stuck?)
                SetNewFlyGoal();
            }
            f_controller.AirMovement.Move(f_speed);
        }

        // Shooting
        {
            ++m_shootingFrames;
            if (m_shootingFrames == f_shootBreakFrames) {
                if (f_availableFeathers.Count == 0) {
                    --m_shootingFrames;
                } else {
                    f_controller.Animator.Play("PrepareAttack");
                }
            }
            if (m_shootingFrames == f_shootBreakFrames + f_spawnFrames) {
                m_shootingFrames = 0;
                f_controller.Animator.Play("Fly");
                Shoot();
            }
        }

        // this is not too nice that I do that every frame

        // I changed it because I don't want to have changing parameters in the game
        if (m_featherNum == 1) {
        //if (m_featherNum == 1 && f_controller.Health.Percentage < 0.5f) {
            ++m_featherNum;
            f_shootBreakFrames = (int)(f_shootBreakFrames * 0.5f);
            f_spawnFrames = (int)(f_spawnFrames * 0.5f);
            m_shootingFrames = (int) (m_shootingFrames * 0.5f) - 1;
            SpawnFeathers();
        }

        return true;
    }

    public override void Abort() {
        //won't happen
    }

    #endregion

    #region [PrivateMethods]

    private void Shoot() {
        if (f_availableFeathers.Count == 0) {
            //TODO; this isn't nice since the animation still got played
            return;
        }

        f_availableFeathers.Shuffle();
        Feather feather = f_availableFeathers[f_availableFeathers.Count - 1];
        f_availableFeathers.RemoveAt(f_availableFeathers.Count - 1);

        //TODO this should probably be changed depending on the difficulty
        feather.Shoot(Consts.Instance.Player.transform.position);
    }

    private void SetNewFlyGoal() {
        m_currentFrames = 0;
        f_controller.AirMovement.Goal = new Vector2(Random.Range(f_rect.bounds.min.x, f_rect.bounds.max.x), Random.Range(f_rect.bounds.min.y, f_rect.bounds.max.y));
        //f_controller.AirMovement.Goal = new Vector2(Random.Range(f_rect.bounds.min.x, f_rect.bounds.max.x), f_controller.AirMovement.Goal.y);
    }

    private Vector2 CalcOffset(int index) {
        return new Vector2(1f * (index % 2 == 0 ? 1 : -1) * (((index / 2) + 1) + 0.5f), 1);
    }

    private void SpawnFeathers() {
        for (int i = m_spawnedFeathers; i < m_featherNum * 2; ++i) {
            GameObject go = Instantiate(preFeather, (Vector2)transform.position + CalcOffset(i), Quaternion.identity, transform);
            go.transform.GetChild(0).localScale = new Vector3(i % 2 == 0 ? 1 : -1, 1, 1);

            f_availableFeathers.Add(go.GetComponent<Feather>());
        }

        m_spawnedFeathers = m_featherNum * 2;

    }

    #endregion

}