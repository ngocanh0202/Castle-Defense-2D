using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common2D.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public Image itemIcon;
        public TextMeshProUGUI amountText;
        public Transform amountTextBackgroundTransform;
        public Button itemButton;
        public int slotIndex;

        private static InventorySlotUI draggedSlot = null;
        private static GameObject draggedItemIcon = null;

        private void Start()
        {
            itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(() =>
            {
                InventoryManager.Instance.inventorySlots[slotIndex].RemoveFromStack(1);
                InventoryManager.Instance.OnInventoryChanged?.Invoke();
            });
        }

        private void OnEnable()
        {
            itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(() =>
            {
                InventoryManager.Instance.inventorySlots[slotIndex].RemoveFromStack(1);
                InventoryManager.Instance.OnInventoryChanged?.Invoke();
            });
        }

        private void OnDisable()
        {
            itemButton.onClick.RemoveAllListeners();
        }


        public void UpdateSlot(InventorySlot slot)
        {
            if (slot.item != null)
            {
                itemIcon.sprite = slot.item.icon;
                itemIcon.color = Color.white;

                if (slot.amount > 1)
                {
                    amountText.text = slot.amount.ToString();
                    amountText.gameObject.SetActive(true);
                    amountTextBackgroundTransform.GetComponent<Image>().color = Color.black;
                }
                else
                {
                    amountText.gameObject.SetActive(false);
                    amountTextBackgroundTransform.GetComponent<Image>().color = Color.clear;
                }

                itemButton.gameObject.SetActive(true);
            }
            else
            {
                itemIcon.sprite = null;
                itemIcon.color = Color.clear;
                amountText.gameObject.SetActive(false);
                amountTextBackgroundTransform.GetComponent<Image>().color = Color.clear;
                itemButton.gameObject.SetActive(false);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (InventoryManager.Instance.inventorySlots[slotIndex].item != null)
            {
                draggedSlot = this;

                draggedItemIcon = new GameObject("Dragged Item");
                draggedItemIcon.transform.SetParent(transform.root);
                Image dragImage = draggedItemIcon.AddComponent<Image>();
                dragImage.sprite = itemIcon.sprite;
                dragImage.raycastTarget = false;
                dragImage.rectTransform.sizeDelta = new Vector2(50, 50);

                itemIcon.color = new Color(1, 1, 1, 0.5f);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (draggedItemIcon != null)
            {
                draggedItemIcon.transform.position = Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (draggedSlot != null)
            {
                itemIcon.color = Color.white;

                if (eventData.pointerCurrentRaycast.gameObject != null)
                {
                    InventorySlotUI targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<InventorySlotUI>();
                    if (targetSlot != null && targetSlot != draggedSlot)
                    {
                        InventoryManager.Instance.SwapItems(draggedSlot.slotIndex, targetSlot.slotIndex);
                    }
                }

                Destroy(draggedItemIcon);
                draggedItemIcon = null;
                draggedSlot = null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (InventoryManager.Instance.inventorySlots[slotIndex].item != null)
                {
                    if (InventoryManager.Instance.inventorySlots[slotIndex].item.itemType == Item.ItemType.Consumable)
                    {
                        InventoryManager.Instance.inventorySlots[slotIndex].RemoveFromStack(1);
                        InventoryManager.Instance.OnInventoryChanged?.Invoke();
                    }
                }
            }
        }
    }
}
