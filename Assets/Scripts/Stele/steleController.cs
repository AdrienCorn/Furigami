using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class steleController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePower;
    [SerializeField] private GameObject platformPower;
    [SerializeField] private GameObject waterPower;

    private string stelePower;
    private string actualPower;

    private Vector3 Ray;

    private const byte STELE_ACTUALISATION = 5;

    private void OnEnable()
    {
        PlayerController.onSteleInteraction += OnSteleInteraction;
        PlayerController.Reset_Scene_Event += Reset_Scene;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnSteleActualisation;
    }

    private void OnDisable()
    {
        PlayerController.onSteleInteraction -= OnSteleInteraction;
        PlayerController.Reset_Scene_Event -= Reset_Scene;
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnSteleActualisation;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (projectilePower.activeSelf)
            stelePower = "projectilePower";
        else if (platformPower.activeSelf)
            stelePower = "platformPower";
        else if (waterPower.activeSelf)
            stelePower = "waterPower";
        else
            stelePower = null;
    }

    private void Update()
    {

    }

    private void OnSteleInteraction(GameObject Player)
    {
        Ray.x = Player.transform.position.x - transform.position.x;
        Ray.y = Player.transform.position.y - transform.position.y;
        Ray.z = Player.transform.position.z - transform.position.z;
        if (Ray.magnitude <= 5 && Input.GetKeyDown("e"))
        {
            //Player.GetComponent<PlayerController>().setDefaultSkin();
            GetActualPlayerPower(Player);
            if (stelePower == "projectilePower")
            {
                Player.GetComponent<Tir>().enabled = true;
                Player.GetComponent<PlayerController>().setNutSkin();
            }
            else if (stelePower == "platformPower")
            {
                Player.GetComponent<PlateformPower>().enabled = true;
                Player.GetComponent<PlayerController>().setHatSkin();
            }
            else if (stelePower == "waterPower")
            {
                Player.GetComponent<PlayerController>().playerType = 2;
                Player.GetComponent<PlayerController>().setSlimeSkin();
            }
            else
                Player.GetComponent<PlayerController>().setDefaultSkin();
            ActualiseStelePower(actualPower);
            string encoded_data = this.name + "|" + actualPower;
            object[] content = new object[] { encoded_data };
            PhotonNetwork.RaiseEvent(STELE_ACTUALISATION, content[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    private void GetActualPlayerPower(GameObject Player)
    {
        if (Player.GetComponent<Tir>().enabled)
        {
            actualPower = "projectilePower";
            Player.GetComponent<Tir>().enabled = false;
        }
        else if (Player.GetComponent<PlateformPower>().enabled)
        {
            actualPower = "platformPower";
            Player.GetComponent<PlateformPower>().enabled = false;
        }
        else if (Player.GetComponent<PlayerController>().playerType == 2)
        {
            actualPower = "waterPower";
            Player.GetComponent<PlayerController>().playerType = 0;
        }
        else
            actualPower = null;
        Debug.Log("actualPower : " + actualPower);
    }

    private void ActualiseStelePower(string newStelePower)
    {
        projectilePower.SetActive(newStelePower == "projectilePower");
        platformPower.SetActive(newStelePower == "platformPower");
        waterPower.SetActive(newStelePower == "waterPower");
        stelePower = newStelePower;
    }

    private void NetworkingClient_OnSteleActualisation(EventData obj)
    {
        if (obj.Code == STELE_ACTUALISATION)
        {
            string data = (string)obj.CustomData;
            string[] subs = data.Split('|');
            string obj_name = subs[0];
            string obj_newPower = subs[1];

            if (obj_name == this.gameObject.name)
                ActualiseStelePower(obj_newPower);
        }
    }

    private void Reset_Scene()
    {
        ActualiseStelePower(this.name);
    }
}
