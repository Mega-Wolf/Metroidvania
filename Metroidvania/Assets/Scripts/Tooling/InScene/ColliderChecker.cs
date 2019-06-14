#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class ColliderChecker : MonoBehaviour {

    #region [Types]

    private enum EVisualisation {
        Never,
        IfWrong,
        Always
    }

    #endregion

    //TODO; the maxes are probably dependend on characters (enemies, etc.)

    #region [MemberFields]

    [Header("Logic")]

    [SerializeField]
    private float m_maxAbsSlope = 30;

    [SerializeField]
    private float m_maxRelAngle = 360;

    [Header("Visualisation")]

    [SerializeField]
    private bool m_showOnlySelected;

    [SerializeField]
    private Color m_allowedColor = new Color(0, 0.5f, 0);

    [SerializeField]
    private Color m_unallowedColor = new Color(0.5f, 0, 0);

    [SerializeField]
    private Color m_narrowColor = Color.black;

    [SerializeField]
    private EVisualisation m_visualisationText;

    [SerializeField]
    private EVisualisation m_visualisationAngles;

    #endregion

    #region [PrivateVariables]

    private EdgeCollider2D[] m_edgeColliders;
    private List<PolygonCollider2D> m_polygonColliders;

    #endregion

    #region [Init]

    [Button("Refresh")]
    private void OnValidate() {
        m_edgeColliders = GameObject.FindObjectsOfType<EdgeCollider2D>();

        PolygonCollider2D[] allPolies = GameObject.FindObjectsOfType<PolygonCollider2D>();
        m_polygonColliders = new List<PolygonCollider2D>();

        foreach (PolygonCollider2D collider in allPolies) {
            if (collider.gameObject.GetComponent<IDamageTaker>() == null) {
                m_polygonColliders.Add(collider);
            }
        }
    }

    #endregion

    #region [Gizmos]

    private void OnDrawGizmos() {

        foreach (EdgeCollider2D edgeCollider in m_edgeColliders) {
            DrawCollider(edgeCollider.points, edgeCollider.transform);
        }

        foreach (PolygonCollider2D polygonCollider in m_polygonColliders) {
            Vector2[] points = new Vector2[polygonCollider.GetPath(0).Length + 1];
            Array.Copy(polygonCollider.GetPath(0), points, polygonCollider.GetPath(0).Length);
            points[points.Length - 1] = points[0];
            DrawCollider(points, polygonCollider.transform);
        }

    }

    #endregion

    #region [PrivateMethods]

    /// <summary>
    /// Draws the shape and info of an edge or polygon collider
    /// </summary>
    /// <param name="points">All the points of the collider; if the collider is a polygon collider; add the first point at the end</param>
    private void DrawCollider(Vector2[] points, Transform transform) {

        if (m_showOnlySelected) {
            bool found = false;
            foreach (UnityEngine.Object obj in Selection.objects) {
                if (obj is GameObject gameObject) {
                    if (gameObject.transform == transform) {
                        found = true;
                        break;
                    }
                }
            }
            if (!found) {
                return;
            }
        }


        float savedAngle;
        Vector2 savedDiff = Vector2.zero;
        for (int i = 0; i < points.Length - 1; ++i) {

            // straight
            Vector2 diffTransformedFirst;
            float angleFirst;
            Vector3 transformed0;
            {
                diffTransformedFirst = transform.TransformVector(points[i + 1] - points[i]);
                angleFirst = Vector3.SignedAngle(Vector3.right, diffTransformedFirst, Vector3.forward);

                if (Mathf.Abs(angleFirst) > m_maxAbsSlope) {
                    Gizmos.color = m_unallowedColor;
                } else {
                    Gizmos.color = m_allowedColor;
                }

                transformed0 = transform.TransformPoint(points[i]);
                Vector3 transformed1 = transform.TransformPoint(points[i + 1]);

                Gizmos.DrawLine(transformed0, transformed1);
                

                if (m_visualisationText == EVisualisation.Always || m_visualisationText == EVisualisation.IfWrong && Mathf.Abs(angleFirst) > m_maxAbsSlope) {
                    Gizmos.DrawCube((transformed0 + transformed1) / 2, Vector3.one / 10f);
                    Handles.Label((transformed0 + transformed1) / 2, "abs: " + angleFirst + "°");
                }

            }

            // angle
            {
                if (i != 0) {

                    float angle = Vector3.SignedAngle(diffTransformedFirst, -savedDiff, Vector3.forward);
                    if (angle < 0) {
                        angle += 360;
                    }

                    if (angle > m_maxRelAngle) {
                        Handles.color = m_unallowedColor;
                    } else {
                        Handles.color = m_allowedColor;
                    }

                    if (angle < 90) {
                        Handles.color = m_narrowColor;
                    }

                    if (m_visualisationAngles == EVisualisation.Always || m_visualisationAngles == EVisualisation.IfWrong && (angle > m_maxRelAngle || angle < 90)) {
                        if (angle < 90) {
                            Handles.DrawSolidArc(transformed0, Vector3.forward, diffTransformedFirst, angle, 0.5f);
                        } else {
                            Handles.DrawWireArc(transformed0, Vector3.forward, diffTransformedFirst, angle, 0.5f);
                        }
                    }

                    if (m_visualisationText == EVisualisation.Always || m_visualisationText == EVisualisation.IfWrong && (angle > m_maxRelAngle || angle < 90)) {
                        Handles.Label(transformed0, "rel: " + angle + "°" + "\n");
                    }

                }

                savedAngle = angleFirst;
                savedDiff = diffTransformedFirst;

            }
        }
    }

    #endregion
}

#endif