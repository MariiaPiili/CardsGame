using System.Collections.Generic;
using UnityEngine;
using uVegas.UI;

public class Bot : MonoBehaviour
{
    public List<UICard> BotCards = new List<UICard>();

    [SerializeField] private GameManager gameManager;
    [SerializeField] private CardsManager cardsManager;

    private float allTime;

    private void Update()
    {
        if (gameManager.CurrentPlayer == Players.Bot)
        {
            if (!gameManager.isWaitingForAttack)
            {
                allTime += Time.deltaTime;
                if (gameManager.CountCard == 0)
                {
                    if (allTime >= 2f)
                    {
                        allTime = 0;
                        gameManager.SetCard(BotCards[Random.Range(0, BotCards.Count)].currentCard);
                    }
                }
                else
                {
                    {
                        allTime = 0;
                        foreach (var card in BotCards)
                        {
                            if (gameManager.SetCard(card.currentCard))
                            {
                                return;
                            }
                        }
                    }

                }
            }
        }
        else
        {
            if (gameManager.isWaitingForAttack)
            {
                allTime += Time.deltaTime;
                if (allTime >= 2f)
                {
                    allTime = 0;
                    for (int i = 0; i < BotCards.Count; i++)
                    {
                        bool successClose = gameManager.CloseCard(BotCards[i].currentCard);
                        if (successClose)
                        {
                            return;
                        }
                    }

                    cardsManager.TakeCards(BotCards, false);
                }

            }
        }
    }
}

