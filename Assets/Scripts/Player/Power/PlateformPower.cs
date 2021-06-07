using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformPower : MonoBehaviour
{
    public static event Action<Transform> onPlateformPowerUp;
    public static event Action<Transform> onPlateformPowerDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            onPlateformPowerUp?.Invoke(this.transform);
        }
        if  (Input.GetKeyUp("b"))
        {
            onPlateformPowerDown?.Invoke(this.transform);
        }
        if (Input.GetKey("b"))
        {
            GetComponent<PlayerController>().enabled = false;
        }
        else
            GetComponent<PlayerController>().enabled = true;
    }
}
