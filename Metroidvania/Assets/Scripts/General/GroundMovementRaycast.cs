using System.Collections.Generic;
using UnityEngine;

public class GroundMovementRaycast {

    #region [Consts]

    //TODO; they will come as an input
    private float MAX_ABS_SLOPE = 360;
    private float MAX_REL_ANGLE = 360;

    private const float EXTRA_RAY_LENGTH = 0.1f;
    private const float OUTER_EXTEND = 0.25f;

    #endregion

    #region [FinalVariables]

    private Controller f_controller;

    private float f_speed;
    private float f_halfWidth;
    private float f_halfHeight;

    #endregion

    #region [Constructors]

    public GroundMovementRaycast(Controller controller, float height, float halfWidth, float speed) {
        f_controller = controller;
        f_halfWidth = halfWidth;
        f_halfHeight = height / 2;
        f_speed = speed;
    }

    #endregion

#if UNITY_EDITOR
    public void OnDrawGizmos() {

        Vector2 transformedDownward = f_controller.transform.TransformVector(Vector2.down);

        LayerMask GROUND_MASK = LayerMask.GetMask("Default");
        float RAY_LENGTH = f_halfHeight + EXTRA_RAY_LENGTH;

        RaycastHit2D hitL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitHL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitC = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(0, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitHR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth + OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);

        // Debug lines
        {
            Debug.DrawLine(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)), f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)) + (Vector3)transformedDownward * RAY_LENGTH, Color.cyan);
            Debug.DrawLine(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)), f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)) + (Vector3)transformedDownward * RAY_LENGTH, Color.cyan);
            Debug.DrawLine(f_controller.transform.TransformPoint(new Vector3(f_halfWidth, f_halfHeight, 0)), f_controller.transform.TransformPoint(new Vector3(f_halfWidth, f_halfHeight, 0)) + (Vector3)transformedDownward * RAY_LENGTH, Color.cyan);
            Debug.DrawLine(f_controller.transform.TransformPoint(new Vector3(f_halfWidth + OUTER_EXTEND, f_halfHeight, 0)), f_controller.transform.TransformPoint(new Vector3(f_halfWidth + OUTER_EXTEND, f_halfHeight, 0)) + (Vector3)transformedDownward * RAY_LENGTH, Color.cyan);

            Debug.DrawLine(hitL.point, hitHL.point, Color.magenta);
            Debug.DrawLine(hitHL.point, hitHR.point, Color.yellow);
            Debug.DrawLine(hitR.point, hitHR.point, Color.magenta);
            Debug.DrawLine(hitL.point, hitR.point, Color.blue);
        }
    }
#endif

    #region [PublicMethods]

    public void Move(bool forward) {

        // Do 3 downward raycasts
        // Downward = downward in regards to character?
        {
            Vector2 transformedDownward = f_controller.transform.TransformVector(Vector2.down);

            LayerMask GROUND_MASK = LayerMask.GetMask("Default");
            float RAY_LENGTH = f_halfHeight + EXTRA_RAY_LENGTH;

            RaycastHit2D hitL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitHL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitC = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(0, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitHR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth + OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);



            if (forward) {
                if (hitR && hitHR && Vector2.Angle(Vector2.right, hitR.point - hitHR.point) > MAX_ABS_SLOPE) {
                    Debug.Log("Abort Right: " + Vector2.Angle(Vector2.right, hitR.point - hitHR.point));
                    return;
                }
            } else {
                if (hitL && hitHL && Vector2.Angle(Vector2.left, hitL.point - hitHL.point) > MAX_ABS_SLOPE) {
                    Debug.Log("Abort Left: " + Vector2.Angle(Vector2.left, hitL.point - hitHL.point));
                    return;
                }
            }


            Vector2 middleVector = (hitHR.point - hitHL.point).normalized;

            if (hitHL && hitHR) {
                // corrrecting the curent transform
                f_controller.transform.rotation = Quaternion.FromToRotation(Vector2.up, Vector3.Cross(middleVector, Vector3.back));

                if (hitC && (hitL || hitR)) {
                    f_controller.transform.position = hitC.point;
                } else {
                    f_controller.transform.position = (hitHR.point + hitHL.point) / 2f;
                }
            }

            //TODO: What do I do if not both inner ones hit (pointy edge?)



            // preparing the velocity change for the transform
            // this is a bit complicated due to many different cases
            if (forward) {
                if (hitHR) {
                    f_controller.Velocity = middleVector * f_speed;
                } else if (hitC) {
                    f_controller.Velocity = (hitC.point - hitHL.point).normalized * f_speed;
                } else {
                    f_controller.Velocity = (hitHL.point - hitL.point).normalized * f_speed;
                }
            } else {
                if (hitHL) {
                    f_controller.Velocity = -middleVector * f_speed;
                } else if (hitC) {
                    f_controller.Velocity = (hitC.point - hitHR.point).normalized * f_speed;
                } else {
                    f_controller.Velocity = (hitHR.point - hitR.point).normalized * f_speed;
                }
            }

            if (!(hitL && hitR && hitHL && hitHR)) {
                return;
            }

            Vector2 intersection = MathExtension.Inters(hitL.point, hitHL.point, hitR.point, hitHR.point);

            Debug.DrawLine(intersection, intersection + Vector2.up * 0.1f, Color.red);

        }

    }

    #endregion

}