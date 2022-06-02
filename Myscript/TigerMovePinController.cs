using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ステージ５でTigerの移動に使うPinのスクリプト
/// </summary>
public class TigerMovePinController : MonoBehaviour
{

    [SerializeField] private Transform tigerMovePin;
    [SerializeField] private Transform toFallBox;
    
    public void OnClick()
    {
        //プレイヤーの落下判定タグ
        toFallBox.tag = "ToFall";
        //PinTigerMoveをPinに変更し、Tigerが移動状態にならないようにする
        tigerMovePin.tag = "Pin";
        for (int i = 0; i < 2; i++)
        {
            tigerMovePin.GetChild(i).tag = "Pin";
        }
    }
}
