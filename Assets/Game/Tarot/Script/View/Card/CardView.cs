using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卡牌视图脚本
public class CardView : MonoBehaviour
{
    private Material frontMaterial;
    private Material backMaterial;

    public Vector3 meshSize;

    void Start()
    {
        Material[] materials = GetComponent<Renderer>().materials;
        backMaterial = materials[0];
        frontMaterial = materials[2];

        Vector3 bounds = GetComponent<MeshFilter>().mesh.bounds.size;

        meshSize = new Vector3(bounds.x * transform.localScale.x, bounds.y * transform.localScale.y, bounds.z * transform.localScale.z);
    }

    void Update()
    {
        
    }

    private CardVO _dataSource;
    public CardVO dataSource
    {
        set
        {
            if (_dataSource != value)
            {
                //数据原改变解除对老数据原的监听
                _dataSource?.Off("cardFront", SetCardFront);
                _dataSource?.Off("isReversed", SetIsReversed);

                _dataSource = value;

                //增加状态改变时的监听回调
                _dataSource?.On("cardFront", SetCardFront);
                _dataSource?.On("isReversed", SetIsReversed);

                UpdateView();
            }
        }
    }

    private void UpdateView()
    {
        SetCardFront(_dataSource.cardFront);
        SetIsReversed(_dataSource.isReversed);
    }

    private void SetCardFront(object value)
    {
        if (value != null)
        {
            string cardFront = (string)value;

            LoadManager.LoadTexture("Game/Tarot/Res/Textures/Card/" + cardFront + ".png", (texture) =>
            {
                frontMaterial.SetTexture("_ImageTex", texture);
            });
        }
    }

    private void SetIsReversed(object value)
    {
        bool isReversed = (bool)value;

        Vector3 vector = new Vector3(transform.localScale.x, Math.Abs(transform.localScale.y), transform.localScale.z);
        if (isReversed)
        {
            vector.y = -vector.y;
        }

        transform.localScale = vector;
    }
}
