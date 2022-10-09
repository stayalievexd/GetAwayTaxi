using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    [Header("Steering Wheel settings")]

    [Tooltip("max angle of steering wheel in either direction")]
    [SerializeField] private float maxSteerAngle = 180;

    [Tooltip("The amount the steer angle is changed in a second (Used for pc)")]
    [SerializeField] private float steeringSpeed = 180;//used for pc controlls

    [Tooltip("The Speed the wheel returns to 0 when let go")]
    [SerializeField] private float returnSteerSpeed = 200;//the speed the steer returns to 0;

    [Header("Controllers")]
    [SerializeField] private OVRInput.Button[] grabInputs = new OVRInput.Button[2];
    [SerializeField] private Transform[] grabPoints = new Transform[2];//transforms of the holding points of the steering wheel
    [SerializeField] private Transform[] HandPostions = new Transform[2];//controllers
    [SerializeField] private Transform[] HandVisual = new Transform[2];//controllers
    [SerializeField] private string[] handTags = {"",""};//controllers tags

    [Header("Private data")]
    private float steerAngle = 0; // current angle of the steering wheel
    private bool[] intriggerHand = new bool[2];//if the controllers are in the trigger
    [SerializeField] private int[] holding = new int[2];//if controller is on the wheel
    private bool[] posHeld = new bool[2];//if transform is being held

    /*
        This script will be used for rotating the steering wheel while holding it

        Needs to return the value 
        Needs to return if holding 

        Needs to rotate the wheel based on how much the hand is moved from the original holding position
    */

    private void Start()
    {
        holding[0] = -1;
        holding[1] = -1;

        // holding[0] = 0;
        // holding[1] = 1;
    }

    private void Update()
    {
        steering();
        checkGrip();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        string tagName = other.gameObject.tag;
        checkTrigger(true,tagName);
    }
  
    private void OnTriggerExit(Collider other)
    {
        string tagName = other.gameObject.tag;
        checkTrigger(false,tagName);
    }

    private void checkTrigger(bool enter,string tagCol)
    {
        for(int i=0; i<handTags.Length; i++)
        {
            if(tagCol == handTags[i])
            {
                intriggerHand[i] = enter;
            }
        }
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
            if(holding[i] != -1)//if hand is holding
            {
                if(!checkDistance())//trigger let go or the distance is to far // || !OVRInput.Get(grabInputs[i])
                {
                    letgo(i); 
                    //hand lets go
                }
                setHandPositions(i);
            }
            else
            {
                if(intriggerHand[i])
                {
                    if(OVRInput.Get(grabInputs[i]))//trigger pressed
                    {
                        tryHolding(i);
                    }
                }
            }
        }
    }

    private void setHandPositions(int handId)
    {
        HandVisual[handId].transform.position = new Vector3(grabPoints[holding[handId]].position.x,grabPoints[holding[handId]].position.y,grabPoints[holding[handId]].position.z);
    }
    
    private bool checkDistance()
    {
        return true;//if true the distance is close enough
        //check if the distance isnt to far from the steering wheel other wise let go
    }

    private void letgo(int handId)//lets go the hand of the given id
    {
        posHeld[holding[handId]] = false;
        holding[handId] = -1;//sets hand holding back to not holding
        HandVisual[handId].transform.localPosition = new Vector3(0,0,0);//reset controller to origin position
    }

    private void tryHolding(int hand)
    {
        int idClose = checkClosed(hand);
        if(idClose > -1)//if can hold
        {
            holding[hand] = idClose;
            posHeld[idClose] = true;//sets is holding
        }
    }

    private int checkClosed(int hand)
    {
        float dist1 = Vector3.Distance(grabPoints[0].position, HandPostions[hand].position);
        float dist2 = Vector3.Distance(grabPoints[1].position, HandPostions[hand].position);

        int setSlot = 0;

        if(dist1 > dist2)
        {
            setSlot = 1;
        }

        if(posHeld[setSlot])
        {
            setSlot = -1;
        }

        return setSlot;
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

            setSteering();//temp controlls for pc playing
        }
        else//VR
        {
            if(isHeld())//if wheel is not beeing held by any hand
            {
                Vector3 currentAngle = transform.eulerAngles;
                if(holding[0] != -1 && holding[1] != -1)//both hands on the wheel
                {
                    // Vector3 angle1 = Hands[0].transform.position - transform.position;
                    // Vector3 angle2 = Hands[1].transform.position - transform.position;

                    // currentAngle = angle1 - angle2;
                }
                else if(holding[0] != -1)//left hand on wheel
                {
                    // currentAngle = HandPostions[0].transform.position - transform.position;
                    // Quaternion rot = Quaternion.LookRotation(currentAngle,-transform.right);
                    // transform.rotation = rot;

                    // Vector3 currentRot = transform.localEulerAngles;
                    // currentRot.x = 0;
                    // // currentRot.y = currentRot.y + 90;
                    // currentRot.y = 0;
                    // // currentRot.z = 0;
                    // transform.localEulerAngles = currentRot;
                    // // Quaternion rot = Quaternion.LookRotation(currentAngle,transform.up);
                    // // rot.x=0;
                    // // rot.z=0;
                    // Debug.Log("test" + currentAngle);
                }
                else if(holding[1] != -1)//right hand on the wheel
                {
                    currentAngle = HandPostions[1].transform.position - transform.position;
                }

                // Quaternion rot = Quaternion.LookRotation(currentAngle,transform.up);
                // currentAngle.y = 0;
                // currentAngle.z = 0;
                // // transform.LookAt(HandPostions[0].transform);
                // transform.localEulerAngles = currentAngle;
                // Debug.Log(rot);
                // Vector3 normalRotation = transform.localEulerAngles;
                // transform.rotation = rot;
            }
            else
            {
                steerAngle = returnZero(steerAngle,returnSteerSpeed);//returns steering wheel to 0
                setSteering();//temp controlls for pc playing
            }
        }

    }

    private void setSteering()//temp until turned with vr hands
    {
        Vector3 currentAngle = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentAngle.x,currentAngle.y,-steerAngle);
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
        if(holding[0] == -1 && holding[1] == -1)
        {
            return false;
        }

        return holdingWheel;
    }
}
