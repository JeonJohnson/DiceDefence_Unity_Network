using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using Enums;

public class PoolingManager: Manager<PoolingManager>
{

    public GameObject[] prefabs; //프리팹들 담아둘곳

	GameObject[] objBoxes; //각 오브젝트 담아 놓을 박스
    //빈 게임오브젝트, 인스펙터창에 실제로 만들꺼임
    //그니까 각 오브젝트 최상위 EmptyGameObject


    public Dictionary<string, Queue<GameObject>> poolingObjDic;


	public void CreateBoxes()
	{
		objBoxes = new GameObject[prefabs.Length];

		for(int i = 0; i<prefabs.Length; ++i)
		{
			if (prefabs[i] == null)
			{
				continue;
			}

			GameObject box = new GameObject(prefabs[i].name + "_Box");
			box.transform.SetParent(this.gameObject.transform);
			objBoxes[i] = box;
		}
	}

	public void FillAllObjects(int iCount = 50)
	{
		poolingObjDic = new Dictionary<string, Queue<GameObject>>();

//		foreach (var prefab in prefabs)
		for(int i = 0; i<prefabs.Length; ++i)
		{
			if (prefabs[i] == null)
			{
				continue;
			}

			GameObject prefab = prefabs[i];

			Queue<GameObject> tempQueue = new Queue<GameObject>();

			for (int k = 0; k < iCount; ++k)
			{
				GameObject tempObj = Instantiate(prefab, objBoxes[i].transform);
				tempObj.name = tempObj.name.Replace("(Clone)", string.Empty);
				tempObj.SetActive(false);
				tempQueue.Enqueue(tempObj);
			}

			poolingObjDic.Add(prefab.name, tempQueue);
		}
	}

	public void FillObject(string objName, int count)
	{
		var tempPair = poolingObjDic.FirstOrDefault(t => t.Key == objName);

		//List<GameObject> tempList = prefabs.ToList();
		//GameObject prefab = tempList.Find(x => x.name == objName);
		string boxName = objName + "_Box";
		for (int i = 0; i < objBoxes.Length; ++i)
		{
			if (objBoxes[i] == null)
			{
				continue;
			}

			if (objBoxes[i].name.Equals(boxName))
			{
				GameObject newObj = Instantiate(prefabs[i], objBoxes[i].transform);
				newObj.name = newObj.name.Replace("(Clone)", string.Empty);
				newObj.SetActive(false);
				tempPair.Value.Enqueue(newObj);
			}
		}
	}

	public GameObject LentalObj(string objName, int count = 1)
	{
		//디스이즈 람다식
		var tempPair = poolingObjDic.FirstOrDefault(t => t.Key == objName);
		if (tempPair.Value.Count < count)
		{
			FillObject(objName, count * 2);
			return LentalObj(objName, count);
		}
		else 
		{
			GameObject tempObj = tempPair.Value.Dequeue();
			tempObj.SetActive(true);
			tempObj.transform.SetParent(null);
			return tempObj;
		}
	}

	public void ReturnObj(GameObject obj)
	{
		//무~~주건 밖에서 리지드바디나 뭐 트랜스폼들 초기화 시키고 보내셈
		//아니면 반품처리함

		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
		//obj.transform.localScale = new Vector3(1f, 1f, 1f);
		
		string realName = obj.name.Replace("(Clone)", string.Empty);
		var tempPair = poolingObjDic.FirstOrDefault(t => t.Key == realName);
		string boxName = realName + "_Box";

		for (int i = 0; i < objBoxes.Length; ++i)
		{
			if (objBoxes[i] == null)
			{
				continue;
			}

			if (objBoxes[i].name.Equals(boxName))
			{
				obj.transform.SetParent(objBoxes[i].transform);
				obj.SetActive(false);
				tempPair.Value.Enqueue(obj);
			}
		}
	}

	void Awake()
	{
        CreateBoxes();
		FillAllObjects();

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
