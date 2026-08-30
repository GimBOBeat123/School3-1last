namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 아이템을 JSON으로 저장할 때 사용
    /// </summary>
    [System.Serializable]
    public class ItemData
    {
        /// <summary>
        /// 아이템 고유 ID
        /// </summary>
        public string ItemId;

        /// <summary>
        /// 아이템 이름
        /// </summary>
        public string ItemName;

        /// <summary>
        /// 아이템 설명
        /// </summary>
        public string Description;

        /// <summary>
        /// 수량
        /// </summary>
        public int Quantity;

        /// <summary>
        /// 아이템 타입
        /// "Weapon" 또는 "Item"
        /// </summary>
        public string ItemType;

        /// <summary>
        /// 무기 공격력 (무기인 경우)
        /// </summary>
        public int AttackPower;

        /// <summary>
        /// 무기 치명타율 (무기인 경우)
        /// </summary>
        public float CriticalChance;

        /// <summary>
        /// 무기 등급 (무기인 경우)
        /// </summary>
        public int Rarity;

        /// <summary>
        /// 아이템 데이터 생성
        /// </summary>
        public ItemData() { }

        /// <summary>
        /// 아이템 객체를 데이터로 변환
        /// </summary>
        public static ItemData FromItem(Item item)
        {
            var data = new ItemData
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Description = item.Description,
                Quantity = item.Quantity,
                ItemType = item is Weapon ? "Weapon" : "Item"
            };

            // 무기면 추가 정보 저장
            if (item is Weapon weapon)
            {
                data.AttackPower = weapon.AttackPower;
                data.CriticalChance = weapon.CriticalChance;
                data.Rarity = weapon.Rarity;
            }

            return data;
        }

        /// <summary>
        /// 저장된 데이터를 아이템 객체로 복원
        /// </summary>
        public Item ToItem()
        {
            if (ItemType == "Weapon")
                return new Weapon(ItemId, ItemName, AttackPower, CriticalChance, Rarity, Description);
            return new Item(ItemId, ItemName, Description);
        }
    }
}