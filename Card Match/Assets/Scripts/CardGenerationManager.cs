using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerationManager : MonoBehaviour
{


	// Public Variables
    #region

    public Sprite[] spriteCardBack;
    public Sprite[] spriteCardFront;

	// How fast to uncover a card, higher values = faster
    [Range(1f, 10f)]
    public float uncoverTime = 4f;

	// How fast to deal one card, higher values = faster
    [Range(1f, 10f)]
    public float dealTime = 4f;

    // How fast to match 2 card, higher values = slower
    [Range(0.1f, 10f)]

    public float checkPairTime = 0.5f;

	// The Padding between 2 Cards
    [Range(2, 6)]
    public int cardsPadding = 4;

    // Row count
    [Range(2,6)]
    public int rowCount = 2;

	// Column Count
    [Range(2,6)]
    public int columnCount = 2;

	// Base card object to Instantiate
    public GameObject cardObject;

    //Audio Controller Component
    [SerializeField] private AudioManager audioManager;

    #endregion

    // Private Variables
    #region
    
    int pairCount = 4;

    int chosenCardsBack = 0;
    int[] chosenCardsFront;

    Vector3 dealingStartPosition = new Vector3(-12800, -12800, -8);


    int totalMoves = 0;
    int bestMoves = 0;
    int uncoveredCards = 0;
    Transform[] selectedCards = new Transform[2];

    Stack<Transform> openedCards = new Stack<Transform>();

    int oldPairCount;
    
    // Input check
    bool isTouching = false;
    bool isUncovering = false;
    bool isDealing = false;
    #endregion

    private void Start()
    {
        CreateDeck();
    }

    public void CreateDeck()
    {
        GameObject cardParent = GameObject.Find("CardParent");


        // clear the game field and reset variables
        if (cardParent)
        {
            Destroy(cardParent);
        }
        openedCards.Clear();
        totalMoves = 0;
        selectedCards=new Transform[2];
        pairCount = (rowCount * columnCount) / 2;


        Debug.Log(pairCount);
        // randomly chose the back graphic of the cards
        chosenCardsBack = Random.Range(0, spriteCardBack.Length);

        // randomly chose the card motives to play with
        List<int> tmp = new List<int>();
        for (int i = 0; i < spriteCardFront.Length; i++)
        {
            tmp.Add(i);
        }
        tmp.Shuffle();

        chosenCardsFront = tmp.GetRange(0, pairCount).ToArray();

        // this holds all the cards
        cardParent = new GameObject("DeckParent");
        GameObject temp = new GameObject("Temp");
        int cur = 0;

        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;

        int cCards = pairCount * 2;

        List<int> deck = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            deck.AddRange(new int[] { i, i });
        }
        deck.Shuffle();


        int cardWidth = 0;
        int cardHeight = 0;

        for (int x = 0; x < columnCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                if (cur > cCards - 1)
                    break;

                GameObject tempCard = Instantiate(cardObject);
                GameObject destination = new GameObject("Destination");

                tempCard.GetComponent<Card>().cardFront.sprite = spriteCardFront[chosenCardsFront[deck[cur]]];
                tempCard.GetComponent<Card>().cardFront.sortingOrder = -1;

                tempCard.GetComponent<Card>().cardBack.sprite = spriteCardBack[chosenCardsBack];
                tempCard.GetComponent<Card>().cardBack.sortingOrder = 1;

                cardWidth = (int)spriteCardBack[chosenCardsBack].rect.width;
                cardHeight = (int)spriteCardBack[chosenCardsBack].rect.height;

                tempCard.transform.parent = cardParent.transform;
                tempCard.transform.position = dealingStartPosition;
                tempCard.GetComponent<BoxCollider2D>().size= new Vector2 (cardWidth, cardHeight);
                tempCard.GetComponent<Card>().Pair = deck[cur];

                destination.tag = "Destination";
                destination.transform.parent = temp.transform;
                destination.transform.position = new Vector3(x * (cardWidth + cardsPadding), y * (cardHeight + cardsPadding));

                cur++;

                Vector3 pos = destination.transform.position;
                minX = Mathf.Min(minX, pos.x - cardWidth);
                minY = Mathf.Min(minY, pos.y - cardHeight);
                maxX = Mathf.Max(maxX, pos.x + cardWidth );
                maxY = Mathf.Max(maxY, pos.y + cardHeight );
            }
        }

        float tableScale = (GameObject.Find("Table") == null) ? 1f : GameObject.Find("Table").transform.localScale.x;
        float scale = tableScale / (maxX + cardsPadding);

        Vector2 point = LineIntersectionPoint(
            new Vector2(minX, maxY),
            new Vector2(maxX, minY),
            new Vector2(minX, minY),
            new Vector2(maxX, maxY)
            );

        temp.transform.position -= new Vector3(point.x * scale, point.y * scale);



        cardParent.transform.localScale = new Vector3(scale, scale, scale);
        temp.transform.localScale = new Vector3(scale, scale, scale);


        DealCards();
    }

    void DealCards()
    {
        StartCoroutine(dealCards());
    }


    IEnumerator dealCards()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        GameObject[] destinations = GameObject.FindGameObjectsWithTag("Destination");

        for (int i = 0; i < cards.Length; i++)
        {
            float t = 0;

            if (audioManager. soundDealCard != null)
                audioManager.PlaySound(audioManager.soundDealCard);

            while (t < 1f)
            {
                t += Time.deltaTime * dealTime;

                cards[i].transform.position = Vector3.Lerp(
                    dealingStartPosition, destinations[i].transform.position, t);

                

                yield return null;
            }
            yield return null;
        }

        isDealing = false;

        yield return 0;
    }

    Vector2 LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
    {
        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = pe1.y - ps1.y;
        float B1 = ps1.x - pe1.x;
        float C1 = A1 * ps1.x + B1 * ps1.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = pe2.y - ps2.y;
        float B2 = ps2.x - pe2.x;
        float C2 = A2 * ps2.x + B2 * ps2.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
            return new Vector2();

        // now return the Vector2 intersection point
        return new Vector2(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta
            );
    }
}
