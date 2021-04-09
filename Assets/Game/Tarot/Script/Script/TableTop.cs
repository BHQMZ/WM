using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTop : MonoBehaviour
{
    public Group group;

    public Transform showCardNode;

    public List<Transform> CardArray; 

    //抽取的卡牌
    private List<Card> drawList = new List<Card>();

    void Start()
    {
        EventManager.On("DrawCard", onDrawCard);
        EventManager.On("ShowCard", onShowCard);
    }
    void Update()
    {
        
    }

    private void onDrawCard(object index)
    {
        Draw((int)index);
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
}
