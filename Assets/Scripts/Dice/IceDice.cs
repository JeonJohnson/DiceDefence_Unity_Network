using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDice : Dice
{
	public override void Attack()
	{
		base.Attack();

		GameObject bullet = PoolingManager.Instance.LentalObj("Bullet");
		bullet.transform.position = transform.position;
		bullet.transform.rotation = Quaternion.identity;

		Bullet bulletScript = bullet.GetComponent<Bullet>();

		bulletScript.target = targetObj;
		bulletScript.atkType = status.atkType;

		bulletScript.isCrit = CritCheck();
		bulletScript.dmg = (int)(bulletScript.isCrit ? status.dmg * 2 : status.dmg);
	}


	protected override IEnumerator AttackCoroutine()
	{
		yield return StartCoroutine(base.AttackCoroutine());

		while (!targetObj)
		{
			yield return null;
		}
		Attack();

		StartCoroutine(AttackCoroutine());
	}


	public override void Skill()
	{
		base.Skill();
	}

	protected override IEnumerator SkillCoroutine()
	{
		yield return StartCoroutine(base.SkillCoroutine());
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

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}
}
