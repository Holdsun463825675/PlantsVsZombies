using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public GameObject cardListUI;
    public GameObject cardPanelUI;

    private List<Card> cardList = new List<Card>();
    public List<Card> cardPanel;

    public List<Transform> cardListCardPlace;
    public List<Transform> cardPanelCardPlace;

    private void Awake()
    {
        Instance = this;
        setState(GameState.NotStarted);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.state == GameState.SelectingCard)
        {
            // 设置位置与父物体
            for (int i = 0; i < cardList.Count; i++)
            {
                moveToPlace(cardList[i], cardListCardPlace[i]);
                cardList[i].transform.SetParent(cardListUI.transform);
            }
            for (int i = 0; i < cardPanel.Count; i++)
            {
                if (!cardList.Contains(cardPanel[i]))
                {
                    moveToPlace(cardPanel[i], cardPanelCardPlace[i]);
                    cardPanel[i].transform.SetParent(cardPanelUI.transform);
                } 
            } 
        }
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.NotStarted:
                cardListUI.SetActive(false);
                cardPanelUI.SetActive(false);
                break;
            case GameState.Previewing:
                cardListUI.SetActive(false);
                cardPanelUI.SetActive(false);
                break;
            case GameState.SelectingCard:
                foreach (Card card in cardList) if (card) card.setState(CardState.SelectingCard_Selected);
                cardListUI.SetActive(true);
                foreach (Card card in cardPanel) if (card) card.setState(CardState.SelectingCard_NotSelected);
                cardPanelUI.SetActive(true);
                break;
            case GameState.Ready:
                foreach (Card card in cardList) if (card) card.setState(CardState.GameReady);
                cardListUI.SetActive(true);
                cardPanelUI.SetActive(false);
                break;
            case GameState.Processing:
                foreach (Card card in cardList) if (card) card.setState(CardState.CoolingDown);
                cardListUI.SetActive(true);
                cardPanelUI.SetActive(false);
                break;
            case GameState.Paused:
                cardListUI.SetActive(true);
                cardPanelUI.SetActive(false);
                break;
            case GameState.Losing:
                cardListUI.SetActive(false);
                cardPanelUI.SetActive(false);
                break;
            case GameState.Winning:
                cardListUI.SetActive(false);
                cardPanelUI.SetActive(false);
                break;
            default:
                break;
        }
    }

    public bool addCard(Card card)
    {
        if (cardList.Contains(card) || cardList.Count == cardListCardPlace.Count) return false;
        cardList.Add(card);
        return true;
    }

    public bool removeCard(Card card)
    {
        if (cardList.Contains(card))
        {
            cardList.Remove(card);
            return true;
        }
        return false;
    }

    private void moveToPlace(Card card, Transform trans)
    {
        card.transform.position = trans.transform.position;
    }

    public void onCompleteSelectingCard()
    {
        GameManager.Instance.setState(GameState.Ready);
    }
}
