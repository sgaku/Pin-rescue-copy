using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 牢屋のドアの挙動のスクリプト
/// </summary>
public class CageDoorAnimation : MonoBehaviour
{
    [SerializeField] private float moveY;
    [SerializeField] private Vector3 moveScale;
    [SerializeField] private float moveTime;

    /// <summary>
    /// 牢屋のドアの挙動
    /// </summary>
    public void CageDoorMove()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(moveY, moveTime));
        sequence.Join(transform.DOScale(moveScale, moveTime));
        sequence.Play();
        Invoke(nameof(MoveKill), 2f);
    }
    /// <summary>
    /// DoTweenのCapacityが限界値を超えないよう設定
    /// </summary>
    void MoveKill()
    {
        transform.DOKill();
    }
}
