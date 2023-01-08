using System.Collections.Generic;
using System.Linq;

using Enums;
using UnityEngine;

[System.Serializable]
public class DiceOriginStatus
{
    //인스펙터 창에서 실제 개발자가 정해주는 1렙 기준 스텟
    
    public Enums.eDiceName eName;
    public string strName;

    public Enums.eTargetPriority targetPriority;

    public Enums.eAttackType atkType;

    public float dmg;
    public float dmgIncrease;

    [Range(0, 100f)]
    public float critPercent;
    public float critIncrease;

    public float atkTime;
    public float atkTimeIncrease;

    public float coolTime; //스킬 쿨타임
    public float coolTimeIncrease;

    public float skillDuration; //스킬 지속시간
    public float skillDurationIncrease;
}

public class ObjectManager : Manager<ObjectManager>
{

    [Header("Player")]
    public GameObject playerPrefab;
    public Player playerScript;

    [Header("Field")]
    public GameObject boardPrefab;
    public DiceBoard diceBoardScript;
    public EnemyPath enemyPathScript;

    [Header("Dices")]
    public DiceOriginStatus[] diceOriginStat;
    public Texture[] diceTexList;
    public Color[] colorList;
    public Texture[] spotTexList;

    public List<Dice> diceList;

    public Dice selectedDice;
    public Dice mouseOnDice;
    


    [Header("Enemies")]
    public List<Enemy> enemyList; //생성된 순서대로


    public void InitPlayer()
    {
        GameObject playerObj = Instantiate(playerPrefab);
        playerScript = playerObj.GetComponent<Player>();
    }



    public void EnemySpawn(int hp, bool isBoss = false)
    {
        GameObject tempEnemy = null;
        tempEnemy = isBoss ? PoolingManager.Instance.LentalObj("Boss"): PoolingManager.Instance.LentalObj("Enemy");


        tempEnemy.transform.position = enemyPathScript.enemyGenTr.position;
        Enemy tempEnemyScript = tempEnemy.GetComponent<Enemy>();

        tempEnemyScript.destinations[0] = enemyPathScript.corner1Tr;
        tempEnemyScript.destinations[1] = enemyPathScript.corner2Tr;
        tempEnemyScript.destinations[2] = enemyPathScript.enemyEndTr;

        //tempEnemyScript.isArrive = false;
        tempEnemyScript.isDead = false;
        tempEnemyScript.accumMove = 0f;
        tempEnemyScript.status.maxHp = hp;
        tempEnemyScript.status.curHp = hp;


//        int wave = (int)GameManager.Instance.gamingTime / 10;

        enemyList.Add(tempEnemyScript);
    }

    


    public Enemy GetEnemy(Dice dice)
    {
        Enemy target = null;
		switch (dice.status.targetPriority)
        {
            //linq의 orderby 쓰면 원본은 그대로 있음. 새로운 리스트 만듦.
            //거기서 다른 함수로 바로 원소 뽑아 오면 ㄱㅊ은디
            //아니면 .ToList() 붙여서 새로 받아와야함.

            case eTargetPriority.Front:
                {
                    target = enemyList.OrderByDescending(x => x.accumMove).FirstOrDefault();
                }
				break;
			case eTargetPriority.Rear:
                {
                    target = enemyList.OrderBy(x => x.accumMove).FirstOrDefault();
                }
				break;
			case eTargetPriority.Close:
                {
                    //float dist = float.MaxValue;
                    //for (int i = 0; i < enemyList.Count; ++i)
                    //{
                    //    float tempDist = Vector3.Distance(dice.transform.position, enemyList[i].transform.position);

                    //    if (tempDist < dist)
                    //    {
                    //        dist = tempDist;
                    //        target = enemyList[i];
                    //    }

                    target = enemyList.OrderBy(x => Vector3.Distance(dice.transform.position, x.transform.position)).FirstOrDefault();

                }
				break;
			case eTargetPriority.Random:
                {
                    int randIndex = Random.Range(0, enemyList.Count);

                    target = enemyList[randIndex];
                }
				break;
			case eTargetPriority.MaxHpAmount:
				break;
			case eTargetPriority.MinHpAmount:
				break;
			case eTargetPriority.CurHp:
                {
                    target = enemyList.OrderByDescending(x => x.status.curHp).FirstOrDefault();
                }
				break;
			case eTargetPriority.End:
				break;
			default:
				break;
		}

        return target;
    }

