using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public Material material;

    public GameObject testGameObject;
    // Start is called before the first frame update
    void Start()
    {
        //var shader = Shader.Find("Test/test");
        //camera.SetReplacementShader(shader, "RenderType");
        //camera.GetComponent<Skybox>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTestClick()
    {
        LoadManager.LoadGameObject("plane", "plane", (gameObject)=> {
            Instantiate<GameObject>(gameObject).transform.SetParent(testGameObject.transform);
        },(progress) => {
            Debug.Log(progress);
        });

        Debug.Log("点下去了");
    }
}
