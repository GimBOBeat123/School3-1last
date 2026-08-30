namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 장비를 JSON으로 저장할 때 사용
    /// </summary>
    [System.Serializable]
    public class EquipmentData
    {
        /// <summary>
        /// 장착 무기 정보
        /// </summary>
        public ItemData EquippedWeapon;

        /// <summary>
        /// 추가 공격력
        /// </summary>
        public int AdditionalAttack;

        /// <summary>
        /// 장비 데이터 생성
        /// </summary>
        public EquipmentData() { }

        /// <summary>
        /// 장비 객체를 데이터로 변환
        /// </summary>
        public static EquipmentData FromEquipment(Equipment equipment)
        {
            return new EquipmentData
            {
                EquippedWeapon = equipment.EquippedWeapon.Value != null
                    ? ItemData.FromItem(equipment.EquippedWeapon.Value)
                    : null,
                AdditionalAttack = equipment.AdditionalAttack.Value
            };
        }

        /// <summary>
        /// 저장된 데이터를 장비 객체로 복원
        /// </summary>
        public Equipment ToEquipment()
        {
            var equipment = new Equipment();
            if (EquippedWeapon != null)
            {
                equipment.EquipWeapon(EquippedWeapon.ToItem() as Weapon);
            }
            return equipment;
        }
    }
}