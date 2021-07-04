using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//卡牌核心脚本
public class Card : MonoBehaviour
{
    public static string url = "Prefab/Card";

    public static void CREATE(UnityAction<GameObject> onComplete = null)
    {
        LoadManager.LoadGameObject(url, "Card", (card) => {
            if (card)
            {
                //DestroyImmediate(card, true);

                onComplete?.Invoke(card);
            }
            else
            {
                Debug.Log("创建卡牌失败");
            }
        });
    }

    private static Card[] _AllCard;
    public static Card[] AllCard
    {
        get
        {
            return _AllCard;
        }
    }

    public static void CreateAllCard(UnityAction onComplete = null)
    {
        string[] names = {"愚人","魔术师","女祭司","皇后","皇帝","教皇","恋人","战车","力量","隐士","命运之轮","正义","吊人","死神","节制","恶魔","高塔","星星","月亮","太阳","审判","世界",
                        "圣杯1", "圣杯2", "圣杯3", "圣杯4", "圣杯5", "圣杯6", "圣杯7", "圣杯8", "圣杯9", "圣杯10","圣杯侍从","圣杯骑士","圣杯王后","圣杯国王",
                        "宝剑1", "宝剑2", "宝剑3", "宝剑4", "宝剑5", "宝剑6", "宝剑7", "宝剑8", "宝剑9", "宝剑10","宝剑侍从","宝剑骑士","宝剑王后","宝剑国王",
                        "星币1", "星币2", "星币3", "星币4", "星币5", "星币6", "星币7", "星币8", "星币9", "星币10","星币侍从","星币骑士","星币王后","星币国王",
                        "权杖1", "权杖2", "权杖3", "权杖4", "权杖5", "权杖6", "权杖7", "权杖8", "权杖9", "权杖10","权杖侍从","权杖骑士","权杖王后","权杖国王"};

        string[] fronts = {"TheFool","TheMagician","TheHighPriestess","TheEmpress","TheEmperor","TheHierophant","TheLovers","TheChariot","Strength","TheHermit","TheWheelOfFortune","Justice","TheHangedMan","Death","Temperance","TheDevil","TheTower","TheStar","TheMoon","TheSun","Judgement","TheWorld",
        "AceOfCups", "TwoOfCups", "ThreeOfCups", "FourOfCups", "FiveOfCups", "SixOfCups", "SevenOfCups", "EightOfCups", "NineOfCups", "TenOfCups", "PageOfCups", "KnightOfCups", "QueenOfCups", "KingOfCups",
        "AceOfSwords", "TwoOfSwords", "ThreeOfSwords", "FourOfSwords", "FiveOfSwords", "SixOfSwords", "SevenOfSwords", "EightOfSwords", "NineOfSwords", "TenOfSwords", "PageOfSwords", "KnightOfSwords", "QueenOfSwords", "KingOfSwords",
        "AceOfCoins", "TwoOfCoins", "ThreeOfCoins", "FourOfCoins", "FiveOfCoins", "SixOfCoins", "SevenOfCoins", "EightOfCoins", "NineOfCoins", "TenOfCoins", "PageOfCoins", "KnightOfCoins", "QueenOfCoins", "KingOfCoins",
        "AceOfWands", "TwoOfWands", "ThreeOfWands", "FourOfWands", "FiveOfWands", "SixOfWands", "SevenOfWands", "EightOfWands", "NineOfWands", "TenOfWands", "PageOfWands", "KnightOfWands", "QueenOfWands", "KingOfWands"};

        List<CardVO> vos = new List<CardVO>();
        for (int i = 0; i < names.Length; i++)
        {
            CardVO vo = new CardVO();
            vo.name = names[i];
            vo.cardFront = fronts[i];

            vos.Add(vo);
        }

        CREATE((gb) =>
        {
            Card[] cards = new Card[vos.Count];
            for (int i = 0; i < vos.Count; i++)
            {
                Card card = Instantiate(gb).GetComponent<Card>();

                card.dataSource = vos[i];

                card.gameObject.SetActive(true);

                cards[i] = card;
            }

            _AllCard = cards;

            onComplete?.Invoke();
        });
    }

    public CardView cardView;
    public CardCollider cardCollider;

    void Awake()
    {
        if (!cardView)
        {
            Debug.LogError("卡牌缺少视图脚本");
        }
        if (!cardCollider)
        {
            Debug.LogError("卡牌缺少碰撞体脚本");
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private CardVO _dataSource;

    public CardVO dataSource
    {
        get
        {
            return _dataSource;
        }
        set
        {
            if (_dataSource != value)
            {
                _dataSource = value;

                //设置视图数据
                cardView.dataSource = value;

                cardCollider.dataSource = value;
            }
        }
    }

    public CARD_STATE state
    {
        get
        {
            return dataSource.state;
        }
        set
        {
            dataSource.state = value;
        }
    }

    public bool isReversed
    {
        get
        {
            return dataSource.isReversed;
        }
        set
        {
            dataSource.isReversed = value;
        }
    }

    /// <summary>
    /// 洗牌（设置卡牌位置并随机正逆位）
    /// </summary>
    public void Shuffle(int index)
    {
        dataSource.index = index;

        isReversed = UnityEngine.Random.Range(0f, 1f) >= 0.5;

        ResetCard();
    }

    public void ResetCard()
    {
        cardCollider.ResetState();

        transform.localEulerAngles = Vector3.zero;
    }

    public void Draw(Action onComplete = null)
    {
        cardCollider.Draw(onComplete);
    }

    public void DrawMoveTo(Vector3 position)
    {
        cardCollider.DrawMoveTo(position);
    }

    public void SpreadOut()
    {
        int count = Card.AllCard.Length;
        int index = dataSource.index;
        Vector3 meshSize = cardView.meshSize;

        float interval = 1.4f / count;

        float x = (index - count) * interval;
        float y = 0;

        double r = 0;

        if (index != count)
        {
            r = Math.Atan2(meshSize.z, interval);

            y = (float)(Math.Sin(r) * (meshSize.x / 2));
        }

        Vector3 position = transform.parent.TransformPoint(new Vector3(x, 0, -y));
        Vector3 rotation = new Vector3(0, (float)(r * 180 / Math.PI), 0);

        cardCollider.SpreadOut(position, rotation);
    }

    public void Float(Transform targetT)
    {
        transform.SetParent(targetT, true);

        Vector3[] circlePath = GameUtils.DrawCircle(Vector3.zero,0.5f,0.001f);

        Vector3[] path = new Vector3[circlePath.Length + 1];

        for (int i = 0; i < circlePath.Length; i++)
        {
            path[i] = targetT.TransformPoint(circlePath[i]);
        }

        path[circlePath.Length] = path[0];

        cardCollider.Float(Vector3.zero, path);
    }
}
