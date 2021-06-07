using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bridge;

    public void MoveBridge()
    {
        Debug.Log("bridge move");
        bridge.transform.localScale = new Vector3(bridge.transform.localScale.x, bridge.transform.localScale.y, 22);
    }
}
