using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false;
    public Vector3 startPosition;
    public Transform startParent;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startParent = transform.parent;

        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;

        startPosition = transform.position;
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    void OnMouseUp()
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1;

        if (gameManager == null)
        {
            ReturnToOringinalPosition();
            return;
        }

        bool wasInMergeArea = startParent == gameManager.mergeArea;

        if(IsOverArea(gameManager.handArea))
        {
            Debug.Log("���� �������� �̵�");

            if(wasInMergeArea)
            {
                for(int i = 0; i < gameManager.mergeCount; i ++)
                {
                    if (gameManager.mergeCards[i] == gameObject)
                    {
                        for(int j = i; j < gameManager.mergeCount -1; j++)
                        {
                            gameManager.mergeCards[j] = gameManager.mergeCards[j + 1];
                        }
                        gameManager.mergeCards[gameManager.mergeCount - 1] = null;
                        gameManager.mergeCount--;

                        transform.SetParent(gameManager.handArea);
                        gameManager.handcards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand();
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else
            {
                gameManager.ArrangeHand();
            }
        }
        else if (IsOverArea(gameManager.mergeArea))
        {
            if(gameManager.mergeCount >= gameManager.maxMergeSize)
            {
                Debug.Log("���� ������ ���� á���ϴ�.");
                ReturnToOringinalPosition();
            }
            else
            {
                gameManager.MoveCardToMerge(gameObject);
            }
        }
        else
        {
            gameManager.MoveCardToMerge(gameObject);
        }
        if(wasInMergeArea)
        {
            if(gameManager.mergeButton != null)
            {
                bool canMerge = (gameManager.mergeCount == 2 || gameManager.mergeCount == 3);
                gameManager.mergeButton.interactable = canMerge;
            }
        }
    }

    void ReturnToOringinalPosition()
    {
        startPosition = transform.position;
        startParent = transform.parent;

        if(gameManager != null)
        {
            if(startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand();
            }
            if (startParent == gameManager.mergeArea)
            {
                gameManager.ArrangeMerge();
            }
        }
    }
    bool IsOverArea(Transform area)
    {
        if(area == null)
        {
            return false;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != null && hit.collider.transform == area)
            {
                Debug.Log(area.name + "���� ������");
                return true;
            }
        }
        return false;
    }
}
