using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TarotUI : MonoBehaviour
{
    public Text text;

    public TableTop tableTop;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void onButtonClick()
    {
        //if (text.text!="")
        //{
        //    tableTop.Draw(int.Parse(text.text));
        //}
        uint id = SoundManager.Instance.PlayBGMSound("Play_PREL_SSFX_MUSICAL_PO01_218_L");
        SoundManager.Instance.StopPlayingID(id, 1);
    }
}
