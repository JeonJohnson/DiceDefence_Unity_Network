using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class WindDice : Dice
{

	Sequence skillSeq;
	[SerializeField]float rotSpd;



	//쿨타임마다 공속 빨라지는 친구
	public override void Attack()
	{
		base.Attack();

		GameObject bullet = PoolingManager.Instance.LentalObj("WindBullet");
		bullet.transform.position = transform.position;
		bullet.transform.rotation = Quaternion.identity;

		Bullet bulletScript = bullet.GetComponent<Bullet>();

		bulletScript.target = targetObj;
		bulletScript.atkType = status.atkType;
		bulletScript.dir = (targetObj.centerTr.position - transform.position).normalized;

		bulletScript.isCrit = CritCheck();
		bulletScript.dmg = (int)(bulletScript.isCrit ? status.dmg * 2 : status.dmg);

		bulletScript.PlaySound();
	}

	protected override IEnumerator AttackCoroutine()
	{
		yield return StartCoroutine(base.AttackCoroutine());

		while (true)
		{
			//if (targetObj && targetObj.enabled && !targetObj.isDead)
			//{
			//	SearchTarget();
			//}

			if ((targetObj && targetObj.enabled && targetObj.isDead)
				|| (targetObj && !targetObj.enabled))
			{
				SearchTarget();
			}

			if (targetObj)
			{
				break;
			}

			yield return null;
		}

		Attack();

		StartCoroutine(AttackCoroutine());
	}

	protected override IEnumerator SkillCoroutine()
	{
		yield return StartCoroutine(base.SkillCoroutine());
		Skill();
		yield return new WaitForSeconds(status.skillDuration);
		SkillEnd();

		StartCoroutine(SkillCoroutine());
	}

	public override void Skill()
	{
		base.Skill();
		status.atkTime *= 0.5f;
		rotSpd = 1500f;
	}

	private void SkillEnd()
	{
		rotSpd = 0f;
		status.atkTime = ObjectManager.Instance.diceOriginStat[(int)status.eName].atkTime;
		transform.DORotate(new Vector3(0, 0, 0), 1f);
	}


	//{

	//}

	protected override void Awake()
	{
		base.Awake();
	}

	// Start is called before the first frame update
	protected override void  Start()
    {
		base.Start();


		
		//StartCoroutine(AttackCoroutine());
	}

	// Update is called once per frame
	protected override void Update()
    {
		base.Update();


		transform.Rotate(0f, Time.deltaTime * rotSpd, 0f);
	}


}
