using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 아이템 20개 슬롯 관리
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// 인벤토리 슬롯 개수
        /// </summary>
        public const int INVENTORY_SIZE = 20;

        private List<InventorySlot> slots = new();

        /// <summary>
        /// 인벤토리 슬롯 (반응형)
        /// </summary>
        public ReactiveCollection<InventorySlot> Slots { get; private set; } = new();

        /// <summary>
        /// 현재 아이템 개수
        /// </summary>
        public int ItemCount
        {
            get
            {
                int count = 0;
                foreach (var slot in slots)
                    if (!slot.IsEmpty)
                        count++;
                return count;
            }
        }

        /// <summary>
        /// 인벤토리 생성
        /// 20개 빈 슬롯 초기화
        /// </summary>
        public Inventory()
        {
            for (int i = 0; i < INVENTORY_SIZE; i++)
            {
                var slot = new InventorySlot(i);
                slots.Add(slot);
                Slots.Add(slot);
            }

            Debug.Log($"[Inventory] {INVENTORY_SIZE}개 슬롯으로 초기화됨");
        }

        /// <summary>
        /// 첫 번째 빈 슬롯 찾기
        /// </summary>
        public InventorySlot FindEmptySlot()
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                    return slot;
            }
            return null;
        }

        /// <summary>
        /// 아이템 추가
        /// 빈 슬롯에 아이템 저장
        /// </summary>
        public bool AddItem(Item item)
        {
            if (item == null)
                return false;

            var emptySlot = FindEmptySlot();
            if (emptySlot == null)
            {
                Debug.LogWarning("[Inventory] 비어있는 슬롯 없음!");
                return false;
            }

            emptySlot.SetItem(item);
            Debug.Log($"[Inventory] 아이템 추가됨: {item.ItemName}");
            return true;
        }

        /// <summary>
        /// 슬롯에서 아이템 제거
        /// </summary>
        public Item RemoveItemFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count)
                return null;

            var item = slots[slotIndex].GetItem();
            slots[slotIndex].RemoveItem();
            return item;
        }

        /// <summary>
        /// 두 슬롯 교환
        /// </summary>
        public bool SwapSlots(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= slots.Count ||
                toIndex < 0 || toIndex >= slots.Count)
                return false;

            slots[fromIndex].SwapWith(slots[toIndex]);
            return true;
        }

        /// <summary>
        /// 자동 정렬
        /// 모든 아이템을 앞으로 정렬
        /// </summary>
        public void AutoArrange()
        {
            var items = new List<Item>();

            // 모든 아이템 수집
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty)
                    items.Add(slot.GetItem());
                slot.Clear();
            }

            // 앞부터 다시 배치
            for (int i = 0; i < items.Count && i < slots.Count; i++)
            {
                slots[i].SetItem(items[i]);
            }

            Debug.Log($"[Inventory] {items.Count}개 아이템 정렬됨");
        }

        /// <summary>
        /// 무기 ID로 찾기
        /// </summary>
        public InventorySlot FindWeapon(string weaponId)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.GetItem() is Weapon weapon && weapon.ItemId == weaponId)
                    return slot;
            }
            return null;
        }

        /// <summary>
        /// 모든 무기 반환
        /// </summary>
        public List<Weapon> GetAllWeapons()
        {
            var weapons = new List<Weapon>();
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.GetItem() is Weapon weapon)
                    weapons.Add(weapon);
            }
            return weapons;
        }

        /// <summary>
        /// 가장 강한 무기 반환
        /// </summary>
        public Weapon GetBestWeapon()
        {
            var weapons = GetAllWeapons();
            if (weapons.Count == 0)
                return null;

            Weapon best = weapons[0];
            foreach (var weapon in weapons)
            {
                if (weapon.AttackPower > best.AttackPower)
                    best = weapon;
                else if (weapon.AttackPower == best.AttackPower && weapon.Rarity > best.Rarity)
                    best = weapon;
            }

            return best;
        }

        /// <summary>
        /// 인벤토리 비우기
        /// </summary>
        public void Clear()
        {
            foreach (var slot in slots)
                slot.Clear();
        }

        /// <summary>
        /// 특정 아이템 개수 반환
        /// </summary>
        public int GetItemCount(string itemId)
        {
            int count = 0;
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.GetItem().ItemId == itemId)
                    count++;
            }
            return count;
        }
    }
}