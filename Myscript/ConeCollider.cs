using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// コーンのColliderの処理
/// </summary>
public class ConeCollider : MonoBehaviour
{
    private Collider boxCollider;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           boxCollider.enabled = false;
        }
    }

    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }
}
