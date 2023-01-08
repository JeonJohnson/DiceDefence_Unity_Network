using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

using Enums;
using Structs;


public class InGameUIManager : Manager<InGameUIManager>
{
    [Header("Hud UIs")]
    public CanvasGroup hudCG;
    public TMP_Text WaveInfoTxt;
    public Image[] roundIcon;
    public void UpdateWaveInfo(int wave)
    {
        WaveInfoTxt.text = $"Wave : {wave}";
    }
    public void UpdateRoundUI(int wave)
    {
        int index = wave % 5;

        for (int i = 0; i < roundIcon.Length; ++i)
        {
            roundIcon[i].color = Color.black;
        }

        if (index != 0)
        {
            roundIcon[index - 1].color = Color.white;
        }
        else
        {
            roundIcon[4].color = Color.red;
        }
    }


    public TextMeshProUGUI spTxt;
    public void UpdateSpText(float sp)
    {
        spTxt.text = $"SP : {(int)sp}";
    }
    public void UpdateDicePrice(float price)
    {
        dicePriceTxt.text = $"SP : {(int)price}";
    }


    public TextMeshProUGUI hpTxt;
    public void UpdateHpText(int curHp, int fullHp)
    {
        hpTxt.text = $"Hp : {curHp} / {fullHp}";
    }


    public Button BuyDiceBtn;
    public TextMeshProUGUI dicePriceTxt;
    public void BuyDiceEventSetting()
    {
        BuyDiceBtn.onClick.AddListener((EventManager.Instance.OnBuyDiceButtonClick));
    }

    [Header("Notice")]
    public CanvasGroup noticeCG;
    public TextMeshProUGUI noticeTxt;

    public void WaveEndUI()
    {

    }


    public DiceBuyButton diceBuyBtn;


    [Header("Option")]
    public Button pasueBtn;
    public Button spd1Btn;
    public Button spd2Btn;

    public void SetGameSpdEvnet()
    {
        pasueBtn.onClick.AddListener(() => OnClickSpdBtn(0f, pasueBtn));
        spd1Btn.onClick.AddListener(() => OnClickSpdBtn(1f, spd1Btn));
        spd2Btn.onClick.AddListener(() => OnClickSpdBtn(2f, spd2Btn));
    }

    public void OnClickSpdBtn(float spd, Button btn)
    {
        Time.timeScale = spd;
        pasueBtn.GetComponent<Image>().color = Color.white;
        spd1Btn.GetComponent<Image>().color = Color.white; ;
        spd2Btn.GetComponent<Image>().color = Color.white; ;


        btn.GetComponent<Image>().color = Color.gray;

        if (spd <= 0f)
        {
            gameScreenBlurMat.SetFloat("_BlurAmount", 1f);
            gameScreenEffectCG.alpha = 1f;
        }
        else
        {
            gameScreenBlurMat.SetFloat("_BlurAmount", 0f);
            gameScreenEffectCG.alpha = 0f;
        }
    }


    [Header("ScreenEffect")]
    public Material blurMatOrigin;


    [Header("GameScreenEffect")]
    public CanvasGroup gameScreenEffectCG;
    public Material gameScreenBlurMat;
    public Image gameScreenBlurPanel;

    [Header("ScreenOverlayEffect")]
    public CanvasGroup screenOverlayEffectCG;
    public Material screenOverlayBlurMat;
    public Image screenOverlayBlurPanel;

    public void CopyBlurMat()
    {
        gameScreenBlurMat = Instantiate(blurMatOrigin);
        gameScreenBlurPanel.material = gameScreenBlurMat;

        screenOverlayBlurMat = Instantiate(blurMatOrigin);
        screenOverlayBlurPanel.material = screenOverlayBlurMat;
    }

    public void SetScreenOverlayBlur(float ratio)
    {
        screenOverlayEffectCG.alpha = 1f;
        screenOverlayBlurMat.SetFloat("_BlurAmount", ratio);
    }

    public float GetScreenOverlayBlur()
    {
        return screenOverlayBlurMat.GetFloat("_BlurAmount");
    }
    //public Image blurImg;
    //public Material blurMat;

    //public void FindBlurMaterial()
    //{
    //    blurMat = blurImg.material;

    //    blurMat.SetFloat("_BlurAmount", 0f);
    //}

    //public Button restartBtn;
    //public TMP_Text noticeTmp;

    //public void RestartBtnSetting()
    //{
    //    restartBtn.onClick.AddListener(EventManager.Instance.OnClickNextScene);
    //    restartBtn.gameObject.SetActive(false);
    //}

    //public void AlphaSet(float alpha)
    //{
    //    Color temp = Color.white;
    //    temp.a = alpha;
    //    restartBtn.GetComponent<Image>().color = temp;

    //    Color NoticeTemp = Color.black;
    //    NoticeTemp.a = alpha;
    //    noticeTmp.color = NoticeTemp;
    //}

    ////For Test


    private void Awake()
	{

    }
	// Start is called before the first frame update
	void Start()
    {
        SetGameSpdEvnet();
        BuyDiceEventSetting();
        CopyBlurMat();
        gameScreenEffectCG.GetComponent<Canvas>().worldCamera = Camera.main;
        hudCG.GetComponent<Canvas>().worldCamera = Camera.main;

        gameScreenEffectCG.alpha = 0f;
        screenOverlayEffectCG.alpha = 0f;
        noticeCG.alpha = 0f;


    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    SetScreenOverlayBlur(1f);
    }

  }

