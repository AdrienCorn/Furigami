using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tir : MonoBehaviour
{
    public GameObject proj;

    public static event Action<GameObject> onProjectilShoot;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            
            if (GameObject.FindGameObjectsWithTag("projectile").Length < 3)
            {
                Vector3 tmpPos = this.transform.position + new Vector3(-2,0,-1);
                tmpPos.x += 2;
                PhotonNetwork.Instantiate("Projectile-power", tmpPos, Quaternion.identity, 0).GetComponent<Projectile>().projectileIsNew = true;
                onProjectilShoot?.Invoke(this.gameObject);
                
            }

        }
    }
}
