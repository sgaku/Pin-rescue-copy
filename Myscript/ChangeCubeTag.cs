using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// CubeのColliderのタグを変更するスクリプト
/// </summary>
public class ChangeCubeTag : MonoBehaviour
{


    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            gameObject.tag = "Untagged";
        }
    }
  
}
