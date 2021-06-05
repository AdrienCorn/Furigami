using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPower : MonoBehaviour
{
    public Rigidbody theRB;
    public LayerMask theWater;
    public Transform groundPoint;
    private bool isDrowning;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, theWater))
        {
            if (playerType != 2)
			{
                theRB.transform.position = new Vector3(theRB.position.x + 10 , 1, theRB.position.z);
            }
            else // if has power to float
			{
                Debug.Log("eau touchée");
                //this.transform.Rotate(90, -90, this.transform.rotation.z);
                //this.transform.eulerAngles = new Vector3(90, -90, this.transform.rotation.z);
            }
        }

        
    }
}
