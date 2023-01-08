using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using DG.Tweening;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	#region singleton
	private static NetworkManager instance = null;

    public static NetworkManager Instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject newObj = new GameObject(typeof(NetworkManager).Name);
                instance = newObj.AddComponent<NetworkManager>();
            }

            return instance;
        }
    }
    #endregion

    public TextMeshProUGUI serverStateTxt;
    public Button gameStartButton;
   

	private void Awake()
	{
		
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ServerSetting()
    {
        PhotonNetwork.GameVersion = Application.version;
           

    }

    public void TryConnectMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();

        serverStateTxt.text = "Connecting to MasterServer...";
        Debug.Log("������ ���� ���� �õ���");
    }

	public override void OnConnectedToMaster()
	{
        serverStateTxt.text = "Connect to MasterServer Complete!";
        Debug.Log("������ ���� ���� ����");
    }

	public override void OnDisconnected(DisconnectCause cause)
	{
        serverStateTxt.text = "Connect to MasterServer failed!\nReconnecting to masterServer...";
        Debug.Log("������ ���� ���� ����, ��õ�");
       
        PhotonNetwork.ConnectUsingSettings();
    }

    
}

