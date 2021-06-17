using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Script : MonoBehaviour
{

    private const byte RESET_SCENE = 22;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset_Scene_Function()
    {
        string name = PhotonNetwork.LocalPlayer.NickName;
        PhotonNetwork.RaiseEvent(RESET_SCENE, "", RaiseEventOptions.Default, SendOptions.SendUnreliable);
        GameObject.Find(name).GetComponent<PlayerController>().Reset_Scene_Function();

    }
}
