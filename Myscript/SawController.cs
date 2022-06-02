using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ４　障害物の挙動のスクリプト
/// </summary>
public class SawController : MonoBehaviour
{

    Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        SawAnimation();
    }

    void SawAnimation()
    {
        myTransform.Rotate(0, 0, 10);
    }
}
