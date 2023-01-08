using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CamShakeClass;

namespace CamShakeClass
{
	//구조체나 클래스 요거 해주면 inspector창에서도 멤버변수들 나옴 껄껄
	[System.Serializable]
	public class CamShakeWave
	{
		public float amplitude; //진폭(세기)
		public float frequency; //주파수(횟수)
		public float offset; //사인그래프에서 시작 위치
		public float additionalOffset; //사인그래프 자체 오프셋, y축 기준치 
									   //위쪽으로만 올라가거나 밑으로만 내려가야하는 쉐이킹일 때
	}

	[System.Serializable]
	public class CamShakeDesc
	{
		public Vector3 eventPos;//발생할 지점.

		public float timer = 0f; //ElapsedTime

		public float duration = 0.5f; //시간 
		public float blendInTime = 0f; //FadeIn Time 
		public float blendOutTime = 2.5f; //FadeOut Time

		public float ShakeSpd = 1f;

		public float innerRadius = 5f; // 쉐이크의 감소가 시작되는 영향 범위
		public float outerRadius = 10f; // 최대 영향 범위 (이 너머로는 영향을 안받음)
		public float distanceRate = 1f; // 거리 배율

		public float amplitudeRate = 1f; //세기의 배율

		public void CalcDistanceRate(Vector3 camWorldPos)
		{
			//카메라 위치하고 쉐이킹 진원지하고 거리 계산

			float dist = Vector3.Distance(eventPos, camWorldPos);

			if (dist > outerRadius)
			{
				distanceRate = 0f;
			}
			else if (dist > innerRadius)
			{
				distanceRate = 1f - ((dist - innerRadius) / (outerRadius - innerRadius));
			}
			else
			{
				distanceRate = 1f;
			}
		}

		public float AdvanceSinWave(CamShakeWave wave)
		{
			if (wave.amplitude == 0f)
			{
				return 0f;
			}

			
			float sinFrequency = 2f * Mathf.PI * wave.frequency;
			float curTimeLine = sinFrequency * (timer + wave.offset);

			return (Mathf.Sin(curTimeLine) * wave.amplitude + wave.additionalOffset) * amplitudeRate;

		}

	}
}


public class CameraShakeEvent : MonoBehaviour
{

	public CamShakeDesc desc = new CamShakeDesc();

	/* Roate Shake */
	public CamShakeWave pitchWave = new CamShakeWave();
	public CamShakeWave yawWave = new CamShakeWave();
	public CamShakeWave rollWave = new CamShakeWave();

	/* Move Shake */
	public CamShakeWave xWave = new CamShakeWave();
	public CamShakeWave yWave = new CamShakeWave();
	public CamShakeWave zWave = new CamShakeWave();


	//public void Awake()
	//{
	//	//desc = new CamShakeDesc();

	//	//pitchWave = new CamShakeWave();
	//	//yawWave = new CamShakeWave();
	//	//rollWave = new CamShakeWave();

	//	//xWave = new CamShakeWave();
	//	//yWave = new CamShakeWave();
	//	//zWave = new CamShakeWave();
	//}

	//public void Start()
	//{
		
	//}

	//public void Update()
	//{
		
	//}



}




