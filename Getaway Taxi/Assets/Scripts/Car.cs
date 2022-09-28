using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Hover Settings")]

    [Tooltip("position the ground is checked from")]
    [SerializeField] private Transform[] HoverPoints;

    [Tooltip("Power of hover trusters")]
    [SerializeField] private float hoverPower = 100;

    [Tooltip("Min hover height and Max hover height")]
    [SerializeField] private Vector2 hoverHeights = new Vector2(0.6f,5.0f);

    [Tooltip("The speed the car goes up and down in height")]
    [SerializeField] private float heighChangeSpeed = 2.5f;

    [Tooltip("Max incline angle of car up and down")]
    [SerializeField] private float maxIncline = 45;

    [Tooltip("Speed car angles down or up")]
    [SerializeField] private float inclineSpeed = 50;

    [Tooltip("Speed car angle returns to 0")]
    [SerializeField] private float returnAngle = 100;

    [Header("Car Movement Speed:")]

    [Tooltip("Engine Forward Speed")]
    [SerializeField] private float carSpeed = 50;

    [Tooltip("The dragg of the car when brake enabled")]
    [SerializeField] private float brakeDrag = 5;
    
    [Header("Steering Wheel settings")]

    [Tooltip("Speed car turns at")]
    [SerializeField] private float turningSpeed = 100;

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

    [Tooltip("Gear stick Animator")]
    [SerializeField] private Animator gearAnim;

    [Header("Scripts")]

    [Tooltip("Script for the ui in the car")]
    [SerializeField] private CarUI carUIScript;

    [Header("Private Data")]
    private float currentHoverHeight = 3;
    private float gear = 0; // - = reverse // 0 = neutral // + = forward
    private float steerAngle = 0; // current angle of the steering wheel
    private float carAngle = 0; // current down angle of the car
    private float defaultDrag = 0;
    private float defaultHeight;

    /*
        Controls for now to test with pc : 

        a-d to steer left to right hold space to "hold" the steering weel
        w-s to go up and down 
        left mouse button to accelerate
        right mouse button to brake
        1 to go in to forward 
        2 to go in to neutral
        3 to go in to reverse

        controlls will be maped to controller of the vr headset
    */

    void Start()
    {
        defaultDrag = carRb.drag;
        defaultHeight = transform.position.y;
    }

    void Update()
    {
        // setGear();//gets gear inputs temp for pc playing
        // checkBrake();//checks if the brake button is pressed//not used anymore kept as a backup
        accelerate();//acelerating forward or backwards function
        steering();//steering left and right
        // inclineCar();//incline car angle down and up //not used anymore car stays at one height
        // keepHeight();//keeps the height of the car //not used anymore kept as a backup
    }

    private void accelerate()
    {
        int gass = (Input.GetMouseButton(0) ? 1 : 0) + (Input.GetMouseButton(1) ? -1 : 0);/////with the vr controller it can be like a real gass pedle where you dont fully press the trigger down
        carUIScript.setGear(gass);
        carRb.AddForceAtPosition(trustPos.forward * carSpeed * gass * Time.deltaTime,trustPos.position,ForceMode.VelocityChange);//moves car forward
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
        setSteering();//temp controlls for pc playing
    }


    private void keepHeight()
    {   
        foreach(Transform HoverPoint in HoverPoints)
        {
            RaycastHit hit;
            if(Physics.Raycast(HoverPoint.position, transform.TransformDirection(Vector3.down), out hit, hoverHeights.y))
            {
                carRb.AddForceAtPosition(transform.TransformDirection(Vector3.up) * Mathf.Pow(hit.distance,2)/hoverHeights.y * hoverPower,HoverPoint.position * Time.deltaTime);
            }
        }
    }

    private void inclineCar()
    {
        float inputVerticle = Input.GetAxis("Vertical");//using W and S for forward and backward
        carAngle += (inputVerticle * inclineSpeed) * Time.deltaTime;
        carAngle = Mathf.Clamp(carAngle, -maxIncline, maxIncline);

        if(inputVerticle == 0)//down
        {
           carAngle = returnZero(carAngle,returnAngle);
        }
        else
        {
            changeHeight(inputVerticle);
        }
    }

    private void changeHeight(float dir)
    {
        /*
        Vector3 originalPos = transform.position;
        Vector3 goPos = originalPos;
        goPos.y = defaultHeight + newHeight;
        Vector3 newPos = Vector3.Lerp(originalPos, goPos, heighChangeSpeed * Time.deltaTime);
        transform.position = newPos;
        */

        transform.Translate(Vector3.up * dir * heighChangeSpeed * Time.deltaTime);
        Vector3 originalPos = transform.position;
        float setHeight = Mathf.Clamp(originalPos.y, defaultHeight-hoverHeights.x, defaultHeight+hoverHeights.y);
        originalPos.y = setHeight;
        transform.position = originalPos;

    }

    private void checkBrake()
    {
        if(Input.GetMouseButtonDown(1))//right mouse button to break
        {
            carRb.drag = brakeDrag;
        }
        if(Input.GetMouseButtonUp(1))
        {
            carRb.drag = defaultDrag;
        }
    }

    private void setGear()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            gear = 1;//forward
            carUIScript.setGear(1);
            gearAnim.SetInteger("Gear",1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            gear = 0;//neutral
            carUIScript.setGear(0);
            gearAnim.SetInteger("Gear",0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            gear = -0.5f;//reverse
            carUIScript.setGear(-1);
            gearAnim.SetInteger("Gear",-1);
        }
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

}
