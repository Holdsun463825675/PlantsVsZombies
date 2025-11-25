using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public GameObject cardListUI;
    public GameObject cardPanelUI;

    private float UIMoveTime = 0.2f;
    private float cardMoveTime = 0.2f;
    private List<Card> cardList = new List<Card>();
    public List<Transform> cardListCardPlace;
    public List<Card> cardPanel;
    public List<Transform> cardPanelCardPlace;

    public Transform cardListUIBeginPlace;
    public Transform cardListUIEndPlace;
    public Transform cardPanelUIBeginPlace;
    public Transform cardPanelUIEndPlace;
    public Button readyButton;

    private void Awake()
    {
        Instance = this;
        showCards();
        setState(GameState.NotStarted);
    }

    private Dictionary<Card, Tween> activeCardTweens = new Dictionary<Card, Tween>();

    private void Update()
    {
        if (GameManager.Instance.state == GameState.SelectingCard)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                MoveCardWithTween(cardList[i], cardListCardPlace[i], cardListUI.transform);
            }

            bool selectedFlag = true; // 卡片是否都被选了
            for (int i = 0; i < cardPanel.Count; i++)
            {
                if (!cardList.Contains(cardPanel[i]))
                {
                    MoveCardWithTween(cardPanel[i], cardPanelCardPlace[i], cardPanelUI.transform);
                    if (cardPanel[i].gameObject.activeSelf) selectedFlag = false;
                }
            }

            // 卡槽选满或卡片全被选才能开始游戏
            if (cardList.Count == cardListCardPlace.Count || selectedFlag) readyButton.enabled = true;
            else readyButton.enabled = false;
        }
    }

    private void MoveCardWithTween(Card card, Transform targetPlace, Transform parent)
    {
        // 如果目标位置与当前位置相同，跳过
        if (card.transform.position == targetPlace.position) return;

        // 检查是否已经有活跃的Tween
        if (activeCardTweens.ContainsKey(card))
        {
            var existingTween = activeCardTweens[card];
            if (existingTween != null && existingTween.IsActive())
            {
                // 如果目标位置没变，继续当前的Tween
                if (existingTween is Tweener tweener) return;
            }
            else activeCardTweens.Remove(card);
        }

        // 创建新的Tween
        Tween tween = card.transform.DOMove(targetPlace.position, cardMoveTime)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                // Tween完成后的清理
                if (activeCardTweens.ContainsKey(card)) activeCardTweens.Remove(card);
            });

        activeCardTweens[card] = tween;
    }

    private void showCards()
    {
        if (!JSONSaveSystem.Instance) // 测试用
        {
            foreach (Card card in cardPanel) card.gameObject.SetActive(true);
            return;
        }

        foreach (Card card in cardPanel)
        {
            if (JSONSaveSystem.Instance.userData.unlockedPlants.Contains(card.plantID)) card.gameObject.SetActive(true);
            else card.gameObject.SetActive(false);
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
                cardListUI.transform.position = cardListUIBeginPlace.position;
                cardPanelUI.transform.position = cardPanelUIBeginPlace.position;
                break;
            case GameState.SelectingCard:
                foreach (Card card in cardList) if (card) card.setState(CardState.SelectingCard_Selected);
                cardListUI.SetActive(true);
                cardListUI.transform.DOMove(cardListUIEndPlace.position, UIMoveTime);
                foreach (Card card in cardPanel) if (card) card.setState(CardState.SelectingCard_NotSelected);
                cardPanelUI.SetActive(true);
                cardPanelUI.transform.DOMove(cardPanelUIEndPlace.position, UIMoveTime);

                break;
            case GameState.Ready:
                foreach (Card card in cardList)
                {
                    if (card)
                    {
                        card.transform.SetParent(cardListUI.transform);
                        card.setState(CardState.GameReady);
                    } 
                }
                cardListUI.SetActive(true);
                cardPanelUI.transform.DOMove(cardPanelUIBeginPlace.position, UIMoveTime).OnComplete(() => GameManager.Instance.setState(GameState.Ready));
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

    public void onCompleteSelectingCard()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_tap);
        setState(GameState.Ready);
    }
}
