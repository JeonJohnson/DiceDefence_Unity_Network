using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TitleDiceSprite : MonoBehaviour
{
    RectTransform rectTr;

    public float moveSpd;
    public float rotSpd;

    public Vector3 goalPos;
    public Vector3 dir;
    public float dist;


    public void CheckBoundary()
    {
        Vector2 viewPos = Camera.main.ScreenToViewportPoint(rectTr.position);
        //Debug.Log(viewPos);

        //if (viewPos.x <= 0f || viewPos.x >= 1f || viewPos.y <= 0f || viewPos.y >= 1f)
        //{
        //    RandomGoalPos();
        //}

        if (dist <= 1f)
        {
            RandomGoalPos();
        }
    }

    public void RandomGoalPos()
    {
        //dir = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f);
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        goalPos = Camera.main.ViewportToScreenPoint(new Vector3(x, y, 0f));
    }

    public void RandomScale()
    {
        float scale = Random.Range(64f, 160f);
        rectTr.sizeDelta = new Vector2(scale, scale);
    }

    public void RandomPos()
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        rectTr.position = Camera.main.ViewportToScreenPoint(new Vector3(x, y, 0f));
    }

    private void Awake()
	{
        rectTr = GetComponent<RectTransform>();


	}
	// Start is called before the first frame update
	void Start()
    {
        RandomPos();
        RandomScale();
        RandomGoalPos();
        moveSpd = Random.Range(50f, 125f);
        rotSpd = Random.Range(25f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        dir = (goalPos - rectTr.position).normalized;
        dist = (goalPos - rectTr.position).magnitude;
        rectTr.position += dir * Time.deltaTime * moveSpd;
        rectTr.Rotate(Vector3.forward * Time.deltaTime * rotSpd);
        CheckBoundary();


    }
}
