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

        ReturnToOringinalPosition();
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
        }
    }
    bool IsOverArea(Transform area)
    {
        if(area == null)
        {
            return false;
        }
        Collider2D areaCollider = area.GetComponent<Collider2D>();
        if (areaCollider == null)
            return false;

        return areaCollider.bounds.Contains(transform.position);
    }
}
