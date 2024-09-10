using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Serialized Fields \\
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private float rotateSpeed = 10f;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 followOffset = Vector3.zero;
    [SerializeField] private Vector3 menuOffset = Vector3.zero;

    public bool inMenu = true;

    private Transform target;

    private void Start()
    {
        target = FindObjectOfType<DiceMove>().transform;
    }

    private void Update()
    {
        if (!target) { Debug.LogWarning("No Target Found"); return; }

        Vector3 offset = menuOffset;

        if (!inMenu)
        {
            offset = followOffset;
        }

        // Slowly move the camera to follow the player
        Vector3 targetPos = new Vector3(target.transform.position.x + offset.x, offset.y, target.transform.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);

        // Slowly rotate the camera to lok at the player
        Vector3 adjustedPlayerPos = new Vector3(target.position.x, 0, target.position.z);
        Vector3 targetDirection = (adjustedPlayerPos - transform.position);

        Quaternion targetRot = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
    }

    public Vector3 GetFollowOffset()
    {
        return followOffset;
    }
}
