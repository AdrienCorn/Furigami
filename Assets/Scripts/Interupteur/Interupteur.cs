using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interupteur : MonoBehaviour
{
    [SerializeField] private GameObject bridge;

    private Vector3 Ray;

    private void OnEnable()
    {
        PlayerController.onSteleInteraction += OnTriggerInteraction;
    }

    private void OnDisable()
    {
        PlayerController.onSteleInteraction -= OnTriggerInteraction;
    }

    private void OnTriggerInteraction(GameObject Player)
    {
        Ray.x = Player.transform.position.x - transform.position.x;
        Ray.y = Player.transform.position.y - transform.position.y;
        Ray.z = Player.transform.position.z - transform.position.z;
        if (Ray.magnitude <= 5 && Input.GetKeyDown("t"))
        {
            MoveBridge();
        }
    }

    public void MoveBridge()
    {
        Debug.Log("bridge move");
        bridge.transform.localScale = new Vector3(bridge.transform.localScale.x, bridge.transform.localScale.y, 0);
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
