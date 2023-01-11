using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RoomSlotButton : MonoBehaviour
{

    LobbyController lobby;
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI roomMasterName;
    public TextMeshProUGUI roomPlayerCount;
    private void OnClick()
    {
        lobby.selectedRoomName = roomName.text;
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
