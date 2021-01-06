using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于执行各种需要在主线程中才能执行的回调
public class GameProcess : MonoBehaviour
{
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AssetBundleManager.IsLoad)
        {
            StartCoroutine(AssetBundleManager.LoadStart());
        }

        if (GameObjectManager.IsLoad)
        {
            StartCoroutine(GameObjectManager.LoadStart());
        }

        if (WebRequestManager.IsRequest)
        {
            StartCoroutine(WebRequestManager.WebRequestStart());
        }

        if (LoadSceneManager.IsLoad)
        {
            StartCoroutine(LoadSceneManager.LoadStart());
        }

        SocketManager.SocketEngineInvoke();
    }
}
