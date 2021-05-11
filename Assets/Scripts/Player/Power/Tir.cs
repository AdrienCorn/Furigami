﻿using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;


public class Tir : MonoBehaviour
{
    public GameObject proj;

    public static event Action<Transform, Vector3> onProjectilShoot;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            
            if (GameObject.FindGameObjectsWithTag("projectile").Length < 3)
            {
                Vector3 tmpPos = this.transform.position;
                tmpPos.x += 2;
                //GameObject tmpBall = Instantiate(proj, tmpPos, Quaternion.identity);
                PhotonNetwork.Instantiate("Projectile-power", tmpPos, Quaternion.identity, 0);
                onProjectilShoot?.Invoke(this.transform, Vector3.forward);
                
            }

        }
    }
}