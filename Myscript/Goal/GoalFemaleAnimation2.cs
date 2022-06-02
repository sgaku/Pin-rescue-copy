using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ５　女性の挙動のスクリプト
/// </summary>
public class GoalFemaleAnimation2 : MonoBehaviour
{


    [SerializeField] private Animator animator;
   

    // Update is called once per frame
    void Update()
    {
        if (Locator.i?.playerAnimation.isTouchRope== true)
        {
            FemaleGoalMove2();
        }
    }

    public void FemaleGoalMove2()
    {
        transform.rotation = Quaternion.Euler(0, 270, 0);
        animator.SetTrigger("Goal");
    }
}
