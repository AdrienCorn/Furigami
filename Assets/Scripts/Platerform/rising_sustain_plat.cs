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
    private float RiseState = 1.0f; // The state to give to the platform (rising or collapsing)

    private const byte PLATEFORM_MOVE = 0;

    private void OnEnable()
    {
        PlateformPower.onPlateformPowerUp += OnPlateformePowerUp;
        PlateformPower.onPlateformPowerDown += OnPlateformePowerDown;
        PlayerController.Reset_Scene_Event += Reset_Scene;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnPlateformePower;
    }

    private void OnDisable()
    {
        PlateformPower.onPlateformPowerUp -= OnPlateformePowerUp;
        PlateformPower.onPlateformPowerDown -= OnPlateformePowerDown;
        PlayerController.Reset_Scene_Event -= Reset_Scene;
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
        /*if(transform.position.y > startHight && !Input.GetKey("b"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.04f, transform.position.z);
        }*/
    }

    private void OnPlateformePowerUp(Transform PlayerTransform)
    {
        Ray.x = PlayerTransform.position.x - transform.position.x;
        Ray.y = 0;
        Ray.z = PlayerTransform.position.z - transform.position.z;
        if (Ray.magnitude <= 7 /*&& Input.GetKeyDown("b")*/)
        {
            RiseState = 1.0f;
            StartCoroutine("Rising");
            string encoded_data = this.name + "|" + "Rise";
            object[] content = new object[] {encoded_data};
            PhotonNetwork.RaiseEvent(PLATEFORM_MOVE, content[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);

        }
    }

    private void OnPlateformePowerDown(Transform PlayerTransform)
    {
        Debug.Log("plateform down");
        Ray.x = PlayerTransform.position.x - transform.position.x;
        Ray.y = 0;
        Ray.z = PlayerTransform.position.z - transform.position.z;
        if (Ray.magnitude <= 7 /* && Input.GetKeyDown("b")*/)
        {
            RiseState = -1.0f;
            StartCoroutine("Rising");
            string encoded_data = this.name + "|" + "Collapse";
            object[] content = new object[] { encoded_data };
            PhotonNetwork.RaiseEvent(PLATEFORM_MOVE, content[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);

        }
    }

    private void NetworkingClient_OnPlateformePower(EventData obj)
    {
        if (obj.Code == PLATEFORM_MOVE && ((string)obj.CustomData == this.name+"|Rise" || (string)obj.CustomData == this.name + "|Collapse"))
        {
            Debug.Log(obj.CustomData);
            string data = (string)obj.CustomData;
            string[] subs = data.Split('|');
            string obj_name = subs[0];
            string obj_rise_state = subs[1];

            if (obj_rise_state == "Rise") { RiseState = 1.0f; }
            else { RiseState = -1.0f; }

            StartCoroutine("Rising");
        }
    }

    // Coroutine to rise or collapse plateform
    IEnumerator Rising()
    {
        //CurrentHeight = transform.position.y;
        while (RiseState * transform.position.y < RiseState * (MiddleHeight + RiseState * HalfHeight))
        {
            transform.Translate(0.0f, RiseState * RisingVelocity, 0.0f);
            yield return null;
        }
    }

    private void Reset_Scene()
    {
        RiseState = -1.0f;
        StartCoroutine("Rising");
    }
}
