using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PinController : MonoBehaviour
{
    private float speed;
    [SerializeField] private Transform parentPin;

    //ピンの状態管理
    public enum PinState
    {
        UnTouched,
        Touched,
        TouchedPinFall,
        TouchedPinMove,
    }
    public PinState currentPinState;
    public bool isTouchedTigerMovePin;

    // Start is called before the first frame update
    void Start()
    {
        //ゲーム開始時はタップされていない
        currentPinState = PinState.UnTouched;
        isTouchedTigerMovePin = false;
        speed = 0f;
        Input.multiTouchEnabled = false;
    }
    void Update()
    {
        parentPin.position += parentPin.transform.right * speed * Time.deltaTime;
        //タッチ判定をRayCastに変更
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == null) return;
                if (hit.collider.gameObject.CompareTag("PinFall") || hit.collider.gameObject.CompareTag("PinMove")
                || hit.collider.gameObject.CompareTag("PinTigerMove") || hit.collider.gameObject.CompareTag("Pin"))
                {
                    speed = 120f;
                    parentPin = hit.collider.transform.parent;
                    StartCoroutine(ChangeActive(parentPin));
                    ChangePinState(parentPin);
                }
            }
        }
    }
    public void ChangePinState(Transform trans)
    {    //イベントの二重発生を防止
        if (currentPinState == PinController.PinState.TouchedPinFall) return;
        if (currentPinState == PinController.PinState.TouchedPinMove) return;

        if (trans.CompareTag("PinFall"))
        {
            currentPinState = PinController.PinState.TouchedPinFall;
        }
        else if (trans.CompareTag("PinMove"))
        {
            currentPinState = PinController.PinState.TouchedPinMove;
        }
        else if (trans.CompareTag("PinTigerMove"))
        {
            //ステージ５でトラがプレイヤーが落ちた時に動かなくなるバグがあったため、トラの動きのトリガーとなるPinはState管理ではなく
            //Boolを使って管理しています
            isTouchedTigerMovePin = true;
        }
        else
        {
            //Tigerの動きのバグを防止
            if (isTouchedTigerMovePin == true) return;
            currentPinState = PinController.PinState.Touched;
        }

    }
    IEnumerator ChangeActive(Transform trans)
    {
        yield return new WaitForSeconds(3);
        trans.gameObject.SetActive(false);
    }


}
