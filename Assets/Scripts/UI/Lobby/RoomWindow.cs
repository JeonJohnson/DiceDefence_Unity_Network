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

    [SerializeField]
    TextMeshProUGUI roomNameTmp;

    [SerializeField]
    List<GameObject> playerList;//이거 나중에 각각 PlayerList 혹은 Slot으로 코드만들어서 넣어주기

    [SerializeField]
    TMP_InputField chatInputField;

   //[SerializeField]
   public string chatLog;


    public void ChatSend() 
    { 
    
    }

    [PunRPC]
    public void ChatRPC(string msg)
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




    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        chatLog += $"\n {PhotonNetwork.NickName}님이(가) 입장하셨스빈다.";
    }
}
