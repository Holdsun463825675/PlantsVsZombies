using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public bool isShovel;

    private float UIMoveTime = 0.2f;
    private float selectionCardMoveTime = 0.2f;
    private float generateCardTime, generateCardTimer;
    private float conveyorCardMoveSpeed = 0.5f;

    private List<PlantID> fixedCards;

    public GameObject cardListUI;
    public GameObject cardPanelUI;
    public GameObject slotUI;
    public GameObject ConveyorUI;

    private List<Card> cardList = new List<Card>();
    public List<Transform> cardListCardPlace;
    public List<Card> cardPanel;
    public List<Transform> cardPanelCardPlace;

    public Transform cardListUIBeginPlace;
    public Transform cardListUIEndPlace;
    public Transform cardPanelUIBeginPlace;
    public Transform cardPanelUIEndPlace;
    public Transform slotPlace;
    public Button readyButton;

    private void Awake()
    {
        Instance = this;
        generateCardTimer = 0.0f;
        showCards();
        setState(GameState.NotStarted);
    }

    private Dictionary<Card, Tween> activeCardTweens = new Dictionary<Card, Tween>();

    private void Update()
    {
        switch (GameManager.Instance.state)
        {
            case GameState.SelectingCard:
                SelectingCardUpdate();
                break;
            case GameState.Processing:
                ProcessingUpdate();
                break;
            default:
                break;
        }
    }

    public void Pause()
    {
        foreach (Card card in cardList) if (card) card.transform.DOPause();
    }

    public void Continue()
    {
        foreach (Card card in cardList) if (card) card.transform.DOPlay();
    }

    private void MoveCardWithTween(Card card, Transform targetPlace, float moveTime, Ease ease=Ease.OutCubic)
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
        Tween tween = card.transform.DOMove(targetPlace.position, moveTime)
            .SetEase(ease)
            .OnComplete(() =>
            {
                // Tween完成后的清理
                if (activeCardTweens.ContainsKey(card)) activeCardTweens.Remove(card);
            });
        activeCardTweens[card] = tween;
    }

    private void SelectingCardUpdate()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            MoveCardWithTween(cardList[i], cardListCardPlace[i], selectionCardMoveTime);
        }

        bool selectedFlag = true; // 卡片是否都被选了
        for (int i = 0; i < cardPanel.Count; i++)
        {
            if (!cardList.Contains(cardPanel[i]))
            {
                MoveCardWithTween(cardPanel[i], cardPanelCardPlace[i], selectionCardMoveTime);
                if (cardPanel[i].gameObject.activeSelf) selectedFlag = false;
            }
        }

        // 卡槽选满或卡片全被选才能开始游戏
        if (cardList.Count == cardListCardPlace.Count || selectedFlag) readyButton.enabled = true;
        else readyButton.enabled = false;
    }

    private void ProcessingUpdate()
    {
        generateCardTimer += Time.deltaTime;
        if (generateCardTimer >= generateCardTime)
        {
            generateCard();
            generateCardTimer = 0.0f;
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            float moveTime = Vector3.Distance(cardList[i].transform.position, cardListCardPlace[i].transform.position) / conveyorCardMoveSpeed;
            MoveCardWithTween(cardList[i], cardListCardPlace[i], moveTime, Ease.Linear);
        }
    }

    private void showCards()
    {
        if (!JSONSaveSystem.Instance) // 测试用
        {
            foreach (Card card in cardPanel) if (card) card.gameObject.SetActive(true);
            return;
        }

        foreach (Card card in cardPanel)
        {
            if (JSONSaveSystem.Instance.userData.unlockedPlants.Contains(card.plantID)) card.gameObject.SetActive(true);
            else card.gameObject.SetActive(false);
        }
    }

    public void setConfigs(List<PlantID> fixedCards, bool shovel, float generateCardTime)
    {
        this.fixedCards = fixedCards;
        if (GameManager.Instance.currLevelConfig.cardType == TypeOfCard.Autonomy || GameManager.Instance.currLevelConfig.cardType == TypeOfCard.Fixation)
        {
            foreach (PlantID ID in this.fixedCards)
            {
                foreach (Card card in cardPanel)
                {
                    if (card.plantID == ID)
                    {
                        addCard(card);
                        card.setState(CardState.SelectingCard_Selected);
                        card.GetComponent<Button>().enabled = false;
                    }
                }
            }
        }

        isShovel = shovel;
        if (JSONSaveSystem.Instance) isShovel &= JSONSaveSystem.Instance.userData.shovel;
        this.generateCardTime = generateCardTime;
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.NotStarted:
                break;
            case GameState.Previewing:
                cardListUI.SetActive(false);
                cardPanelUI.SetActive(false);
                slotUI.SetActive(false);
                cardListUI.transform.position = cardListUIBeginPlace.position;
                cardPanelUI.transform.position = cardPanelUIBeginPlace.position;
                break;
            case GameState.SelectingCard:
                slotUI.SetActive(false);
                foreach (Card card in cardList) if (card) card.setState(CardState.SelectingCard_Selected);
                cardListUI.SetActive(GameManager.Instance.currLevelConfig.cardType == TypeOfCard.Autonomy);
                cardListUI.transform.DOMove(cardListUIEndPlace.position, UIMoveTime);
                foreach (Card card in cardPanel) if (card) card.setState(CardState.SelectingCard_NotSelected);
                cardPanelUI.SetActive(GameManager.Instance.currLevelConfig.cardType == TypeOfCard.Autonomy);
                cardPanelUI.transform.DOMove(cardPanelUIEndPlace.position, UIMoveTime)
                    .OnComplete(() => {
                        if (GameManager.Instance.currLevelConfig.cardType != TypeOfCard.Autonomy) setState(GameState.Ready);
                    });
                break;
            case GameState.Ready:
                slotUI.SetActive(false);
                foreach (Card card in cardList)
                {
                    if (card)
                    {
                        card.GetComponent<Button>().enabled = true;
                        card.transform.SetParent(cardListUI.transform);
                        card.setState(CardState.GameReady);
                    }
                }
                cardPanelUI.transform.DOMove(cardPanelUIBeginPlace.position, UIMoveTime).OnComplete(() => GameManager.Instance.setState(GameState.Ready));
                break;
            case GameState.Processing:
                foreach (Card card in cardList) if (card) card.setState(CardState.CoolingDown);
                if (GameManager.Instance.currLevelConfig.cardType == TypeOfCard.Conveyor) ConveyorUI.SetActive(true);
                else cardListUI.SetActive(true);
                cardPanelUI.SetActive(false);
                slotUI.SetActive(isShovel);
                break;
            case GameState.Paused:
                break;
            case GameState.Losing:
                cardListUI.SetActive(false);
                cardPanelUI.SetActive(false);
                slotUI.SetActive(false);
                ConveyorUI.SetActive(false);
                break;
            case GameState.Winning:
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

    public List<Card> getCardList()
    {
        return cardList;
    }

    private void generateCard()
    {
        PlantID ID = fixedCards[Random.Range(0, fixedCards.Count)];
        Card cardPrefab = null;
        foreach (Card card in cardPanel)
        {
            if (card.plantID == ID) cardPrefab = card;
        }
        Card newCard = GameObject.Instantiate(cardPrefab, cardListCardPlace[cardListCardPlace.Count - 1].position, Quaternion.identity);
        bool flag = addCard(newCard);
        if (flag)
        {
            newCard.setState(CardState.Ready);
            newCard.transform.SetParent(ConveyorUI.transform);
        } 
        else Destroy(newCard);
    }
}