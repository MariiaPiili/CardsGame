using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uVegas.Core.Cards;
using uVegas.UI;

public enum Players
{
    Player,
    Bot
}

public class GameManager : MonoBehaviour
{
    public Players CurrentPlayer;

    public List<UICard> attackCards = new List<UICard>();
    public List<UICard> defenseCards = new List<UICard>();

    public int CountCard;
    public bool isWaitingForAttack;

    [SerializeField] private CardsManager cardsManager;
    [SerializeField] private CardTheme theme;

    public bool SetCard(Card card)
    {
        cardsManager.CheckGameOver();
        if (CountCard == 0)
        {
            return SettingCard(card);
        }
        else
        {
            if (CanGiveCards(card))
            {
                StopAllCoroutines();

                return SettingCard(card);
            }
            return false;
        }
    }

    private bool SettingCard(Card card)
    {
        defenseCards[CountCard].gameObject.SetActive(true);
        defenseCards[CountCard].Init(card, theme);
        isWaitingForAttack = true;
        cardsManager.RemoveCards(card);

        return true;
    }

    public bool CloseCard(Card card)
    {
        cardsManager.CheckGameOver();
        Suit suitTrumpCard = cardsManager.TrumpCardUI.currentCard.suit;
        Suit suitToCloseCard = defenseCards[CountCard].currentCard.suit;
        Rank rankToCloseCard = defenseCards[CountCard].currentCard.rank;

        if (card.suit == suitToCloseCard && card.rank > rankToCloseCard)
        {
            return ClosingCard(card);
        }
        if (suitToCloseCard == suitTrumpCard && card.suit == suitTrumpCard && card.rank > rankToCloseCard)
        {
            return ClosingCard(card);
        }
        if (card.suit == suitTrumpCard && suitToCloseCard != suitTrumpCard)
        {
            return ClosingCard(card);
        }

        return false;
    }

    private bool ClosingCard(Card card)
    {
        attackCards[CountCard].gameObject.SetActive(true);
        attackCards[CountCard].Init(card, theme);
        CountCard++;

        cardsManager.RemoveCards(card);
        isWaitingForAttack = false;
        StartCoroutine(CallChangeMainPlayer());

        return true;
    }

    public void ChangeMainPlayer()
    {
        ClearCardsInGame();

        if (CurrentPlayer == Players.Player)
        {
            CurrentPlayer = Players.Bot;
        }
        else
        {
            CurrentPlayer = Players.Player;
        }

        CountCard = 0;

        isWaitingForAttack = false;
    }

    IEnumerator CallChangeMainPlayer()
    {
        while (CurrentPlayer == Players.Bot)
        {
            yield return new WaitForSeconds(1f);

            ChangeMainPlayer();
            yield return null;
        }
    }

    public void ClearCardsInGame()
    {
        defenseCards.ForEach(card => { card.gameObject.SetActive(false); });
        attackCards.ForEach(card => { card.gameObject.SetActive(false); });

        CountCard = 0;
        isWaitingForAttack = false;

        cardsManager.AddCards();
    }

    private bool CanGiveCards(Card card)
    {
        List<Rank> ranks = new List<Rank>();

        for (int i = 0; i < defenseCards.Count; i++)
        {
            if (defenseCards[i].gameObject.activeSelf)
            {
                ranks.Add(defenseCards[i].currentCard.rank);
            }
        }

        for (int i = 0; i < attackCards.Count; i++)
        {
            if (attackCards[i].gameObject.activeSelf)
            {
                ranks.Add(attackCards[i].currentCard.rank);
            }
        }

        return ranks.Contains(card.rank) && CountCard < 6;
    }
}
