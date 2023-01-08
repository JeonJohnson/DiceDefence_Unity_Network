using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : Manager<CoroutineHelper>
{
    //모노비헤이비어 상속 안받는 클래스들이나
    //비활성화(disable된) 객체 코루틴 써야할때
    
    //=> 코루틴이 모노비헤이비어에서 굴러가기 때문에
    //비활성화되면 안굴러감

    private MonoBehaviour monoInstance = null;
    public new Coroutine StartCoroutine(IEnumerator coroutine)
    {
        //if (monoInstance == null)
        //{
        //    monoInstance = GetComponent<MonoBehaviour>();
        //}

        return monoInstance.StartCoroutine(coroutine);
    }

    public new void StopCoroutine(Coroutine coroutine)
    {
        //if (monoInstance == null)
        //{
        //    monoInstance = GetComponent<MonoBehaviour>();
        //}

        monoInstance.StopCoroutine(coroutine);
    }

	public void Awake()
	{
        if (monoInstance == null)
        {
            monoInstance = GetComponent<MonoBehaviour>();
        }
	}

}
