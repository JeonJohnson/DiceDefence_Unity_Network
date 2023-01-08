using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Enums;
using Structs;

public enum SkillState
{ 
    WAIT,
    //CAN,
    ING,
    END
}


[System.Serializable]
public class DiceStatus
{
    public void InitStat(DiceOriginStatus originStat)
    {
        eName = originStat.eName;
        strName = originStat.strName;

        dmg = originStat.dmg;

        targetPriority = originStat.targetPriority;
        atkType = originStat.atkType;

        critialPercent = originStat.critPercent;
        atkTime = originStat.atkTime;
        coolTime = originStat.coolTime;
        skillDuration = originStat.skillDuration; 
    }

    public void SpotUp(DiceOriginStatus originStat)
    {
        ++spot;

        DiceOriginStatus origin = ObjectManager.Instance.diceOriginStat[(int)eName];

        dmg = origin.dmg + (origin.dmgIncrease * (spot - 1));
        critialPercent = origin.critPercent + (origin.critIncrease * (spot - 1));
        atkTime = origin.atkTime - (origin.atkTimeIncrease * (spot - 1));
        coolTime   = origin.coolTime - (origin.coolTimeIncrease * (spot - 1));
    }

    public void ResetReset(DiceOriginStatus originStat)
    { 
        
    }
    [ReadOnly]
    public Enums.eDiceName eName;

    [ReadOnly]
    public string strName;

    [ReadOnly]
    public int spot;

    [ReadOnly]
    public Index index;

    [ReadOnly]
    public Enums.eTargetPriority targetPriority;

    [ReadOnly]
    public Enums.eAttackType atkType;
    //[ReadOnly]
    //public Buff atkEffect;

    [ReadOnly]
    public float dmg;
    //public float dmgIncre;

    [ReadOnly]
    [Range(0, 100f)]
    public float critialPercent;

    [ReadOnly]
    public float atkTime;

    [ReadOnly]
    public float coolTime; //스킬 쿨타임

    [ReadOnly]
    public float skillDuration; //스킬 지속시간


    //[ReadOnly]
    //public Buff buff;//현재 걸린거

}


public class Dice : MonoBehaviour
{

    //[ReadOnly]
    public DiceStatus status;

    //[SerializeField]private bool isInstiate = false;
    [SerializeField] protected bool isFresh = true;

    public Enemy targetObj;
    public Vector3 targetDir;
    public float targetDist;

    public SkillState skillState;

    public Material mat;
    Color themaColor;

    public AudioSource auds;


    public void SetTextures(Texture diceTex, Texture spotTex, Color spotColor)
    {
        mat.SetTexture("_MainTex", diceTex);
        mat.SetTexture("_SubTex", spotTex);
        themaColor = spotColor;
        mat.SetColor("_Color", spotColor);
    }

    public void SetAlpha(float alpha)
    {
        mat.SetFloat("_Alpha", alpha);
    }

    public void SetGray(bool isGray)
    {
        mat.SetFloat("_Gray", Funcs.B2I(isGray));
    }

    public void CalcAboutTarget()
    {
        if (targetObj == null)
        {
            return;
        }

        targetDir = (targetObj.transform.position - transform.position).normalized;
        targetDist = Vector3.Distance(targetObj.transform.position, transform.position);
    }

    public void SearchTarget()
    {
		if (ObjectManager.Instance.enemyList.Count <= 0)
		{
			targetObj = null;

			return;
		}

		targetObj = ObjectManager.Instance.GetEnemy(this);
	}

