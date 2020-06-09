using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shovel : MonoBehaviour
{
    EdgeCollider2D edge;
    LineRenderer lr;
    Camera mainCamera;

    public int lineSegments = 10;

    List<Vector2> positions = new List<Vector2>();
    List<Vector3> positionsV3 = new List<Vector3>();

    private Vector2 scenePos;
    private Vector2 lastPos;

    void Start()
    {
        mainCamera = Camera.main;
        edge = GetComponent<EdgeCollider2D>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = lineSegments;
        lastPos = GetScenePosition();

        positions.Add(scenePos);
        positionsV3.Add(scenePos);
    }
    void Update()
    {
        scenePos = GetScenePosition();
        
        if (Vector2.Distance(scenePos, positions[positions.Count - 1]) > .1f)
        {
            positions.Add(scenePos);
            positionsV3.Add(scenePos);
        }
        if(positions.Count > lineSegments)
        {
            positions.RemoveAt(0);
            positionsV3.RemoveAt(0);
        }
        edge.points = positions.ToArray();
        lr.SetPositions(positionsV3.ToArray());

        lastPos = scenePos;
    }
    Vector2 GetScenePosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
