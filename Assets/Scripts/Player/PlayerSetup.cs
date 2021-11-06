using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject fpsCamera;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    private void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            fpsCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            fpsCamera.GetComponent<Camera>().enabled = false;
        }

        SetPlayerUI();
    }


    private void SetPlayerUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = photonView.Owner.NickName;
        }
        
    }
}
