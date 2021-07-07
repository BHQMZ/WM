using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    public static GameObject Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadSceneManager.Switch("Tarot", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
