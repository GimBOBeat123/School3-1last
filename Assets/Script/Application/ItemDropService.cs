using System.Collections.Generic;
using Domain.Entities;
using UniRx;
using UnityEngine;
using Infrastructure;

namespace Application
{
    /// <summary>
    /// 모델
    /// 몬스터 처치 시 무기 드롭 및 습득
    /// </summary>
    public class ItemDropService
    {
        /// <summary>
        /// 드롭된 무기 목록
        /// </summary>
        public ReactiveCollection<Weapon> DroppedWeapons { get; private set; } = new();

        private InventoryService inventoryService;

        /// <summary>
        /// 아이템 드롭 서비스 생성
        /// </summary>
        public ItemDropService(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
            Debug.Log("[ItemDropService] 생성됨");
        }

        /// <summary>
        /// 몬스터 처치 시 무기 드롭
        /// 확률에 따라 무기 결정
        /// </summary>
        public Weapon DropRandomWeaponOnMonsterKill(Vector3 killPosition)
        {
            float randomValue = Random.value;
            float cumulativeRate = 0f;

            // 드롭 확률 체크
            foreach (var weaponId in WeaponDatabase.GetAllWeaponIds())
            {
                var info = WeaponDatabase.GetWeaponInfo(weaponId);
                cumulativeRate += info.DropRate;

                if (randomValue <= cumulativeRate)
                {
                    // 무기 드롭
                    var weapon = WeaponDatabase.CreateWeapon(weaponId);
                    weapon.DropPosition = killPosition;
                    DroppedWeapons.Add(weapon);

                    Debug.Log($"[ItemDrop] {killPosition} 위치에 드롭: {weapon.ItemName} (확률: {info.DropRate * 100:F0}%)");
                    return weapon;
                }
            }

            // 드롭 안됨
            Debug.Log("[ItemDrop] 무기 드롭 안됨");
            return null;
        }

        /// <summary>
        /// 드롭된 무기 습득
        /// 인벤토리에 추가
        /// </summary>
        public bool PickupWeapon(Weapon weapon)
        {
            if (weapon == null)
                return false;

            bool success = inventoryService.AddItem(weapon);
            if (success)
            {
                DroppedWeapons.Remove(weapon);
                Debug.Log($"[ItemDrop] 습득됨: {weapon.ItemName}");
            }
            else
            {
                Debug.Log("[ItemDrop] 인벤토리 가득! 습득 불가");
            }

            return success;
        }

        /// <summary>
        /// 위치 기반 근처 드롭 아이템 검색
        /// </summary>
        public List<Weapon> GetNearbyWeapons(Vector3 position, float range)
        {
            var nearby = new List<Weapon>();
            foreach (var weapon in DroppedWeapons)
            {
                if (Vector3.Distance(weapon.DropPosition, position) <= range)
                    nearby.Add(weapon);
            }
            return nearby;
        }

        /// <summary>
        /// 드롭된 모든 무기 제거
        /// </summary>
        public void ClearDroppedWeapons()
        {
            DroppedWeapons.Clear();
        }
    }
}