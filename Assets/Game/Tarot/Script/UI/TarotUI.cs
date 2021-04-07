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
        if (text.text!="")
        {
            tableTop.Draw(int.Parse(text.text));
        }
    }
}
