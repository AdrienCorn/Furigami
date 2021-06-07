using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PseudoManager : MonoBehaviour
{
    public void PseudoChanged()
    {
        PhotonNetwork.LocalPlayer.NickName = GetComponent<InputField>().text;
    }
}
