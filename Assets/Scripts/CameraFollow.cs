using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;
    private static bool cameraExists;

    /*private void Start()
    {
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
    }
    }*/
    void LateUpdate()
    {
        if (Player != null)
            transform.position = Player.position + Offset;
    }
}
