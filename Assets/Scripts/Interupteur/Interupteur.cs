using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interupteur : MonoBehaviour
{
    [SerializeField] private GameObject objectTrigger;

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
            if(this.gameObject.name=="interupteur2")
                MoveBridge();
            if (this.gameObject.name == "interupteur3")
                MoveObstacle();
        }
    }

    public void MoveBridge()
    {
        Debug.Log("bridge move");
        objectTrigger.transform.localScale = new Vector3(objectTrigger.transform.localScale.x, objectTrigger.transform.localScale.y, 0);
        StartCoroutine(BridgeCoroutine());
    }

    public IEnumerator BridgeCoroutine()
    {
        while (objectTrigger.transform.localScale.z < 22)
        {
            yield return new WaitForSeconds(0);
            objectTrigger.transform.localScale += new Vector3(0, 0, 0.2f);
        }
    }

    public void MoveObstacle()
    {
        Debug.Log("obstacle move");
        objectTrigger.transform.localScale = new Vector3(objectTrigger.transform.localScale.x, objectTrigger.transform.localScale.y, objectTrigger.transform.localScale.z);
        StartCoroutine(ObstacleCoroutine());
    }

    public IEnumerator ObstacleCoroutine()
    {
        while (objectTrigger.transform.localScale.y > 0)
        {
            yield return new WaitForSeconds(0);
            objectTrigger.transform.localScale += new Vector3(0, -0.04f, 0);
        }
    }
}
