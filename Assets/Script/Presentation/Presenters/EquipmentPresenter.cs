using System.Collections.Generic;
using Application;
using Presentation.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Presentation.Presenters
{
    /// <summary>
    /// 프레젠터
    /// 장비 뷰와 서비스 연결
    /// </summary>
    public class EquipmentPresenter
    {
        private EquipmentView view;
        private EquipmentService equipmentService;
        private ItemDropService itemDropService;
        private InventoryService inventoryService;
        private CompositeDisposable disposables = new();

        [Inject]
        public void Construct(
            EquipmentService equipmentService,
            ItemDropService itemDropService,
            InventoryService inventoryService,
            EquipmentView view)
        {
            this.equipmentService = equipmentService;
            this.itemDropService = itemDropService;
            this.inventoryService = inventoryService;
            this.view = view;

            Debug.Log("[EquipmentPresenter] 주입 완료");
            Initialize();
        }

        /// <summary>
        /// 프레젠터 초기화
        /// 뷰 설정 및 이벤트 구독
        /// </summary>
        private void Initialize()
        {
            if (view == null)
            {
                Debug.LogError("[EquipmentPresenter] 뷰가 널!");
                return;
            }

            view.SetPresenter(this);
            Debug.Log("[EquipmentPresenter] 초기화");

            // 장착 무기 변경 감시
            equipmentService.EquippedWeapon
                .Subscribe(weapon =>
                {
                    Debug.Log($"[EquipmentPresenter] 장착 무기 변경: {(weapon != null ? weapon.ItemName : "없음")}");
                    OnEquipmentChanged();
                })
                .AddTo(disposables);

            // 총 공격력 변경 감시
            equipmentService.TotalAttack
                .Subscribe(attack =>
                {
                    Debug.Log($"[EquipmentPresenter] 총 공격력 변경: {attack}");
                    OnEquipmentChanged();
                })
                .AddTo(disposables);

            // 드롭된 무기 변경 감시
            itemDropService.DroppedWeapons
                .ObserveAdd()
                .Subscribe(_ => OnDroppedItemsChanged())
                .AddTo(disposables);

            itemDropService.DroppedWeapons
                .ObserveRemove()
                .Subscribe(_ => OnDroppedItemsChanged())
                .AddTo(disposables);

            OnEquipmentChanged();
            OnDroppedItemsChanged();

            Debug.Log("[EquipmentPresenter] 설정 완료");
        }

        /// <summary>
        /// 무기 해제 버튼 클릭 처리
        /// 해제된 무기를 인벤토리에 추가
        /// </summary>
        public void OnUnequipButtonClicked()
        {
            Debug.Log("[EquipmentPresenter] 해제 클릭");

            var equippedWeapon = equipmentService.EquippedWeapon.Value;

            if (equippedWeapon != null)
            {
                Debug.Log($"[해제 전] 장착: {equippedWeapon.ItemName}, 공격력: {equipmentService.TotalAttack.Value}");

                // 무기 해제
                equipmentService.UnequipWeapon();

                // 해제된 무기를 인벤토리에 추가
                if (inventoryService.AddItem(equippedWeapon))
                {
                    Debug.Log($"[EquipmentPresenter] {equippedWeapon.ItemName} 인벤토리 추가됨");
                }
                else
                {
                    Debug.Log("[EquipmentPresenter] 인벤토리 가득! 무기 손실");
                }

                Debug.Log($"[해제 후] 장착: 없음, 공격력: {equipmentService.TotalAttack.Value}");
            }
            else
            {
                Debug.Log("[EquipmentPresenter] 장착된 무기 없음");
            }

            OnEquipmentChanged();
        }

        /// <summary>
        /// 자동 장착 버튼 클릭 처리
        /// 가장 강한 무기 자동 장착
        /// </summary>
        public void OnAutoEquipButtonClicked()
        {
            Debug.Log("[EquipmentPresenter] 자동 장착 클릭");

            var availableWeapons = inventoryService.GetAllWeapons();
            if (availableWeapons.Count == 0)
            {
                Debug.Log("[EquipmentPresenter] 인벤토리에 무기 없음");
                return;
            }

            // 가장 공격력 높은 무기 찾기
            Domain.Entities.Weapon bestWeapon = availableWeapons[0];
            foreach (var weapon in availableWeapons)
            {
                if (weapon.AttackPower > bestWeapon.AttackPower)
                    bestWeapon = weapon;
            }

            Debug.Log($"[EquipmentPresenter] 최고 무기: {bestWeapon.ItemName} (공격력: {bestWeapon.AttackPower})");

            // 현재 장착 무기보다 좋으면 교체
            if (equipmentService.EquippedWeapon.Value == null ||
                bestWeapon.AttackPower > equipmentService.EquippedWeapon.Value.AttackPower)
            {
                var prevWeapon = equipmentService.EquippedWeapon.Value;
                equipmentService.EquipWeapon(bestWeapon);

                // 새로 장착된 무기를 인벤토리에서 제거
                var slot = inventoryService.Inventory.FindWeapon(bestWeapon.ItemId);
                if (slot != null)
                {
                    inventoryService.RemoveItemFromSlot(slot.SlotIndex);
                    Debug.Log($"[EquipmentPresenter] {bestWeapon.ItemName} 인벤토리에서 제거됨 (장착됨)");
                }

                Debug.Log($"[EquipmentPresenter] 장착됨: {bestWeapon.ItemName}, 공격력: {equipmentService.TotalAttack.Value}");
            }
            else
            {
                Debug.Log("[EquipmentPresenter] 현재 무기가 더 강함");
            }

            OnEquipmentChanged();
        }

        /// <summary>
        /// 모두 줍기 버튼 클릭 처리
        /// 드롭된 모든 무기 습득
        /// </summary>
        public void OnPickupAllButtonClicked()
        {
            Debug.Log("[EquipmentPresenter] 모두 줍기 클릭");

            var weaponsToPickup = new List<Domain.Entities.Weapon>(itemDropService.DroppedWeapons);

            int pickedCount = 0;
            foreach (var weapon in weaponsToPickup)
            {
                if (inventoryService.AddItem(weapon))
                {
                    itemDropService.DroppedWeapons.Remove(weapon);
                    pickedCount++;
                    Debug.Log($"[EquipmentPresenter] 줍기: {weapon.ItemName} (공격력: {weapon.AttackPower})");
                }
                else
                {
                    Debug.Log("[EquipmentPresenter] 인벤토리 가득!");
                    break;
                }
            }

            Debug.Log($"[EquipmentPresenter] 총 줍기: {pickedCount}");
            OnEquipmentChanged();
            OnDroppedItemsChanged();
        }

        /// <summary>
        /// 장비 변경 시 뷰 업데이트
        /// </summary>
        private void OnEquipmentChanged()
        {
            if (view == null) return;

            view.DisplayEquippedWeapon(
                equipmentService.EquippedWeapon.Value,
                equipmentService.TotalAttack.Value
            );
        }

        /// <summary>
        /// 드롭된 아이템 변경 시 뷰 업데이트
        /// </summary>
        private void OnDroppedItemsChanged()
        {
            if (view == null) return;

            var droppedWeapons = new List<Domain.Entities.Weapon>(itemDropService.DroppedWeapons);
            view.DisplayDroppedItems(droppedWeapons);
        }

        /// <summary>
        /// 프레젠터 정리
        /// </summary>
        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}