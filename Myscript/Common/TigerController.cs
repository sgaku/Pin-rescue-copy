using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TigerController : MonoBehaviour
{

    //斜めに移動する際のスピード
    [SerializeField] private float diagonalSpeed;
    [SerializeField] private float diagonalFallSpeed;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Animator animator;
    //落下地点のパラメータ 
    [SerializeField] private Vector3 tigerFallSpeed;
    [SerializeField] private float fallTime;
    [SerializeField] private float addValue;
    [SerializeField] private Vector3 movePosition;
    //死亡エフェクト
    [SerializeField] private ParticleSystem tigerDeadEffect;
    //Collision判定
    private bool isCollision;
    //プレイヤーへの攻撃回数
    private bool isDiagonal;
    private bool isPlayerAttack;
    //クマへの攻撃回数
    private bool isBearAttack;
    //トラの移動遷移の制御変数
    [SerializeField] private bool isChangeTigerMove;

    /// <summary>
    /// 虎の状態
    /// </summary>
    public enum TigerState
    {
        Idle,
        Move,
        Attack,
        Fall,
        Dead,
    }


    public TigerState currentTigerState;

    void OnTriggerEnter(Collider other)
    {
        //死亡時の挙動
        if (other.gameObject.CompareTag("DeadObj"))
        {
            currentTigerState = TigerState.Dead;
            tigerDeadEffect.gameObject.SetActive(true);
            animator.SetTrigger("Dead");
            Invoke(nameof(ActiceFalse), 3f);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        isCollision = true;
        //プレイヤーかクマに衝突した時攻撃アニメーションへ
        if (collision.gameObject.CompareTag("Player") && isPlayerAttack != true)
        {
            animator.SetTrigger("Attack");
            rigidBody.isKinematic = true;
            currentTigerState = TigerState.Attack;
            isPlayerAttack = true;
            Invoke(nameof(TigerIdle), 1f);
        }
        else if (collision.gameObject.CompareTag("Animal")
       && isBearAttack != true)
        {
            animator.SetTrigger("Attack");
            currentTigerState = TigerState.Attack;
            Invoke(nameof(TigerIdle), 1f);
            isBearAttack = true;
        }
        //斜めに落下移動するスタート地点
        if (collision.gameObject.CompareTag("ChangeMove"))
        {
            isDiagonal = true;
        }
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        //プレイヤーが死んでいた際は落下しない
        if (Locator.i?.playerController.currentPlayerState == PlayerController.PlayerState.Dead) return;
        isCollision = false;
    }

    void Awake()
    {
        currentTigerState = TigerState.Idle;
        isPlayerAttack = false;
        isBearAttack = false;
        isCollision = true;
        isDiagonal = false;
        isChangeTigerMove = true;
        //ステージ４で強制的にTrueにすることでnullエラーを防止
        if (BearController.i?.IsBearStateChange == false)
        {
            BearController.i.IsBearStateChange = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentTigerState == TigerState.Dead) return;
        //移動状態へ遷移する条件１・２
        if (Locator.i?.playerAnimation.pinCheck == PlayerAnimation.OnPinCheck.Others
        && Locator.i?.playerController.currentPlayerState == PlayerController.PlayerState.Idle)
        {
            if (isChangeTigerMove == false) return;
            TigerMove();
        }
        else if (Locator.i.pinController.isTouchedTigerMovePin == true)
        {
            TigerMove();
        }
        if (isDiagonal == true)
        {
            TigerMove2();
        }
        //Collision判定がなく、velocity.yの値が一定以下になった時よばれるよう設定
        // 移動する際に足が離れ、衝突判定がなくなりよきせぬタイミングで関数がよばれるバグを防止するために、落下判定はvelocity.yの値で判断するように変更しました
        if (isCollision == false && rigidBody.velocity.y < -0.5f)
        {
            TigerFallVelocity();
            StartCoroutine(AddFallTime());
        }
    }

    /// <summary>
    /// 移動アニメーション
    /// </summary>
    void TigerMove()
    {
        //早期リターン　
        if (isBearAttack || isPlayerAttack) return;
        currentTigerState = TigerState.Move;
        animator.SetTrigger("Move");
        rigidBody.MovePosition(rigidBody.position + movePosition * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 斜めへ落下移動するアニメーション
    /// </summary>
    void TigerMove2()
    {
        if (currentTigerState == TigerState.Dead) return;
        isChangeTigerMove = false;
        transform.position += Vector3.left * diagonalSpeed * Time.deltaTime;
        transform.position += Vector3.down * diagonalFallSpeed * Time.deltaTime;
    }

    //非アクティブに
    void ActiceFalse()
    {
        gameObject.SetActive(false);
    }

    void TigerIdle()
    {
        currentTigerState = TigerState.Idle;
        animator.SetTrigger("idle");
    }
    IEnumerator AddFallTime()
    {
        while (true)
        {
            // ステージ４での斜めの挙動の際に規定値0.001だと加速が速すぎる挙動になったので、TigerにだけaddValueという変数をつけ加速値の変更ができるようにしています
            fallTime += addValue;
            yield return new WaitForSeconds(104);
        }
    }

    /// <summary>
    /// 落下時の挙動
    /// </summary>
    void TigerFallVelocity()
    {
        if (currentTigerState == TigerState.Idle)
        {
            currentTigerState = TigerState.Fall;
        }
        tigerFallSpeed.y -= fallTime;
        rigidBody.isKinematic = false;
        rigidBody.AddForce(tigerFallSpeed, ForceMode.Impulse);
    }
}