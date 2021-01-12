using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkerboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector4[] vectors1 = { new Vector4(1, 1)};
        gameObject.GetComponent<Renderer>().material.SetVectorArray("_Plyer1", vectors1);

        //Vector4[] vectors2 = { new Vector4(4, 5), new Vector4(6, 6), new Vector4(4, 4) };
        //gameObject.GetComponent<Renderer>().material.SetVectorArray("_Plyer2", vectors2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
