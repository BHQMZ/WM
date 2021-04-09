using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCardDialog : DialogBase<ShowCardDialog>
{
    public static string url = "Dialog/ShowCardDialog";

    private Vector3[] cardDefault = new Vector3[3];
    public Transform showCardNode;
    private CardCollider card;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnDrag()
    {
        float offsetX = Input.GetAxis("Mouse X");

        float offsetY = Input.GetAxis("Mouse Y");

        card.transform.Rotate(new Vector3(offsetY, -offsetX, 0) * 10, Space.World);

        Debug.Log("1");
    }

    public void OnEndDrag()
    {
        card.RotateTo(showCardNode.eulerAngles);

        Debug.Log("2");
    }

    public void ShowCard(CardCollider card, Transform showCardNode)
    {
        Transform cardT = card.gameObject.transform;
        cardDefault[0] = cardT.position;
        cardDefault[1] = cardT.eulerAngles;
        cardDefault[2] = cardT.lossyScale;

        this.card = card;
        this.showCardNode = showCardNode;

        card.Show(showCardNode.position, showCardNode.eulerAngles, showCardNode.lossyScale);

        Vector3 vector3 = Camera.main.WorldToScreenPoint(showCardNode.position);

        Debug.Log(vector3);
    }

    public void HideCard()
    {
        card.Hide(cardDefault[0], cardDefault[1], cardDefault[2]);

        Hide();
    }

    public static void Show(CardCollider card,Transform showCardNode)
    {
        Show((alert) => {
            alert.ShowCard(card, showCardNode);
        });
    }
}
