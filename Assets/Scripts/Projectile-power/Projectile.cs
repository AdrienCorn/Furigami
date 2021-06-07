using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    //public Transform player;
    public Material active;
    public Material inactive;
    public bool projectileIsNew = true;

    private const byte THROW_PROJECTILE = 1; // byte to be sent by photon
    // TODO make an enum with all the bytes, to ensure unicity of it
    private const byte SYNCRONISE_ID = 3;

    public int velocity;

    private void Start()
    {
    }

    #region event handler from player

    private void OnEnable()
    {
        Tir.onProjectilShoot += OnProjectilShoot;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnProjectileShhot;
    }

    private void OnDisable()
    {
        Tir.onProjectilShoot -= OnProjectilShoot;
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnProjectileShhot;
    }
    
    private void OnProjectilShoot(GameObject Player)
    {
        if (projectileIsNew)
        {
            projectileIsNew = false;
            //Debug.Log("local event");
            //Debug.Log(Player.transform.GetChild(0).rotation.eulerAngles.y);
            int direction;
            if (Player.transform.GetChild(0).rotation.eulerAngles.y > 90)
            {
                Debug.Log("droite");
                direction = 1;
            }
            else
            {
                Debug.Log("gauche");
                direction = -1;
            }
            this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(direction, 0, 0) * velocity);
            //Debug.Log(new Vector3(-Player.transform.GetChild(0).rotation.eulerAngles.y, 0, 0));
            object[] datas = new object[] { direction };
            PhotonNetwork.RaiseEvent(THROW_PROJECTILE, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    private void NetworkingClient_OnProjectileShhot(EventData obj)
    {
        if (obj.Code == THROW_PROJECTILE && projectileIsNew)
        {
            projectileIsNew = false;
            {
                this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3((int)obj.CustomData, 0, 0) * velocity); //to follow player rotation
            }
        }
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (other.tag == "interrupteur")
        {
            Debug.Log("activé");
            other.GetComponent<Renderer>().material = this.active;
            other.GetComponent<TargetTrigger>().MoveBridge();
        }

        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        StartCoroutine(ExecuteAfterTime(2));

    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // TODO supprimer le projectile instant
        Destroy(this.gameObject);
    }
}
