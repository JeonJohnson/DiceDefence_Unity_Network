using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;
using TMPro;

using DG.Tweening;

using Enums;
using Structs;

public class Enemy : MonoBehaviour
{
    public EnemyStatus status;

    public bool isDead = false;
    //public bool isArrive = false;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI normalDmgTxt;
    public TextMeshProUGUI critDmgTxt;

    Sequence normalDmgSeq;
    Sequence critDmgSeq;

    public float accumMove = 0f;


    public Transform[] destinations;//첫번째 코너, 두번째 코너, 도착지점
    private int curDestIndex = 0;

    public Transform centerTr;

    //Components
    public Rigidbody rd;
    public AudioSource auds;
    public AudioClip hitClip;
    public AudioClip deathClip;

    //Components

    //Test
    public MeshRenderer mr;
    public Material mat;
    public float deathSpd;
    public IEnumerator DeathShaderCoroutine(Color color)
    {
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        float progress = 0f;
        
        hpText.DOFade(0f, 0.5f);

        mat.SetColor("_DissolveColor", color);

        while (progress <= 0.95)
        {
            progress += Time.deltaTime * deathSpd;

            mat.SetFloat("_Progress", progress);
            yield return null;
        }

        PoolingManager.Instance.ReturnObj(this.gameObject);
    }
    //Test

 

    public void EnemyMove()
    {
        if (isDead)
        {
            return;
        }

        Vector3 dir = (destinations[curDestIndex].position - transform.position).normalized;
        float dist = Vector3.Distance(destinations[curDestIndex].position, transform.position);

        if (dist <= 0.1f)
        {
            if (curDestIndex != destinations.Length - 1)
            { ++curDestIndex; }
            else
            {
                ArriveToGoal();
            }
        }
        accumMove += (dir * Time.deltaTime * status.moveSpd).magnitude;

        rd.MovePosition(transform.position + (dir * Time.deltaTime * status.moveSpd));
    }

   

    public void ArriveToGoal()
    {
        //if (isArrive)
        //{
        //    //--ObjectManager.Instance.playerScript.status.curHp;
        //    //ObjectManager.Instance.enemyList.Remove(this);

        isDead = true;
        ObjectManager.Instance.ArriveEnemy(this);
        StartCoroutine(DeathShaderCoroutine(Color.blue));
        //}
    }

    public void Death()
    {
        //if (isDead)
        //{
        //    ObjectManager.Instance.DeathEnemy(this);

        //    StartCoroutine(DeathShaderCoroutine());
        //}

        isDead = true;
        ObjectManager.Instance.DeathEnemy(this);
        StartCoroutine(DeathShaderCoroutine(Color.red));
    }

    public void Hit(DamagedStruct dmg)
	{
        if (isDead)
        {
            return;
        }

        float realDmg = dmg.isCrit ? dmg.dmg * 2f : dmg.dmg;
        
        HitEffect(dmg);
        status.curHp -= realDmg;
 
        if (status.curHp <= 0)
        {
            status.curHp = 0;
            Death();

            auds.PlayOneShot(deathClip);
        }
        else
        {
            auds.PlayOneShot(hitClip);
        }
    }

   

    void HitEffect(DamagedStruct dmgSt)
    {

        if (dmgSt.isCrit)
        {
            critDmgTxt.text = $"{dmgSt.dmg}";
            //Color startColor = tmp.color;
            //startColor.a = 1;
            critDmgTxt.color = Funcs.SetAlpha(critDmgTxt.color, 1f);


            critDmgSeq.Kill();
            critDmgSeq = DOTween.Sequence();
            critDmgSeq.Append(critDmgTxt.DOFade(0f, 0.5f));
        }
        else
        {
            normalDmgTxt.text = $"{dmgSt.dmg}";
            //Color startColor = tmp.color;
            //startColor.a = 1;
            normalDmgTxt.color = Funcs.SetAlpha(normalDmgTxt.color, 1f);


           normalDmgSeq.Kill();
           normalDmgSeq = DOTween.Sequence();
           normalDmgSeq.Append(normalDmgTxt.DOFade(0f, 0.5f));

        }
        
    }



    void Awake()
	{
        rd = GetComponent<Rigidbody>();
        destinations = new Transform[3];

        mat = Instantiate(mr.material);
        mr.material = mat;
        mat.SetFloat("_EffectSize", Random.Range(0.2f, 0.8f));

        auds = GetComponent<AudioSource>();
	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   //     DeathCheck();

        if (!isDead)
        {
            //if (status.curHp <= 0f)
            //{
            //    isDead = true;
            //}

            hpText.text = ((int)status.curHp).ToString();
            EnemyMove();
            //ArriveCheck();
        }
    }

	private void OnEnable()
	{
        hpText.color = Funcs.SetAlpha(hpText.color, 1f);
        normalDmgTxt.color = Funcs.SetAlpha(normalDmgTxt.color, 0f);
        critDmgTxt.color = Funcs.SetAlpha(critDmgTxt.color, 0f);

        mat.SetFloat("_Progress", 0f);
    }
}
