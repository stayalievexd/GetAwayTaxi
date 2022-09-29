using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [Header("settings")]
    [SerializeField] private Vector3 offset;//the offset from the player where the camera should be
    [SerializeField] private Transform target;//target position
    [SerializeField] private float translateSpeed;//speed for movement schanges
    [SerializeField] private float rotationSpeed;//speed for rotation schanges


    private void FixedUpdate()
    {
        positionLerping();//lerps the postion to the offset position
        rotationLerping();//wasent needed anymore after we changed to a track system
    }

    private void positionLerping()//lerps the postion to the offset position
    {
        var targetPosition = target.TransformPoint(offset);//the target position where the camera wants to be
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);//sets the postion of the camera
    }

    private void rotationLerping()//not used anymore
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

}
