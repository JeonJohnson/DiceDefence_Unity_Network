using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDice : Dice
{

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

	public override void Attack()
	{
		base.Attack();

		GameObject bullet = PoolingManager.Instance.LentalObj("DefaultBullet");
		bullet.transform.position = transform.position;
		bullet.transform.rotation = Quaternion.identity;

		Bullet bulletScript = bullet.GetComponent<Bullet>();

		bulletScript.target = targetObj;
		bulletScript.dir = (targetObj.centerTr.position - transform.position).normalized;

		bulletScript.atkType = status.atkType;

		bulletScript.isCrit = CritCheck();
		bulletScript.dmg = (int)(bulletScript.isCrit ? status.dmg * 2 : status.dmg);

		bulletScript.PlaySound();

	}


	protected override IEnumerator SkillCoroutine()
	{
		yield return StartCoroutine(base.SkillCoroutine());
	}
	public override void Skill()
	{
		base.Skill();
	}


	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}



}
