using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サービスロケーター
/// </summary>
public class Locator : MonoBehaviour
{

    public PlayerAnimation playerAnimation;
    public PlayerController playerController;
    public PinController pinController;
    public PlayerFallVelocity playerFallVelocity;
    public static Locator i;

    private void Awake()
    {
        i = this;
    }

}
