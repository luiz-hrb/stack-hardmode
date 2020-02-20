using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public GameManager gameManager;
    public List<Card> cards;
    public Color[] stackColors;
    public Card prefab_card;
    public float fittingTolerance = 0.05f;
    public int cardsPerColorStage = 10;
    public AudioSource audioCutCard;
    public ComboManager comboSound;
    public Vector3 cardDimensions;
    private CardDirection cardDirection = CardDirection.AxisX;

    private Card currentCard;
    private Card oldCard;

    #region Monobehaviour Events

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StopCard();
    }
    #endregion

    public void StartStack()
    {
        oldCard = cards[cards.Count - 1];
        currentCard = NewCard();
    }

    public void StopCard()
    {
        if (gameManager.gameMode == GameMode.Playing)
        {
            currentCard.StopMovement();
            if (CardFitted())
            {
                FitCard();
                Scored();
                comboSound.Hit();
            }
            else if (CardOut())
            {
                MissedCard();
                comboSound.Miss();
            }
            else
            {
                CutCard();
                Scored();
                comboSound.Miss();
            }
        }
    }

    public Card GetCard()
    {
        return oldCard;
    }

    private void Scored()
    {
        InvertAxis();
        oldCard = currentCard;
        currentCard = NewCard();
        gameManager.Scored();
    }

    private Card NewCard()
    {
        Card newCard = Instantiate(prefab_card, transform);
        Color cardColor = GetCardColor();
        newCard.SetDirection(cardDirection);
        newCard.SetColor(cardColor);
        newCard.SetScale(oldCard.transform.localScale);
        newCard.SetPosition(oldCard.transform.localPosition + new Vector3(0f, cardDimensions.y, 0f));
        cards.Add(newCard);
        return newCard;
    }

    private Color GetCardColor()
    {
        int colorID = cards.Count / cardsPerColorStage;
        Color colorA = stackColors[colorID % stackColors.Length];
        Color colorB = stackColors[(colorID + 1) % stackColors.Length];
        float ratio = (float)(cards.Count % cardsPerColorStage) / cardsPerColorStage;
        return Color.Lerp(colorA, colorB, ratio);
    }

    private void InvertAxis()
    {
        if (cardDirection == CardDirection.AxisX)
            cardDirection = CardDirection.AxisZ;
        else
            cardDirection = CardDirection.AxisX;
    }
    
    private bool CardOut()
    {
        float distance = GetCardDistance();
        if (cardDirection == CardDirection.AxisX)
            return distance > oldCard.transform.localScale.x;
        else
            return distance > oldCard.transform.localScale.z;
    }

    private bool CardFitted()
    {
        float distance = GetCardDistance();
        float oldCardScale;
        if (cardDirection == CardDirection.AxisX)
            oldCardScale = oldCard.transform.localScale.x;
        else
            oldCardScale = oldCard.transform.localScale.z;
        return distance < fittingTolerance * (0.6f + (oldCardScale * 0.4f));
    }

    private float GetCardDistance()
    {
        return Vector2.Distance(currentCard.GetPositionXZ(), oldCard.GetPositionXZ());
    }

    private void MissedCard()
    {
        currentCard.ActivatePhysics();
        gameManager.EndGame();
    }

    private void FitCard()
    {
        currentCard.transform.position = oldCard.transform.position + new Vector3(0f, cardDimensions.y, 0f);
        // Efeitos
        // ...
    }

    private void CutCard()
    {
        float distance = GetCardDistance();
        Card cardPiece = Instantiate(currentCard);
        Vector3 offsetPosition, offsetScale;
        Vector3 cardPiecePosition, cardPieceScale = cardPiece.transform.localScale;
        if (cardDirection == CardDirection.AxisX)
        {
            offsetPosition = new Vector3(distance, 0f, 0f);
            offsetScale = offsetPosition;
            cardPiecePosition = offsetPosition;
            if (currentCard.transform.position.x > oldCard.transform.position.x)
                offsetPosition *= -1f;

            cardPieceScale.x = distance;
        }
        else
        {
            offsetPosition = new Vector3(0f, 0f, distance);
            offsetScale = offsetPosition;
            cardPiecePosition = offsetPosition;
            if (currentCard.transform.position.z > oldCard.transform.position.z)
                offsetPosition *= -1f;

            cardPieceScale.z = distance;
        }
        currentCard.transform.localScale -= offsetScale;
        currentCard.transform.position += offsetPosition / 2f;
        cardPiece.transform.localScale = cardPieceScale;

        // Gambiarra
        cardPiecePosition = currentCard.transform.position;
        if (cardDirection == CardDirection.AxisX)
        {
            if (currentCard.transform.position.x > oldCard.transform.position.x)
                cardPiecePosition.x = currentCard.transform.localPosition.x +
                    ((currentCard.transform.localScale.x + cardPieceScale.x) / 2f);
            else
                cardPiecePosition.x = currentCard.transform.localPosition.x -
                    ((currentCard.transform.localScale.x + cardPieceScale.x) / 2f);
        }
        else
        {
            if (currentCard.transform.position.z > oldCard.transform.position.z)
                cardPiecePosition.z = currentCard.transform.localPosition.z +
                    ((currentCard.transform.localScale.z + cardPieceScale.z) / 2f);
            else
                cardPiecePosition.z = currentCard.transform.localPosition.z -
                    ((currentCard.transform.localScale.z + cardPieceScale.z) / 2f);
        }
        cardPiecePosition.y = cardPiece.transform.localPosition.y;
        cardPiece.transform.localPosition = cardPiecePosition;

        cardPiece.ActivatePhysics();
        audioCutCard.Play();
    }
}
