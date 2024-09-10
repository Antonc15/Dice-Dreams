using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsFollowCam : MonoBehaviour
{
    private CameraFollow camFollow;

    private void Start()
    {
        camFollow = FindObjectOfType<CameraFollow>();
    }

    private void Update()
    {
        transform.position = new Vector3(camFollow.transform.position.x, 0, camFollow.transform.position.z);
    }
}
