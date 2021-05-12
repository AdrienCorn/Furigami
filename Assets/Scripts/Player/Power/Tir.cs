using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;


public class Tir : MonoBehaviour
{
    public GameObject proj;

    public static event Action<GameObject, Vector3> onProjectilShoot;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            
            if (GameObject.FindGameObjectsWithTag("projectile").Length < 3)
            {
                Vector3 tmpPos = this.transform.position + new Vector3(-2,0,-1);
                tmpPos.x += 2;
                //GameObject tmpBall = Instantiate(proj, tmpPos, Quaternion.identity);
                PhotonNetwork.Instantiate("Projectile-power", tmpPos, Quaternion.identity, 0);
                onProjectilShoot?.Invoke(this.gameObject, Vector3.forward);
                
            }

        }
    }
}
