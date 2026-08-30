using UniRx;
using UnityEngine;

namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 현재 장착한 무기 및 추가 능력 관리
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// 현재 장착한 무기
        /// </summary>
        public ReactiveProperty<Weapon> EquippedWeapon = new();

        /// <summary>
        /// 무기로부터 추가되는 공격력
        /// </summary>
        public ReactiveProperty<int> AdditionalAttack = new(0);

        /// <summary>
        /// 장착 무기 여부
        /// </summary>
        public bool HasEquippedWeapon => EquippedWeapon.Value != null;

        /// <summary>
        /// 장비 생성
        /// </summary>
        public Equipment()
        {
            EquippedWeapon.Value = null;
            AdditionalAttack.Value = 0;
        }

        /// <summary>
        /// 무기 장착
        /// 기존 무기가 있으면 먼저 해제
        /// </summary>
        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                UnequipWeapon();
                return;
            }

            // 기존 무기 해제
            UnequipWeapon();

            // 새 무기 장착
            EquippedWeapon.Value = weapon;
            AdditionalAttack.Value = weapon.AttackPower;

            Debug.Log($"[Equipment] 장착됨: {weapon.ItemName} (+{weapon.AttackPower} 공격력)");
        }

        /// <summary>
        /// 무기 해제
        /// </summary>
        public void UnequipWeapon()
        {
            EquippedWeapon.Value = null;
            AdditionalAttack.Value = 0;

            Debug.Log("[Equipment] 무기 해제됨");
        }

        /// <summary>
        /// 총 공격력 계산
        /// 기본 공격력 + 무기 공격력
        /// </summary>
        public int GetTotalAttack(int baseAttack)
        {
            return baseAttack + AdditionalAttack.Value;
        }

        /// <summary>
        /// 현재 장착 무기의 치명타율 반환
        /// </summary>
        public float GetCriticalChance()
        {
            if (EquippedWeapon.Value == null)
                return 0;

            return EquippedWeapon.Value.CriticalChance;
        }
    }
}