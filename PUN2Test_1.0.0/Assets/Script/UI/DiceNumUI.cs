using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DiceNumUI : MonoBehaviour
{
    private TextMeshProUGUI _diceNumTMP = null;

    void Start()
    {
        _diceNumTMP = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        
    }

    public void WhiteDiceNum(int diceNum)
    {
        _diceNumTMP.text = diceNum.ToString();
    }
}