    public virtual void Attack()
    {

    }
    protected virtual IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(status.atkTime);
    
    }



    public bool CritCheck()
    {
        int percent = Random.Range(0, 100);

        if (percent <= status.critialPercent)
        {
            return true;         
        }
        return false;
    }

    public virtual void Skill()
    {

    }
    protected virtual IEnumerator SkillCoroutine()
    {

        yield return new WaitForSeconds(status.coolTime);
    }

	public void SelectDice()
	{
        mat.SetFloat("_Alpha", 0.5f);


        GameObject dummy = PoolingManager.Instance.LentalObj("DummyDice");
        dummy.transform.position = transform.position;
        DummyDice script = dummy.GetComponent<DummyDice>();

		Texture diceTex = mat.GetTexture("_MainTex");
		Texture spotTex = mat.GetTexture("_SubTex");
        Color spotColor = themaColor;
        script.Select(diceTex, spotTex, spotColor,this);

        ObjectManager.Instance.SelectDice(this);

	}

	public void Merge()
	{
        //StopAllCoroutines();
        SetAlpha(1f);
        status.SpotUp(ObjectManager.Instance.GetOriginStatus(status.eName));

        Texture spotTex = ObjectManager.Instance.spotTexList[status.spot -1];
        mat.SetTexture("_SubTex", spotTex);

    }

    public void ReturnPool()
    {
        mat.SetFloat("_Alpha", 1f);
        mat.SetFloat("_Gray", 0);
        targetObj = null;
        ObjectManager.Instance.diceList.Remove(this);
        PoolingManager.Instance.ReturnObj(gameObject);
    }


	protected virtual void Awake()
	{
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mat = Instantiate(mr.material);
        mr.material = mat;

        mat.SetFloat("_Alpha", 1f);
        mat.SetFloat("_Gray", 0f);

        auds = GetComponent<AudioSource>();
            

    }
	// Start is called before the first frame update
	protected virtual void Start()
    {
        if (isFresh)
        {
            //StartCoroutine(AttackCoroutine());
            StartCoroutine(SkillCoroutine());
            isFresh = false;
        }


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if (targetObj)
        //{
			if ((targetObj && targetObj.enabled && targetObj.isDead) 
                //isDead는 됐지만, 아직 SetActive true일 때.
                || (targetObj && !targetObj.enabled))
                //SetActive False일 때 
			{
                //isDead는 됐지만, 아직 SetActive True일 때
                //거나
                //SetActive자체가 false일때

				targetObj = null;

			}

			//if (!targetObj.enabled)
   //         {
   //             targetObj = null;
   //         }
   //         else if (targetObj.isDead)
   //         {
   //             targetObj = null;
   //         }
        //}

        //if (!targetObj.enabled || targetObj.isDead)
        //{
        //    targetObj = null;
        //첫번째 조건이 true(꺼져있으면)가 나오면 2번째껀 검사 안해도 되는건 맞지만

        //이렇게 해버리면, 첫번째 조건이 false이면 2번째 것도 검사해버리는데
        //그렇다면 enable이 된놈 접근하는 거니까 말이 안됨.

        //}

    
        

        if (!targetObj)
        {
            SearchTarget();
        }

    }


    protected virtual void OnEnable()
    {
        if (!isFresh)
        {
            //오브젝트 생성시에도 OnEnable은 실행됨.
            //바로 SetActive(false)를 한다고해도
            //Awake -> OnEnable -> SetActive(false) -> OnDisable
            //근데 그거 때문에 지금 생성 되자말자 코루틴실행 됐다가  
            //Disable 하면서 StopAll해주는데도
            //다시 실행될때 이전에 wait 먹여놨던거 씹하고 바로 그다음으로 넘어감.
            //구글링해도 이유는 모르겠는데
            //일단은 Start에서 이렇게 쓰자...
            StartCoroutine(SkillCoroutine());
        }

        StartCoroutine(AttackCoroutine());
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }


    public void OnMouseEnter()
    {
        Debug.Log($"{gameObject.name} : 마우스 들어옴 응기잇");  
        ObjectManager.Instance.mouseOnDice = this;
    }

    public void OnMouseOver()
    {
        Debug.Log($"{gameObject.name} : 마우스 over중");

        if (Input.GetMouseButtonDown(0))
        {
            if (status.spot != 7)
            { SelectDice(); }
          Debug.Log("클릭까지됌ㅋㅋ");
        }
    }


    public void OnMouseExit()
    {
        Debug.Log($"{gameObject.name} : 마우스 나감ㅅㄱㅇ");
        ObjectManager.Instance.mouseOnDice = null;
    }
}
