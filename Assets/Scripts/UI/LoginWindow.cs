using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

using Photon.Pun;
using Photon.Realtime;


public class LoginWindow : MonoBehaviourPunCallbacks
{

    public TMP_InputField nickNameIF;
    public Button enterBtn;
    public TextMeshProUGUI nickNameCheckTxt;

    public Button lobbySceneBtn;

	public void QuitWindow()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
	}

    public void EnterNickName()
    {
        GameManager.Instance.NickName = nickNameIF.text;
        nickNameCheckTxt.text = $"ur nickName : ''{GameManager.Instance.NickName}''\n³ª°¥²¨¸é ESCÅ° ¤¡";

        nickNameIF.text = string.Empty;
    }


	private void Awake()
	{
		
	}

	// Start is called before the first frame update
	void Start()
    {
        enterBtn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!nickNameIF.text.Equals(string.Empty))
        {
            enterBtn.interactable = true;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                EnterNickName();
            }
        }
        else
        {
            enterBtn.interactable = false;

        }

        QuitWindow();


        lobbySceneBtn.interactable = !GameManager.Instance.NickName.Equals(string.Empty);
    }

}
