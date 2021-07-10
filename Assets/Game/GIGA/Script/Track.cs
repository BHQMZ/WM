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
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void UpdateProgress(float progress)
    {
        transform.position = new Vector3(progress * 0.5f, transform.position.y, transform.position.z);
    }
}
