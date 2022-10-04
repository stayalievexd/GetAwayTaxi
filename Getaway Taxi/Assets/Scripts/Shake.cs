using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{

    /*
        Tried using this for the car shake when the car starts up
    
    */

    private Vector3 originalPos;
    void Start()
    {
        originalPos = this.transform.localPosition;
    }

    public void startShake(float duration, float magnitude)
    {
        IEnumerator coroutine = ShakeObject(duration,magnitude);
        StartCoroutine(coroutine);
    }

    public IEnumerator ShakeObject (float duration, float magnitude)
    {
        float elepsed = 0.0f;

        while(elepsed < duration)
        {
            Debug.Log("Shake");
            float x = Random.Range(-2f,2f) * magnitude;
            float y = Random.Range(-0.2f,0.2f) * magnitude;
            float z = Random.Range(-0.2f,0.2f) * magnitude;
            
            this.transform.localPosition = new Vector3(x,y,z);

            elepsed += Time.deltaTime;

            yield return null;
        }

        this.transform.localPosition = originalPos;
    }
}
