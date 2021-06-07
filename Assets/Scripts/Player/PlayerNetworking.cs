using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworking : MonoBehaviour
{
    public MonoBehaviour[] ScriptToIgnore;

    public Camera cameraToIgnore;

    public AudioListener audioListenerToIgnore;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            foreach (var script in ScriptToIgnore)
            {
                script.enabled = false;
            }
            cameraToIgnore.enabled = false;

            audioListenerToIgnore.enabled = false;
        }
    }
}
