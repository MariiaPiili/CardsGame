using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uVegas.UI;

public class Player : MonoBehaviour
{
    public List<UICard> PlayersCards = new List<UICard>();
    public List<UICard> PlayersCardsSort = new List<UICard>();

    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button takeCardButton;
    [SerializeField] private Button BeatButton;
    [SerializeField] private CardsManager cardsManager;

    private void Update()
    {
        if (gameManager.CurrentPlayer != Players.Player && gameManager.isWaitingForAttack)
        {
            takeCardButton.gameObject.SetActive(true);
        }
        else
        {
            takeCardButton.gameObject.SetActive(false);
        }


        if (!gameManager.isWaitingForAttack && gameManager.CurrentPlayer == Players.Player && gameManager.CountCard > 0)
        {
            BeatButton.gameObject.SetActive(true);
        }
        else
        {
            BeatButton.gameObject.SetActive(false);
        }

        PlayersCardsSort.Clear();

        foreach (var item in PlayersCards)
        {
            PlayersCardsSort.Add(item);
        }

        PlayersCardsSort.Sort();

        for (int i = 0; i < PlayersCardsSort.Count; i++)
        {
            PlayersCardsSort[i].transform.parent.SetSiblingIndex(i);
        }
    }

    public void SelectCard(int cardNumber)
    {
        if (!gameManager.isWaitingForAttack)
        {
            if (gameManager.CurrentPlayer == Players.Player)
            {
                Debug.Log(cardNumber);
                gameManager.SetCard(PlayersCards[cardNumber].currentCard);
            }
        }
        else
        {
            if (gameManager.CurrentPlayer == Players.Bot)
            {
                //бот ждёт когда покрое плеер то есть здесь мы должны покрыть
                gameManager.CloseCard(PlayersCards[cardNumber].currentCard);
            }
        }
    }

    public void TakeCard()
    {
        cardsManager.TakeCards(PlayersCards, true);
        cardsManager.CustomizePlayerButtons();
    }
}
