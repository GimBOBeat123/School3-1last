using System;
using Domain.Entities;
using UniRx;
using UnityEngine;

namespace Application
{
    /// <summary>
    /// 모델
    /// 무기 장착, 해제, 공격력 계산
    /// Dispose는 Zenject 컨테이너가 호출 (BindInterfacesAndSelfTo)
    /// </summary>
    public class EquipmentService : IDisposable
    {
        private readonly Hero hero;
        private readonly InventoryService inventoryService;

        /// <summary>
        /// 장비 정보
        /// </summary>
        public Equipment Equipment { get; private set; }

        /// <summary>
        /// 현재 장착한 무기
        /// </summary>
        public ReactiveProperty<Weapon> EquippedWeapon => Equipment.EquippedWeapon;

        /// <summary>
        /// 총 공격력
        /// </summary>
        public ReactiveProperty<int> TotalAttack = new();

        /// <summary>
        /// 기본 공격력 (무기 공격력 제외)
        /// </summary>
        private int baseAttack = 1;

        private CompositeDisposable disposables = new();

        /// <summary>
        /// 장비 서비스 생성
        /// </summary>
        public EquipmentService(Hero hero, InventoryService inventoryService)
        {
            this.hero = hero;
            this.inventoryService = inventoryService;
            Equipment = new Equipment();

            baseAttack = hero.Attack.Value;
            Debug.Log($"[EquipmentService] 기본 공격력: {baseAttack}");

            // 장비 변경 시 공격력 업데이트
            Equipment.EquippedWeapon
                .Subscribe(_ => UpdateTotalAttack())
                .AddTo(disposables);
        }

        /// <summary>
        /// 무기 장착
        /// 이전 무기를 먼저 해제
        /// </summary>
        public bool EquipWeapon(Weapon weapon)
        {
            if (weapon == null)
                return false;

            UnequipWeapon();

            Equipment.EquipWeapon(weapon);
            UpdateTotalAttack();

            Debug.Log($"[EquipmentService] 장착됨: {weapon.ItemName}");
            Debug.Log($"[EquipmentService] 기본: {baseAttack}, 무기: {weapon.AttackPower}, 합계: {baseAttack + weapon.AttackPower}");
            return true;
        }

        /// <summary>
        /// 무기 해제
        /// 해제된 무기를 인벤토리에 추가
        /// </summary>
        public void UnequipWeapon()
        {
            if (Equipment.EquippedWeapon.Value != null)
            {
                Debug.Log($"[EquipmentService] 해제 중: {Equipment.EquippedWeapon.Value.ItemName}");

                var weapon = Equipment.EquippedWeapon.Value;
                Equipment.UnequipWeapon();
                UpdateTotalAttack();

                // 해제된 무기를 인벤토리에 추가
                if (inventoryService.AddItem(weapon))
                {
                    Debug.Log($"[EquipmentService] {weapon.ItemName} 인벤토리 추가");
                }
                else
                {
                    Debug.Log($"[EquipmentService] 인벤토리 가득! {weapon.ItemName} 손실");
                }
            }
            else
            {
                Equipment.UnequipWeapon();
                UpdateTotalAttack();
            }

            Debug.Log($"[EquipmentService] 해제됨. 기본: {baseAttack}, 무기: 0, 합계: {baseAttack}");
        }

        /// <summary>
        /// 가장 강한 무기 자동 장착
        /// 현재 장착 무기보다 좋으면 교체
        /// </summary>
        public void AutoEquipBestWeapon()
        {
            var weapons = inventoryService.GetAllWeapons();
            if (weapons.Count == 0)
            {
                UnequipWeapon();
                return;
            }

            Weapon bestWeapon = weapons[0];
            foreach (var weapon in weapons)
            {
                if (weapon.AttackPower > bestWeapon.AttackPower)
                    bestWeapon = weapon;
            }

            if (Equipment.EquippedWeapon.Value == null ||
                bestWeapon.AttackPower > Equipment.EquippedWeapon.Value.AttackPower)
            {
                EquipWeapon(bestWeapon);
            }
        }

        /// <summary>
        /// 총 공격력 계산 및 업데이트
        /// 기본 공격력 + 무기 공격력
        /// </summary>
        private void UpdateTotalAttack()
        {
            int weaponAttack = Equipment.AdditionalAttack.Value;
            int totalAttack = baseAttack + weaponAttack;

            TotalAttack.Value = totalAttack;

            hero.SetAttack(totalAttack);

            Debug.Log($"[EquipmentService] 총 공격력: {baseAttack} + {weaponAttack} = {totalAttack}");
        }

        /// <summary>
        /// 총 공격력 반환
        /// </summary>
        public int GetTotalAttack()
        {
            return TotalAttack.Value;
        }

        /// <summary>
        /// 서비스 정리
        /// </summary>
        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}