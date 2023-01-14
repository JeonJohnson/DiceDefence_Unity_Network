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

    [SerializeField]
    List<GameObject> playerList;//이거 나중에 각각 PlayerList 혹은 Slot으로 코드만들어서 넣어주기

    [SerializeField]
    TMP_InputField chatInputField;
    [SerializeField]
    Scrollbar chatScrollBar;
    [SerializeField]
    TextMeshProUGUI chatLogTxt;
    [SerializeField]
    Button chatSendBtn;
    
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

    public void ChatNotice(string msg)
    {
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
        
    }

    public void UpdatePlayerList()
    { 
        
    
    }

	void Awake()
	{
        phView = GetComponent<PhotonView>();
	}

	// Start is called before the first frame update
	void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        chatLogTxt.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChatSend();
        }
    }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        ChatNotice($"{PhotonNetwork.NickName}님이(가) 입장하셨스빈다.");
    //chatLogTxt.text += $"\n {PhotonNetwork.NickName}님이(가) 입장하셨스빈다.";
    }

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
        ChatNotice($"{PhotonNetwork.NickName}님이(가) 퇴장하셨스빈다.");
        //chatLogTxt.text += $"\n {PhotonNetwork.NickName}님이(가) 퇴장하셨스빈다.";
    }
}
