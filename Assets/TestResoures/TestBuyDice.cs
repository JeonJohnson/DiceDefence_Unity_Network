using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuyDice : MonoBehaviour
{

    ObjectManager objectManager; 
    public GameObject dicePrefab;
    public void TestDiceSpawn()
    {
        //int rowRandom = Random.Range(0, objectManager.boardScript.initRowCount);
        //int colRandom = Random.Range(0, objectManager.boardScript.initColCount);

        //if (objectManager.diceList.Count >= 12)
        //{
        //    return;
        //}

        //Vector3 pos = objectManager.boardScript.slots[rowRandom, colRandom].transform.position;
        //pos.y = 0.5f;
        //GameObject tempDice = Instantiate(dicePrefab, pos, Quaternion.identity);
        //int iCount = objectManager.boardScript.slots[rowRandom, colRandom].transform.childCount;

        //if (iCount == 0)
        //{
        //    tempDice.transform.SetParent(objectManager.boardScript.slots[rowRandom, colRandom].transform);
        //    objectManager.diceList.Add(tempDice);
        //}
        //else
        //{
        //    TestDiceSpawn();
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        objectManager = ObjectManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
