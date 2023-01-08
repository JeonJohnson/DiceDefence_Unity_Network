using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

using Photon.Realtime;
using Photon.Pun;

public class Chatting : MonoBehaviourPunCallbacks
{

    public PhotonView phView;

    public Button chatEnterBtn;
    public TMP_InputField chatInput;
    public List<string> chtaList;

    public TextMeshProUGUI chatLogTxt;
    public ScrollRect chatScroll;

    public void OnClickSendChat()
    {
        if (chatInput.text.Equals(string.Empty))
        {
            return;
        }

        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, chatInput.text);

        phView.RPC("ReceiveMsg", RpcTarget.All, msg);
        //ReceiveMsg(msg);
        chatInput.ActivateInputField();
        chatInput.text = "";
    }


    [PunRPC]
    public void ReceiveMsg(string msg)
	{
		chatLogTxt.text += "\n" + msg;
        
	}

	private void Awake()
	{
        phView = GetComponent<PhotonView>();
	}

	// Start is called before the first frame update
	void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        chatLogTxt.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
