using System.Collections;
using System.Collections.Generic;

using System.Linq;

using UnityEngine;

public class LightningBullet : MonoBehaviour
{
    public Enemy target;

    public bool isCrit;
    public int dmg;

    public int atkTargetCount;

    [SerializeField] private Vector3 dir;
    [SerializeField] private float spd;

    public ParticleSystem ps;

    public AudioSource sound;

    public void Fire()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);

        Vector3 localScale = transform.localScale;
        localScale.x *= dist;
        transform.localScale = localScale;

        dir = (target.centerTr.position - transform.position).normalized;
        transform.forward = dir;

        sound.Play();
        ps.Play();

        Structs.DamagedStruct temp = new Structs.DamagedStruct();
        temp.dmg = dmg;
        temp.isCrit = isCrit;
        target.Hit(temp);

        //가까운놈 찾기
        var list = ObjectManager.Instance.enemyList.ToList();

        if (list.Count == 1)
        {
            //맞는 놈이 한 마리 뿐일 때
            return;
        }
        else
        {
            list = list.OrderBy(x => Vector3.Distance(target.transform.position, x.transform.position)).ToList();
            //linq의 orderby 쓰면 원본은 그대로 있음. 새로운 리스트 만듦.

            
            //list.OrderBy(x => Vector3.Distance(target.transform.position, x.transform.position));
            list.Remove(target);
            list.RemoveAll(x => x.isDead);

            temp.dmg = dmg / 2;
            temp.isCrit = false;

            //정말 뚜디 맞을 놈만 남음
            int realCount = atkTargetCount;

			//if (realCount > list.Count)
			//{
			//	realCount = list.Count;
			//}
			realCount = atkTargetCount > list.Count ? list.Count : atkTargetCount;
			Debug.Log(realCount);

            for (int i = 0; i < realCount; ++i)
            {
                if (i >= list.Count)
                {
                    break;
                }
                list[i].Hit(temp);
            }

        }

    }

	void Update()
    {
        if (!ps.IsAlive() && !sound.isPlaying)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            PoolingManager.Instance.ReturnObj(this.gameObject);
        }
    }
}
