using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTop : MonoBehaviour
{
    public Group group;

    public List<Transform> CardArray; 

    //抽取的卡牌
    private List<Card> drawList = new List<Card>();

    void Start()
    {
        EventManager.On("DrawCard", onDrawCard);
    }
    void Update()
    {
        
    }

    private void onDrawCard(object index)
    {
        Draw((int)index);
    }

    public void Draw(int index)
    {
        if (drawList.Count < CardArray.Count)
        {
            Card card = group.Draw(index);
            if (card)
            {
                card.transform.SetParent(transform, true);

                drawList.Add(card);

                MoveToCardArray(card, drawList.Count - 1);
            }
            else
            {
                Debug.Log("当前位置没有可抽取卡牌");
            }
        }
    }

    private void MoveToCardArray(Card card, int index)
    {
        Transform t = CardArray[index];
        card.Draw(t.position);
    }
}
