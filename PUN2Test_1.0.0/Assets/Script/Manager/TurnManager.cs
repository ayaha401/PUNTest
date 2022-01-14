using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum PLAYER
{
    player1 = 1,
    player2 = 2,
    nullPlayer = -1,
};

public enum PHASE
{
    diceRoll,
    playerMove,
    playerChange,
    nullPhase,
}

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static PLAYER turnPlayer = PLAYER.nullPlayer;
    public static PHASE phase = PHASE.nullPhase;

    [SerializeField, Tooltip("DiceButtonUI")]
    DiceButtonUI _DiceButtonUI;

    [SerializeField, Tooltip("DiceNumUI")]
    DiceNumUI _DiceNumUI;

    // [SerializeField, Tooltip("SampleScene")]
    // SampleScene _SampleScene;

    private int _diceNum = 0;

    private PlayerMove _PlayerMove = null;


    void Start()
    {
        turnPlayer = PLAYER.player1;

        phase = PHASE.diceRoll;
    }

    void Update()
    {
        // 二人そろってなかったらリターン
        if(PhotonNetwork.PlayerList.Length != 2)
        {
            return;
        }
        
        // マスターじゃなかったら早期リターン
        // if(!PhotonNetwork.IsMasterClient)
        // {
        //     return;
        // }
        
        // ダイスロールフェーズ
        if(phase == PHASE.diceRoll)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                _DiceButtonUI.TurnPlayerUISetActiveTrue();
            }
        }

        // プレイヤー移動フェーズ
        if(phase == PHASE.playerMove)
        {
            // ダイスロール結果
            _diceNum = _DiceButtonUI.SendDiceNum();

            // 数字をUIに書き込む
            _DiceNumUI.WhiteDiceNum(_diceNum);

            // 移動
            if((int)TurnManager.turnPlayer == SampleScene.playerNum)
            {
                GameObject playerObj = GameObject.Find("Player" + (int)turnPlayer);
                _PlayerMove = playerObj.GetComponent<PlayerMove>();
                _PlayerMove.Move(_diceNum);
                // phase = _PlayerMove.Move(_diceNum, _SampleScene._myPlayerObj);
            }
            
        }

        // ターン変更フェーズ
        if(phase == PHASE.playerChange)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(TurnChange), RpcTarget.All);
            }
            phase = PHASE.diceRoll;
        }

        // デバッグ用＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
        if(turnPlayer == PLAYER.player1)
        {
            Debug.Log("Player1ターン");
        }

        if(turnPlayer == PLAYER.player2)
        {
            Debug.Log("Player2ターン");
        }

        Debug.Log("現在のフェーズ = " + phase);
    }

    // ターンを変える
    [PunRPC]
    private void TurnChange()
    {
        if(turnPlayer == PLAYER.player1)
        {
            turnPlayer = PLAYER.player2;
        }
        else
        {
            turnPlayer = PLAYER.player1;
        }
    }
}
