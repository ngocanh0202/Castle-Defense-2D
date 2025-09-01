using Common2D.Singleton;
using UnityEngine;

namespace Common2D.Inventory
{
    public class InventoryUI : Singleton<InventoryUI>
    {
        public GameObject inventoryPanel;
        public GameObject slotPrefab;
        public Transform slotContainer;

        private InventoryManager inventoryManager;
        [SerializeField] private InventorySlotUI[] slotUIs;

        private void Start()
        {
            if (inventoryPanel != null)
            {
                InitInventoryUI();
                inventoryPanel.SetActive(false);
            }
            else
            {
                Debug.LogError("Inventory panel is not assigned in the inspector.");
            }
        }

        private void Update()
        {
            // Todo: Add or change the key to open inventory
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleInventory();
            }
        }

        private void InitInventoryUI()
        {
            slotPrefab = ResourcesManager.GetPrefabInventorySlot();
            slotContainer = inventoryPanel.transform.Find("Viewport/SlotContainer");

            inventoryManager = InventoryManager.Instance;
            slotUIs = new InventorySlotUI[inventoryManager.inventorySize];

            for (int i = 0; i < inventoryManager.inventorySize; i++)
            {
                GameObject slotGO = Instantiate(slotPrefab, slotContainer);
                InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
                slotUI.slotIndex = i;
                slotUIs[i] = slotUI;
            }

            inventoryManager.OnInventoryChanged.AddListener(UpdateUI);
            UpdateUI();
        }

        public void ToggleInventory()
        {
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            }
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (!inventoryPanel.activeSelf)
                return;

            for (int i = 0; i < inventoryManager.inventorySize; i++)
            {
                if (i < inventoryManager.inventorySlots.Count)
                {
                    slotUIs[i].UpdateSlot(inventoryManager.inventorySlots[i]);
                }
            }
        }
    }
}
