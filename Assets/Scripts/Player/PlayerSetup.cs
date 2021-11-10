using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;
public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject thirdPersonCamera;

    [SerializeField]
    GameObject cameraTarget;

    [SerializeField]
    CinemachineFreeLook freeLookCamera;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    private void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            thirdPersonCamera.GetComponent<Camera>().enabled = true;
            transform.GetComponent<CharacterController>().enabled = true;
            freeLookCamera.gameObject.SetActive(true);
            freeLookCamera.Follow = cameraTarget.transform;
            freeLookCamera.LookAt = cameraTarget.transform;
            cameraTarget.SetActive(true);           
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            thirdPersonCamera.GetComponent<Camera>().enabled = false;
            transform.GetComponent<CharacterController>().enabled = false;
            cameraTarget.SetActive(false);
            freeLookCamera.gameObject.SetActive(false);
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
