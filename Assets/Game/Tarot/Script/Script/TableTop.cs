using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTop : MonoBehaviour
{
    public Group group;

    public Transform floatNode;

    public Transform showCardNode;

    public List<Transform> CardArray; 

    //抽取的卡牌
    private List<Card> drawList = new List<Card>();

    void Start()
    {
        EventManager.On("DrawCard", onDrawCard);
        EventManager.On("ShowCard", onShowCard);

        //Vector3[] vectors = GameUtils.DrawBezierCurves(new Vector3[] { 
        //    new Vector3(0,0,0),new Vector3(1,5,6),new Vector3(8,4,5)
        //},0.001f);


        //for (int i = 0; i < vectors.Length - 1; i++)
        //{
        //    Debug.DrawLine(vectors[i], vectors[i+1],Color.yellow);
        //    Debug.Log(vectors[i]);
        //}
    }
    void Update()
    {
        //Vector3[] vectors = GameUtils.DrawCircle(Vector3.up, 5, 0.001f);


        //for (int i = 0; i < vectors.Length - 1; i++)
        //{
        //    Debug.DrawLine(vectors[i], vectors[i + 1], Color.yellow);
        //}
    }

    private void onDrawCard(object index)
    {
        Float((int)index);
        //Draw((int)index);
    }

    private void onShowCard(object obj) 
    {
        CardCollider card = (CardCollider)obj;

        ShowCardDialog.Show(card, showCardNode);
    }

    public void Draw(int index)
    {
        if (drawList.Count < CardArray.Count)
        {
            Card card = group.Draw(index);
            if (card)
            {
                drawList.Add(card);

                Transform t = CardArray[drawList.Count - 1];

                card.Draw(()=> {
                    card.transform.SetParent(transform, true);

                    card.DrawMoveTo(t.position);
                });
            }
            else
            {
                Debug.Log("当前位置没有可抽取卡牌");
            }
        }
    }

    public void Float(int index)
    {
        //foreach (Card card in Card.AllCard)
        //{
        //    card.Float(floatNode);
        //}

        Card card = group.Draw(index);

        card.Float(floatNode);
    }
}
