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
        playerCountTxt.text = $"��ü ���� �ο� : {PhotonNetwork.CountOfPlayers} \n�κ� �ο� : {lobby}";
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
        //    //������ �г����� �׳��� photonView Component�����ͼ� owner.nickName���� �����;���.
        //}

        for (int i = 0; i < maxRoomCount; ++i)
        {
            if (i < roomList.Count)
            {
                RoomInfo info = roomList[i];
                roomSlotList[i].gameObject.SetActive(true);
                //PhotonNetwork.id
                roomSlotList[i].SettingRoomTexts(info.Name, "TestMasterName", info.PlayerCount, info.MaxPlayers);
                //������ �г����� �׳��� photonView Component�����ͼ� owner.nickName���� �����;���.
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
            //�� �� �� ����� �ϱ�
        }

        string roomName;
        roomName = roomNameIF.text.Equals(string.Empty) ? $"�ȿ��� �����_{PhotonNetwork.CountOfRooms + 1 }" : roomNameIF.text;


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
        nickNameTxt.text = $"�ݰ����ϴ�. \n{PhotonNetwork.NickName}��.";
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
        Debug.Log("������ ���� ���� ����");
    }
	public override void OnJoinedLobby()
	{
        Debug.Log("�κ� ���� ����");
    }

	public override void OnJoinedRoom()
	{ 
        roomWindow.gameObject.SetActive(true);


        Debug.Log("�� ���� �Ϸ�");
    }

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{


        Debug.Log($"{newPlayer.NickName}����(��) '{PhotonNetwork.CurrentRoom.Name}'�濡 ���� �߽��ϴ�.");
    }



	public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        //int roomCount = _roomList.Count;

        //roomList = _roomList; �̷������� �׳� �޴°� �ƴ϶� ���� �ִ� ����� �������ְ�
        //���ٳ���� ����� ������ update �ؾ��ϴ� ��...
        for (int i = 0; i < _roomList.Count; ++i)
        {
            RoomInfo serverCurRoom = _roomList[i];

            if (!serverCurRoom.RemovedFromList)
            { //�� ����� ���� bool����.
                if (!roomList.Contains(serverCurRoom))
                {//����Ʈ�� �߰� �Ǿ� ���� ���� ���
                    roomList.Add(serverCurRoom);
                }
                else
                {//����Ʈ�� �̹� �߰��Ǿ� �ִ� ���
                    //���ο�ɷ� ������Ʈ
                    roomList[roomList.IndexOf(serverCurRoom)] = serverCurRoom;
                }
            }
            else if (roomList.IndexOf(_roomList[i]) != -1)
            { //������� �ƴѵ� ����Ʈ�� ���� ���?
                roomList.RemoveAt(roomList.IndexOf(_roomList[i]));
            }
        }

        RefreshRoomSlotUIList();
        Debug.Log("�� ������!");
    }

    //public override void OnCre
}
