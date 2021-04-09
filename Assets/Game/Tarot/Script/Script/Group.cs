using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    //测试用
    //public Card TestCard;

    //卡组
    private List<Card> cardList = new List<Card>();

    public new Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();

        Card.CreateAllCard(()=> {
            foreach (Card card in Card.AllCard)
            {
                card.transform.SetParent(transform, false);

                cardList.Add(card);
            }

            //创建卡牌完成，洗牌
            ShuffleCards();
        });
    }

    void Update()
    {

    }

    void OnMouseDown()
    {

        foreach (Card card in cardList)
        {
            card.SpreadOut();
        }

        collider.enabled = false;
    }

    public void ShuffleCards()
    {
        Debug.Log("开始洗牌");

        if (cardList.Count > 0)
        {
            List<Card> newCards = GameUtils.RandomSortList(cardList);

            int index = 1;

            foreach (Card card in newCards)
            {
                card.Shuffle(index++);
            }

            cardList = newCards;
        }

        UpdateGroup();
    }

    private void UpdateGroup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Card card = transform.GetChild(i).GetComponent<Card>();

            int index = cardList.IndexOf(card);

            card.transform.localPosition = new Vector3(0, 0, 0.00151f * (index + 1 - cardList.Count));
        }
    }

    public Card Draw()
    {
        return Draw(cardList[0].dataSource.index);
    }

    public Card Draw(int index)
    {
        Card card = cardList.Find(value => { return value.dataSource.index == index; });

        if (card != null)
        {
            //将抽取的卡牌从牌组队列中去除
            cardList.Remove(card);

            //抽取后需要更新卡组卡牌堆叠位置
            //UpdateGroup();

            CardVO vo = card.dataSource;

            Debug.Log("抽取 " + vo.name + ":" + (vo.isReversed ? "逆位" : "正位"));

            return card;
        }
        else
        {
            Debug.Log("该位置没有牌");

            return null;
        }
    }
}
