﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Security;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace GameOfWars
{
    internal class GameOfWar
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
@"
================================================================================
||                      Welcome to the Game of                                ||
|| 			                                                      ||                                          
|| HOW TO PLAY: 						              ||
|| + Each of the two players are dealt one half of a shuffled deck of cards.  ||
|| + Each turn, each player draws one card from their deck.                   ||
|| + The player that drew the card with higher value gets both cards.         ||
|| + Both cards return to the winner&#39;s deck.                              ||
|| + If there is a draw, both players place the next three cards face down    ||
|| and then another card face-up. The owner of the higher face-up             ||
|| card gets all the cards on the table.                                      ||
||                                                                            ||
|| HOW TO WIN:                                                                ||
|| + The player who collects all the cards wins.                              ||
||                                                                            ||
|| CONTROLS:                                                                  ||
|| + Press [Enter] to draw a new card until we have a winner.                 ||
||                                                                            ||
||                              Have fun!                                     ||
================================================================================"
);

            List<Cards> deck = GenerateDeck();
            ShuffleDeck(deck);
            
            Queue<Cards> firstPlayerDeck = new Queue<Cards>();
            Queue<Cards> secondPlayerDeck = new Queue<Cards>();

            DealCardsToPlayers();

            Cards firstPlayerCard;
            Cards secondPlayerCard;

            int totalMoves = 0;

            while (!GameHasWinner())
            {
                Console.ReadLine();
                DrawPlayerCards();

                Queue<Cards> pool = new Queue<Cards>();
                pool.Enqueue(firstPlayerCard);
                pool.Enqueue(secondPlayerCard);

                ProcessWar(pool);
                DetermineRoundWinner(pool);

                Console.WriteLine("================================================================================");
                Console.WriteLine($"First player currently has {firstPlayerDeck.Count} cards.");
                Console.WriteLine($"Second player currently has {secondPlayerDeck.Count} cards.");
                Console.WriteLine("================================================================================");
                totalMoves++;
            }
             List<Cards> GenerateDeck()
            {
                List<Cards> deck = new List<Cards>();
                CardFace[] faces = (CardFace[])Enum.GetValues(typeof(CardFace));
                CardSuit[] suits = (CardSuit[])Enum.GetValues(typeof(CardSuit));
                
                for(int suit = 0; suit< suits.Length; suit++)
                {
                    for(int face = 0; face< faces.Length; face++)
                    {
                        CardFace currentFace = faces[face];
                        CardSuit currentSuit = suits[suit];
                        deck.Add(new Cards
                        {
                            Face = currentFace,
                            Suite = currentSuit
                        });
                    }
                  
                }

                
                return deck;
            }
            void ShuffleDeck(List<Cards> deck)
            {
                Random random = new Random();   
                for(int i = 0; i< deck.Count; i++)
                {
                    int firstCardIndex = random.Next(deck.Count);
                    Cards tempCard = deck[firstCardIndex];
                    deck[firstCardIndex] = deck[i];
                    deck[i] = tempCard;
                }


            }
            void DealCardsToPlayers()
            {
                Queue<Cards> firstPlayerDeck = new Queue<Cards>();
                Queue<Cards> secondPlayerDeck = new Queue<Cards>();

                while (deck.Count > 0)
                {
                    Cards[] firstTwoDrawnCards = deck.Take(2).ToArray();
                    deck.RemoveRange(0, 2);
                    firstPlayerDeck.Enqueue(firstTwoDrawnCards[0]);
                    secondPlayerDeck.Enqueue(firstTwoDrawnCards[1]);
                    
                    
                }


            }
            bool GameHasWinner()
            {
                if (!firstPlayerDeck.Any())
                {
                    Console.WriteLine($"After a total of {totalMoves} moves, the second player has won!");
                    return true;

                }
                if (!secondPlayerDeck.Any())
                {

                    Console.WriteLine($"After a total of {totalMoves} moves, the first player has won!");
                    return true;
                }
                return false;   

                

            }
            void DrawPlayerCards()
            {
                

                firstPlayerCard = firstPlayerDeck.Dequeue();
                Console.WriteLine($"First player has drawn: {firstPlayerCard}");
                secondPlayerCard = secondPlayerDeck.Dequeue();
                Console.WriteLine($"Second player has drawn: {secondPlayerCard}");

                while (!GameHasWinner())
                {
                    Console.ReadLine();
                    DrawPlayerCards();

                    Queue<Cards> pool = new Queue<Cards>();
                    pool.Enqueue(firstPlayerCard);
                    pool.Enqueue(secondPlayerCard);

                    ProcessWar(pool);
                    DetermineRoundWinner(pool);

                    Console.WriteLine("================================================================================");
                    Console.WriteLine($"First player currently has {firstPlayerDeck.Count} cards.");
                    Console.WriteLine($"Second player currently has {secondPlayerDeck.Count} cards.");
                    Console.WriteLine("================================================================================");
                    totalMoves++;
                }

            }
            void ProcessWar(Queue<Cards> pool)
            {
                while((int)firstPlayerCard.Face == (int)secondPlayerCard.Face)
                {
                    Console.WriteLine("WAR!");
                    if (firstPlayerDeck.Count < 4)
                    {
                        AddCardsToWinnerDeck(firstPlayerDeck, secondPlayerDeck);
                        Console.WriteLine($"First player does not have enough card to continue playing...");
                        break;



                    }
                    if (secondPlayerDeck.Count < 4)
                    {
                        AddCardsToWinnerDeck(firstPlayerDeck, secondPlayerDeck);
                        Console.WriteLine($"Second player does not have enough card to continue playing...");
                        break;



                    }
                    AddWarCardsToPool(pool);

                    firstPlayerCard = firstPlayerDeck.Dequeue();
                    Console.WriteLine($"First player has drawn: {firstPlayerCard}");
                    secondPlayerCard = secondPlayerDeck.Dequeue();
                    Console.WriteLine($"Second player has drawn: {secondPlayerCard}");
                    pool.Enqueue(firstPlayerCard);
                    pool.Enqueue(secondPlayerCard);

                }
            }
            void AddCardsToWinnerDeck(Queue<Cards> loserDeck, Queue<Cards> winnerDeck)
            {
               while(loserDeck.Count > 0)
                {
                    winnerDeck.Enqueue(loserDeck.Dequeue());

                }
            }
            void AddWarCardsToPool(Queue<Cards> pool)
            {
                for(int i = 0; i<3; i++)
                {
                    pool.Enqueue(firstPlayerDeck.Dequeue());
                    pool.Enqueue(secondPlayerDeck.Dequeue());

                }
            }
            void DetermineRoundWinner(Queue<Cards> pool) 
            {
                if((int)firstPlayerCard.Face > (int)secondPlayerCard.Face)
                {
                    Console.WriteLine("The first player has won the cards!");
                    
                    foreach(var card in pool)
                    {
                        firstPlayerDeck.Enqueue(card);

                    }
                }
                else
                {
                    Console.WriteLine("The seconf player has won the cards!");

                    foreach (var card in pool)
                    {
                        secondPlayerDeck.Enqueue(card);

                    }
                }
            }
           
        }
       
    }
}