using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //public Transform player;
    public Material active;
    public Material inactive;

    private const byte THROW_PROJECTILE = 0; // byte to be sent by photon
    // TODO make an enum with all the bytes, to ensure unicity of it

    public int velocity = 10;

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
    
    private void OnProjectilShoot(Transform PlayerTransform, Vector3 playerForward)
    {
        object[] datas = new object[] { playerForward };
        PhotonNetwork.RaiseEvent(THROW_PROJECTILE, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    private void NetworkingClient_OnProjectileShhot(EventData obj)
    {
        Debug.Log("here");
        if (obj.Code == THROW_PROJECTILE)
        {
            
            this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(this.transform.position * velocity); //to follow player rotation
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
