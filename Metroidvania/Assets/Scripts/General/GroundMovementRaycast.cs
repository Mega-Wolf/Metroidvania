using System.Collections.Generic;
using UnityEngine;

//TODO
//BUGS
// When landing...
// ... on an edge: The landing is not recognised: The character falls through the floor, gets set back up and then falls through again
//     (attention: I have to make sure that I am not at an angle)
//     Possible solution: When the first one touches, I cast a raycast and check where the other one touches; that determines the angle of it
// ... on an inside edge: The character adjusts to the wall although it normally would not be allowed to come that close to it -.-
//     ignore the tilt when too big
//     also: when walking towards wall: maybe allow to walk closer towards it if tilting is forbidden



public class GroundMovementRaycast {

    #region [Static]

    public enum GroundTouch {
        Left = 1,
        HalfLeft = 2,
        Centre= 4,
        HalfRight = 8,
        Right = 16
    }

    public static int AirDirection(GroundTouch gt) {
        int ret= 0;

        if ((gt & GroundTouch.Left) == 0) {
            ret -= 1;
        }
        if ((gt & GroundTouch.HalfLeft) == 0) {
            ret -= 2;
        }
        
        if ((gt & GroundTouch.HalfRight) == 0) {
            ret += 2;
        }
        if ((gt & GroundTouch.Right) == 0) {
            ret += 1;
        }

        return ret;
    }

    #endregion

    #region [Consts]

    //TODO; they will come as an input
    private float MAX_ABS_SLOPE = 25.1f;
    private float MAX_REL_ANGLE = 360;

    private const float EXTRA_RAY_LENGTH = 0.1f;
    private const float OUTER_EXTEND = 0.25f;

    #endregion

    #region [FinalVariables]

    private Controller f_controller;
    private float f_halfWidth;
    private float f_halfHeight;

    #endregion

    #region [Constructors]

    public GroundMovementRaycast(Controller controller, float height, float halfWidth) {
        f_controller = controller;
        f_halfWidth = halfWidth;
        f_halfHeight = height / 2;
    }

