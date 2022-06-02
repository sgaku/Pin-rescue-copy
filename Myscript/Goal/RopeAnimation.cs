using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロープの挙動のスクリプト
/// </summary>
public class RopeAnimation : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Vector3 fallPosition;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Locator.i?.playerAnimation.isTouchRope == true)
        {
            ArchesMove();
        }
    }

    void ArchesMove()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        rigidBody.AddForce(fallPosition, ForceMode.Force);
        Invoke(nameof(ActiveFalse), 1f);
    }

    //ロープを非アクティブに
    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
