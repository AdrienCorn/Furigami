using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rising_sustain_plat : MonoBehaviour
{
    public float RisingVelocity; // The rising speed of the platform
    private Vector3 Ray; // The vector between the plateform pointer and the player
    private float HalfHeight; // The half of the size of the plateform
    private float MiddleHeight; // The middle of rising state height in world space
    private float startHight;

    private const byte PLATEFORM_MOVE = 0;

    private void OnEnable()
    {
        PlateformPower.onPlateformPower += OnPlateformePower;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnPlateformePower;
    }

    private void OnDisable()
    {
        PlateformPower.onPlateformPower -= OnPlateformePower;
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnPlateformePower;
    }

    // Start is called before the first frame update
    void Start()
    {
        HalfHeight = transform.GetChild(0).transform.lossyScale.y / 2;
        MiddleHeight = transform.position.y + HalfHeight;
        startHight = this.transform.position.y;
    }

    private void Update()
    {
        if(transform.position.y > startHight && !Input.GetKey("b"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.04f, transform.position.z);
        }
    }

    private void OnPlateformePower(Transform PlayerTransform)
    {
        Ray.x = PlayerTransform.position.x - transform.position.x;
        Ray.y = PlayerTransform.position.y - transform.position.y;
        Ray.z = PlayerTransform.position.z - transform.position.z;
        if (Ray.magnitude <= 5 && Input.GetKeyDown("b"))
        {
            StartCoroutine("Rising");
            object[] datas = new object[] { this.name };
            PhotonNetwork.RaiseEvent(PLATEFORM_MOVE, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
        } 
    }

    private void NetworkingClient_OnPlateformePower(EventData obj)
    {
        if(obj.Code == PLATEFORM_MOVE && (string)obj.CustomData == this.name)
        {
            StartCoroutine("Rising");
        }
    }

    // Coroutine to rise or collapse plateform
    IEnumerator Rising()
    {
        //CurrentHeight = transform.position.y;
        while (transform.position.y < (MiddleHeight + HalfHeight))
        {
            transform.Translate(0.0f, RisingVelocity, 0.0f);
            yield return null;
        }
    }
}
