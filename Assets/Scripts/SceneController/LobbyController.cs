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

    public List<RoomInfo> roomList = new List<RoomInfo>();
    public GameObject roomSlotPrefab;
    public Transform roomListTr;
    public List<RoomSlotButton> roomSlotList;

    public int maxRoomCount;

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
        playerCountTxt.text = $"전체 접속 인원 : {PhotonNetwork.CountOfPlayers} \n로비 인원 : {lobby}";
    }

    private void UpdateLobbyNetworkState()
    {
        serverStateTxt.text = PhotonNetwork.NetworkClientState.ToString();
    }

    private void CreateRoomSlots()
    {
        roomSlotList = new List<RoomSlotButton>();
        int firstYPos = 345;
        for (int i = 0; i < maxRoomCount; ++i)
        {
            GameObject slot = Instantiate(roomSlotPrefab,roomListTr);
            Vector2 pos = new Vector2(0, firstYPos - (110 * i));
            slot.GetComponent<RectTransform>().anchoredPosition = pos;
            roomSlotList.Add(slot.GetComponent<RoomSlotButton>());
            slot.SetActive(false);
        }

    }

    public void RefreshLobby()
    {
        PhotonNetwork.JoinLobby();
    }


    public void RefreshRoomSlotUIList()
    {
        //for (int i = 0; i < roomList.Count; ++i)
        //{
        //    RoomInfo info = roomList[i];
        //    roomSlotList[i].gameObject.SetActive(true);
        //    //PhotonNetwork.id
        //    roomSlotList[i].SettingRoomTexts(info.Name, "TestMasterName", info.PlayerCount, info.MaxPlayers);
        //    //마스터 닉네임은 그놈의 photonView Component가져와서 owner.nickName으로 가져와야함.
        //}

        for (int i = 0; i < maxRoomCount; ++i)
        {
            if (i < roomList.Count)
            {
                RoomInfo info = roomList[i];
                roomSlotList[i].gameObject.SetActive(true);
                //PhotonNetwork.id
                roomSlotList[i].SettingRoomTexts(info.Name, "TestMasterName", info.PlayerCount, info.MaxPlayers);
                //마스터 닉네임은 그놈의 photonView Component가져와서 owner.nickName으로 가져와야함.
            }
            else
			{
                roomSlotList[i].gameObject.SetActive(false);
            }
        }
    }


	public void CreateRoom()
    {
        if (PhotonNetwork.CountOfRooms >= 4)
        {
            return;
            //방 더 못 만들게 하기
        }

        string roomName;
        roomName = roomNameIF.text.Equals(string.Empty) ? $"안오면 지상렬_{PhotonNetwork.CountOfRooms + 1 }" : roomNameIF.text;


        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;
        //roomOption.IsOpen = true;
        //roomOption.IsVisible = true;

        PhotonNetwork.CreateRoom(roomName, roomOption);
    }
    public void EnterRoom()
    {
        if (!selectedRoomName.Equals(string.Empty))
        {
            PhotonNetwork.JoinRoom(selectedRoomName);
        }
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
    

    public void SelectRoom(RoomSlotButton script)
	{
		if (selectedRoomName.Equals(script.roomNameTmp.text))
		{
            script.SelectRelease();
            selectedRoomName = string.Empty;
            return;
        }

		selectedRoomName = script.roomNameTmp.text;
        for (int i = 0; i < roomList.Count; ++i)
        {
            if (roomSlotList[i] != script)
            { roomSlotList[i].SelectRelease(); }
            else
            { script.Selected(); }
        }

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

        CreateRoomSlots();
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


        Debug.Log("방 입장 완료");
    }

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{


        Debug.Log($"{newPlayer.NickName}님이(가) '{PhotonNetwork.CurrentRoom.Name}'방에 접속 했습니다.");
    }



	public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        //int roomCount = _roomList.Count;

        //roomList = _roomList; 이런식으로 그냥 받는게 아니라 따로 있는 놈들은 업뎃해주고
        //없앨놈들은 지우는 식으로 update 해야하는 듯...
        for (int i = 0; i < _roomList.Count; ++i)
        {
            RoomInfo serverCurRoom = _roomList[i];

            if (!serverCurRoom.RemovedFromList)
            { //방 지우기 예약 bool값임.
                if (!roomList.Contains(serverCurRoom))
                {//리스트에 추가 되어 있지 않은 경우
                    roomList.Add(serverCurRoom);
                }
                else
                {//리스트에 이미 추가되어 있는 경우
                    //새로운걸로 업데이트
                    roomList[roomList.IndexOf(serverCurRoom)] = serverCurRoom;
                }
            }
            else if (roomList.IndexOf(_roomList[i]) != -1)
            { //지울놈은 아닌데 리스트에 없는 경우?
                roomList.RemoveAt(roomList.IndexOf(_roomList[i]));
            }
        }

        RefreshRoomSlotUIList();
        Debug.Log("룸 업뎃됨!");
    }

    //public override void OnCre
}
