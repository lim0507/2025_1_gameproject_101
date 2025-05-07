using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardImages;

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

    public Transform mergeArea;
    public Button mergeButton;
    public int maxMergeSize = 3;

    public GameObject[] mergeCards;
    public int mergeCount;

    // Start is called before the first frame update
    void Start()
    {
        deckCards = new GameObject[prefedinedDeck.Length];
        handcards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        InitializeDeck();
        ShuffleDeck();

        if (drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);
        }


        if (mergeButton != null)
        {
           mergeButton.onClick.AddListener(OnMergeButtonClicked);
           mergeButton.interactable = false;
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

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];
            int imageIndex = value - 1;
            if (imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;
            }

            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(false);

            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;
        }
    }

    public void ArrangeHand()
    {
        if (handCount == 0)
            return;

        float startX = -(handCount - 1) * cardSpacing / 2;

        for (int i = 0; i < handCount; i++)
        {
            if (handcards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handcards[i].transform.position = newPos;
            }
        }
    }

    public void ArrangeMerge()
    {
        if (mergeCount == 0)
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos;
            }
        }
    }


    void OnDrawButtonClicked()
    {
        DrawCardToHand();
    }

    public void DrawCardToHand()
    {
        if (handCount + mergeCount >= maxHandSize)
        {
            Debug.Log("손패가 가득 찼습니다.");
            return;
        }
        if (deckCount <= 0)
        {
            Debug.Log("덱에 더 이상 카드가 없습니다.");
            return;
        }

        GameObject drawCard = deckCards[0];

        for (int i = 0; i < deckCount - 1; i++)
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

    public void MoveCardToMerge(GameObject card)
    {
        if (mergeCount >= maxMergeSize)
        {
            Debug.Log("머지 영역이 가득 찼습니다.");
            return;
        }

        for (int i = 0; i < handCount; i++)
        {
            if (handcards[i] == card)
            {
                for(int j = i; j< handCount - 1; j++)
                {
                    handcards[j] = handcards[j + 1];
                }
                handcards[handCount - 1] = null;
                handCount--;

                ArrangeHand();
                break;
            }
        }

        mergeCards[mergeCount] = card;
        mergeCount++;

        card.transform.SetParent(mergeArea);
        ArrangeMerge();
        UpdateMergeButtonState();
    }

    void UpdateMergeButtonState()
    {
        if (mergeButton != null)
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);
        }
    }

    void MergeCards()
    {
        if (mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("머지를 하려면 카드가 2개 또는 3개가 필요합니다.");
            return;
        }
        int firstCard = mergeCards[0].GetComponent<Card>().cardValue;
        for (int i = 1; i < mergeCount; i++)     
        {
            Card card = mergeCards[i].GetComponent<Card>();
            if(card == null || card.cardValue != firstCard)
            {
                Debug.Log("같은 숫자의 카드만 머지 할 수 있습니다.");
                return;
            }
        }

        int newValue = firstCard + 1;

        if (newValue > cardImages.Length)
        {
            Debug.Log("최대 카드 값에 도달 했습니다.");
            return;
        }

        for(int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                mergeCards[i].SetActive(false);
            }
        }

       GameObject newCard = Instantiate(cardPrefab, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();
        if(newCardTemp != null)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        for(int i = 0; i < maxMergeSize; i++)
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;

        UpdateMergeButtonState();

        handcards[handCount] = newCard;
        handCount++;
        newCard.transform.SetParent(handArea);

        ArrangeHand();
    }

    void OnMergeButtonClicked()
    {
        MergeCards();
    }

}
