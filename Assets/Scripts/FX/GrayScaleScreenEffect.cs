using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScaleScreenEffect : MonoBehaviour
{
    public Material mat;


	public void Awake()
	{
        mat.SetFloat("_GrayScaleAmount", 0f);

    }

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //기본 렌더링들이 끝나고 렌더타겟들에 나갔을 때
        //우리가 만든 메테리얼을 가지고 렌더텍스쳐 건드리는거임.
        
        Graphics.Blit(source, destination, mat);
    }

}
