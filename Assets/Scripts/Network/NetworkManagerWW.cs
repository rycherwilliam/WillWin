using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class NetworkManagerWW : MonoBehaviourPunCallbacks
{
    [Header("UI Panels")]
    public GameObject[] panels;

    [Header("Connection Status")]
    public Text connectionStatusText;


    [Header("Login UI Panel")]
    public InputField playerNameInput;
    public GameObject loginPanel;

    [Header("Loading UI Panel")]
    public GameObject loadingPanel;

    [Header("Create or Join Room UI Panel")]
    public GameObject joinCreateRoomPanel;    
    public InputField createRoomInput;    
    public InputField joinRoomInput;    
    public InputField maxPlayersInput;

    [Header("Room List UI Panel")]
    public GameObject showRoomListPanel;
    public GameObject roomListEntryPrefab;
    public GameObject roomListParentGameObject;

    [Header("Inside Room UI Panel")]
    public GameObject insideRoomPanel;
    public GameObject insideRoomPanelStartButton;
    public TextMeshProUGUI insideRoomPanelText;
    public GameObject playerListPrefab;
    public GameObject playerListContent;

    [Header("Menu UI Panel")]
    public GameObject menuPanel;

    [Header("Game Options UI Panel")]

    public Dictionary<int, GameObject> playerListGameObjects;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Photon.Realtime.Player[] pList;

    #region Unity Method



    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    private void Start()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();

        //pList = PhotonNetwork.PlayerList;
        //foreach (Photon.Realtime.Player player in pList)
        //{
        //    Debug.Log(player.NickName);
        //}
    }

    private void Update()
    {
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;
    }

    #endregion

    #region UI Callbacks
    public void OnLoginBUttonClicked()
    {
        //Get the PlayerName Value from Input Text
        string playerName = playerNameInput.text;


        //Verify playerName Empty or Null
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;

            //SetActive True to LoadingPanel          
            ActivatePanel(loadingPanel.name);          

            //Connect On Photon Network as Configured in the PhotonServerSetting file
            PhotonNetwork.ConnectUsingSettings();
        }
        else
            Debug.Log("Playername is Invalid!");


    }

    public void OnPlayBUttonClicked()
    {
        PhotonNetwork.JoinLobby();
        ActivatePanel(joinCreateRoomPanel.name);     
    }

    public void OnStartGameBUttonClicked()
    {
        PhotonNetwork.LoadLevel("Map_v1");        
    }

    public void OnExitBUttonClicked()
    {
        PhotonNetwork.Disconnect();
        ActivatePanel(loginPanel.name);      
    }

    public void OnBackBUttonClicked()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }  
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(menuPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = createRoomInput.text;
        
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1000, 10000);
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(maxPlayersInput.text);
        PhotonNetwork.CreateRoom(roomName, roomOptions);        
    }

    public void OnJoinRoomButtonClicked()
    {        
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public void OnShowRoomListButtonClicked()
    {   
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        ActivatePanel(showRoomListPanel.name);
    }

    #endregion


    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {        
        ActivatePanel(menuPanel.name);        
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "is connected to Photon");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "is disconnected to Photon");
    }

    public override void OnJoinedRoom()
    {
        ActivatePanel(insideRoomPanel.name);
        insideRoomPanelText.text = PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            insideRoomPanelStartButton.SetActive(true);
        }
        else
        {
            insideRoomPanelStartButton.SetActive(false);
        }


        if (playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }
            


        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListGameObject = Instantiate(playerListPrefab);
            playerListGameObject.transform.SetParent(playerListContent.transform, false);
            playerListGameObject.transform.localScale = Vector3.one;            

            playerListGameObject.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName;

            if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            }
            else
            {
                playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }            

            playerListGameObjects.Add(player.ActorNumber, playerListGameObject);            
        }

    }

    public override void OnCreatedRoom()
    {
        ActivatePanel(insideRoomPanel.name);        
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }

    public override void OnLeftRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " left the room: ");
        
        foreach(GameObject playerListGameObject in playerListGameObjects.Values)
        {
            Destroy(playerListGameObject);
        }

        playerListGameObjects.Clear();
        playerListGameObjects = null;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();        

        foreach(RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList) 
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList.Remove(room.Name);
                }
            }
            else
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList[room.Name] = room;
                }
                else
                {
                    cachedRoomList.Add(room.Name, room);
                }               
            }
           
        }

        foreach (RoomInfo room in cachedRoomList.Values)
        {
            GameObject roomListEntryGameObject = Instantiate(roomListEntryPrefab, roomListParentGameObject.transform);
            roomListEntryGameObject.transform.SetParent(roomListParentGameObject.transform);
            roomListEntryGameObject.transform.localScale = Vector3.one;

            roomListEntryGameObject.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;
            roomListEntryGameObject.transform.Find("RoomPlayersText").GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;
            roomListEntryGameObject.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomByListButtonClicked(room.Name));

            roomListGameObjects.Add(room.Name, roomListEntryGameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerListGameObject = Instantiate(playerListPrefab);        
        playerListGameObject.transform.SetParent(playerListContent.transform, false);        
        playerListGameObject.transform.localScale = Vector3.one;        
        playerListGameObject.transform.Find("PlayerNameText").GetComponent<Text>().text = newPlayer.NickName;

        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }        

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);        
        insideRoomPanelText.text = PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        insideRoomPanelText.text = PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            insideRoomPanelStartButton.SetActive(true);
        }
    }
    
    #endregion

    #region Private Methods
    private void OnJoinRoomByListButtonClicked(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(_roomName);
    }

    private void ClearRoomListView()
    {
        foreach (var roomListGameObject in roomListGameObjects.Values)
        {
            Destroy(roomListGameObject);
        }

        roomListGameObjects.Clear();
    }

    #endregion


    #region Public Methods
    public void ActivatePanel(string panelToBeActivated)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if (panelToBeActivated.Equals(panels[i].name))
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }        
    }
    #endregion

}
