using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public Slider slider;

    public static string NextScenes;

    private float progressValue = 0;

    private AsyncOperation nextOperation;

    // Start is called before the first frame update
    void Start()
    {
        this.progressValue = 0;
        this.slider.value = 0;
    }

    void OnEnable()
    {
        LoadSceneManager.Load(NextScenes,(operation) => {
            nextOperation = operation;
        },(progress) => {
            UpdateProgress(progress);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (this.slider.value < progressValue)
        {
            this.slider.value = Mathf.MoveTowards(this.slider.value, progressValue, 2 * Time.deltaTime);
        }
        else if(nextOperation!=null)
        {
            nextOperation.allowSceneActivation = true;
        }
    }

    /// <summary>
    /// 更新加载进度
    /// </summary>
    /// <param name="value">完成度</param>
    public void UpdateProgress(float value)
    {
        progressValue = value;
    }
}
