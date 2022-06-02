using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TutrialHandAnimation : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] bool isOnTutrial;
    [SerializeField] private Image image;
    Vector3[] path = new Vector3[2];
    Sequence sequence;

    void Start()
    {
        //移動の始点と終点
        path[0] = new Vector3(-40f, -54, 0);
        path[1] = new Vector3(300f, -54, 0);

        if (isOnTutrial)
        {
            MoveHand();
        }
    }

    void MoveHand()
    {
        Init(path[0]);
        sequence = DOTween.Sequence()
             .Append(image.rectTransform.DOScale(Vector3.one * 0.8f, 0.5f))
             .Append(rectTransform.DOLocalPath(path, 0.5f).SetEase(Ease.Linear))
             .Append(image.rectTransform.DOScale(Vector3.one, 0.5f))
             .Join(DOTween.ToAlpha(() => image.color, color => image.color = color, 0f, 0.5f)).SetLoops(-1, LoopType.Restart);
    }
    void Init(Vector3 startPos)
    {
        rectTransform.anchoredPosition = startPos;
        image.color = Color.white;
        image.rectTransform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sequence.Kill();
            gameObject.SetActive(false);
        }
    }
}
