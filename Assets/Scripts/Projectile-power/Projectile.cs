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

    private const byte THROW_PROJECTILE = 1; // byte to be sent by photon
    // TODO make an enum with all the bytes, to ensure unicity of it

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
    
    private void OnProjectilShoot(GameObject Player, Vector3 playerForward)
    {
        //Debug.Log("local event");
        //Debug.Log(Player.transform.GetChild(0).rotation.eulerAngles.y);
        Vector3 direction;
        if (Player.transform.GetChild(0).rotation.eulerAngles.y > 90)
        {
            Debug.Log("droite");
            direction = new Vector3(1, 0, 0);
        }
        else
        {
            Debug.Log("gauche");
            direction = new Vector3(-1, 0, 0);
        }
        this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(direction * velocity);
        //Debug.Log(new Vector3(-Player.transform.GetChild(0).rotation.eulerAngles.y, 0, 0));
        object[] datas = new object[] { direction };
        PhotonNetwork.RaiseEvent(THROW_PROJECTILE, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    private void NetworkingClient_OnProjectileShhot(EventData obj)
    {
        if (obj.Code == THROW_PROJECTILE)
        {
            Debug.Log("succes");
            this.GetComponent<Rigidbody>().velocity = transform.TransformDirection((Vector3)obj.CustomData * velocity); //to follow player rotation
        }
        else
        {
            Debug.Log(obj.Code.ToString());
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
