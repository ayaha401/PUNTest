using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    private GameObject _playerObj = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // プレイヤーを動かす
    public void Move(int diceNum)
    {
        Vector3 playerPos = this.gameObject.transform.position;
        playerPos.x += diceNum * 1.5f;

        if(photonView.IsMine)
        {
            this.gameObject.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
            photonView.RPC(nameof(MovePlayerChangePhaseRPC), RpcTarget.All);
        }
    }

    [PunRPC]
    private void MovePlayerChangePhaseRPC()
    {
        if(TurnManager.phase == PHASE.playerMove)
        {
            TurnManager.phase = PHASE.playerChange;
        }
    }
}
