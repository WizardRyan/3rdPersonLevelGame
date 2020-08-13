using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCollectibleController : MonoBehaviour
{

    private float rotationAmount = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuScript.isPaused)
        {
            transform.Rotate(transform.rotation.x + rotationAmount, transform.rotation.y + rotationAmount, transform.rotation.z + rotationAmount);   
        }
    }
}
