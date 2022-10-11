using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSteeringWheel : MonoBehaviour
{
 
    [Header("Steering Wheel settings")]

    [Tooltip("max angle of steering wheel in either direction")]
    [SerializeField] private float maxSteerAngle = 180;

    [Tooltip("The amount the steer angle is changed in a second (Used for pc)")]
    [SerializeField] private float steeringSpeed = 180;//used for pc controlls

    [Tooltip("The Speed the wheel returns to 0 when let go")]
    [SerializeField] private float returnSteerSpeed = 200;//the speed the steer returns to 0;
    [SerializeField] private Vector3[] visualRot = new Vector3[2];
    [SerializeField] private Vector3 returnPos;

    [Header("Controllers")]
    [SerializeField] private OVRInput.Button[] grabInputs = new OVRInput.Button[2];
    [SerializeField] private Transform[] grabPoints = new Transform[2];//transforms of the holding points of the steering wheel
    [SerializeField] private Transform[] HandPostions = new Transform[2];//controllers
    [SerializeField] private string[] handTags = {"",""};//controllers tags

    [Header("Visuals")]
    private Transform visualSteeringWheel;
    private Transform[] realController = new Transform[2];//controllers
    private Transform[] HandVisual = new Transform[2];//controllers
    private Transform[] visualGrabPositions = new Transform[2];//transforms of the holding points of the steering wheel

    [Header("Private data")]
    private float steerAngle = 0; // current angle of the steering wheel
    private bool[] intriggerHand = new bool[2];//if the controllers are in the trigger
    [SerializeField] private bool[] posHeld = new bool[2];//if transform is being held

    public void setStart(Transform newWheel,Transform[] newHands,Transform[] newVisuals,Transform[] newHold)
    {
        visualSteeringWheel = newWheel;
        realController = newHands;
        HandVisual = newVisuals;
        visualGrabPositions = newHold;

        returnPos.x = transform.position.x;
    }

    private void Update()
    {
        setFollowHands();
        steering();
        checkGrip();
    }
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     string tagName = other.gameObject.tag;
    //     checkTrigger(true,tagName);
    // }
  
    // private void OnTriggerExit(Collider other)
    // {
    //     string tagName = other.gameObject.tag;
    //     checkTrigger(false,tagName);
    // }
    
    // private void checkTrigger(bool enter,string tagCol)
    // {
    //     for(int i=0; i<handTags.Length; i++)
    //     {
    //         if(tagCol == handTags[i])
    //         {
    //             intriggerHand[i] = enter;
    //         }
    //     }
    // }

    private void setFollowHands()
    {
        HandPostions[0].localPosition = realController[0].transform.localPosition;
        HandPostions[1].localPosition = realController[1].transform.localPosition;
    }
    
    private void checkGrip()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            letgo(0);
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            letgo(1);
        }

        for(int i=0; i<intriggerHand.Length; i++)
        {
            if(posHeld[i])//if hand is holding
            {
                setHandPositions(i);
                if(!OVRInput.Get(grabInputs[i]))//trigger let go or the distance is to far // || !OVRInput.Get(grabInputs[i])
                {
                    letgo(i);//hand lets go
                }
            }
            else
            {
                // if(intriggerHand[i])
                // {
                if(OVRInput.Get(grabInputs[i]))//trigger pressed
                {
                    posHeld[i] = true;//sets is holding
                }
                // }
            }
        }
    }

    private void setHandPositions(int handId)
    {
        HandVisual[handId].transform.position = new Vector3(visualGrabPositions[handId].position.x,visualGrabPositions[handId].position.y,visualGrabPositions[handId].position.z);
    }
    
    private void letgo(int handId)//lets go the hand of the given id
    {
        posHeld[handId] = false;
        HandVisual[handId].transform.localPosition = new Vector3(0,0,-0.0298f);//reset controller to origin position
    }

    private void steering()
    {
        if(Application.isEditor)//if in unity so playing on pc
        {
            float inputHorizontal = Input.GetAxis("Horizontal");//gets horizontal turn rate later needs to be gotten from steering wheel
            steerAngle += (inputHorizontal * steeringSpeed) * Time.deltaTime;
            steerAngle = Mathf.Clamp(steerAngle, -maxSteerAngle, maxSteerAngle);

            if(inputHorizontal == 0 && !Input.GetKey(KeyCode.LeftShift))//left shift is to mimic when holding the steering wheel still
            {
                steerAngle = returnZero(steerAngle,returnSteerSpeed);//returns steering wheel to 0
            }
            setSteering();//moves steering wheel
        }
        else//VR
        {
            if(isHeld())//if wheel is not beeing held by any hand
            {
                Vector3 currentAngle = transform.eulerAngles;
                if(posHeld[0] && posHeld[1])//both hands on the wheel
                {
                    Vector3 handPos1 = HandPostions[0].transform.position;
                    Vector3 handPos2 = HandPostions[1].transform.position;
                    handPos1.x = transform.position.x;
                    handPos2.x = transform.position.x;

                    Vector3 direction = handPos1 - handPos2;
                    
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, 1, 0.0f);

                    newDirection.x = 0;
                    Quaternion rot = Quaternion.LookRotation(newDirection);

                    clampRotation(rot);
                }
                else if(posHeld[0])//left hand on wheel
                {
                    oneHanded(0);
                }
                else if(posHeld[1])//right hand on the wheel
                {
                    oneHanded(1);
                }
            }
            else
            {
                returnVr();
                calculateSteer();
            }
            setChild(transform.localEulerAngles.y > 200 ? 0 : 1);//sets steering wheel child object right rotation
        }
        setVrSteering();
    }

    private void oneHanded(int handId)
    {
        Vector3 handPos = HandPostions[handId].transform.position;
        handPos.x = transform.position.x;

        Vector3 targetDirection = handPos - transform.position; 
        targetDirection *= (handId == 0 ? 1 : -1);

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1, 0.0f);

        newDirection.x = 0;
        Quaternion rot = Quaternion.LookRotation(newDirection);

        clampRotation(rot);
    }

    private void returnVr()
    {
        Vector3 targetDirection = returnPos - transform.position; 

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.05f, 0.0f);
        newDirection.x = 0;
        
        Quaternion rot = Quaternion.LookRotation(newDirection);

        clampRotation(rot);
    }
    
    private void clampRotation(Quaternion rot)
    {
        Vector3 oldRot = transform.localEulerAngles;
  
        oldRot.y = -90;
        oldRot.z = 0;
        
        bool oldRightSide = (transform.position.z > grabPoints[1].position.z);
        bool oldLeftSide = (transform.position.z > grabPoints[0].position.z);

        transform.rotation = rot;

        float height = (transform.position.y - grabPoints[0].position.y) - (transform.position.y - grabPoints[1].position.y) * 100;
        bool newRightSide = (transform.position.z > grabPoints[1].position.z);
        bool newLeftSide = (transform.position.z > grabPoints[0].position.z);

        if(oldRightSide != newRightSide && height < 0)
        {
            transform.localEulerAngles = oldRot;
        }
        else if(oldLeftSide != newLeftSide && height > 0)
        {
            transform.localEulerAngles = oldRot;
        }
        else
        {
            Vector3 newRot = transform.localEulerAngles;
            newRot.y = -90;
            newRot.z = 0;
            transform.localEulerAngles = newRot;
        }

        calculateSteer();
    }

    private void calculateSteer()
    {
        float newHeight = (transform.position.y - grabPoints[0].position.y) - (transform.position.y - grabPoints[1].position.y) * 100;
        float rotationProcentage = ((newHeight - -3.8f) * 100) / (3.8f - -3.8f);
        steerAngle = -(-maxSteerAngle - ((-maxSteerAngle - maxSteerAngle) / 100 * rotationProcentage));

        if(Mathf.Abs(steerAngle) < 20 && !isHeld())
        {
            steerAngle = 0;
        }
        // Debug.Log(steerAngle);
    }

    private void setSteering()//for pc steering
    {
        Vector3 currentAngle = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(-steerAngle,-90,0);
    }

    private void setVrSteering()
    {
        visualSteeringWheel.localEulerAngles = transform.localEulerAngles;
    }

    private void setChild(int set)
    {
        transform.GetChild(0).transform.localEulerAngles = visualRot[set];
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

    public float procentageAngle()//gets the procentage of the current angle of the steering wheel 
    {
        float procentage = (steerAngle - 0) / (maxSteerAngle - 0);
      
        return procentage;
    }

    private bool isHeld()
    {
        bool holdingWheel = true;
        if(!posHeld[0] && !posHeld[1])
        {
            return false;
        }

        return holdingWheel;
    }
}
