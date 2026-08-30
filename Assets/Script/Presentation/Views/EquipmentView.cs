using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Views
{
    /// <summary>
    /// 뷰
    /// 현재 장착한 무기와 드롭된 아이템을 표시
    /// </summary>
    public class EquipmentView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI equipmentTextDisplay;
        [SerializeField] private TextMeshProUGUI totalAttackText;
        [SerializeField] private TextMeshProUGUI droppedItemsText;
        [SerializeField] private Button unequipButton;
        [SerializeField] private Button autoEquipButton;
        [SerializeField] private Button pickupAllButton;

        private Presentation.Presenters.EquipmentPresenter presenter;

        /// <summary>
        /// 프레젠터를 설정하고 버튼을 연결
        /// </summary>
        public void SetPresenter(Presentation.Presenters.EquipmentPresenter presenter)
        {
            this.presenter = presenter;
            Debug.Log("[EquipmentView] Presenter set");

            SubscribeToButtons();
        }

        /// <summary>
        /// 버튼 클릭 이벤트를 프레젠터에 연결
        /// </summary>
        private void SubscribeToButtons()
        {
            if (unequipButton != null)
            {
                unequipButton.onClick.AddListener(() =>
                {
                    Debug.Log("[EquipmentView] Unequip button clicked");
                    presenter.OnUnequipButtonClicked();
                });
            }

            if (autoEquipButton != null)
            {
                autoEquipButton.onClick.AddListener(() =>
                {
                    Debug.Log("[EquipmentView] Auto Equip button clicked");
                    presenter.OnAutoEquipButtonClicked();
                });
            }

            if (pickupAllButton != null)
            {
                pickupAllButton.onClick.AddListener(() =>
                {
                    Debug.Log("[EquipmentView] Pickup All button clicked");
                    presenter.OnPickupAllButtonClicked();
                });
            }
        }

        /// <summary>
        /// 현재 장착한 무기를 화면에 표시
        /// </summary>
        public void DisplayEquippedWeapon(Domain.Entities.Weapon weapon, int totalAttack)
        {
            if (equipmentTextDisplay == null) return;

            string equipmentText = "=== EQUIPPED WEAPON ===\n";

            if (weapon != null)
            {
                equipmentText += $"Name: {weapon.ItemName}\n";
                equipmentText += $"Rarity: {GetRarityText(weapon.Rarity)}\n";
                equipmentText += $"ATK: +{weapon.AttackPower}\n";
                equipmentText += $"CRI: {weapon.CriticalChance * 100:F0}%\n";
                equipmentText += $"\n{weapon.Description}";
            }
            else
            {
                equipmentText += "No weapon equipped\n";
            }

            equipmentTextDisplay.text = equipmentText;

            if (totalAttackText != null)
                totalAttackText.text = $"TOTAL ATK: {totalAttack}";
        }

        /// <summary>
        /// 드롭된 아이템 목록을 화면에 표시
        /// </summary>
        public void DisplayDroppedItems(List<Domain.Entities.Weapon> droppedWeapons)
        {
            if (droppedItemsText == null) return;

            string droppedText = "=== DROPPED ITEMS ===\n";

            if (droppedWeapons.Count == 0)
            {
                droppedText += "No items dropped\n";
            }
            else
            {
                droppedText += $"Found {droppedWeapons.Count} item(s):\n";
                droppedText += "════════════════════════════════════════\n";
                int index = 1;
                foreach (var weapon in droppedWeapons)
                {
                    droppedText += $"{index}. {weapon.ItemName}\n";
                    droppedText += $"   ATK: +{weapon.AttackPower}\n";
                    index++;
                }
                droppedText += "\n[Click: Pickup All]";
            }

            droppedItemsText.text = droppedText;
        }

        /// <summary>
        /// 무기 등급을 텍스트로 변환
        /// </summary>
        private string GetRarityText(int rarity)
        {
            return rarity switch
            {
                0 => "Common",
                1 => "Rare",
                2 => "Epic",
                3 => "Legendary",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// 메시지를 화면에 표시
        /// </summary>
        public void ShowMessage(string message)
        {
            Debug.Log($"[EquipmentView] {message}");
            if (equipmentTextDisplay != null)
                equipmentTextDisplay.text = message;
        }
    }
}