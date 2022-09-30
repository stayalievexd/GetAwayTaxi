using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Hover Settings")]

    [Tooltip("Min hover height and Max hover height")]
    [SerializeField] private Vector2 hoverHeights = new Vector2(0.6f,5.0f);

    [Tooltip("The speed the car goes up and down in height")]
    [SerializeField] private float heighChangeSpeed = 2.5f;

    [Header("Car Movement Speed:")]

    [Tooltip("Max Car Speed")]
    [SerializeField] private Vector2 maxSpeed = new Vector2(50,30);

    [Tooltip("Engine Forward Speed")]
    [SerializeField] private float accelerateSpeed = 50;

    [Tooltip("Engine stopping speed")]
    [SerializeField] private float decerateSpeed = 25;

    [Header("Steering Wheel settings")]

    [Tooltip("max angle of steering wheel in either direction")]
    [SerializeField] private float maxSteerAngle = 180;

    [Tooltip("The amount the steer angle is changed in a second (Used for pc)")]
    [SerializeField] private float steeringSpeed = 60;//temp untill vr steering used

    [Tooltip("The Speed the wheel returns to 0 when let go")]
    [SerializeField] private float returnSteerSpeed = 80;//the speed the steer returns to 0;

    [Header("Set Data")]

    [Tooltip("The transform parent of the steering wheel so it can turn")]
    [SerializeField] private Transform steeringWheel;

    [Tooltip("The transform of where the object gets pushed")]
    [SerializeField] private Transform trustPos;

    [Tooltip("Rigidbody of car gameobject")]
    [SerializeField] private Rigidbody carRb;

    [Header("Scripts")]

    [Tooltip("Script for the ui in the car")]
    [SerializeField] private CarUI carUIScript;

    [Header("Private Data")]
    private float currentHoverHeight = 3;
    private float gear = 0; // - = reverse // 0 = neutral // + = forward
    private float steerAngle = 0; // current angle of the steering wheel
    private float carAngle = 0; // current down angle of the car
    private float defaultDrag = 0;
    private float acelleration = 0;
    private float defaultHeight;

    /*
        Controls for now to test with pc : 

        a-d to steer left to right hold space to "hold" the steering weel
        w-s to go up and down //disabled
        left mouse button to accelerate forwards
        right mouse button to accelerate backwards

        controlls will be maped to controller of the vr headset
    */

    void Start()
    {
        defaultDrag = carRb.drag;
        defaultHeight = transform.position.y;
    }

    void Update()
    {
        accelerate();//acelerating forward or backwards function
        steering();//steering left and right
        // inclineCar();//moves car down and up //not used anymore car stays at one height
    }

    private void accelerate()
    {
        int gass = (Input.GetMouseButton(0) ? 1 : 0) + (Input.GetMouseButton(1) ? -1 : 0);/////with the vr controller it can be like a real gass pedle where you dont fully press the trigger down
        carUIScript.setGear(gass);
        addGass(gass);
        carRb.AddForceAtPosition(trustPos.forward * acelleration * Time.deltaTime,trustPos.position,ForceMode.VelocityChange);//moves car forward
        // carRb.AddForceAtPosition(trustPos.forward * carSpeed * gass * Time.deltaTime,trustPos.position,ForceMode.VelocityChange);//moves car forward
    }

    private void addGass(int gass)
    {
        if(gass == 0)
        {
            acelleration = returnZero(acelleration,decerateSpeed);
        }
        else
        {
            acelleration += gass * accelerateSpeed * Time.deltaTime; 
            acelleration = Mathf.Clamp(acelleration,-maxSpeed.y,maxSpeed.x);
        }
    }

    private void steering()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");//gets horizontal turn rate later needs to be gotten from steering wheel
        steerAngle += (inputHorizontal * steeringSpeed) * Time.deltaTime;
        steerAngle = Mathf.Clamp(steerAngle, -maxSteerAngle, maxSteerAngle);

        if(inputHorizontal == 0 && !Input.GetKey(KeyCode.Space))//space bar is to mimic when holding the steering wheel still
        {
            steerAngle = returnZero(steerAngle,returnSteerSpeed);//returns steering wheel to 0
        }

        transform.Rotate(Vector3.up * procentageAngle() * Time.deltaTime * 150);//rotates car in steering direction
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
        setSteering();//temp controlls for pc playing
    }

    private void inclineCar()
    {
        float inputVerticle = Input.GetAxis("Vertical");//using W and S for forward and backward
        if(inputVerticle != 0)
        {
            changeHeight(inputVerticle);
        }
    }

    private void changeHeight(float dir)
    {
        transform.Translate(Vector3.up * dir * heighChangeSpeed * Time.deltaTime);
        Vector3 originalPos = transform.position;
        float setHeight = Mathf.Clamp(originalPos.y, defaultHeight-hoverHeights.x, defaultHeight+hoverHeights.y);
        originalPos.y = setHeight;
        transform.position = originalPos;

    }

    private void setSteering()//temp until turned with vr hands
    {
        Vector3 currentAngle = steeringWheel.localEulerAngles;
        steeringWheel.localEulerAngles = new Vector3(currentAngle.x,currentAngle.y,-steerAngle);
    }

    private float returnZero(float currentAmount, float returnSpeed)
    {   
        float addAmount = returnSpeed * Time.deltaTime;
       
        if(currentAmount > 0)
        {
            if(currentAmount - addAmount > 0)
            {
                currentAmount -= addAmount;
            }
            else
            {
                currentAmount = 0; 
            }
        }
        else if(currentAmount < 0)
        {
            if(currentAmount + addAmount < 0)
            {
                currentAmount += addAmount;
            }
            else
            {
                currentAmount = 0; 
            }
        }

        return currentAmount;
    }

    ////////////////////// get values

    private float procentageAngle()//gets the procentage of the current angle of the steering wheel 
    {
        return (steerAngle - 0) / (maxSteerAngle - 0);
    }

    public float getSpeed()//for displaying car speed
    {
        return carRb.velocity.magnitude;
    }

    public float getAccel()
    {
        return acelleration;
    }

}
