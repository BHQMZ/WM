using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkerboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.SetVector("_Test", new Vector4(2,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
