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
        if(this.name == "interupteur") 
        {
            bridge.transform.localScale = new Vector3(bridge.transform.localScale.x, bridge.transform.localScale.y, 1);
            StartCoroutine(BridgeCoroutine());
        }
        if (this.name == "interupteur_final")
        {
            StartCoroutine(BarrierCoroutine());
        }

    }

    public IEnumerator BridgeCoroutine()
    {
        while (bridge.transform.localScale.z < 22)
        {
            yield return new WaitForSeconds(0);
            bridge.transform.localScale += new Vector3(0, 0, 0.2f);
        }
    }

    public IEnumerator BarrierCoroutine()
    {
        while (bridge.transform.position.y > -6)
        {
            yield return new WaitForSeconds(0);
            bridge.transform.position += new Vector3(0, -0.2f, 0);
        }
    }

    public void Reset_Scene()
    {
        if (this.name == "interupteur")
        {
            bridge.transform.localScale = new Vector3(2.0f, 0.2f, 1.0f);
            this.GetComponent<Renderer>().material = this.inactive;
        }
        if (this.name == "interupteur_final")
        {
            bridge.transform.position = new Vector3(12.5f, -4.3f, 5.62f);
            this.GetComponent<Renderer>().material = this.inactive;
        }
    }
}
