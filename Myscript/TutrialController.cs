using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutrialController : MonoBehaviour
{

    [SerializeField] private TutrialHandAnimation tutrialHandAnimation;
    // Start is called before the first frame update
    void Start()
    {
        tutrialHandAnimation.gameObject.SetActive(true);
    }
}
