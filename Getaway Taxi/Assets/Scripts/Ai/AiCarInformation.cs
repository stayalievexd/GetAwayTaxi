using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AiCar", menuName = "Scriptables/AiCar")]

public class AiCarInformation : ScriptableObject
{
    [Header("Strings")]
    public string Name = "AI-Car";

    [Header("Gameobjects")]
    public GameObject spawnObject;

    [Header("Patrolling")]

    [Tooltip("The min and max speed the car gets for patrolling")]
    public Vector2 patrolSpeed = new Vector2(3,9);

    [Tooltip("The min and max speed the car gets for chasing")]
    public Vector2 chaseSpeed = new Vector2(10,15);

    [Header("Ints")]
    public int rarity = 1;

    [Header("Bools")]
    public bool police = false;
}
