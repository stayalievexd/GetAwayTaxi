using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrashedState : State
{   
    [Header("Crash settings")]

    [Tooltip("Car drag sets the falling speed of the car")]
    [SerializeField] private Vector2 carfallDrag = new Vector2(2,6);

    [Tooltip("Final forward speed boost")]
    [SerializeField] private Vector2 finalBoost = new Vector2(50,100);

    [Tooltip("Final boost time")]
    [SerializeField] private float boostTime = 2;

    [Tooltip("Time after crash when car gets removed")]
    [SerializeField] private float removeTime = 10.0f;

    [Header("Set Components")]

    [Tooltip("Car rigidbody from root object")]
    [SerializeField] private Rigidbody carRb;

    [Tooltip("Animator from carbody object")]
    [SerializeField] private Animator carAnim;
    
    [Tooltip("Agent from rootObject")]
    [SerializeField] private NavMeshAgent agent;
    
    [Tooltip("CrashEffect")]
    [SerializeField] private ParticleSystem crashEffect;

    [Tooltip("endExplotion")]
    [SerializeField] private ParticleSystem explotionEffect;
    
    [Tooltip("Spinning out animation")]
    [SerializeField] private Animation spinOutAnim;

    [Header("Private Data")]
    private float speed;
    private float randomAddSpeed;
    private float followSpeed = 0.1f;
    public override State runThisState()
    {
        if(boostTime > 0)
        {
            carRb.AddForce(transform.forward * speed * randomAddSpeed * Time.deltaTime,ForceMode.VelocityChange);

            Vector3 backAngle = Vector3.Lerp(carRb.transform.eulerAngles, transform.eulerAngles, followSpeed * Time.deltaTime);
            carRb.transform.eulerAngles = backAngle;

            boostTime -= 1 * Time.deltaTime;
        }

        return this;
    }

    public void crash(Vector3 addedForce)
    {
        carAnim.enabled = false;//turns off the bobbing hover effect
        agent.enabled = false;//disables ai nav movement
        speed = agent.speed;//gets last speed of agent this effects the final boost speed
        randomAddSpeed = Random.Range(finalBoost.x,finalBoost.y);//random final boost speed
        carRb.isKinematic = false;
        carRb.drag = Random.Range(carfallDrag.x,carfallDrag.y);
        followSpeed = Random.Range(0.2f,0.4f);//random speed of spinning out animation
        carRb.AddForce(addedForce,ForceMode.Impulse);
        spinOutAnim.Play();//plays spinning out animation //can be tweeked for better effect
        
        if(crashEffect)//if has final crashing particle effect
        {
            crashEffect.Play();
        }

        Invoke("explode",removeTime);//calls clean up explotion timer 
    }

    private void explode()
    {
        if(explotionEffect)
        {
            explotionEffect.Play();
            Destroy(this.transform.root.gameObject,explotionEffect.main.duration);
        }
        else
        {
            Destroy(this.transform.root.gameObject,1.0f);
        }
        carAnim.gameObject.SetActive(false);//turns of the body of the car
    }
}
