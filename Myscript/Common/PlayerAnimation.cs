using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    //死亡エフェクト
    [SerializeField] private ParticleSystem deadEffect;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 fallSpeed;
    [SerializeField] private float fallTime;
    [SerializeField] private Vector3 rightMovePosition;
    [SerializeField] private Vector3 leftMovePosition;
    //Moveアニメーションの制御変数
    [SerializeField] private bool isChangeMove;
    //ドアに当たった時のプレイヤーの挙動のパラメータ
    [SerializeField] private float playerDoorPosition;
    [SerializeField] private float playerDoorTime;

    //ToGoalAnimationへ遷移するまでの時間
    [SerializeField] private float invokeToGoalTime;
    //プレイヤーの向きを戻す時間
    [SerializeField] private float resetRotationTime;
    //Ropeに触れたかどうか
    public bool isTouchRope;
    //プレイヤーの物理挙動を制御するフラグ
    private bool isDoorOpen;
    public bool IsAnimalCollision { get; private set; }
    [SerializeField] private CageAnimation cageAnimation;
    [SerializeField] private CageDoorAnimation cageDoorAnimation;
    [SerializeField] private GoalFemaleAnimation goalFemaleAnimation;

    //プレイヤーの急な状態遷移を防止
    [SerializeField] private bool isChangePlayerState;

    //ピンの上にいるかを管理
    public enum OnPinCheck
    {
        OnPin,
        Others,
    }
    public OnPinCheck pinCheck;

    // Start is called before the first frame update
    void Start()
    {
        isChangeMove = false;
        isChangePlayerState = true;
        isTouchRope = false;
        isDoorOpen = false;
        IsAnimalCollision = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        //早期リターン
        if (collision.gameObject.CompareTag("MoveObj")) return;
        if (isChangePlayerState == false) return;
        //Pinの上にいる時
        if (collision.gameObject.CompareTag("PinFall"))
        {
            isChangeMove = false;
            pinCheck = OnPinCheck.OnPin;
        }
        // 移動アニメーションへ
        else if (collision.gameObject.CompareTag("ChangeMove"))
        {
            GroundAnimation();
            isChangeMove = true;
            pinCheck = OnPinCheck.Others;
        }
        //移動状態からゴールへ
        else if (collision.gameObject.CompareTag("Finish"))
        {
            Locator.i.playerFallVelocity.isFall = false;
            Invoke(nameof(ToGoalAnimation), invokeToGoalTime);
            pinCheck = OnPinCheck.Others;
        }
        //着地時アニメーションへ
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Locator.i.playerFallVelocity.isFall = false;
            isChangeMove = false;
            GroundAnimation();
            pinCheck = OnPinCheck.Others;
        }
        //牢屋に当たった時
        else if (collision.gameObject.CompareTag("Cage"))
        {
            isChangeMove = false;
            cageAnimation.CageMove();
            cageDoorAnimation.CageDoorMove();
            goalFemaleAnimation.FemaleMove();
            pinCheck = OnPinCheck.Others;
            GoalAnimation();
        }
        //左への方向転換
        else if (collision.gameObject.CompareTag("ChangeLeftMove"))
        {
            GroundAnimation();
            Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Left;
            isChangeMove = true;
            pinCheck = OnPinCheck.Others;
        }
        //ステージ５でロープに当たった時
        else if (collision.gameObject.CompareTag("Rope"))
        {
            isChangeMove = false;
            isTouchRope = true;
            GoalAnimation2();
        }
        //死亡判定１　障害物に当たった時
        else if (collision.gameObject.CompareTag("DeadObj") && IsAnimalCollision != true)
        {
            Locator.i.playerFallVelocity.isFall = false;
            //死亡時に別のアニメーションが呼ばれるのを防止
            isChangePlayerState = false;
            isChangeMove = false;
            DeadAnimation();
        }
        //死亡判定２　動物に当たった時
        else if (collision.gameObject.CompareTag("Animal") && IsAnimalCollision != true)
        {
            IsAnimalCollision = true;
            isChangePlayerState = false;
            isChangeMove = false;
            DeadAnimation();
            pinCheck = OnPinCheck.Others;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isDoorOpen) return;
        //ドアに当たったらドアアニメーションを呼ぶ
        if (other.gameObject.CompareTag("Door"))
        {
            isDoorOpen = true;
            DoorAnimation doorAnimation = other.GetComponent<DoorAnimation>();
            DOVirtual.DelayedCall(0.05f, doorAnimation.DoorAnim);
        }
    }
    void Update()
    {
        if (Locator.i.playerController.currentPlayerState == PlayerController.PlayerState.Fall)
        {
            StartCoroutine(AddFallTime());
        }
    }

    IEnumerator AddFallTime()
    {
        fallTime += 0.001f;
        yield return new WaitForSeconds(104);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsAnimalCollision)
        {
            Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Dead;
        }
        //Move制御がなく、かつプレイヤーの状態遷移が許されている場合Move状態への遷移を許可
        if (isChangeMove == true && isChangePlayerState == true)
        {
            MoveAnimation();
        }
    }

    /// <summary>
    /// Idle状態からの落下アニメーション
    /// </summary>
    public void FallAnimation()
    {
        rigidBody.isKinematic = false;
        fallSpeed.y -= fallTime;
        animator.SetTrigger("Fall");
        rigidBody.AddForce(fallSpeed, ForceMode.Impulse);
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Fall;
    }

    /// <summary>
    /// 着地アニメーション
    /// </summary>
    public void GroundAnimation()
    {
        if (Locator.i?.playerController.currentPlayerState == PlayerController.PlayerState.Dead) return;
        if (isChangeMove == true) return;
        Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Front;
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Idle;
        animator.SetTrigger("Ground");
    }

    /// <summary>
    /// 移動アニメーション
    /// </summary>
    public void MoveAnimation()
    {
        animator.SetTrigger("Move");
        //プレイヤーの向きが正面なら右へ変更
        if (Locator.i?.playerController.currentBodyDirection == PlayerController.BodyDirection.Front)
        {
            Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Right;
        }
        //　プレイヤーがIdle状態の時は移動までに遅延を入れる
        if (Locator.i.playerController.currentPlayerState == PlayerController.PlayerState.Idle)
        {
            Invoke(nameof(PlayerMove), 0.2f);
        }
        else
        {
            PlayerMove();
        }
    }

    /// <summary>
    /// 移動アニメーション時の動きの処理
    /// </summary>
    public void PlayerMove()
    {
        if (Locator.i.playerController.currentPlayerState == PlayerController.PlayerState.Fall) return;
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Move;
        rigidBody.isKinematic = true;
        switch (Locator.i?.playerController.currentBodyDirection)
        {
            case PlayerController.BodyDirection.Right:
                rigidBody.MovePosition(rigidBody.position + rightMovePosition * Time.fixedDeltaTime);
                break;
            case PlayerController.BodyDirection.Left:
                rigidBody.MovePosition(rigidBody.position + leftMovePosition * Time.fixedDeltaTime);
                break;
        }
    }

    /// <summary>
    /// 死亡アニメーション
    /// </summary>
    public void DeadAnimation()
    {
        rigidBody.isKinematic = true;
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Dead;
        Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Right;
        animator.SetTrigger("Dead");
        deadEffect.gameObject.SetActive(true);
        if (Variables.screenState != ScreenState.Game) return;
        Variables.screenState = ScreenState.Failed;
    }

    void ClearScreen()
    {
        if (Variables.screenState != ScreenState.Game) return;
        Variables.screenState = ScreenState.Clear;
    }

    /// <summary>
    /// ゴールアニメーション
    /// </summary>
    public void GoalAnimation()
    {
        rigidBody.isKinematic = true;
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Finish;
        Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Front;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        ClearScreen();
        animator.SetTrigger("Goal");
    }

    /// <summary>
    /// ステージ５ ゴールアニメーション
    /// </summary>
    public void GoalAnimation2()
    {
        rigidBody.isKinematic = true;
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Finish;
        Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Right;
        ClearScreen();
        animator.SetTrigger("Goal");
    }

    /// <summary>
    /// プレイヤーの向きを正面にし、ゴールアニメーションへ
    /// </summary>
    public void ResetRotation()
    {
        animator.SetTrigger("Ground");
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Idle;
        Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Front;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        Invoke(nameof(GoalAnimation), 0.1f);
    }

    /// <summary>
    /// ドアに触れた時の挙動
    /// </summary>
    public void PlayerDoorAnim()
    {
        Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Back;
        transform.DOMoveZ(playerDoorPosition, playerDoorTime);
        Invoke(nameof(ResetRotation), resetRotationTime);
    }
    /// <summary>
    /// 移動状態からドアに触れた時の挙動
    /// </summary>
    public void ToGoalAnimation()
    {
        isChangePlayerState = false;
        Locator.i.playerController.currentBodyDirection = PlayerController.BodyDirection.Back;
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Move;
        animator.SetTrigger("Move");
        transform.DOMoveZ(playerDoorPosition, playerDoorTime);
        Invoke(nameof(ResetRotation), resetRotationTime);
    }

}
