using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour
{
    private Character thePlayer;
    private CameraFollow theCamera;

    void Start()
    {
        thePlayer = FindObjectOfType<Character>();
        thePlayer.transform.position = transform.position;

        theCamera = FindObjectOfType<CameraFollow>();
        theCamera.transform.position = new Vector3(transform.position.x, transform.position.y, theCamera.transform.position.z);
    }
}
