//using System.Collections;
//using System.Collections.Generic;
using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public static class Funcs
{
	public static string GetEnumName<T>(int index) where T : struct, IConvertible
	{//where 조건 struct, IConvertible => Enum으로 제한
		return Enum.GetName(typeof(T), index);
	}

	public static string TrimUnderBar(string str)
	{
		return str.Replace('_', ' '); 
	}

	public static string ExpandUnderBar(string str)
	{
		return str.Replace(' ', '_');
	}


	public static int B2I(bool boolean)
    {
		//false => 값 무 (0)
		//true => 값 유 
        return Convert.ToInt32(boolean);
    }

	public static bool I2B(int integer)
	{
		return Convert.ToBoolean(integer);
	}

	public static bool IntegerRandomCheck(int percent)
	{
		int rand = UnityEngine.Random.Range(1, 101);

		if (rand > percent)
		{
			return false;
		}

		return true;
	}


	public static int DontOverlapRand(int curNum, int min, int ExclusiveMax)
	{
		int iRand;

		while (true)
		{
			iRand = UnityEngine.Random.Range(min, ExclusiveMax);

			if (iRand != curNum)
			{
				break;
			}
		}

		return iRand;
	}


	public static List<T> Shuffle<T>(List<T> list)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
			int rnd = random.Next(0, i);
			T temp = list[i];
			list[i] = list[rnd];
			list[rnd] = temp;
		}
		return list;
	}
	public static Vector3 Random(Vector3 min, Vector3 max)
	{
		float x = UnityEngine.Random.Range(min.x, max.x);
		float y = UnityEngine.Random.Range(min.y, max.y);
		float z = UnityEngine.Random.Range(min.z, max.z);

		return new Vector3(x, y, z);
	}

	public static Vector3 Vec3_Random(float min, float max)
	{
		float x = UnityEngine.Random.Range(min, max);
		float y = UnityEngine.Random.Range(min, max);
		float z = UnityEngine.Random.Range(min, max);

		return new Vector3(x, y, z);
	}

	public static Structs.RayResult RayToWorld(Vector2 screenPos)
	{
		//이걸 그냥 충돌한 놈이 그라운드 일때만 리턴하게?
		//아니면 소환하는 곳에서 충돌된 놈이 그라운드가 아니면 그 새기 크기 판단해서 옆에 생성되게?

		Structs.RayResult rayResult = new Structs.RayResult();

		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		RaycastHit castHit;

		if (Physics.Raycast(ray, out castHit))
		{
			rayResult.hitPosition = castHit.point;
			rayResult.hitPosition.y = 0f;
			rayResult.hitObj = castHit.transform.gameObject;
			rayResult.isHit = true;
			rayResult.ray = ray;
			rayResult.rayHit = castHit;
		}
		else
		{
			rayResult.isHit = false;
		}

		return rayResult;
	}

	public static void ChangeMesh(GameObject origin, Mesh mesh)
	{
		MeshFilter tempFilter = origin.GetComponent<MeshFilter>();

		if (tempFilter != null)
		{
			tempFilter.mesh = mesh;
		}
	}



	public static GameObject FindGameObjectInChildrenByName(GameObject Parent, string ObjName)
	{
		if (Parent == null)
		{
			return null;
		}

		//그냥 transform.Find 로 찾으면 한 단계 아래 자식들만 확인함.
		int childrenCount = Parent.transform.childCount;

		GameObject[] findObjs = new GameObject[childrenCount];

		if (Parent.name == ObjName)
		{
			return Parent;
		}

		if (childrenCount == 0)
		{
			return null;
		}
		else
		{
			for (int i = 0; i < childrenCount; ++i)
			{
				findObjs[i] = FindGameObjectInChildrenByName(Parent.transform.GetChild(i).gameObject, ObjName);

				if (findObjs[i] != null && findObjs[i].name == ObjName)
				{
					return findObjs[i];
				}
			}

			return null;
		}
	}

	public static GameObject FindGameObjectInChildrenByTag(GameObject Parent, string ObjTag)
	{
		if (Parent == null)
		{
			return null;
		}

		int childrenCount = Parent.transform.childCount;

		GameObject[] findObjs = new GameObject[childrenCount];

		if (Parent.CompareTag(ObjTag))
		{
			return Parent;
		}

		if (childrenCount == 0)
		{
			return null;
		}
		else
		{
			for (int i = 0; i < childrenCount; ++i)
			{
				findObjs[i] = FindGameObjectInChildrenByTag(Parent.transform.GetChild(i).gameObject, ObjTag);

				if (findObjs[i] != null && findObjs[i].CompareTag(ObjTag))
				{
					return findObjs[i];
				}
			}
			return null;
		}
	}
	public static T FindComponentInNearestParent<T>(Transform curTransform) where T : Component
	{
		if (curTransform == null)
		{
			return null;
		}

		T tempComponent = curTransform.GetComponent<T>();

		if (tempComponent == null)
		{
			if (curTransform.parent != null)
			{
				tempComponent = FindComponentInNearestParent<T>(curTransform.parent);
			}
			else
			{
				return null;
			}
		}

		return tempComponent;
	}


	public static void RagdollObjTransformSetting(Transform originObj, Transform ragdollObj)
	{
		for (int i = 0; i < originObj.childCount; ++i)
		{
			if (originObj.childCount != 0)
			{
				RagdollObjTransformSetting(originObj.GetChild(i), ragdollObj.GetChild(i));
			}

			ragdollObj.GetChild(i).localPosition = originObj.GetChild(i).localPosition;
			ragdollObj.GetChild(i).localRotation = originObj.GetChild(i).localRotation;
		}
	}

	

	public static bool IsAnimationAlmostFinish(Animator animCtrl, string animationName, float ratio = 0.95f )
	{
		if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
		{//여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
			if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsAnimationCompletelyFinish(Animator animCtrl, string animationName, float ratio = 1.0f)
	{
		if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
		{//여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
			if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
			{
				return true;
			}
		}
		return false;
	}

	public static T FindResourceFile<T>(string path) where T : UnityEngine.Object
	{
		T temp = Resources.Load<T>(path);
		
		if (temp == null)
		{
			Debug.Log(path + "\nhas not exist!");
		}

		return temp;
	}

	public static GameObject CheckGameObjectExist(string name)
	{
		GameObject temp = GameObject.Find(name);

		if (temp == null)
		{
			temp = new GameObject(name);
		}

		return temp;
	}

	public static GameObject CheckGameObjectExist<T>(string objName) where T : Component
	{
		GameObject tempObj = GameObject.Find(objName);

		if (tempObj == null)
		{
			tempObj = new GameObject(objName);
		}

		T tempComponent = tempObj.GetComponent<T>();

		if (tempComponent == null)
		{
			tempObj.AddComponent<T>();
		}

		return tempObj;
	}

	public static T CheckComponentExist<T>(string gameObjectName) where T : Component
	{
		GameObject temp = GameObject.Find(gameObjectName);

		if (temp == null)
		{
			temp = new GameObject(gameObjectName);
			//temp.name = gameObjectName;
		}

		T tempComponent = temp.GetComponent<T>();

		if (tempComponent == null)
		{
			tempComponent = temp.AddComponent<T>();
		}

		return tempComponent;
	}


	public static string ReadTextFile(string filePath)
	{ //StreamingAssets/리소스내의 폴더 위치 적어주면 댐.
	  //유니티가 최종 실행파일을 빌드할때 Resources 내의 모든 파일들을 바이너리화 해서 뽑지 않음.
	  //사용되는것들(ex. 프리팹,모델,스크립트,Resources.load등으로 불러오는것만)만 뽑음
	  //StreamingAssets폴더내의 있는 파일들은 다 뽑아내기때문에
	  //유니티에서 지원하는?? 파일형식 제외하고는 저기 넣어야 안정적이겠즤

		//UnityEngine.Networking. UnityWebRequest 를 이용하여 안드로이드, PC모두 읽어올 수 있도록
		string androidPath = Application.streamingAssetsPath + "/" + filePath + ".txt";

		UnityWebRequest uwrFile = UnityWebRequest.Get(androidPath);
		uwrFile.SendWebRequest();
		while (!uwrFile.isDone)
		{
			//파일 읽어올때까지 대기하기 이거 비동기방식임.	
		}
		string str = "";
		// str = uwrFile.downloadHandler.text;
		if (uwrFile.result == UnityWebRequest.Result.Success)
		{
			str = uwrFile.downloadHandler.text;
		}
		else/*(uwrFile.result == UnityWebRequest.Result.ProtocolError)*/
		{
			Debug.Log(filePath + "에는 파일이 없읍니다. 확인요망");
			str = "TextFile has no exist";
		}
		return str;
	}


	public static Color SetAlpha(Color color, float alpha)
	{
		Color tempColor = color;
		tempColor.a = alpha;
		return tempColor;
	}

	public static void SetAlpha(ref Color color, float alpha)
	{
		Color tempColor = color;
		tempColor.a = alpha;
		color = tempColor;
	}
}

public static class Defines
{
	public const int right = 1;
	public const int left = 0;

	public const int ally = 0;
	public const int enemy = 1;

	public const float winCX = 1600f;
	public const float winCY = 900f;

	public const float gravity = -9.8f;

	public const float PI = 3.14159265f;

	public const float diceDefaultScale = 1f;

	public static string managerPrfabFolderPath = "Prefabs/Managers/";

	public static bool DESTROY = false;
	public static bool DONT_DESTROY = true;

	public static Vector3[] test =
	{
		new Vector3(0f,0f,0f)
	};

}

namespace Enums
{
	public enum eScenes
	{ 
		//Intro,
		Title,
		//Menu,
		InGame,
		End
	}

	public enum ePoolingObj
	{ 
		End
	
	}

	public enum eDiceName
	{ 
		Default,
		Electric,
		//Ice,
		Iron,
		//Poison,
		Wind,
		End
	}

	public enum eTargetPriority
	{ 
		Front,
		Rear,
		Close,
		Random,
		MaxHpAmount,
		MinHpAmount,
		CurHp,
		End
	}

	public enum eAttackType
	{ 
		Default =1,
		Splash = 2, //스플뎀
		Range = 4, //범위 공격(설치형 등)
		Penetrate = 6, //관통형 (레이져)

	}
	

	public enum eBuff
	{ 
		Dmg = 2,
		AttackSpd = 4,

		//De_ 붙은건 디버프들
		De_Dmg = 8,
		De_AttackSpd = 16,
		De_Lock = 32,
		De_Defense = 64,
		De_Slow = 128,
	}

	//public enum eDebuff
	//{ 
	//	DecreaseDmg = 2,
	//	DecreaseSpd = 4,
	//	Lock = 6,
	//	DecreaseDefense = 8,
	//	Slow = 10,
	//}

}

namespace Structs
{
	[System.Serializable]
	public struct RayResult
	{
		public bool isHit;
		public Vector3 hitPosition;
		public GameObject hitObj;
		public Ray ray;
		public RaycastHit rayHit;
	}
	[System.Serializable]
	public struct Buff
	{
		public Buff(int buffBit)
		{
			buffFlag = (Enums.eBuff)(1 << buffBit);
		}

		public Buff(Enums.eBuff eBuffBit)
		{
			buffFlag = (Enums.eBuff)(1 << (int)eBuffBit);
		}

		public static Buff operator +(Buff b1, Buff b2)
		{
			//버프 추가하기
			return new Buff(b1.buffFlag | b2.buffFlag);
		}

		public static Buff operator +(Buff b1, Enums.eBuff b2)
		{
			//버프 추가하기
			return new Buff(b1.buffFlag | b2);
		}

		public static Buff operator -(Buff b1, Buff b2)
		{
			return new Buff(b1.buffFlag ^ b2.buffFlag);
		}

		public static Buff operator -(Buff b1, Enums.eBuff b2)
		{
			return new Buff(b1.buffFlag ^ b2);
		}

		public static bool operator & (Buff b1, Enums.eBuff flag)
		{
			if ((b1.buffFlag & flag) == 0)
			{
				return false;
			}
			else { return true; }
		}

		//public static bool operator !=(Buff b1, Buff b2)
		//{
		//	if ((b1.bitMask & b2.bitMask) == 0)
		//	{
		//		return true;
		//	}
		//	else { return false; }
		//}


		//public override bool Equals(object obj)
		//{//== 를 연산자 오버라이딩 하기 위해서는 이렇게 기본 Equals 재정의 해야함.
		//	return obj is Buff buff &&
		//		   bitMask == buff.bitMask;
		//}
		[EnumFlags]
		public Enums.eBuff buffFlag;

		public void SetBuff(Enums.eBuff bit)
		{
			buffFlag = bit;
		}

		public void AddBuff(Enums.eBuff bit)
		{
			buffFlag = buffFlag | bit;
		}

		public void RemoveBuff(Enums.eBuff bit)
		{
			buffFlag = buffFlag ^ bit;
		}

	}

	[System.Serializable]
	public struct PlayerStatus
	{
		public int maxHp;
		public int curHp;


		public int gold; //로비에서쓸것

		public float Sp;

		public float SpSpd;

		public float dicePrice;
		public float dicePriceInc;
	}

	[System.Serializable]
	public struct Index
	{
		public void SetIndex(int x, int y)
		{
			row = x;
			col = y;
		}
		int row;
		int col;
	}

	//[System.Serializable]
	//public struct DiceStatus
	//{
	//	public Enums.eDiceName eName;
	//	public string strName;
	//	public int spot;

	//	public Index index;

	//	public Enums.eTargetPriority targetPriority;

	//	public Enums.eAttackType atkType;
	//	public Buff atkEffect;

	//	public float dmg;
	//	//public float dmgIncre;

	//	[Range(0, 100f)]
	//	public float critialPercent;

	//	public float atkMaxTime;
	//	public float atkCurTime;
		
	//	public float coolMaxTime; //스킬 쿨타임
	//	public float coolCurTime;

	//	public float skillMaxDuration; //스킬 지속시간
	//	public float skillCurDuration; //스킬 지속시간

	//	public Buff buff;//현재 걸린거
	//}
	
	[System.Serializable]
	public struct EnemyStatus
	{
		public float maxHp;
		public float curHp;

		public float moveSpd;

		public float coolMaxTime;
		public float coolCurTime;

		public Buff buff;
	}

	

	[System.Serializable]
	public struct DamagedStruct
	{
		public int dmg;
		public Enums.eAttackType type;
		public float tickTime;
		public bool isCrit;
		//public Vector3 explosionPos;
		//public Vector3 dmgDir;
		//public Vector3 hitWordPosition;

		//public bool isDebug;
	}


}

