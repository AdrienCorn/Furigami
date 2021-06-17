using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerController : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed, jumpForce;
    public float playerType;
    private Vector2 moveInput;

    public Transform barque;
    public LayerMask theGround;
    public LayerMask theWater;
    public LayerMask theBoat;
    public Transform groundPoint;
    private bool isDrowning;
    private bool isGrounded;
    public bool isFloating;

    public GameObject defaultSkin;
    public GameObject hatSkin;
    public GameObject slimeSkin;
    public GameObject nutSkin;

    //public Animator anim;

    //public SpriteRenderer theSR;

    public Animator flipAnim;
    private bool isFacingLeft = true;

    public static event Action<GameObject> onSteleInteraction;
    public static event Action Reset_Scene_Event;
    public static event Action Reset_Player_Event;

    private const byte SWITCH_SKIN = 20;
    private const byte SET_PLAYER_PHOTON_NAME = 21;
    private const byte RESET_SCENE = 22;

    public bool loopBreak = false;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnSkinSwitch;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_OnSetPlayerPhotonName;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnSkinSwitch;
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_OnSetPlayerPhotonName;
    }

    // Start is called before the first frame update
    void Start()
    {
        object[] datas = new object[] { this.name };
        PhotonNetwork.RaiseEvent(SET_PLAYER_PHOTON_NAME, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    public void NetworkingClient_OnSetPlayerPhotonName(EventData obj)
    {
        if (obj.Code == SET_PLAYER_PHOTON_NAME)
        {
            if (GameObject.Find("Player(Clone)"))
            {
                GameObject.Find("Player(Clone)").name = (string)obj.CustomData;
                object[] datas = new object[] { this.name };
                PhotonNetwork.RaiseEvent(SET_PLAYER_PHOTON_NAME, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
            }     
        }
        if (obj.Code == RESET_SCENE)
        {
            Reset_Scene_Function();
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();
        if (!PauseMenu.isPaused) {
        theRB.velocity = new Vector3(moveInput.x * moveSpeed, theRB.velocity.y, moveInput.y * moveSpeed);
        }
        //anim.SetFloat("moveSpeed", theRB.velocity.magnitude);

		#region check status
		RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, theGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, theWater))
        {
            isDrowning = true;
        }
        else
        {
            isDrowning = false;
        }

        //if (playerType != 2 && Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, theBoat))
        //{
        //    isFloating = true;
        //}
        //else
        //{
        //    isFloating = false;
        //}

        if (isDrowning)
		{
            if (playerType != 2)
			{
                theRB.transform.position = new Vector3(theRB.position.x - 10 , theRB.position.y + 1, theRB.position.z);
            }
            else // if has power to float
			{
                Debug.Log("eau touchée");
                //this.transform.Rotate(90, -90, this.transform.rotation.z);
                //this.transform.eulerAngles = new Vector3(90, -90, this.transform.rotation.z);
            }
        }

        //if (isFloating)
        //{
            //this.transform.parent = barque; ;
        //    Debug.Log(barque.transform.position.y);
        //    theRB.transform.position = new Vector3(barque.transform.position.x, barque.transform.position.y + 1.64f, barque.transform.position.z);            
        //    if (Input.GetButtonDown("Jump"))
        //    {
        //        theRB.velocity += new Vector3(0f, jumpForce, 0f);
        //    }
        //}
        //else
		//{
        //    //this.transform.parent = null;
		//}

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            theRB.velocity += new Vector3(0f, jumpForce, 0f);
        }
		#endregion

		#region flip G/d
		//      if (!theSR.flipX && moveInput.x < 0)
		//      {
		//          theSR.flipX = true;
		//          flipAnim.SetTrigger("Flip");
		//      } else if (theSR.flipX && moveInput.x > 0)
		//{
		//          theSR.flipX = false;

		//          flipAnim.SetTrigger("Flip");
		//      }

		if (moveInput.x != 0)
		{
            if (moveInput.x < 0 && !isFacingLeft)
            {
                // tourner pour regarder la gauche
                //Debug.Log("tourne G");
                flipAnim.SetTrigger("RtoL");
                isFacingLeft = true;
                //rotation
                //this.transform.Rotate(0, -180, 0);                
            }
            else if (moveInput.x > 0 && isFacingLeft )
            {
                //Debug.Log("tourne D");
                flipAnim.SetTrigger("LtoR");
                isFacingLeft = false;
                //this.transform.Rotate(0, 180, 0);                
            }
            //flipAnim.SetTrigger("Flip");
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.E))
        {
            onSteleInteraction?.Invoke(this.gameObject);
        }
    }

    public void setDefaultSkin()
    {
        loopBreak = true;
        defaultSkin.SetActive(true);
        hatSkin.SetActive(false);
        slimeSkin.SetActive(false);
        nutSkin.SetActive(false);
        string encoded_data = this.name + "|" + "Default";
        object[] datas = new object[] { encoded_data };
        PhotonNetwork.RaiseEvent(SWITCH_SKIN, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    public void setHatSkin()
    {
        loopBreak = true;
        defaultSkin.SetActive(false);
        hatSkin.SetActive(true);
        slimeSkin.SetActive(false);
        nutSkin.SetActive(false);
        string encoded_data = this.name + "|" + "Hat";
        object[] datas = new object[] { encoded_data };
        PhotonNetwork.RaiseEvent(SWITCH_SKIN, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    public void setSlimeSkin()
    {
        loopBreak = true;
        defaultSkin.SetActive(false);
        hatSkin.SetActive(false);
        slimeSkin.SetActive(true);
        nutSkin.SetActive(false);
        string encoded_data = this.name + "|" + "Slime";
        object[] datas = new object[] { encoded_data };
        PhotonNetwork.RaiseEvent(SWITCH_SKIN, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    public void setNutSkin()
    {
        loopBreak = true;
        defaultSkin.SetActive(false);
        hatSkin.SetActive(false);
        slimeSkin.SetActive(false);
        nutSkin.SetActive(true);
        string encoded_data = this.name + "|" + "Nut";
        object[] datas = new object[] { encoded_data };
        PhotonNetwork.RaiseEvent(SWITCH_SKIN, datas[0], RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    private void NetworkingClient_OnSkinSwitch(EventData obj)
    {
        if (obj.Code == SWITCH_SKIN)
        {
            string data = (string)obj.CustomData;
            string[] subs = data.Split('|');
            string name = subs[0];
            string skin = subs[1];
            Debug.Log("coucou " + name);
            if (loopBreak) { }
            else
            {
                if (skin == "Default")
                    GameObject.Find(name).GetComponent<PlayerController>().setDefaultSkin();
                if (skin == "Hat")
                    GameObject.Find(name).GetComponent<PlayerController>().setHatSkin();
                if (skin == "Slime")
                    GameObject.Find(name).GetComponent<PlayerController>().setSlimeSkin();
                if (skin == "Nut")
                    GameObject.Find(name).GetComponent<PlayerController>().setNutSkin();
            }
            loopBreak = false;
        }
    }

    public void Reset_Scene_Function()
    {
        Reset_Scene_Event?.Invoke();
        this.GetComponent<Tir>().enabled = false;
        this.GetComponent<PlateformPower>().enabled = false;
        this.GetComponent<PlayerController>().playerType = 0;
        setDefaultSkin();
        this.transform.position = new Vector3(0.0f, 4.0f, 0.0f);
    }

}
