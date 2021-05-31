using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PseudoManager : MonoBehaviour
{
    public void PseudoChanged()
    {
        PhotonNetwork.LocalPlayer.NickName = GetComponent<TMP_InputField>().text;
    }
}
