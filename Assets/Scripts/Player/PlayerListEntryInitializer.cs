using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListEntryInitializer : MonoBehaviour
{
    [Header("UI References")]
    public Text PlayerNameText;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;
    private bool isPlayerReady = false;

    // Update is called once per frame
    public void Initialize(int playerID, string playerName)
    {

        PlayerNameText.text = playerName;

        if (PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { PlayerType.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);


                ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable() { { PlayerType.PLAYER_READY, isPlayerReady } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
            });
        }


    }


    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.gameObject.SetActive(playerReady);

        if (playerReady)        
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready!";        
        else       
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready?";
        
    }

}
