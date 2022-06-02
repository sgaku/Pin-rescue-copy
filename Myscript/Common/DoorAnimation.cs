using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 doorEndRotation;
    [SerializeField] private Transform doorPosition;
    [SerializeField] private float invokeTime;
    [SerializeField] private float rotationTime;

    /// <summary>
    /// ドアアニメーション
    /// </summary>
    public void DoorAnim()
    {
        doorPosition.DOLocalRotate(doorEndRotation, rotationTime);
        if (Locator.i?.playerController.currentPlayerState == PlayerController.PlayerState.Move) return;
        //プレイヤーがMove状態ではないときはPlayerDoorAnimへ
        DOVirtual.DelayedCall(invokeTime, Locator.i.playerAnimation.PlayerDoorAnim);
    }



}
