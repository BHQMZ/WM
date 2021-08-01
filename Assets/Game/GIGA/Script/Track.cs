using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    private float _progress = 0;
    /// <summary>
    /// 获取进度
    /// </summary>
    public float progress
    {
        set
        {
            _progress = value;

            UpdateProgress(value);

            Activation(Mathf.Abs(value) >= 3);
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    private void UpdateProgress(float progress)
    {
        transform.position = new Vector3(progress * 0.62f / 3, transform.position.y, transform.position.z);
    }

    public void Init()
    {
        progress = 0;
    }

    private void Activation(bool isActivation)
    {
        if (isActivation)
        {

        }
    }
}
