using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    public Material active;
    public Material inactive;

    private void OnEnable()
    {
        PlayerController.Reset_Scene_Event += Reset_Scene;
    }

    private void OnDisable()
    {
        PlayerController.Reset_Scene_Event -= Reset_Scene;
    }

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

    public void Reset_Scene()
    {
        bridge.transform.localScale = new Vector3(2.0f, 0.2f, 1.0f);
        this.GetComponent<Renderer>().material = this.inactive;
    }
}
