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

    

	private void Awake()
	{

	}

	// Start is called before the first frame update
	void Start()
    {
        CheckServerSetting();
        nickNameTxt.text = $"반갑읍니다. \n{PhotonNetwork.NickName}님.";
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
}
