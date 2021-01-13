using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Checkerboard : MonoBehaviour
{
    Player player = new Player();
    // Start is called before the first frame update
    void Start()
    {
        //Vector4[] vectors1 = { new Vector4(1, 1)};
        //gameObject.GetComponent<Renderer>().material.SetVectorArray("_Plyer1", vectors1);

        player.ShaderName = "_Plyer1";

        //Vector4[] vectors2 = { new Vector4(4, 5), new Vector4(6, 6), new Vector4(4, 4) };
        //gameObject.GetComponent<Renderer>().material.SetVectorArray("_Plyer2", vectors2);
    }
    Ray ray;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.textureCoord);
                Vector4 vector = new Vector4(Mathf.Floor(hit.textureCoord.x * 7), Mathf.Floor(hit.textureCoord.y * 7));

                player.BuyChecks(vector, vector);

                Vector4[] vector1 = new Vector4[49];
                for (int i = 0; i < player.checks.Count; i++)
                {

                }
                gameObject.GetComponent<Renderer>().material.SetVectorArray(player.ShaderName, player.checks.ToArray());
            }
        }

        
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

    }

    public void OnMouseDown()
    {

    }
}
