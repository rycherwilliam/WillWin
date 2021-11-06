using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManagerWW : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefab;
    public static GameManagerWW instance;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        { 
            if (playerPrefab != null)
            {
                int randomPoint = Random.Range(2, 6);
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 3, randomPoint), Quaternion.identity);
            }            
        }
               
    }   

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #region Photon Methods

    public override void OnJoinedRoom()
    {   
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }    

    public override void OnLeftRoom()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Lobby");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

}
