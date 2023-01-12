using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RoomSlotButton : MonoBehaviour
{

    LobbyController lobby;
    public TextMeshProUGUI roomNameTmp;
    public TextMeshProUGUI roomMasterNameTmp;
    public TextMeshProUGUI roomPlayerCountTmp;
    private void OnClick()
    {
        lobby.selectedRoomName = roomNameTmp.text;
    }


    public void SettingRoomTexts(string roomName, string masterName, int curCount, int maxCount)
    {
        roomNameTmp.text = roomName;
        roomMasterNameTmp.text = masterName;
        roomPlayerCountTmp.text = $"{curCount} / {maxCount}";
    }

    

	private void Awake()
	{
		
	}

	// Start is called before the first frame update
	void Start()
    {
        lobby = GameObject.FindObjectOfType<LobbyController>();
        //GetComponent<Button>().onClick.AddListener(OnClick);
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
