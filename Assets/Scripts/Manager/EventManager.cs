using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;

public class EventManager : Manager<EventManager>
{

    public void OnClickNextScene()
    {
        SceneManager.LoadScene(1);
    }
    
    

    public void OnBuyDiceButtonClick()
    {
        //Player script = ObjectManager.Instance.playerScript;

        //if (script.status.Sp >= script.status.dicePrice)
        //{
        //    script.status.Sp -= script.status.dicePrice;

        //    ObjectManager.Instance.TestDiceSpawn();

        //    script.status.dicePrice += 20f;
        //   //UIManager.Instance.UpdateDicePrice(script.status.dicePrice);
        //}
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
