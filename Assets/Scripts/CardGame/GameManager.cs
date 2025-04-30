using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardImage;

    public Transform deckArea;
    public Transform handArea;

    public Button drawButton;
    public TextMeshProUGUI deckCountText;

    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;

    public GameObject[] deckCards;
    public int deckCount;

    public GameObject[] handcards;
    public int handCount;

    private int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,
        2,2,2,2,2,2,
        3,3,3,3,
        4,4
    };

    // Start is called before the first frame update
    void Start()
    {
        deckCards = new GameObject[prefedinedDeck.Length];
        handcards = new GameObject[maxHandSize];

        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShuffleDeck()
    {
        for (int i = 0; i < deckCount - 1; i++)
        {
            int j = Random.Range(i, deckCount);
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }

    void InitializeDeck()
    {
        deckCount = prefedinedDeck.Length;

        for(int i=0; i<prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];
            int imageIndex = value - 1;
            if(imageIndex >= cardImage.Length || imageIndex<0)
            {
                imageIndex = 0;
            }

            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(false);

            Card cardComp = newCardObj.GetComponent<Card>();
            if(cardComp != null)
            {
                cardComp.InitCard(value, cardImage[imageIndex]);
            }
            deckCards[i] = newCardObj;
        }
    }

    public void ArrangeHand()
    {
        if (handCount == 0)
            return;
        float startX = -(handCount - 1) * cardSpacing / 2;

        for(int i=0; i < handCount; i++)
        {
            if (handcards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handcards[i].transform.position = newPos;
            }
        }
    }
    
    void OnDrawButtonClicked()
    {
        DrawCardToHand();
    }

    public void DrawCardToHand()
    {
        if(handCount >= maxHandSize)
        {
            Debug.Log("손패가 가득 찼습니다.");
            return;
        }
        if(deckCount<=0)
        {
            Debug.Log("덱에 더 이상 카드가 없습니다.");
            return;
        }

        GameObject drawCard = deckCards[0];

        for(int i = 0; i<deckCount - 1; i++)
        {
            deckCards[i] = deckCards[i + 1];
        }
        deckCount--;

        drawCard.SetActive(true);
        handcards[handCount] = drawCard;
        handCount++;

        drawCard.transform.SetParent(handArea);

        ArrangeHand();
    }


}
