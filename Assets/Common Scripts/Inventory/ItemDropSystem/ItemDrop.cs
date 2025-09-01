using System;
using Common2D.CreateGameObject2D;
using Common2D.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : CommonPoolObject
{
    [SerializeField] public Item item;
    [SerializeField] public int quantity;
    [SerializeField] float rangeRadiusToTake = 0.3f;
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float lifeDuration = 15f;
    [SerializeField] float timer = 0f;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform imageTransform;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform textCountdownPrefab;
    [SerializeField] TextMeshPro existingTextMeshPro;
    public override string PoolName { get => KeyOfObjPooler.ItemDrop.ToString(); set => throw new NotImplementedException(); }
    void Start()
    {
        InitializedComponents();
    }
    void Update()
    {
        imageTransform.Rotate(0, 360 * Time.deltaTime, 0);
        if (IsPlayerInArea())
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        if (timer < lifeDuration)
        {
            timer += Time.deltaTime;
            if (existingTextMeshPro != null)
                existingTextMeshPro.text = Mathf.Ceil(lifeDuration - timer).ToString();
        }
        else
            gameObject.SetActive(false);
        

    }

    void OnEnable()
    {
        timer = 0f;
        InitializedComponents();
        spriteRenderer.sprite = item.icon;
    }

    void InitializedComponents()
    {
        if (playerTransform == null)
            playerTransform = FindObjectOfType<PlayerController>().transform;
        if (imageTransform == null)
            imageTransform = transform.Find("Image");
        if (boxCollider2D == null)
            boxCollider2D = GetComponent<BoxCollider2D>();
        if (spriteRenderer == null)
            spriteRenderer = imageTransform.GetComponent<SpriteRenderer>();
        if (playerLayerMask == 0)
            playerLayerMask = LayerMask.GetMask("Player");
        if (textCountdownPrefab == null)
        {
            textCountdownPrefab = transform.Find("Countdown Timer");
            existingTextMeshPro = textCountdownPrefab.GetComponent<TextMeshPro>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            InventoryManager.Instance.AddItem(item, quantity);
            gameObject.SetActive(false);
        }
    }

    bool IsPlayerInArea()
    {
       Collider2D hit = Physics2D.OverlapCircle(
            boxCollider2D.bounds.center,
            rangeRadiusToTake,
            playerLayerMask
        );
        return hit != null;
    }

    void OnDrawGizmos()
    {
       if (boxCollider2D != null)
        {
            if (IsPlayerInArea())
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(boxCollider2D.bounds.center, rangeRadiusToTake);
        }
    }
}
