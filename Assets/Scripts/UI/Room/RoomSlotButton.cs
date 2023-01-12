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

    Color pressedColor;
    Color normalColor;

    bool isSelected = false;
    Button btn;
    private void OnClick()
    {
        lobby.SelectRoom(this);
        //if (isSelected)
        //{
        //    isSelected = false;
        //    lobby.selectedRoomName = string.Empty;

        //    ColorBlock temp = btn.colors;
        //    temp.normalColor = normalColor;
        //    btn.colors = temp;
        //}
        //else
        //{
        //    isSelected = true;
        //    lobby.selectedRoomName = roomNameTmp.text;

        //    ColorBlock temp = btn.colors;
        //    temp.normalColor = pressedColor;
        //    btn.colors = temp;
        //}
    }

	public void Selected()
	{
		isSelected = true;
		//lobby.selectedRoomName = roomNameTmp.text;

		ColorBlock temp = btn.colors;
		temp.normalColor = pressedColor;
        temp.selectedColor = pressedColor;
        btn.colors = temp;
	}

    public void SelectRelease()
    {
		isSelected = false;
		//lobby.selectedRoomName = string.Empty;

		ColorBlock temp = btn.colors;
		temp.normalColor = normalColor;
        temp.selectedColor = normalColor;
		btn.colors = temp;
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
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        normalColor = btn.colors.normalColor;
        pressedColor =  btn.colors.pressedColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
