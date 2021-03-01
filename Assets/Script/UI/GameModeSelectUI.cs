using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameModeSelectUI : MonoBehaviour
{
    //加载提示文字
    public Text loadingText;

    public Graphic UIBox;

    private bool isGameStart = false;

    void Start()
    {
        loadingText.text = "";
    }

    void Update()
    {
        if (isGameStart)
        {
            if (UIBox.transform.localScale.x > 0)
            {
                UIBox.transform.localScale = Vector3.MoveTowards(UIBox.transform.localScale, new Vector3(0,0,1), 4 * Time.deltaTime);
            }
            else
            {
                isGameStart = false;
                LoadSceneManager.Switch("GameScene", true);
            }
        }
    }

    //单机模式按钮点击事件
    public void OfflineOnClickEvent()
    {
        //Alert.Show("模式未开发");

        //LoadSceneManager.Switch("GameScene",true);

        isGameStart = true;
    }

    //联机模式按钮点击事件
    public void OnlineOnClickEvent()
    {
        loadingText.text = "正在连接到服务器";

        SocketManager.Instance().Link(_LinkSuccess, _LinkFail);
    }

    private void _LinkSuccess()
    {
        loadingText.text = "";
    }

    private void _LinkFail()
    {
        loadingText.text = "";
        Alert.Show("连接服务器失败");
    }
}
