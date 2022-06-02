using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 吹き出しのスクリプト
/// </summary>
public class Canvas : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        //プレイヤーがロープにタッチしたら非アクティブに
        if (Locator.i?.playerAnimation.isTouchRope == true)
        {
            gameObject.SetActive(false);
        }
    }
}
