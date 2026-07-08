using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using uVegas.Core.Cards;
using uVegas.UI;

public class CardsManager : MonoBehaviour
{
    public UICard TrumpCardUI;
    public UICard MixedCardsObject;

    [SerializeField] private List<Card> cards = new List<Card>();
    [SerializeField] private List<Card> mixedCards = new List<Card>();
    [SerializeField] private CardTheme theme;
    [SerializeField] private GameObject playerParent;
    [SerializeField] private GameObject botParent;
    [SerializeField] private GameObject prefabCard;
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject finalScreen;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button restartButton;

    private bool isGameOver = false;

    void Start()
    {
        MixCards();
        ShareCards();
        CustomizeTrumpCard();

        restartButton.onClick.AddListener(RestartGame);
    }

    //[ContextMenu ("A")]
    //public void A()
    //{
    //    for (int i = 1; i <= 4; i++)
    //    {
    //        for(int j = 6; j <=14; j++)
    //        {
    //            cards.Add(new Card((Suit)i, (Rank)j));
    //        }
    //    }
    //}

    private void MixCards()
    {
        System.Random random = new System.Random();

        mixedCards = cards.OrderBy(_ => random.Next()).ToList(); //каждому элементу дает рандомное число и сортирует по этим числам
        //для OrderBy нам нужен каой-то элемент, а для нашей гененации нам не важно знать эоемент, поэтому ставим _ дискарт. намеренно игнорируем использование элемента
    }

    private void ShareCards()
    {
        for (int i = 0; i < 12; i++)
        {
            Card card = mixedCards[i];
            mixedCards.Remove(card);
            if (i % 2 != 0)
            {
                CreateCard(card, playerParent, player.PlayersCards, false);
            }
            else
            {
                CreateCard(card, botParent, bot.BotCards, true);
            }
        }

        CustomizePlayerButtons();
    }

    private void CustomizeTrumpCard()
    {
        Card trumpCard = mixedCards[0];
        TrumpCardUI.Init(trumpCard, theme);

        Card temp = mixedCards[0];
        mixedCards[0] = mixedCards[23];
        mixedCards[23] = temp;
    }

    private UICard CreateCard(Card card, GameObject parent, List<UICard> UICards, bool isHidden)
    {
        GameObject spawnedCard = Instantiate(prefabCard, parent.transform);
        UICard uiCard = spawnedCard.transform.GetChild(0).GetComponent<UICard>();
        uiCard.Init(card, theme, isHidden);
        UICards.Add(uiCard);

        return uiCard;
    }

    public void RemoveCards(Card card)
    {
        RemoveCards(player.PlayersCards, card);
        RemoveCards(bot.BotCards, card);
        CustomizePlayerButtons();
    }

    private void RemoveCards(List<UICard> UICards, Card card)
    {
        for (int i = 0; i < UICards.Count; i++)
        {
            if (UICards[i].currentCard == card)
            {
                Destroy(UICards[i].transform.parent.gameObject);
                UICards.RemoveAt(i);

                break;
            }
        }
    }

    public void CustomizePlayerButtons()
    {
        for (int j = 0; j < player.PlayersCards.Count; j++)
        {
            Button buttonPlayer = player.PlayersCards[j].GetComponent<Button>();
            int cardIndex = j;
            buttonPlayer.onClick.RemoveAllListeners();
            buttonPlayer.onClick.AddListener(() => player.SelectCard(cardIndex));
        }
    }

    public void AddCards()
    {
        if (mixedCards.Count > 0)
        {
            Players mainPlayer = gameManager.CurrentPlayer;

            if (mainPlayer == Players.Player)
            {
                AddCards(player.PlayersCards, playerParent, false);
                CustomizePlayerButtons();
                AddCards(bot.BotCards, botParent, true);
            }
            else
            {
                AddCards(bot.BotCards, botParent, true);
                AddCards(player.PlayersCards, playerParent, false);
                CustomizePlayerButtons();
            }
        }

        if (mixedCards.Count == 0)
        {
            TrumpCardUI.gameObject.SetActive(false);
            MixedCardsObject.gameObject.SetActive(false);
        }

        
    }

    private void AddCards(List<UICard> UICards, GameObject parent, bool isHidden)
    {
        if (UICards.Count < 6)
        {
            int countNeededCards = 6 - UICards.Count;

            if (countNeededCards > mixedCards.Count)
            {
                countNeededCards = mixedCards.Count;
            }

            for (int i = 0; i < countNeededCards; i++)
            {
                Card card = null;
                card = mixedCards[0];
                mixedCards.Remove(card);

                CreateCard(card, parent, UICards, isHidden);
            }
        }
    }

    public void TakeCards(List<UICard> UICards, bool isPlayer)
    {
        GameObject parent = isPlayer ? playerParent : botParent;
        List<UICard> allActiveCards = new List<UICard>();
        allActiveCards.AddRange(gameManager.defenseCards);
        allActiveCards.AddRange(gameManager.attackCards);

        foreach (var item in allActiveCards)
        {
            if (!item.gameObject.activeSelf)
            {
                continue;
            }

            Card card = item.currentCard;

            bool isHidden = !isPlayer;
            CreateCard(card, parent, UICards, isHidden);
        }

        gameManager.ClearCardsInGame();
    }

    public void CheckGameOver()
    {
        if (isGameOver)
            return;

        if (mixedCards.Count > 0)
            return;

        if (player.PlayersCards.Count == 0)
        {
            isGameOver = true;
            Debug.Log("You won");

            finalScreen.SetActive(true);
            resultText.text = "YOU WON!";
        }

        if (bot.BotCards.Count == 0)
        {
            isGameOver = true;
            Debug.Log("You lost");

            finalScreen.SetActive(true);
            resultText.text = "YOU LOST!";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
