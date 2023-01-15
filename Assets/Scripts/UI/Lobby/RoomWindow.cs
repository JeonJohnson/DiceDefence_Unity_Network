using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using DG.Tweening;

public class RoomWindow : MonoBehaviourPunCallbacks
{
    public PhotonView phView;

    [SerializeField]
    TextMeshProUGUI roomNameTmp;


    public GameObject PlayerSlotPrefab;
    public RectTransform playerListTr;
    public List<RoomPlayerSlot> playerSlotList = new List<RoomPlayerSlot>();
    public RoomPlayerSlot mySlot;

    [SerializeField]
    TMP_InputField chatInputField;
    [SerializeField]
    Scrollbar chatScrollBar;
    [SerializeField]
    TextMeshProUGUI chatLogTxt;
    [SerializeField]
    Button chatSendBtn;

    [SerializeField]
    Button startBtn;


    //List<string> chatMsgList;

    public void ChatSend()
    {
        if (chatInputField.text.Equals(string.Empty))
        {
            return;
        }

        string msg = $"[{PhotonNetwork.LocalPlayer.NickName}] : {chatInputField.text}";


        phView.RPC("ChatRPC", RpcTarget.All, msg);
        chatInputField.ActivateInputField();
        chatInputField.text = string.Empty;
    }

    public void ChatNotice(string _msg)
    {
        string msg = "**Notice** " + _msg;
        phView.RPC("ChatRPC", RpcTarget.All, msg);
        chatInputField.ActivateInputField();
        chatInputField.text = string.Empty;
    }

    [PunRPC]
    public void ChatRPC(string msg)
    {
        chatLogTxt.text += "\n" + msg;
        //chatScrollBar.value = 0f;
        StartCoroutine(ScrollBarValueCoroutine());
    }

    public IEnumerator ScrollBarValueCoroutine()
    {
        yield return null;
        yield return null;
        chatScrollBar.value = 0f;
    }

    public void ExitLobby()
    {
        this.gameObject.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void GetReady()
    {
        mySlot.Ready(!mySlot.isReady);
    }


    public void GetStart()
    {
        int playerCount = PhotonNetwork.CountOfPlayersInRooms;

        if (playerCount == 1)
        {
            PhotonNetwork.LoadLevel("03_InGame");
        }
        else
        {

            Photon.Realtime.Player[] playerTempList = PhotonNetwork.PlayerList;

            int readyCount = 0;
            for (int i = 0; i < playerCount; ++i)
            {
                readyCount += (playerSlotList[i].isReady ? 1 : 0);
            }

            if (readyCount == playerCount - 1)
            {
                PhotonNetwork.LoadLevel("03_InGame");
            }
        }
    }

    private void SettingButton(bool isMaster)
    {
        TextMeshProUGUI text = startBtn.GetComponentInChildren<TextMeshProUGUI>();
        startBtn.onClick.RemoveAllListeners();

        if (isMaster)
        {
            text.text = "Start";
            startBtn.onClick.AddListener(GetStart);
        }
        else
        {
            text.text = "Ready";
            startBtn.onClick.AddListener(GetReady);
        }
    }

    private void SettingPlayerSlots()
    {
        if (playerSlotList.Count != 0)
        {
            return;
        }

        Vector2 pos = new Vector2(0f, -12f);
        for (int i = 0; i < 4; ++i)
        {
            //GameObject newSlot = Instantiate(PlayerSlotPrefab, playerListTr);
            GameObject newSlot = PhotonNetwork.Instantiate("PlayerSlot", Vector3.zero, Quaternion.identity);
            newSlot.transform.parent = playerListTr;
            pos.x = -384f + (i*256f);
            newSlot.GetComponent<RectTransform>().anchoredPosition = pos;
            playerSlotList.Add(newSlot.GetComponent<RoomPlayerSlot>());
        }
    }

    public void UpdatePlayerList()
    {
        //이것도 그냥 RPC 보내고 해야할듯...?
        //하나씩 들어올때마다 새로 만들고 나가면 지우기,,,???

        if (playerSlotList.Count == 0)
        {
            SettingPlayerSlots();
        }


        Photon.Realtime.Player[] playerTempList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerSlotList.Count; ++i)
        {
            if (i >= playerTempList.Length)
            {
                playerSlotList[i].UpdateSlotInfo();
                continue;
            }

            bool isMaster = i == 0 ? true : false;
            string nickName = playerTempList[i].NickName;

            if (nickName == PhotonNetwork.NickName)
			{
				mySlot = playerSlotList[i];
                ChatNotice($"{mySlot.GetHashCode()}가 my Slot");
			}
			playerSlotList[i].UpdateSlotInfo(isMaster, nickName);

            SettingButton(isMaster);
        }

    }

	void Awake()
	{
        phView = GetComponent<PhotonView>();
        Debug.Log(phView.ViewID);
        
        chatLogTxt.text = string.Empty;
        PhotonNetwork.IsMessageQueueRunning = true;
    }

	// Start is called before the first frame update
	void Start()
    {
        SettingPlayerSlots();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChatSend();
        }
    }

	public override void OnJoinedRoom()
	{
        //UpdatePlayerList();
    }

	public override void OnLeftRoom()
	{
        mySlot = null;
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        ChatNotice($"{PhotonNetwork.NickName}님이(가) 입장하셨스빈다.");
        //chatLogTxt.text += $"\n {PhotonNetwork.NickName}님이(가) 입장하셨스빈다.";

        UpdatePlayerList();
    }

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
        ChatNotice($"{PhotonNetwork.NickName}님이(가) 퇴장하셨스빈다.");
        //chatLogTxt.text += $"\n {PhotonNetwork.NickName}님이(가) 퇴장하셨스빈다.";

        UpdatePlayerList();
    }
}
