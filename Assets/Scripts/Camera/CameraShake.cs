using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;
using CamShakeClass;

//namespace CamShakeStruct
//{
//	public class CamShakeWave
//	{
//		public float amplitude = 0f; //진폭(세기)
//		public float frequency = 0f; //주파수(횟수)
//		public float offset = 0f; //사인그래프에서 시작 위치
//		public float additionalOffset = 0f; //사인그래프 자체 오프셋, y축 기준치 
//									   //위쪽으로만 올라가거나 밑으로만 내려가야하는 쉐이킹일 때
//	}

//	public class CamShakeEvent
//	{
//		public	CamShakeEvent()
//			pitchWave = new CamShakeWave();
//		
//			yawWave = new CamShakeWave();
//			rollWave = new CamShakeWave();

//			xWave = new CamShakeWave();
//			yWave = new CamShakeWave();
//			zWave = new CamShakeWave();
//		}

//		public void CalcDistanceRate(Vector3 camWorldPos)
//		{
//			float dist = Vector3.Distance(eventPos, camWorldPos);

//			if (dist >
//			erRadius)
//			{
//				distanceRate = 0f;
//			}
//			else if (dist > innerRadius)
//			{
//				distanceRate = 1f - ((dist - innerRadius) / (outerRadius - innerRadius));
//			}
//			else 
//			{
//				distanceRate = 1f; 
//			}
//		}

//		public float AdvanceSinWave(CamShakeWave wave)
//		{
//			if (wave.amplitude == 0f)
//			{
//				return 0f;
//			}

//			float sinFrequency = 2f * 3.14f * wave.frequency;
//			float curTimeLine = sinFrequency * (timer + wave.offset);

//			return (Mathf.Sin(curTimeLine) * wave.amplitude + wave.additionalOffset) * amplitudeRate;

//		}

//		public float timer = 0f; //ElapsedTime

//		public float duration = 0.5f; //시간 
//		public float blendInTime = 0f; //FadeIn Time 
//		public float blendOutTime = 0f; //FadeOut Time

//		public float ShakeSpd = 1f;

//		public float innerRadius = 5f; // 쉐이크의 감소가 시작되는 영향 범위
//		public float outerRadius = 10f; // 최대 영향 범위 (이 이후로는 영향을 안받음)
//		public float distanceRate = 1f; // 

//		public float amplitudeRate = 1f; //세기의 배율

//		/* Roate Shake */
//		public CamShakeWave pitchWave = null;
//		public CamShakeWave yawWave = null;
//		public CamShakeWave rollWave = null;

//		/* Move Shake */
//		public CamShakeWave xWave = null;
//		public CamShakeWave yWave = null;
//		public CamShakeWave zWave = null;

//		public Vector3 eventPos;//발생할 지점.
//	}

//}

public class CameraShake : Manager<CameraShake>
{

	Vector3 initLocalPos;
	Quaternion initLocalRot;

	public List<CameraShakeEvent> shakeEvents;

	public void Shake()
	{
		if (shakeEvents.Count == 0)
		{
			Camera.main.transform.localPosition = initLocalPos;
			Camera.main.transform.localRotation = initLocalRot;
			
			return;
		}

		Vector3 accPos = new Vector3();
		Vector3 accRot = new Vector3();
		Vector3 camPos = Camera.main.transform.position;

		int iCount = shakeEvents.Count;

		for (int i = 0; i < iCount; ++i)
		{
			shakeEvents[i].desc.timer += Time.deltaTime * shakeEvents[i].desc.ShakeSpd;

			shakeEvents[i].desc.CalcDistanceRate(camPos);

			if (shakeEvents[i].desc.timer < shakeEvents[i].desc.blendInTime)
			{
				shakeEvents[i].desc.amplitudeRate = shakeEvents[i].desc.timer / shakeEvents[i].desc.blendInTime;
			}
			else if (shakeEvents[i].desc.timer > shakeEvents[i].desc.duration - shakeEvents[i].desc.blendOutTime)
			{
				if (shakeEvents[i].desc.blendOutTime != 0f)
				{
					shakeEvents[i].desc.amplitudeRate = (shakeEvents[i].desc.duration - shakeEvents[i].desc.timer) / shakeEvents[i].desc.blendOutTime;
				}
				else
				{
					shakeEvents[i].desc.amplitudeRate = 0f;
				}
			}
			else
			{
				shakeEvents[i].desc.amplitudeRate = 1f;
			}

			shakeEvents[i].desc.amplitudeRate *= shakeEvents[i].desc.distanceRate;

			accRot.x += shakeEvents[i].desc.AdvanceSinWave(shakeEvents[i].pitchWave);
			accRot.y += shakeEvents[i].desc.AdvanceSinWave(shakeEvents[i].yawWave);
			accRot.z += shakeEvents[i].desc.AdvanceSinWave(shakeEvents[i].rollWave);

			accPos.x += shakeEvents[i].desc.AdvanceSinWave(shakeEvents[i].xWave);
			accPos.y += shakeEvents[i].desc.AdvanceSinWave(shakeEvents[i].yWave);
			accPos.z += shakeEvents[i].desc.AdvanceSinWave(shakeEvents[i].zWave);

			if (shakeEvents[i].desc.timer > shakeEvents[i].desc.duration)
			{
				//c#에는 이터레이터가.. 읎네..?
				shakeEvents.Remove(shakeEvents[i]);
				--iCount;
			}
		}

		Camera.main.transform.Rotate(new Vector3(accRot.x, accRot.y, accRot.z));
		Camera.main.transform.localPosition += accPos;
	}

	public void PlayShake(CameraShakeEvent shakeEvent, Vector3 shakePos)
	{
		shakeEvent.desc.eventPos = shakePos;
		shakeEvent.desc.timer = 0f;
		shakeEvents.Add(shakeEvent);
	}

	public void PlayShake(/*CameraShakeEvent shakeEvent, */GameObject shakeObj)
	{
		CameraShakeEvent shakeEvent = shakeObj.GetComponent<CameraShakeEvent>();

		if (shakeEvent == null)
		{
			return;
		}
		shakeEvent.desc.eventPos = shakeObj.transform.position;
		shakeEvent.desc.timer = 0f;
		shakeEvents.Add(shakeEvent);
	}

	public void StopAllShake()
	{
		shakeEvents.Clear();
	}
	
	
	public void Awake()
	{
		shakeEvents = new List<CameraShakeEvent>();
	}

    void Start()
    {
		initLocalPos = Camera.main.transform.localPosition;
		initLocalRot = Camera.main.transform.localRotation;
	 }

    // Update is called once per frame
    void Update()
    {
		Shake();

    }
}
