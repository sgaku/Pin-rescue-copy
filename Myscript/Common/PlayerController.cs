using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    public enum PlayerState
    {
        Idle,
        Move,
        Fall,
        Dead,
        Finish,
    }

    public PlayerState currentPlayerState;

    /// <summary>
    /// プレイヤーの体の向き
    /// </summary>
    public enum BodyDirection
    {
        Front,
        Right,
        Left,
        Back,
    }

    public BodyDirection currentBodyDirection;
    private bool isTurn;

    void OnTriggerEnter(Collider other)
    {
        //プレイヤーの体の向きを変更させるトリガー
        if (Locator.i.playerAnimation.IsAnimalCollision) return;
        if (!other.gameObject.CompareTag("Turn")) return;
        if (isTurn) return;
        isTurn = true;
        switch (currentBodyDirection)
        {
            case BodyDirection.Right:
                currentBodyDirection = BodyDirection.Left;
                break;
            case BodyDirection.Left:
                currentBodyDirection = BodyDirection.Right;
                break;
        }
        Locator.i.playerFallVelocity.isFall = false;
        currentPlayerState = PlayerState.Move;
    }
    // Start is called before the first frame update
    void Start()
    {
        //ゲーム開始時の体の向きは正面
        currentBodyDirection = BodyDirection.Front;
        currentPlayerState = PlayerState.Idle;
        isTurn = false;

    }

    // Update is called once per frame
    void Update()
    {
        CheckBodyDirection();
    }

    void FixedUpdate()
    {
        ToMove();
        ToFall();
    }

    /// <summary>
    /// 移動アニメーションへの遷移関数
    /// </summary>
    void ToMove()
    {
        if (Locator.i?.playerAnimation.pinCheck != PlayerAnimation.OnPinCheck.OnPin) return;
        if (currentPlayerState == PlayerState.Fall) return;
        if (Locator.i?.pinController.currentPinState == PinController.PinState.TouchedPinMove)
        {
            Locator.i.playerAnimation.MoveAnimation();
        }
    }
    /// <summary>
    /// 落下アニメーションへの遷移関数
    /// </summary>
    void ToFall()
    {
        if (Locator.i?.playerAnimation.pinCheck != PlayerAnimation.OnPinCheck.OnPin) return;
        if (Locator.i?.pinController.currentPinState == PinController.PinState.TouchedPinFall)
        {
            Locator.i.playerAnimation.FallAnimation();
        }
    }

    /// <summary>
    /// 体の向きの状態管理
    /// </summary>
    void CheckBodyDirection()
    {
        switch (currentBodyDirection)
        {
            case BodyDirection.Back:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case BodyDirection.Left:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case BodyDirection.Right:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
    }
}