    #endregion

#if UNITY_EDITOR
    public void OnDrawGizmos() {

        Vector2 transformedDownward = f_controller.transform.TransformVector(Vector2.down).normalized;

        LayerMask GROUND_MASK = LayerMask.GetMask("Default");
        float RAY_LENGTH = f_halfHeight * f_controller.transform.localScale.y + EXTRA_RAY_LENGTH;

        RaycastHit2D hitL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitHL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitC = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(0, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitHR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth + OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);

        // Debug lines
        {
            Debug.DrawLine(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)), f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)) + (Vector3)transformedDownward * RAY_LENGTH, Color.cyan);
            Debug.DrawLine(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)), f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)) + (Vector3)transformedDownward * RAY_LENGTH, Color.cyan);
            Debug.DrawLine(f_controller.transform.TransformPoint(new Vector3(0, f_halfHeight, 0)), f_controller.transform.TransformPoint(new Vector3(0, f_halfHeight, 0)) + (Vector3)transformedDownward * RAY_LENGTH, Color.yellow);
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

    public GroundTouch Move(float speed) {

        // Do 5 downward raycasts
        // Downward = downward in regards to character
        {
            GroundTouch ret = 0;

            Vector2 transformedDownward = f_controller.transform.TransformVector(Vector2.down);

            LayerMask GROUND_MASK = LayerMask.GetMask("Default");
            float RAY_LENGTH = f_halfHeight * f_controller.transform.localScale.y + EXTRA_RAY_LENGTH;

            RaycastHit2D hitL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth - OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitHL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitC = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(0, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitHR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);
            RaycastHit2D hitR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth + OUTER_EXTEND, f_halfHeight, 0)), transformedDownward, RAY_LENGTH, GROUND_MASK);


            if (hitL) {
                ret |= GroundTouch.Left;
            }
            if (hitHL) {
                ret |= GroundTouch.HalfLeft;
            }
            if (hitC) {
                ret |= GroundTouch.Centre;
            }
            if (hitHR) {
                ret |= GroundTouch.HalfRight;
            }
            if (hitR) {
                ret |= GroundTouch.Right;
            }


            if (speed > 0) {
                if (hitR && hitHR && Vector2.Angle(Vector2.right, hitR.point - hitHR.point) > MAX_ABS_SLOPE) {
                    Debug.Log("Abort Right: " + Vector2.Angle(Vector2.right, hitR.point - hitHR.point), f_controller);
                    return ret;
                }
            } else {
                if (hitL && hitHL && Vector2.Angle(Vector2.left, hitL.point - hitHL.point) > MAX_ABS_SLOPE) {
                    Debug.Log("Abort Left: " + Vector2.Angle(Vector2.left, hitL.point - hitHL.point), f_controller);
                    return ret;
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
            } else if (hitC) {
                f_controller.transform.position = hitC.point;
                if (!hitHL && !hitHR) {
                    middleVector = Vector2.right;
                    f_controller.transform.rotation = Quaternion.identity;
                } else if (hitHL) {
                    middleVector = (hitC.point - hitHL.point).normalized;
                    f_controller.transform.rotation = Quaternion.FromToRotation(Vector2.up, Vector3.Cross(middleVector, Vector3.back));
                } else { // hitHR
                    middleVector = (hitHR.point - hitC.point).normalized;
                    f_controller.transform.rotation = Quaternion.FromToRotation(Vector2.up, Vector3.Cross(middleVector, Vector3.back));
                }
            } else {
                if (hitHL) {
                    middleVector = Vector2.right;
                    f_controller.transform.rotation = Quaternion.identity;
                    f_controller.transform.position = hitHL.point + Vector2.right * f_halfWidth;
                } else if (hitHR) {
                    middleVector = Vector2.right;
                    f_controller.transform.rotation = Quaternion.identity;
                    f_controller.transform.position = hitHR.point + Vector2.left * f_halfWidth;
                }
            }

            //TODO: What do I do if not both inner ones hit (pointy edge?)

            // preparing the velocity change for the transform
            // this is a bit complicated due too many different cases
            if (speed > 0) {
                if (hitHR) {
                    f_controller.Velocity = middleVector * speed;
                } else if (hitC) {
                    f_controller.Velocity = (hitC.point - hitHL.point).normalized * speed;
                } else {
                    f_controller.Velocity = (hitHL.point - hitL.point).normalized * speed;
                }
            } else {
                if (hitHL) {
                    f_controller.Velocity = -middleVector * -speed;
                } else if (hitC) {
                    f_controller.Velocity = (hitC.point - hitHR.point).normalized * -speed;
                } else {
                    f_controller.Velocity = (hitHR.point - hitR.point).normalized * -speed;
                }
            }

            //TODO; clean up the above; it doesn't seem right
            // this means only one side touched
            if (ret ==  GroundTouch.Left || ret ==  GroundTouch.HalfLeft || ret ==  GroundTouch.Centre || ret ==  GroundTouch.HalfRight || ret ==  GroundTouch.Right) {
                f_controller.Velocity = f_controller.transform.right* speed;
            }

            if (!(hitL && hitR && hitHL && hitHR)) {
                return ret;
            }

            Vector2 intersection = MathExtension.Inters(hitL.point, hitHL.point, hitR.point, hitHR.point);

            Debug.DrawLine(intersection, intersection + Vector2.up * 0.1f, Color.red);

            return ret;
        }

    }

    public bool TryStickToGround() {

        LayerMask GROUND_MASK = LayerMask.GetMask("Default");
        float RAY_LENGTH = f_halfHeight * f_controller.transform.localScale.y + EXTRA_RAY_LENGTH;

        RaycastHit2D hitHL = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(-f_halfWidth, f_halfHeight, 0)), Vector2.down, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitC = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(0, f_halfHeight, 0)), Vector2.down, RAY_LENGTH, GROUND_MASK);
        RaycastHit2D hitHR = Physics2D.Raycast(f_controller.transform.TransformPoint(new Vector3(f_halfWidth, f_halfHeight, 0)), Vector2.down, RAY_LENGTH, GROUND_MASK);

        if (hitHL && hitHR) {
            f_controller.transform.position = (hitHL.point + hitHR.point) / 2f;
            f_controller.transform.rotation = Quaternion.FromToRotation(Vector2.up, Vector3.Cross((hitHR.point - hitHL.point).normalized, Vector3.back));
            return true;
        } else if (hitC) {
            if (hitHL) {
                f_controller.transform.position = hitC.point;
                f_controller.transform.rotation = Quaternion.FromToRotation(Vector2.up, Vector3.Cross((hitC.point - hitHL.point).normalized, Vector3.back));
                return true;
            } else if (hitHR) {
                f_controller.transform.position = hitC.point;
                f_controller.transform.rotation = Quaternion.FromToRotation(Vector2.up, Vector3.Cross((hitHR.point - hitC.point).normalized, Vector3.back));
                return true;
            }
        }

        return false;
    }

    #endregion

}