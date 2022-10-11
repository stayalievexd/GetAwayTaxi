using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Hover Settings")]

    // [Tooltip("Min hover height and Max hover height")]
    // [SerializeField] private Vector2 hoverHeights = new Vector2(0.6f,5.0f);
    [SerializeField] private float[] hoverHeights = new float[4];

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

    [Tooltip("Speed of what the car turns at based on the steeringwheel angle")]
    [SerializeField] private float turnSpeed = 100;

    [Header("Set Data")]

    [Tooltip("The transform of where the object gets pushed")]
    [SerializeField] private Transform trustPos;

    [Tooltip("Rigidbody of car gameobject")]
    [SerializeField] private Rigidbody carRb;

    [Header("Private Scripts")]
    private CarUI carUIScript;
    private AiManager aiScript;
    private FakeSteeringWheel steerinScript;

    [Header("Private Data")]
    private float defaultDrag = 0;
    private float acelleration = 0;
    private float defaultHeight;
    private int currentHeight = 0;
    private int lastHeight = 0;
    private int dir = 0;
    private bool started = false;

    /*
        Controls for now to test with pc : 

        a-d to steer left to right hold space to "hold" the steering weel
        w-s to go up and down 
        left mouse button to accelerate forwards
        right mouse button to accelerate backwards
        r to start the car

        controlls will be maped to controller of the vr headset
    */
    public void setStart(CarUI newUi,AiManager newManager,FakeSteeringWheel newSteering)
    {
        carUIScript = newUi;
        aiScript = newManager;
        defaultDrag = carRb.drag;
        defaultHeight = transform.position.y;
        steerinScript = newSteering;
    }

    void Update()
    {
        if(!Values.pauzed)
        { 
            if(started)
            {
                accelerate();//acelerating forward or backwards function
                steering();//steering left and right
                CheckChangeHeight();
            }
            goToHeight();
        }
    }

    private void CheckChangeHeight()
    {
        if(Mathf.Abs(defaultHeight+hoverHeights[currentHeight]-transform.position.y) < 0.3f)
        {
            if(Input.GetMouseButton(0))
            {
                if(currentHeight < hoverHeights.Length-1)
                {
                    changeHeight(currentHeight + 1);
                }
            }
            else if(Input.GetMouseButton(1))
            {
                if(currentHeight > 0)
                {
                    changeHeight(currentHeight - 1);
                }
            }
        }
    }

    private void changeHeight(int newHeight)
    {
        Values.heightLayer = newHeight;//for new spawned cars
        lastHeight = currentHeight;
        aiScript.setHeight(currentHeight);
        if(newHeight > currentHeight)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        currentHeight = newHeight;
    }

    private void goToHeight()
    {
        transform.Translate(Vector3.up * dir * heighChangeSpeed * Time.deltaTime);
        Vector3 originalPos = transform.position;
        float setHeight;
        if(dir > 0)
        {
            setHeight = Mathf.Clamp(originalPos.y, defaultHeight+hoverHeights[lastHeight], defaultHeight+hoverHeights[currentHeight]);    
        }
        else
        {
            setHeight = Mathf.Clamp(originalPos.y, defaultHeight+hoverHeights[currentHeight], defaultHeight+hoverHeights[lastHeight]);
        }
        originalPos.y = setHeight;
        transform.position = originalPos;
    }

    private void accelerate()
    {
        float gass;
        if(Application.isEditor)
        {
            gass = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
        }
        else
        {
            gass = (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) + -OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger));
        }

        carUIScript.setGear(gass);
        addGass(gass);
        carRb.AddForceAtPosition(trustPos.forward * acelleration * Time.deltaTime,trustPos.position,ForceMode.VelocityChange);//moves car forward
    }

    private void addGass(float gass)
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
        transform.Rotate(Vector3.up * steerinScript.procentageAngle() * Time.deltaTime * turnSpeed);//rotates car in steering direction
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
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

    //outside script functions

    public void collision(float amount)
    {
        acelleration = returnZero(acelleration,amount * 100);
    }

    public void startCar(bool active)
    {
        started = active;
        if(active)
        {
            changeHeight(1);
        }
        else
        {
            changeHeight(0);
        }
    }

    ////////////////////// get values

    public float getSpeed()//for displaying car speed
    {
        return carRb.velocity.magnitude;
    }

    public float getAccel()
    {
        return acelleration;
    }
}
