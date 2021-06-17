using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private List<string> ferret = new List<string>();
    private const byte RESET_SCENE = 22;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ferret.Count >= 6)
        {
            string name = PhotonNetwork.LocalPlayer.NickName;
            PhotonNetwork.RaiseEvent(RESET_SCENE, "", RaiseEventOptions.Default, SendOptions.SendUnreliable);
            GameObject.Find(name).GetComponent<PlayerController>().Reset_Scene_Function();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ferret.Add(other.name);
        Debug.Log(ferret.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        ferret.Remove(other.name);
        Debug.Log(ferret.Count);
    }
}
