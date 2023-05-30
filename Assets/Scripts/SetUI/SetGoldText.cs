using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetGoldText : MonoBehaviour
{
    private TextMeshProUGUI goldRollText;
    private void Start()
    {
        goldRollText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        goldRollText.SetText(GameManager.Instance.GoldRoll.ToString());
    }
}
