using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{   
    [Header("Private Set Data")]
    [SerializeField] private float sensitivity = 1.5f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private float minAngle = -65;
    [SerializeField] private float maxAngel = 90;

    [Header("Private data")]
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    
    void Update ()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngel);

        transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
