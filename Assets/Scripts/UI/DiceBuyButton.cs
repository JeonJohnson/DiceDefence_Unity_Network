using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DiceBuyButton : MonoBehaviour
{
    public Image clickEffectImg;
    public float effectSpd;

    public TextMeshProUGUI priceTxt;


    IEnumerator ClickEffectCoroutine()
    {
        Color setColor = Color.white;
        clickEffectImg.color = setColor;
        
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * effectSpd;

            setColor.a = alpha;
            clickEffectImg.color = setColor;

            yield return null;
        }
    }

    public void OnBuyDiceButtonClick()
    {
        Player script = ObjectManager.Instance.playerScript;
        script.auds.PlayOneShot(script.diceBuyBtn);

        if (script.status.Sp >= script.status.dicePrice && ObjectManager.Instance.diceList.Count < 12)
        {
            StopCoroutine(ClickEffectCoroutine());
            StartCoroutine(ClickEffectCoroutine());

            script.status.Sp -= script.status.dicePrice;

            Enums.eDiceName randomName = (Enums.eDiceName)(Random.Range((int)Enums.eDiceName.Default, (int)Enums.eDiceName.End));
            ObjectManager.Instance.DiceSpawn(randomName);

            script.status.dicePrice += script.status.dicePriceInc;
            //UIManager.Instance.UpdateDicePrice(script.status.dicePrice);
        }
        else
        { 
        
        }
    }

    public void UpdateDicePriceText(float price)
    {
        priceTxt.text = $"SP : {(int)price}";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
