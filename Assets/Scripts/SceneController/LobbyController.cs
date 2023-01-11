using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using DG.Tweening;

public class LobbyController : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI nickNameTxt;
    public TextMeshProUGUI serverStateTxt;
    public TextMeshProUGUI playerCountTxt;

    public TMP_InputField roomNameIF;

    public RoomWindow roomWindow;

    public string selectedRoomName = string.Empty;

    private void CheckServerSetting()
    {
        if (PhotonNetwork.NickName.Equals(string.Empty))
        {
            GameManager.Instance.NickName = $"Test{PhotonNetwork.CountOfPlayers + 1}";
            PhotonNetwork.GameVersion = Application.version;

            PhotonNetwork.ConnectUsingSettings();
        }
    } 

    private void UpdatePlayerCount()
    {
        int all = PhotonNetwork.CountOfPlayers;
        int lobby = all - PhotonNetwork.CountOfPlayersInRooms;
        playerCountTxt.text = $"전체 접속 인원 : {PhotonNetwork.CountOfPlayers} /n 로비 인원 : {lobby}";
    }

    private void UpdateLobbyNetworkState()
    {
        serverStateTxt.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void RefreshRoomList()
    {
        //for(int i =0; i<PhotonNetwork.room)

    }

    public void CreateRoom()
    {
        if (PhotonNetwork.CountOfRooms >= 4)
        { 
            //방 더 못 만들게 하기
        }

        string roomName;
        roomName = roomNameIF.text.Equals(string.Empty) ? $"안오면 지상렬_{PhotonNetwork.CountOfRooms + 1 }" : roomNameIF.text;


        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;
        roomOption.IsOpen = true;
        roomOption.IsVisible = true;

        PhotonNetwork.CreateRoom(roomName, roomOption);
    }
    public void EnterRoom()
    {
        //PhotonNetwork.JoinRoom

    }

    public void EnterRandomRoom()
    {
        if (PhotonNetwork.CountOfRooms == 0)
        {
            CreateRoom();    
        }
        else
        { 
            PhotonNetwork.JoinRandomRoom();
        }   

    }



 

    public void SelectRoom()
    { 
        
    
    }

	private void Awake()
	{

	}

	// Start is called before the first frame update
	void Start()
    {
        CheckServerSetting();
        nickNameTxt.text = $"반갑읍니다. \n{PhotonNetwork.NickName}님.";
        roomWindow.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLobbyNetworkState();
        UpdatePlayerCount();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("마스터 서버 접속 성공");
    }
	public override void OnJoinedLobby()
	{
        Debug.Log("로비 접속 성공");
    }

	public override void OnJoinedRoom()
	{
        roomWindow.gameObject.SetActive(true);
     
    }



	//public override void OnCre
}
