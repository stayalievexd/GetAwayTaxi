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

    [Header("Floats")]

    [Tooltip("The min amount for the random speed the cars get")]
    public float minSpeed = 3;

    [Tooltip("The max amount for the random speed the cars get")]
    public float maxSpeed = 9;

    [Header("Ints")]
    public int rarity = 1;

    [Header("Bools")]
    public bool police = false;
}
