using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;

public class IntroController : MonoBehaviour
{
    public RectTransform school;
    public Image depart;
    public Image fade;

    Sequence introSeq;

    public AudioClip introSound;
    public AudioSource auds;
    void Awake()
    {
        school.localScale = Vector3.zero;
        depart.color = Funcs.SetAlpha(depart.color, 0f);
        fade.color = Funcs.SetAlpha(fade.color, 1f);
    }

    // Start is called before the first frame update
    void Start()
    {

        introSeq = DOTween.Sequence();

        introSeq.Append(fade.DOFade(0f, 0.5f))/*.OnComplete(() => auds.PlayOneShot(introSound))*/;
        //introSeq.Append();
        introSeq.Append(school.DOScale(1f, 2f).SetEase(Ease.InCubic));
        introSeq.AppendInterval(1f);
        introSeq.Append(depart.DOFade(1f, 0.75f)).SetEase(Ease.OutCubic);
        introSeq.AppendInterval(1.5f);
        introSeq.Append(fade.DOFade(1f, 0.25f));
        //introSeq.AppendInterval(0.5f);
        introSeq.OnComplete(() => GameManager.Instance.LoadScene(eSceneIndex.TITLE));
        //waveStartSeq.AppendInterval(1f);
        //waveStartSeq.Append(InGameUIManager.Instance.noticeCG.DOFade(0f, 1f).SetEase(Ease.OutQuart));

        //waveStartSeq.OnComplete(() => SpawnStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
