using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBoard : MonoBehaviour
{
    public GameObject slotPrefab;
    
    public Transform SlotsBoxTr;
    public GameObject[,] slots;

    public GameObject plane;

    public GameObject refer3by4Slots;

    public int initRowCount;
    public int initColCount;

    private Vector3 firstPos;
    public Vector3 centerPos;

    public float xOffset;
    public float zOffset;

    private float fieldRatio_x;
    private float fieldRatio_z;
    public float fieldScale;

    private void InitializeBoard()
    {
        slots = new GameObject[initRowCount, initColCount];
        //3,4
        if (initRowCount == 3 && initColCount == 4)
        {
            refer3by4Slots.SetActive(true);
            SetReferSlots();
        }
        else
        {
            refer3by4Slots.SetActive(false);
            InstantiateBoard();
        }
    }

    private void SetReferSlots()
    {
        int childCount = refer3by4Slots.transform.childCount;

        int i = 0;
		for (int row = 0; row < initRowCount; ++row)
		{
			for (int col = 0; col < initColCount; ++col)
			{
				GameObject obj = refer3by4Slots.transform.GetChild(i).gameObject;
				slots[row, col] = obj;

				++i;
			}
		}
	}

    private void InstantiateBoard()
    {
        float xPosOffset = (initColCount * 0.5f - 0.5f);
        float zPosOffset = (initRowCount * 0.5f - 0.5f);
        firstPos = new Vector3();
        firstPos.x = centerPos.x - ((Defines.diceDefaultScale * xPosOffset) + ((xOffset * xPosOffset)));
        firstPos.z = centerPos.z + ((Defines.diceDefaultScale * zPosOffset) + ((zOffset * zPosOffset)));
        firstPos.y = centerPos.y - 0.4f;


        for (int i = 0; i < initRowCount; ++i)
        {
            for (int k = 0; k < initColCount; ++k)
            {
                slots[i, k] = Instantiate(slotPrefab);
                slots[i, k].name = slots[i, k].name.Replace("(Clone)", string.Empty);
                slots[i, k].name += $"({i},{k})";

                if (i == 0 && k == 0)
                {
                    slots[i, k].transform.position = firstPos;
                }
                float x = firstPos.x + (k * Defines.diceDefaultScale) + (k * xOffset);
                float y = firstPos.y;
                float z = firstPos.z - ((i * Defines.diceDefaultScale) + (i * zOffset));
                Vector3 pos = new Vector3(x, y, z);
                slots[i, k].transform.position = pos;

                slots[i, k].transform.SetParent(SlotsBoxTr);
            }
        }


		#region Set_Plane_Scale
		//fieldRatio_x = Mathf.Abs(slots[0, 0].transform.position.x) + Mathf.Abs(slots[0, initColCount - 1].transform.position.x) + Defines.diceDefaultScale;
		//fieldRatio_z = Mathf.Abs(slots[0, 0].transform.position.z) + Mathf.Abs(slots[0, initColCount - 1].transform.position.z) + Defines.diceDefaultScale;

		//Vector3 scale = new Vector3(fieldRatio_x * fieldScale, 1, fieldRatio_z * fieldScale);
		//plane.transform.localScale = scale;
		#endregion

	}

	public void Awake()
    {

        InitializeBoard();




    }
}
