
using UnityEngine;

public class Card : MonoBehaviour
{
    // The pair identifier for the card
    public int Pair { get; set; } = 0;

    // Indicates whether the card has been solved
    public bool Solved { get; set; } = false;

    // Indicates whether the card is currently selected
    public bool Selected { get; set; } = false;


    // Indicates whether the card is currently selected
    public SpriteRenderer cardFront;


    // Indicates whether the card is currently selected
    public SpriteRenderer cardBack;


}
