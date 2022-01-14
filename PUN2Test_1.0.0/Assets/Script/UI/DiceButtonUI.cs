using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class DiceButtonUI : MonoBehaviourPunCallbacks, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image _myImage = null;
    private bool _isClickde = false;
    int num = 0;

    void Start()
    {
        _myImage = GetComponent<Image>();
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    // クリックしたら
    public void OnPointerClick(PointerEventData eventData)
    {
        _isClickde = true;
        DecideDiceNum();

        photonView.RPC(nameof(MovePlayerMovePhaseRPC), RpcTarget.All);
    }

    [PunRPC]
    private void MovePlayerMovePhaseRPC()
    {
        if(TurnManager.phase == PHASE.diceRoll)
        {
            TurnManager.phase = PHASE.playerMove;
        }
    }


    // マウスカーソルが入ったら
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SampleScene.playerNum == 1)
        {
            _myImage.color = Color.red;
        }
        else if(SampleScene.playerNum == 2)
        {
            _myImage.color = Color.blue;
        }
        else
        {
            _myImage.color = Color.green;
        }
    }

    // マウスカーソルが出たら
    public void OnPointerExit(PointerEventData eventData)
    {
        _myImage.color = Color.white;
    }

    // ターンのプレイヤーのUIをアクティブ化
    // ターンじゃないプレイヤーのUIは非表示にする
    public void TurnPlayerUISetActiveTrue()
    {
        photonView.RPC(nameof(TurnPlayerUISetActiveTruePRC), RpcTarget.All);
    }

    [PunRPC]
    private void TurnPlayerUISetActiveTruePRC()
    {
        if((int)TurnManager.turnPlayer == SampleScene.playerNum)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // ダイスの目を決定
    private void DecideDiceNum()
    {
        if((int)TurnManager.turnPlayer == SampleScene.playerNum)
        {
            num = Random.Range(1, 4);
        }
        else
        {
            num = 0;
        }
    }

    // ダイスの値を送る
    public int SendDiceNum()
    {
        return num;
    }
}
