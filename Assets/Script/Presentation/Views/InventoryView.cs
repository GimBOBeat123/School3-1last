using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Views
{
    /// <summary>
    /// 뷰
    /// 인벤토리의 아이템을 표시하고, 자동 정렬 기능을 제공
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI inventoryTextDisplay;
        [SerializeField] private TextMeshProUGUI inventoryCountText;
        [SerializeField] private Button autoArrangeButton;
        [SerializeField] private Button toggleInventoryButton;
        [SerializeField] private Canvas inventoryCanvas;

        private Presentation.Presenters.InventoryPresenter presenter;
        private bool isInventoryOpen = false;

        /// <summary>
        /// 프레젠터를 설정하고 버튼 이벤트를 연결
        /// </summary>
        public void SetPresenter(Presentation.Presenters.InventoryPresenter presenter)
        {
            this.presenter = presenter;
            Debug.Log("[InventoryView] Presenter set");

            SubscribeToButtons();
        }

        /// <summary>
        /// 버튼 클릭 이벤트를 프레젠터에 연결
        /// </summary>
        private void SubscribeToButtons()
        {
            if (presenter == null)
            {
                Debug.LogError("[InventoryView] Presenter is null!");
                return;
            }

            if (autoArrangeButton != null)
            {
                autoArrangeButton.onClick.RemoveAllListeners();

                autoArrangeButton.onClick.AddListener(() =>
                {
                    Debug.Log("[InventoryView] Auto Arrange button clicked!");
                    presenter.OnAutoArrangeButtonClicked();
                });

                Debug.Log("[InventoryView] Auto Arrange button connected");
            }
            else
            {
                Debug.LogError("[InventoryView] Auto Arrange button not assigned!");
            }

            if (toggleInventoryButton != null)
            {
                toggleInventoryButton.onClick.RemoveAllListeners();

                toggleInventoryButton.onClick.AddListener(() =>
                {
                    Debug.Log("[InventoryView] Toggle button clicked!");
                    ToggleInventory();
                });

                Debug.Log("[InventoryView] Toggle button connected");
            }
            else
            {
                Debug.LogError("[InventoryView] Toggle button not assigned!");
            }
        }

        private void Start()
        {
            if (inventoryCanvas != null)
                inventoryCanvas.enabled = false;

            Debug.Log("[InventoryView] Start complete");
        }

        /// <summary>
        /// 인벤토리의 아이템을 화면에 표시
        /// </summary>
        public void DisplayInventory(Domain.Entities.Inventory inventory)
        {
            if (inventoryTextDisplay == null) return;

            string inventoryText = "=== INVENTORY (20 SLOTS) ===\n";
            inventoryText += $"Items: {inventory.ItemCount}/20\n";
            inventoryText += "─────────────────────\n";

            int count = 0;
            foreach (var slot in inventory.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Value != null)
                {
                    var item = slot.Item.Value;
                    if (item is Domain.Entities.Weapon weapon)
                    {
                        inventoryText += $"• {weapon.ItemName}\n";
                        inventoryText += $"  ATK: +{weapon.AttackPower}\n";
                        inventoryText += $"  CRI: {weapon.CriticalChance * 100:F0}%\n";
                    }
                    else
                    {
                        inventoryText += $"• {item.ItemName}\n";
                    }
                    count++;
                }
            }

            if (count == 0)
            {
                inventoryText += "(Empty)\n";
            }

            inventoryTextDisplay.text = inventoryText;
        }

        /// <summary>
        /// 인벤토리에 들어있는 아이템 개수를 표시
        /// </summary>
        public void UpdateInventoryCount(int count)
        {
            if (inventoryCountText != null)
                inventoryCountText.text = $"Inventory: {count}/20";
        }

        /// <summary>
        /// 인벤토리 패널을 열고 닫음
        /// </summary>
        public void ToggleInventory()
        {
            if (inventoryCanvas != null)
            {
                isInventoryOpen = !isInventoryOpen;
                inventoryCanvas.enabled = isInventoryOpen;
                Debug.Log($"[InventoryView] 인벤토리 {(isInventoryOpen ? "열음" : "닫음")}");
            }
        }

        /// <summary>
        /// 메시지를 인벤토리 화면에 표시
        /// </summary>
        public void ShowMessage(string message)
        {
            Debug.Log($"[InventoryView] {message}");
            if (inventoryTextDisplay != null)
                inventoryTextDisplay.text = message;
        }
    }
}