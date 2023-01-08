using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    public Canvas canvas;
    public Camera targetCam;

	private void Awake()
	{
        targetCam = Camera.main;
        canvas.worldCamera = targetCam;
	}

	// Update is called once per frame
	void Update()
    {
        if (targetCam != null)
        { 
            transform.LookAt(transform.position + targetCam.transform.forward);
        }
    }
}
