using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using DG.Tweening;

public class RoomPlayerSlot : MonoBehaviour
{
    //width 256 / height 368

    //firstXPos = -384 second -128

    public PhotonView phView;

    public RawImage profileImg;
    public TextMeshProUGUI nickNameTxt;
    public TextMeshProUGUI readyStateTxt;

    public bool amMaster;
    public bool isReady;


    public void UpdateSlotInfo(bool _master, string name)
    {
        amMaster = _master;
        readyStateTxt.text = amMaster ? "Master" : "Ready";
        nickNameTxt.text = name;
    }


	private void Awake()
	{
        phView = GetComponent<PhotonView>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
