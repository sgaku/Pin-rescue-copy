using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動状態からプレイヤーが落下する際の挙動のスクリプト
/// </summary>
public class PlayerFallVelocity : MonoBehaviour
{

    public bool isFall;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Vector3 fallSpeed;
    [SerializeField] private float fallTime;
    [SerializeField] private float velocityX;

    void OnTriggerEnter(Collider other)
    {
        if (Locator.i.playerController.currentPlayerState == PlayerController.PlayerState.Dead) return;
        if (!other.gameObject.CompareTag("ToFall")) return;
        isFall = true;
    }
    void OnTriggerStay(Collider other)
    {
        if (Locator.i.playerController.currentPlayerState == PlayerController.PlayerState.Dead) return;
        if (!other.gameObject.CompareTag("ToFall")) return;
        isFall = true;
    }

    void Start()
    {
        isFall = false;
    }
    void Update()
    {
        if (isFall)
        {
            StartCoroutine(AddFallTime());
        }
    }
    IEnumerator AddFallTime()
    {
        while (true)
        {
            fallTime += 0.001f;
            yield return new WaitForSeconds(104);
        }
    }
    void FixedUpdate()
    {
        if (isFall)
        {
            FallVelocity();
        }
    }
    /// <summary>
    /// 落下時の挙動
    /// </summary>
    public void FallVelocity()
    {
        if (!isFall) return;
        rigidBody.isKinematic = false;
        rigidBody.velocity = new Vector3(velocityX, rigidBody.velocity.y, rigidBody.velocity.z);
        fallSpeed.y -= fallTime;
        Locator.i.playerController.currentPlayerState = PlayerController.PlayerState.Fall;
        rigidBody.AddForce(fallSpeed, ForceMode.Impulse);
    }
}
