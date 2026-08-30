using Domain.Entities;
using Domain.Interfaces;
using UnityEngine;

namespace Application
{
    /// <summary>
    /// 모델
    /// 게임 데이터, 인벤토리, 장비 저장/로드
    /// </summary>
    public class SaveService
    {
        private readonly ISaveRepository repository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly BattleService battle;
        private readonly InventoryService inventoryService;
        private readonly EquipmentService equipmentService;

        /// <summary>
        /// 저장 서비스 생성
        /// </summary>
        public SaveService(
            ISaveRepository repository,
            IInventoryRepository inventoryRepository,
            BattleService battle,
            InventoryService inventoryService,
            EquipmentService equipmentService)
        {
            this.repository = repository;
            this.inventoryRepository = inventoryRepository;
            this.battle = battle;
            this.inventoryService = inventoryService;
            this.equipmentService = equipmentService;

            Debug.Log("[SaveService] 인벤토리 저장소 지원 생성됨");
        }

        /// <summary>
        /// 게임 데이터 저장
        /// 라운드, 골드, 공격력, 인벤토리, 장비 저장
        /// </summary>
        public void Save()
        {
            Debug.Log("[SaveService] ========== 저장 시작 ==========");

            // 게임 데이터 저장
            GameData data = new()
            {
                Attack = battle.Hero.Attack.Value,
                Gold = battle.Hero.Gold.Value,
                Round = battle.CurrentRound.Value
            };

            repository.Save(data);
            Debug.Log($"[SaveService] 게임 데이터 저장 - 라운드: {data.Round}, 골드: {data.Gold}, 공격력: {data.Attack}");

            // 인벤토리 저장
            inventoryRepository.SaveInventory(inventoryService.Inventory);
            Debug.Log($"[SaveService] 인벤토리 저장됨");

            // 장비 저장
            inventoryRepository.SaveEquipment(equipmentService.Equipment);
            Debug.Log($"[SaveService] 장비 저장됨");

            Debug.Log("[SaveService] ========== 저장 완료 ==========");
        }

        /// <summary>
        /// 게임 데이터 로드
        /// 라운드, 골드, 공격력, 인벤토리, 장비 로드
        /// </summary>
        public GameData Load()
        {
            Debug.Log("[SaveService] ========== 로드 시작 ==========");

            // 게임 데이터 로드
            GameData data = repository.Load();
            Debug.Log($"[SaveService] 게임 데이터 로드 - 라운드: {data.Round}, 골드: {data.Gold}, 공격력: {data.Attack}");

            // 인벤토리 로드
            Debug.Log("[SaveService] 인벤토리 로드 중...");
            var loadedInventory = inventoryRepository.LoadInventory();
            Debug.Log($"[SaveService] 인벤토리 로드 완료 {loadedInventory.ItemCount}개 아이템");

            // 로드된 인벤토리를 현재 인벤토리에 복사
            for (int i = 0; i < inventoryService.Inventory.Slots.Count; i++)
            {
                inventoryService.Inventory.Slots[i].Clear();
            }

            // 로드된 아이템 복사
            for (int i = 0; i < loadedInventory.Slots.Count; i++)
            {
                var slot = loadedInventory.Slots[i];
                if (!slot.IsEmpty && slot.Item.Value != null)
                {
                    inventoryService.Inventory.Slots[i].SetItem(slot.Item.Value);
                    Debug.Log($"[SaveService] 슬롯 {i} 복구됨: {slot.Item.Value.ItemName}");
                }
            }

            // 장비 로드
            Debug.Log("[SaveService] 장비 로드 중...");
            var loadedEquipment = inventoryRepository.LoadEquipment();
            Debug.Log($"[SaveService] 장비 로드 완료");

            if (loadedEquipment.EquippedWeapon.Value != null)
            {
                equipmentService.Equipment.EquippedWeapon.Value = loadedEquipment.EquippedWeapon.Value;
                equipmentService.Equipment.AdditionalAttack.Value = loadedEquipment.AdditionalAttack.Value;
                Debug.Log($"[SaveService] 장착 무기 복구됨: {loadedEquipment.EquippedWeapon.Value.ItemName}");
            }

            Debug.Log("[SaveService] ========== 로드 완료 ==========");
            return data;
        }
    }
}