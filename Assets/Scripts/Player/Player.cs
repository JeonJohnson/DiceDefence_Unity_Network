using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Enums;
using Structs;

public class Player : MonoBehaviour
{
    public PlayerStatus status;

    public Dice[] diceDeck = new Dice[4];

    public AudioSource auds;
    public AudioClip looseClip;

    public AudioClip diceBuyBtn;

    public bool isDead;
    
	public void Hit()
	{
        --status.curHp;

        if (!isDead)
        {
            if (status.curHp <= 0)
            {
                isDead = true;
                status.curHp = 0;
                auds.PlayOneShot(looseClip);
                InGameManager.Instance.GameLoose();
            }
        }
	}

	public void GetSPAuto()
    {
        if(!InGameManager.Instance.isGameLoose)
        status.Sp += Time.deltaTime * status.SpSpd;
        //UIManager.Instance.UpdateSpText(status.Sp);
    }

	private void Awake()
	{
        auds = GetComponent<AudioSource>();
        isDead = false;

    }
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetSPAuto();
        InGameUIManager.Instance.UpdateSpText(status.Sp);
        InGameUIManager.Instance.diceBuyBtn.UpdateDicePriceText(status.dicePrice);

        InGameUIManager.Instance.UpdateHpText(status.curHp, status.maxHp);
    }
}
