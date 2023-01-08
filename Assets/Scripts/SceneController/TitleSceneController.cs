using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

using Photon.Pun;
using Photon.Realtime;

public class TitleSceneController : MonoBehaviourPunCallbacks
{
    public Image fade;
    public GameObject diceRandomMoveCanvasPrefab;

    public TextMeshProUGUI serverStateTxt;
    public Button loginBtn;
    public GraphicRaycaster titleUIRaycaster;
	
    public CanvasGroup loginWindowCG;
    //public TMP_InputField nickNameIF;
    //public Button enterBtn;
    //public TextMeshProUGUI nickNameCheckTxt;


    public bool isReady;

	 void Awake()
	{
        fade.color = Funcs.SetAlpha(fade.color, 1f);
        isReady = false;
        loginWindowCG.gameObject.SetActive(false);
    }
	// Start is called before the first frame update
	void Start()
    {
        fade.DOFade(0f, 1f);

        int irandCount = Random.Range(1, 4);
        for (int i = 0; i < irandCount; ++i)
        { Instantiate(diceRandomMoveCanvasPrefab); }

        serverStateTxt.text = "Press Any Key!";
    }

	private void Update()
	{
        if (!isReady && Input.anyKeyDown)
        {
            isReady = true;
            TryConnectMasterServer();
        }
        else if (isReady)
        {
            serverStateTxt.text = PhotonNetwork.NetworkClientState.ToString();
        }
        
	}


	private void GoLobbyScene()
    {
        PhotonNetwork.LoadLevel((int)eSceneIndex.LOBBY);
        //GameManager.Instance.LoadScene(eSceneIndex.LOBBY);
    }

    public void NextScene()
    {
        PhotonNetwork.JoinLobby();
        fade.DOFade(0f, 0.25f).OnComplete(() => GoLobbyScene());
    }

    public void ServerSetting()
    {
        PhotonNetwork.GameVersion = Application.version;
    }

    public void TryConnectMasterServer()
    {
        ServerSetting();
        PhotonNetwork.ConnectUsingSettings();

        serverStateTxt.text = "Connecting to MasterServer...";
        Debug.Log("마스터 서버 접속 시도중");
    }

    public void PopUpLoginWindow()
    {
        loginWindowCG.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("ㅂㅂ ㅅㄱ");
    }


    public override void OnConnectedToMaster()
    {
        serverStateTxt.text = "Connect to MasterServer Complete!\nplz Login!";
        Debug.Log("마스터 서버 접속 성공");

        loginBtn.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        serverStateTxt.text = "Connect to MasterServer failed!\nReconnecting to masterServer...";
        Debug.Log("마스터 서버 접속 실패, 재시도");

        PhotonNetwork.ConnectUsingSettings();
    }
}
