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
        playerCountTxt.text = $"��ü ���� �ο� : {PhotonNetwork.CountOfPlayers} /n �κ� �ο� : {lobby}";
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
            //�� �� �� ����� �ϱ�
        }

        string roomName;
        roomName = roomNameIF.text.Equals(string.Empty) ? $"�ȿ��� �����_{PhotonNetwork.CountOfRooms + 1 }" : roomNameIF.text;


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
        nickNameTxt.text = $"�ݰ����ϴ�. \n{PhotonNetwork.NickName}��.";
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
        Debug.Log("������ ���� ���� ����");
    }
	public override void OnJoinedLobby()
	{
        Debug.Log("�κ� ���� ����");
    }

	public override void OnJoinedRoom()
	{
        roomWindow.gameObject.SetActive(true);
     
    }



	//public override void OnCre
}
