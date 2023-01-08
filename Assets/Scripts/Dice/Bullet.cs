using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Enemy target;
    public Enums.eAttackType atkType;
    public bool isCrit;
    public int dmg;

    public Vector3 dir;
    [SerializeField] private float spd;

    AudioSource Ads;
    public AudioClip sound;
    
    
    public void PlaySound()
    {
        Ads.PlayOneShot(sound);
    }

    IEnumerator ReturnCoroutine()
    {
        yield return new WaitForSeconds(5f);
        PoolingManager.Instance.ReturnObj(gameObject);
    }

	private void Awake()
	{
        Ads = GetComponent<AudioSource>();
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target && target.enabled && !target.isDead)
        {
            dir = (target.centerTr.position - transform.position).normalized;
            transform.Translate(dir * Time.deltaTime * spd);
        }
        else
        {
            PoolingManager.Instance.ReturnObj(gameObject);
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject == target.gameObject)
        {
            Enemy script = other.GetComponent<Enemy>();

            Structs.DamagedStruct dmgSt = new Structs.DamagedStruct();
            dmgSt.dmg = dmg;
            dmgSt.type = atkType;
            dmgSt.isCrit = isCrit;

            script.Hit(dmgSt);

            Debug.Log(dmg);
            PoolingManager.Instance.ReturnObj(gameObject);
        }
	}

	private void OnEnable()
	{
        StartCoroutine(ReturnCoroutine());
	}

}

