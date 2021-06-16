using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rising_plat : MonoBehaviour
{
    public float RisingVelocity; // The rising speed of the platform
    private Vector3 Ray; // The vector between the plateform pointer and the player
    private float RiseState = 1.0f; // The state to give to the platform (rising or collapsing)
    private float HalfHeight; // The half of the size of the plateform
    private float MiddleHeight; // The middle of rising state height in world space

    private const byte PLATEFORM_MOVE = 0;

    public Text text;

    private void OnEnable()
    {
        PlateformPower.onPlateformPowerUp += OnPlateformePowerUp;
        PlayerController.Reset_Scene_Event += Reset_Scene;
        //PlateformPower.onPlateformPowerDown += OnPlateformePowerDown;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnPlateformePower;

        //PlateformPower.onPlateformPower += OnPlateformePower;
        //PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnPlateformePower;
    }

    private void OnDisable()
    {
        PlateformPower.onPlateformPowerUp -= OnPlateformePowerUp;
        PlayerController.Reset_Scene_Event -= Reset_Scene;
        //PlateformPower.onPlateformPowerDown -= OnPlateformePowerDown;
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnPlateformePower;

        //PlateformPower.onPlateformPower -= OnPlateformePower;
        //PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnPlateformePower;
    }

    // Start is called before the first frame update
    void Start()
    {
        HalfHeight = transform.GetChild(0).transform.lossyScale.y / 2;
        MiddleHeight = transform.position.y + HalfHeight;
    }

    private void OnPlateformePowerUp(Transform PlayerTransform)
    {
        Ray.x = PlayerTransform.position.x - transform.position.x;
        Ray.y = 0;
        Ray.z = PlayerTransform.position.z - transform.position.z;
        if (Ray.magnitude <= 7 && Input.GetKeyDown("a"))
        {
            
            RiseState = -RiseState;
            StartCoroutine("Rising");
            object[] datas = new object[] { this.name };
            PhotonNetwork.RaiseEvent(eventCode: PLATEFORM_MOVE, eventContent: datas, raiseEventOptions: RaiseEventOptions.Default, sendOptions: SendOptions.SendUnreliable);

            PhotonNetwork.RaiseEvent(PLATEFORM_MOVE, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);

        }
    }

    private void NetworkingClient_OnPlateformePower(EventData obj)
    {
        if(obj.Code == PLATEFORM_MOVE && (string)obj.CustomData == this.name)
        {
            //RiseState = (float)obj.CustomData;
            RiseState = -RiseState;
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
