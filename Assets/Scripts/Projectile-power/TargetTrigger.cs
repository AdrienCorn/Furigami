using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bridge;

    public void MoveBridge()
    {
        Debug.Log("bridge move");
        bridge.transform.position = new Vector3(0, 0, 0);
    }
}
