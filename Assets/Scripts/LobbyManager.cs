using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using DG.Tweening;

public class LobbyManager : MonoBehaviourPunCallbacks //포톤에서 모노비헤이비어 + 서버 하게 해주는 클라스
{

	//실행하면 자동으로 마스터서버 연결 시도
	//시도 완료되면
	//OnConnectedToMaster 실행
	public TextMeshProUGUI serverStateTxt;
    public Button joinBtn;

	public TMP_InputField nickNameInput;
	public string nickName;

	public void EnterNickName()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (!nickNameInput.text.Equals(string.Empty))
			{
				nickName = nickNameInput.text;
				PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
				Debug.Log($"nickName : {nickName}");
			}
		}
	}

	void Start()
    {
		PhotonNetwork.GameVersion = "0.1";
		PhotonNetwork.ConnectUsingSettings();

		joinBtn.interactable = false;
		serverStateTxt.text = "Connecting to MasterServer...";

		Debug.Log("마스터 서버 접속 시도중");
    }

    void Update()
    {
		EnterNickName();
	}

	public override void OnConnectedToMaster()
	{//마스터 서버 접속 성공시 실행됨
		//base.OnConnectedToMaster();

		joinBtn.interactable = true;
		serverStateTxt.text = "Success Connect to MasterServer!";

		Debug.Log("마스터 서버 접속 성공");
	}

	public override void OnDisconnected(DisconnectCause cause)
	{//마스터 서버 접속 실패시 실행
		//base.OnDisconnected(cause);
		
		joinBtn.interactable = false;

		serverStateTxt.text = "Failed connect to masterServer\nReconnecting to masterServer...";
		PhotonNetwork.ConnectUsingSettings();

		Debug.Log("마스터 서버 접속 실패, 재시도");
	}

	public void TryConnect()
	{//접속 시도(버튼눌렀을때)

		if (nickName.Equals(string.Empty))
		{
			serverStateTxt.text = "Enter ur nickName";
			return;
		}


		joinBtn.interactable = false;

		RoomOptions options = new RoomOptions();
		options.IsVisible = true;
		options.IsOpen = true;
		options.MaxPlayers = 4;
		PhotonNetwork.JoinOrCreateRoom("Room1", options, TypedLobby.Default);

		//if (PhotonNetwork.IsConnected)
		//{//마스터 서버에 연결되고 나서 room에 연결 할 때
		//	serverStateTxt.text = "join to room...";
		//	PhotonNetwork.JoinRandomRoom();
		//}
		//else
		//{
		//	serverStateTxt.text = "Failed connect to masterServer\nReconnecting to masterServer...";
		//	PhotonNetwork.ConnectUsingSettings();
		//}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{//랜덤 룸 참가 실패시 실행
		//base.OnJoinRandomFailed(returnCode, message);

		serverStateTxt.text = "is not exist empty room, \ncreate new room!";
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });

		Debug.Log("룸 참가 실패, 새로운 방 생성");
	}

	public override void OnJoinedRoom()
	{//랜덤 룸 참가 완료시 실행
		//base.OnJoinedLobby();
		Debug.Log("룸 참가 완료");

		serverStateTxt.text = "Joined Room!";




		PhotonNetwork.LoadLevel("03_InGame");

		//GameManager.Instance.LoadScene(3);
	}


	
}