    public void DeathEnemy(Enemy script)
    {
        playerScript.status.Sp += 50;
        enemyList.Remove(script);
    }

    public void ArriveEnemy(Enemy script)
    {
        playerScript.Hit();
        enemyList.Remove(script);
        //PoolingManager.Instance.ReturnObj(script.gameObject);
    }


    public void KillAllEnemy()
    {
        foreach (Enemy script in enemyList)
        {
            script.isDead = true;
        }
    }


    public void DiceSpawn(Enums.eDiceName diceName, int spot = 1)
    {
		if (diceList.Count >= 12)
		{
			return;
		}

        int rowRandom = Random.Range(0, diceBoardScript.initRowCount);
        int colRandom = Random.Range(0, diceBoardScript.initColCount);

        int iCount = diceBoardScript.slots[rowRandom, colRandom].transform.childCount;
		if (iCount == 0)
		{
            int index = (int)diceName;
            string strName = Funcs.GetEnumName<eDiceName>(index);

            DiceOriginStatus origin = diceOriginStat[index];
            Texture diceTex = diceTexList[index];
            Texture spotTex = spotTexList[spot - 1];

            GameObject diceObj = PoolingManager.Instance.LentalObj(strName + "_Dice");
            Dice script = diceObj.GetComponent<Dice>();

            script.SetTextures(diceTex, spotTex,colorList[index]);
            script.status.InitStat(origin);
            
            script.status.spot = spot;
            script.status.index.SetIndex(rowRandom, colRandom);

            diceObj.transform.SetParent(diceBoardScript.slots[rowRandom, colRandom].transform);
            diceObj.transform.localPosition = new Vector3 (0f, 0.5f, 0f);

            diceList.Add(script);
        }
        else
		{
            DiceSpawn(diceName, spot);
		}
	}


    public DiceOriginStatus GetOriginStatus(eDiceName eName)
    {
        return diceOriginStat[(int)eName];
    }

    public void SelectDice(Dice _dice)
    {
        if (_dice)
        {

            selectedDice = _dice;

            var list = diceList.ToList();
            list.RemoveAll(x => x.status.eName == selectedDice.status.eName && x.status.spot == selectedDice.status.spot);

            foreach (Dice dice in list)
            {
                dice.SetGray(true);
            }
        }
        else
        {
            var list = diceList.ToList();
            list.RemoveAll(x => x.status.eName == selectedDice.status.eName && x.status.spot == selectedDice.status.spot);

            foreach (Dice dice in list)
            {
                dice.SetGray(false);
            }

            selectedDice = null;
        }
    
    
    }
    public void KillAllDice()
    { 
        
        
    }
    /// forTest


    public void InitBoard()
    {
        GameObject boardObj = GameObject.FindGameObjectWithTag("Board");
        
        if (boardObj == null)
        {
            boardObj = Instantiate(boardPrefab);
        }

        Board boardScript = boardObj.GetComponent<Board>();

        diceBoardScript = boardScript.diceBoard;
        enemyPathScript = boardScript.enemyPath;
    }


    private void Awake()
	{
        diceList = new List<Dice>();
        enemyList = new List<Enemy>();
        InitPlayer();
        InitBoard();    
    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha4; ++i)
            {
                if (Input.GetKeyDown((KeyCode)i))
                {
                    DiceSpawn((eDiceName)i - (int)KeyCode.Alpha1);
                }
            }
        }

    }
}
