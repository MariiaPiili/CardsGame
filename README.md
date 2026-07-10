# Durak Card Game

A digital implementation of the classic card game **Durak**, built with **Unity** and **C#**.

This project recreates the complete gameplay loop of the traditional game, including attacking, defending, trump card mechanics, AI opponent behaviour, turn management, automatic card replenishment, and game-over detection.

---

## Screenshot

<p align="center">
  <img src="Images/gameplay.png" width="700" alt="Gameplay Screenshot">
</p>

---

## Gameplay

The player competes against a rule-based AI opponent.

The game follows the original Durak rules:

- Shuffle and deal the deck.
- Determine the trump suit.
- Attack using a valid card.
- Defend with a higher card or a trump card.
- Take cards if defence is impossible.
- Replenish both hands after each round.
- Continue until one player runs out of cards.

---

## Features

- Classic Durak gameplay
- Rule-based AI opponent
- Trump card mechanics
- Turn-based gameplay
- Automatic deck shuffling and dealing
- Automatic hand replenishment
- Responsive card layout
- Win/Lose screen
- Restart functionality

---

## Technologies

- Unity
- C#
- Unity UI
- Object-Oriented Programming (OOP)

---

## Project Architecture

The project is organised into independent gameplay systems.

### GameManager

Controls the overall game flow.

Responsibilities:

- manages turn switching;
- validates attacks and defence;
- controls round progression;
- updates the current game state.

---

### CardsManager

Responsible for the deck and card lifecycle.

Responsibilities:

- shuffles the deck;
- deals cards;
- creates card UI;
- determines the trump card;
- replenishes players' hands;
- detects game-over conditions.

---

### Player

Handles player interaction.

Responsibilities:

- card selection;
- card sorting;
- UI button handling;
- taking cards.

---

### Bot

Implements a rule-based AI opponent.

The bot can:

- choose an attacking card;
- search for a valid defence card;
- take cards when necessary;
- perform actions with a short delay to simulate natural gameplay.

---

### UI

The interface automatically adjusts the card layout depending on the number of cards in the player's hand, ensuring the cards remain readable throughout the game.

---

## Key Learning Outcomes

During this project, I gained hands-on experience with:

- implementing turn-based gameplay mechanics;
- managing game state and gameplay flow;
- developing rule-based AI behaviour;
- implementing card game logic;
- designing modular gameplay architecture;
- working with dynamic collections using `List<T>`;
- building interactive UI in Unity;
- applying object-oriented programming principles.

---

## Author

**Maria Piili**

Unity Developer

- LinkedIn: www.linkedin.com/in/mariа-piili-49b71b17b
