using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interupteur : MonoBehaviour
{
    [SerializeField] private GameObject objectTrigger;

    private Vector3 Ray;
    private const byte BRIDGE_MOVE = 10;
    private const byte WALL_MOVE = 11;
    private bool deployed;

    private void OnEnable()
    {
        PlayerController.onSteleInteraction += OnTriggerInteraction;
        PlayerController.Reset_Scene_Event += Reset_Scene;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnTrigger;
    }

    private void OnDisable()
    {
        PlayerController.onSteleInteraction -= OnTriggerInteraction;
        PlayerController.Reset_Scene_Event -= Reset_Scene;
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnTrigger;
    }

    public void Start()
    {
        deployed = false;
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
        if (!deployed)
        {
            deployed = true;
            Debug.Log("bridge move");
            objectTrigger.transform.localScale = new Vector3(objectTrigger.transform.localScale.x, objectTrigger.transform.localScale.y, 0);
            StartCoroutine(BridgeCoroutine());
            object[] datas = new object[] { this.name };
            PhotonNetwork.RaiseEvent(BRIDGE_MOVE, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
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
        if (!deployed)
        {
            deployed = true;
            Debug.Log("obstacle move");
            objectTrigger.transform.localScale = new Vector3(objectTrigger.transform.localScale.x, objectTrigger.transform.localScale.y, objectTrigger.transform.localScale.z);
            StartCoroutine(ObstacleCoroutine());
            object[] datas = new object[] { this.name };
            PhotonNetwork.RaiseEvent(WALL_MOVE, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    public IEnumerator ObstacleCoroutine()
    {
        while (objectTrigger.transform.localScale.y > 0)
        {
            yield return new WaitForSeconds(0);
            objectTrigger.transform.localScale += new Vector3(0, -0.04f, 0);
        }
    }

    private void NetworkingClient_OnTrigger(EventData obj)
    {
        if (obj.Code == BRIDGE_MOVE && (string)obj.CustomData == this.name)
        {
            MoveBridge();
        }
        else if (obj.Code == WALL_MOVE && (string)obj.CustomData == this.name)
        {
            MoveObstacle();
        }
    }

    private void Reset_Scene()
    {
        if (this.gameObject.name == "interupteur2")
            objectTrigger.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            deployed = false;
        if (this.gameObject.name == "interupteur3")
            objectTrigger.transform.localScale = new Vector3(10.0f, 13.0f, 1.0f);
            deployed = false;
    }
}
