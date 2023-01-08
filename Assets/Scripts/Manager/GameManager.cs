using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;

public enum eSceneIndex
{ 
    INTRO,
    TITLE,
    LOBBY,
    MAINGAME,
    END
}

public class GameManager : Manager<GameManager>
{

    public float gamingTime = 0f;
    public bool isGameEnd = false;
    public bool isGameWin = false;
    public float grayScaleMaxTime;

    //public SceneLoader sceneLoader;

    private string nickName = string.Empty;
    public string NickName
    {
        get
        {
            return nickName;
        }
        set
        {
            Photon.Pun.PhotonNetwork.LocalPlayer.NickName = value;
            nickName = Photon.Pun.PhotonNetwork.LocalPlayer.NickName;
        }
    }


    public AudioClip titleBgm;
    public AudioClip ingameBgm;
    public AudioSource bgmAudSource;


    [Header("----For Managers----")]
    public GameObject managerBox;
    public GameObject managerBox_Destory;



    //게임시작시 호출되는 어트리뷰트
    //static 함수만 가능함.
    [RuntimeInitializeOnLoadMethod]
    private static void GameInitialize()
    {
        //여기서는 진짜 GameManager만 생성해주기!
        GameManager.InstantiateManager(Defines.DONT_DESTROY);
    }
    public void InstantiateManagerBoxes(out GameObject box, out GameObject destroyBox)
    {
        box = Funcs.CheckGameObjectExist("ManagerBox");
        DontDestroyOnLoad(box);
        destroyBox = Funcs.CheckGameObjectExist("ManagerBox_Destory");
    }


    public void LoadScene(eSceneIndex sceneIndex)
    {
        SceneManager.LoadScene((int)sceneIndex);
    }

	public void  SceneCheck(eSceneIndex sceneIndex)
    {
		switch (sceneIndex)
		{
			case eSceneIndex.INTRO:
                {
                    InitializeIntroScene();
                }
				break;
			case eSceneIndex.TITLE:
                {
                    InitializeTitleScene();
                }
				break;
            case eSceneIndex.LOBBY:
                {
                    //InitializeTitleScene();
                }
                break;
            case eSceneIndex.MAINGAME:
                {
                    InitializeMainGameScene();
                }
				break;
			case eSceneIndex.END:
				break;
			default:
				break;
		}
	}


    private void InitializeIntroScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void InitializeTitleScene()
    {
        Cursor.lockState = CursorLockMode.None;

        EventManager.InstantiateManager(true);

        bgmAudSource.clip = titleBgm;
        bgmAudSource.Play();
    }

    private void InitializeLobbyScene()
    {

    }

    private void InitializeMainGameScene()
    {
        Time.timeScale = 1f;
        bgmAudSource.clip = ingameBgm;
        bgmAudSource.Play();

        Cursor.lockState = CursorLockMode.None;

        PoolingManager.InstantiateManager(false);
        InGameManager.InstantiateManager(false);
        ObjectManager.InstantiateManager(false);
        InGameUIManager.InstantiateManager(false);
        

        //ObjectManager.InstantiateManagerByPrefabPath(Defines.managerPrfabFolderPath, false);
        //UIManager.InstantiateManager(false);
        //EventManager.InstantiateManager(true);
    }


    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

	public void LoadScene(string sceneStr)
	{
        SceneManager.LoadScene(sceneStr);
    }
	//public void LoadScene(int sceneIndex)
	//{
	//    sceneLoader.LoadScene(sceneIndex);

	//}

	//public void LoadNextScene()
	//{ 

	//}

	private void Awake()
	{
        DontDestroyOnLoad(this.gameObject);
        InstantiateManagerBoxes(out managerBox, out managerBox_Destory);

        Screen.SetResolution(Defines.winCX, Defines.winCY,false);

        DOTween.Init();
    }

    void Start()
    {
        


    }

    void Update()
    {

    }

	public override void OnEnable()
	{
        base.OnEnable();

        SceneCheck((eSceneIndex)SceneManager.GetActiveScene().buildIndex);
    }
	public override void OnDisable()
	{
        base.OnDisable();
    }

	public override void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        bgmAudSource.Stop();
        SceneCheck((eSceneIndex)scene.buildIndex);


    }

}
