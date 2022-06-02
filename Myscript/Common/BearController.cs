using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    //シングルトンのインスタンス
    public static BearController i;
    //移動のスピード
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Animator animator;
    //落下地点のパラメータ
    [SerializeField] private Vector3 bearFallSpeed;
    [SerializeField] private float fallTime;
    [SerializeField] private ParticleSystem bearDeadEffect;
    /// <summary>
    /// クマの状態はパラメータからいじられたくない且つ取得・変更は外部からいじれるようにしたいため、プロパティをつけました
    /// </summary>
    public bool IsBearStateChange { get; set; }
    //プレイヤーへアタックする回数
    private bool isPlayerAttack;
    //Collision判定
    [SerializeField] private bool isCollision;

    /// <summary>
    /// クマの状態
    /// </summary>
    public enum BearState
    {
        Idle,
        Move,
        Attack,
        Dead,
        Stop,
    }
    public BearState currentBearState;

    void OnTriggerEnter(Collider other)
    {
        //死亡時の処理
        if (other.gameObject.CompareTag("DeadObj"))
        {
            currentBearState = BearState.Dead;
            animator.SetTrigger("Dead");
            bearDeadEffect.gameObject.SetActive(true);
            Invoke(nameof(ActiveFalse), 3f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isCollision = true;
        //プレイヤーに当たった時攻撃アニメーションへ
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
            isPlayerAttack = true;
            rigidBody.isKinematic = true;
            currentBearState = BearState.Attack;
            Invoke(nameof(BearStop), 1f);
        }
        //動物に当たった時死亡アニメーションへ
        else if (collision.gameObject.CompareTag("Animal"))
        {
            currentBearState = BearState.Dead;
            animator.SetTrigger("Dead");
            rigidBody.velocity = Vector3.zero;
            bearDeadEffect.gameObject.SetActive(true);
            Invoke(nameof(ActiveFalse), 3f);
        }
    }

    //Collision判定に使う
    void OnCollisionExit(Collision c)
    {
        isCollision = false;
    }
    //シングルトン
    private void Awake()
    {
        i = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isCollision = true;
        currentBearState = BearState.Idle;
        isPlayerAttack = false;
        IsBearStateChange = true;
    }

    // Update is called once per frame
    void Update()
    {
        //状態遷移が許されていない時はリターン
        if (IsBearStateChange != true) return;
        if (Locator.i?.pinController.currentPinState == PinController.PinState.TouchedPinMove
        && currentBearState != BearState.Dead)
        {
            BearMove();
        }
    }
    void FixedUpdate()
    {
        if (isCollision) return;
        //Collision判定がなく、くまの状態がIdleかDeadの時
        if (currentBearState == BearState.Idle || currentBearState == BearState.Dead)
        {
            BearFallVelocity();
            StartCoroutine(AddFallTime());
        }
    }

    /// <summary>
    /// 移動アニメーションと移動時の挙動
    /// </summary>
    public void BearMove()
    {
        //一回アタックした後移動状態になるのを防止
        if (isPlayerAttack) return;
        animator.SetTrigger("Move");
        currentBearState = BearState.Move;
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //死亡時に非アクティブに
    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }

    void BearStop()
    {
        currentBearState = BearState.Stop;
        animator.SetTrigger("idle");
    }
    IEnumerator AddFallTime()
    {
        while (true)
        {
            fallTime += 0.001f;
            yield return new WaitForSeconds(104);
        }
    }

    /// <summary>
    ///  落下時の挙動　落下時は状態遷移しないように設定
    /// </summary>
    void BearFallVelocity()
    {
        bearFallSpeed.y -= fallTime;
        rigidBody.isKinematic = false;
        IsBearStateChange = false;
        rigidBody.AddForce(bearFallSpeed, ForceMode.Impulse);
    }

}
