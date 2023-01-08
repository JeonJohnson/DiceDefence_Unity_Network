using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyDice : MonoBehaviour
{
    public Material mat;
	public Dice originDice;


	
	public AudioClip mergeClip;

	public void Select(Texture diceTex, Texture spotTex, Color spotColor, Dice origin)
	{
		mat.SetTexture("_MainTex", diceTex);
		mat.SetTexture("_SubTex", spotTex);
		mat.SetColor("_Color", spotColor);

		mat.SetFloat("_Alpha", 0.5f);

		mat.renderQueue = 3000;

		originDice = origin;
	}



	private void Awake()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mat = Instantiate(mr.material);
		mr.material = mat;

		mat.SetFloat("_Alpha", 1f);

		
	}


	private void Start()
	{
			
	}


	// Update is called once per frame
	void Update()
    {

        Vector3 mousePos = Input.mousePosition;
		//mousePos.z = Camera.main.nearClipPlane;
		mousePos.z = Camera.main.transform.position.y - 1;
		//Debug.Log("MousePos : " + mousePos);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		//Debug.Log("WorldPos : " + worldPos);
		transform.position = worldPos;


		if (Input.GetMouseButtonUp(0))
		{
			if (ObjectManager.Instance.mouseOnDice 
				&& (ObjectManager.Instance.mouseOnDice.status.eName == originDice.status.eName)
				&&(ObjectManager.Instance.mouseOnDice.status.spot == originDice.status.spot)
				&& (ObjectManager.Instance.mouseOnDice != originDice))
			{

				originDice.ReturnPool();
				ObjectManager.Instance.mouseOnDice.Merge();
				ObjectManager.Instance.SelectDice(null);
				ObjectManager.Instance.playerScript.auds.PlayOneShot(mergeClip);
			}
			else
			{
				originDice.SetAlpha(1f);
				ObjectManager.Instance.SelectDice(null);
			}

			PoolingManager.Instance.ReturnObj(gameObject);
		}

    }
}
