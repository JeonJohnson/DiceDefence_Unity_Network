using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
	//제네뤽 클래스
	//C++의 템플릿 클래스 비슷한거임
	private static T instance = null;
	//public bool isDontDestory;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = InstantiateManager(false);
			}
			return instance;
		}
	}

	public static T InstantiateManager(bool isDontDestroy)
	{
		//1. 하이어라키 창에 있는지 확인.
		//2. 없으면 ManagerPrefabs 폴더안에 같은이름 프리팹 있는지 확인.
		//3. 없으면 새로 만듬.

		if (instance == null)
		{
			string managerName = typeof(T).Name;
			GameObject managerObj = GameObject.Find(managerName);

			if (!managerObj)
			{
				GameObject prefab = Resources.Load(Defines.managerPrfabFolderPath + typeof(T).Name) as GameObject;

				if (prefab)
				{
					managerObj = Instantiate(prefab);
					managerObj.name = managerObj.name.Replace("(Clone)", string.Empty);
				}
				else
				{
					managerObj = new GameObject(managerName);
				}
			}

			instance = managerObj.GetComponent<T>();

			if (!instance)
			{
				managerObj.AddComponent<T>();
			}

			//박스에 담아주기!
			GameObject boxObj = FindManagerBoxes(isDontDestroy);
			managerObj.transform.SetParent(boxObj.transform);
		}

		return instance;
	}


	public static void InstantiateManagerByPrefabPath(string prefabFolderPath, bool isDontDestroy = true)
	{
		//프리팹 있는거 확신 할 때.
		//앵간하면 InstantiateManager로 퉁치삼

		Debug.LogWarning("U should be use \"InstantiateManager\" Funcs instead of \"InstantiateManagerByPrefabPath\".");

		if (instance == null)
		{
			GameObject managerObj = GameObject.Find(typeof(T).Name);

			if (managerObj == null)
			{
				GameObject prefab = Resources.Load(prefabFolderPath + typeof(T).Name) as GameObject;

				if (prefab == null)
				{
					Debug.LogError(typeof(T).Name + "'s prefab is not exist");

					managerObj = new GameObject(typeof(T).Name);
					instance = managerObj.AddComponent<T>();
				}
				else
				{
					managerObj = Instantiate(prefab);
					instance = managerObj.GetComponent<T>();

					if (instance == null)
					{
						Debug.LogError(typeof(T).Name + "'s prefab don't include" + typeof(T).Name + " Script!!");
					}
				}

				managerObj.name = managerObj.name.Replace("(Clone)", string.Empty);
			}

			GameObject boxObj = FindManagerBoxes(isDontDestroy);


			if (boxObj != null)
			{
				managerObj.transform.SetParent(boxObj.transform);
			}

		}
		else { Debug.LogError($"응~~~ {typeof(T).Name}이미 있어~~"); }
	}

	public static void InstantiateManagerByPrefab(GameObject prefab, bool isDontDestroy = true)
	{
		//프리팹 있는거 확신 할 때.
		//앵간하면 InstantiateManager로 퉁치삼

		Debug.LogWarning("U should be use \"InstantiateManager\" Funcs instead of \"InstantiateManagerByPrefab\".");

		if (prefab.name != typeof(T).Name)
		{
			Debug.LogError("매니저 프리팹이랑 실행시키는 곳이랑 다른데?? 확인해확인해확인해확인해");
		}

		GameObject newObj = null;

		if (instance == null)
		{
			newObj = Instantiate(prefab);
			newObj.name = newObj.name.Replace("(Clone)", string.Empty);
			instance = newObj.GetComponent<T>();

			GameObject boxObj = FindManagerBoxes(isDontDestroy);

			if (boxObj != null)
			{
				newObj.transform.SetParent(boxObj.transform);
			}

			if (instance == null)
			{
				Debug.LogError(typeof(T).Name + "'s prefab don't include" + typeof(T).Name + " Script!!");
			}
		}
		else
		{
			Debug.Log($"이미 {typeof(T).Name}있는디?\n확인해확인해확인해");
		}
	}


	public static GameObject FindManagerBoxes(bool isDontDestroy)
	{
		GameObject boxObj = null;

		if (isDontDestroy)
		{
			boxObj = Funcs.CheckGameObjectExist("ManagerBox");
			DontDestroyOnLoad(boxObj);
		}
		else
		{
			boxObj = Funcs.CheckGameObjectExist("ManagerBox_Destory");
		}

		return boxObj;
	}

	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneChanged;

	}

	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneChanged;

	}

	public virtual void OnSceneChanged(Scene scene, LoadSceneMode mode)
	{ }



}
