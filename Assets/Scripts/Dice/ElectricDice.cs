using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDice : Dice
{
	protected override IEnumerator AttackCoroutine()
	{
		yield return StartCoroutine(base.AttackCoroutine());

		while (true)
		{
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

		SearchTarget();

		if (!targetObj)
		{
			return;
		}

		GameObject light = PoolingManager.Instance.LentalObj("LightningBullet");
		light.transform.position = transform.position;

		LightningBullet bulletScript = light.GetComponent<LightningBullet>();

		bulletScript.target = targetObj;
		bulletScript.isCrit = CritCheck();
		bulletScript.dmg = (int)(bulletScript.isCrit ? status.dmg * 2 : status.dmg);
		bulletScript.atkTargetCount = status.spot;


		bulletScript.Fire();
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
