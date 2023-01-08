using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums;

using DG.Tweening;


//public enum RoundType
//{ 
//    ROUND_1,
//    ROUND_2,
//    ROUND_3,
//    ROUND_4,
//    ROUND_BOSS,
//    END
//}

public enum GameState
{
    SpawnWait,
    Spawning,
    SpawnEnd,
    Loose,
    End
}

public class InGameManager : Manager<InGameManager>
{
    //여기선 ObjectManager의 소환함수로 시간에 따른 소환명령등만 내리기
    //그리고 게임 승리, 패배등에 따른 연출 등만 표현


    //초당 1마리씩 10마리 소환 
    
    //각 마리당 체력 (10 * 라운드)만큼 증가 
    
    //데미지UI (좀 더 크게 크리티컬UI), 체력IU

    public int curWave;
    public GameState curState = GameState.SpawnWait;
    public int startHp;

    public float spawnTime;
    public int spawnCount;

    public bool isGameLoose = false;

    //public int dicePrice;
    //public int diceIncrease;
    Sequence gameLooseSeq;
    Sequence waveStartSeq;

    public void UpdateWaveInfo()
    {
        InGameUIManager.Instance.UpdateWaveInfo(curWave);
        InGameUIManager.Instance.UpdateRoundUI(curWave);
    }

    public void SpawnRandomDice()
    {
        Enums.eDiceName randomName = (Enums.eDiceName)(Random.Range((int)Enums.eDiceName.Default, (int)Enums.eDiceName.End));

        ObjectManager.Instance.DiceSpawn(randomName);
    }

    private void WaveStartSeq()
    {
        waveStartSeq = DOTween.Sequence();

        waveStartSeq.Append(InGameUIManager.Instance.noticeCG.DOFade(1f, 1f).SetEase(Ease.OutQuart));
		waveStartSeq.AppendInterval(1f);
		waveStartSeq.Append(InGameUIManager.Instance.noticeCG.DOFade(0f, 1f).SetEase(Ease.OutQuart));

		waveStartSeq.OnComplete(() => SpawnStart());
	}
    public void WaveStartEvent()
    {
        UpdateWaveInfo();
        InGameUIManager.Instance.noticeTxt.text = $"Wave : {curWave}";
        //waveStartSeq.Play();
        WaveStartSeq();
    }

    public void SpawnStart()
    {
        curState = GameState.Spawning;
        StartCoroutine(SpawnEnemy());
    }

    public void WaveEndEvent()
    {

        ++curWave;
        WaveStartEvent();
        //waveStartSeq.Play();
    }

    IEnumerator WaveEndCoroutine()
    {
        curState = GameState.SpawnWait;
        
        yield return new WaitForSeconds(1f);
        WaveEndEvent();
    }


	IEnumerator SpawnEnemy()
	{
        if (curWave % 5 != 0)
        {
            for (int i = 0; i < spawnCount; ++i)
            {
                //startHp += 10 * (curWave);
                startHp += (10 * curWave);
                ObjectManager.Instance.EnemySpawn(startHp);

                yield return new WaitForSeconds(spawnTime);
            }
        }
        else
        {
            ObjectManager.Instance.EnemySpawn(startHp * 3,true);
        }

        curState = GameState.SpawnEnd;
    }

    public void GameLoose()
    {
        isGameLoose = true;

        InGameUIManager.Instance.noticeTxt.text = $"Defeated";

        InGameUIManager.Instance.screenOverlayEffectCG.alpha = 1f;

        //InGameUIManager.Instance.noticeCG.DOFade(1f, 0.25f).SetEase(Ease.OutQuart);
        //InGameUIManager.Instance.screenOverlayBlurMat.DOFloat(1f, "_BlurAmount", 3f);
        //Camera.main.GetComponent<GrayScaleScreenEffect>().mat.DOFloat(1f, "_GrayScaleAmount", 3f)/*.OnComplete(() =>Debug.Log("Ending"))*/;

        gameLooseSeq = DOTween.Sequence();

        gameLooseSeq.Append(InGameUIManager.Instance.noticeCG.DOFade(1f, 0.25f).SetEase(Ease.OutQuart));
        gameLooseSeq.Append(InGameUIManager.Instance.screenOverlayBlurMat.DOFloat(1f, "_BlurAmount", 3f));
        gameLooseSeq.Append(Camera.main.GetComponent<GrayScaleScreenEffect>().mat.DOFloat(1f, "_GrayScaleAmount", 3f));
        gameLooseSeq.AppendInterval(2f);

        gameLooseSeq.OnComplete(() => GameManager.Instance.LoadScene(eSceneIndex.TITLE));
    }




    private void GameCheck()
    {
		switch (curState)
		{
			case GameState.SpawnWait:
                { 
                
                }
				break;
			case GameState.Spawning:
                { 
                
                }
				break;
			case GameState.SpawnEnd:
                {
                    if (ObjectManager.Instance.enemyList.Count <= 0 && !isGameLoose)
                    {
                        //  WaveEndEvent();
                        StartCoroutine(WaveEndCoroutine());
                    }
                }
				break;
			case GameState.Loose:
                { 
                
                }
				break;
			case GameState.End:
				break;
			default:
				break;
		}


	}

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(3f);
        WaveStartEvent();
    }

	private void Awake()
	{
        
	}
    
	void Start()
    {
        StartCoroutine(GameStartCoroutine());
    }

    void Update()
    {
        GameCheck();

       //UpdateWaveInfo();
    }
}
