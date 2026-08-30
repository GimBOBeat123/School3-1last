using System.Collections.Generic;
using Domain.Entities;
using UniRx;
using UnityEngine;
using Item = Domain.Entities.Item;

namespace Application
{
    /// <summary>
    /// 모델
    /// 아이템 추가, 제거, 정렬 처리
    /// </summary>
    public class InventoryService
    {
        /// <summary>
        /// 인벤토리 객체
        /// </summary>
        public Inventory Inventory { get; private set; }

        /// <summary>
        /// 인벤토리 아이템 개수
        /// </summary>
        public ReactiveProperty<int> InventoryCount = new(0);

        /// <summary>
        /// 아이템 변경 알림
        /// </summary>
        public ReactiveCollection<Item> ItemsChanged { get; private set; } = new();

        /// <summary>
        /// 인벤토리 서비스 생성
        /// </summary>
        public InventoryService()
        {
            Inventory = new Inventory();
            Debug.Log("[InventoryService] 생성됨 슬롯: " + Inventory.INVENTORY_SIZE);
        }

        /// <summary>
        /// 인벤토리에 아이템 추가
        /// </summary>
        public bool AddItem(Item item)
        {
            if (item == null)
                return false;

            bool success = Inventory.AddItem(item);
            if (success)
            {
                InventoryCount.Value = GetItemCount();
                ItemsChanged.Add(item);
                Debug.Log($"[Inventory] 추가됨: {item.ItemName}");
            }
            else
            {
                Debug.Log("[Inventory] 추가 실패 - 인벤토리 가득!");
            }
            return success;
        }

        /// <summary>
        /// 인벤토리 슬롯에서 아이템 제거
        /// </summary>
        public Item RemoveItemFromSlot(int slotIndex)
        {
            var item = Inventory.RemoveItemFromSlot(slotIndex);
            if (item != null)
            {
                InventoryCount.Value = GetItemCount();
                Debug.Log($"[Inventory] 제거됨: {item.ItemName}");
            }
            return item;
        }

        /// <summary>
        /// 인벤토리 자동 정렬
        /// 앞부터 연속으로 정렬
        /// </summary>
        public void AutoArrangeInventory()
        {
            Inventory.AutoArrange();
            Debug.Log("[Inventory] 자동 정렬됨");
        }

        /// <summary>
        /// 인벤토리에 들어있는 아이템 개수 반환
        /// </summary>
        public int GetItemCount()
        {
            return Inventory.ItemCount;
        }

        /// <summary>
        /// 지정된 슬롯 반환
        /// </summary>
        public InventorySlot GetSlot(int index)
        {
            if (index >= 0 && index < Inventory.Slots.Count)
                return Inventory.Slots[index];
            return null;
        }

        /// <summary>
        /// 인벤토리의 모든 무기 반환
        /// </summary>
        public List<Weapon> GetAllWeapons()
        {
            return Inventory.GetAllWeapons();
        }

        /// <summary>
        /// 인벤토리 빈 슬롯 확인
        /// </summary>
        public bool HasSpace()
        {
            return Inventory.FindEmptySlot() != null;
        }

        /// <summary>
        /// 인벤토리에서 아이템 제거 (ID로 검색)
        /// </summary>
        public bool RemoveItem(Item item)
        {
            var slot = Inventory.FindWeapon(item.ItemId);
            if (slot != null)
            {
                Inventory.RemoveItemFromSlot(slot.SlotIndex);
                InventoryCount.Value = GetItemCount();
                return true;
            }
            return false;
        }
    }
}