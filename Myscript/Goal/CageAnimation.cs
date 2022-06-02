using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 牢屋の挙動のスクリプト
/// </summary>
public class CageAnimation : MonoBehaviour
{

    [SerializeField] private float moveZ;
    [SerializeField] private float moveTime;

   /// <summary>
   /// 牢屋の挙動
   /// </summary>
    public void CageMove()
    {
        transform.DOMoveZ(moveZ, moveTime);
        Invoke(nameof(KillMove), 2f);
    }

    void KillMove()
    {
        transform.DOKill();
    }
}
