using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ステージ３　女性の挙動のスクリプト
/// </summary>
public class GoalFemaleAnimation : MonoBehaviour
{



    [SerializeField] private Animator animator;
    public void FemaleMove()
    {
        animator.SetTrigger("Goal");
    }

}
