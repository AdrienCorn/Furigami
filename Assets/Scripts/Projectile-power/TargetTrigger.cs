using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bridge;

    public void MoveBridge()
    {
        Debug.Log("bridge move");
        bridge.transform.localScale = new Vector3(bridge.transform.localScale.x, bridge.transform.localScale.y, 1);
        StartCoroutine(BridgeCoroutine());
    }

    public IEnumerator BridgeCoroutine()
    {
        while (bridge.transform.localScale.z < 22)
        {
            yield return new WaitForSeconds(0);
            bridge.transform.localScale += new Vector3(0, 0, 0.2f);
        }
    }
}
