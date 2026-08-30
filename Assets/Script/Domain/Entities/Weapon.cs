using UnityEngine;

namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 아이템을 상속하여 무기 기능 추가
    /// </summary>
    [System.Serializable]
    public class Weapon : Item
    {
        /// <summary>
        /// 무기 공격력
        /// </summary>
        public int AttackPower { get; set; }

        /// <summary>
        /// 무기 치명타율
        /// </summary>
        public float CriticalChance { get; set; }

        /// <summary>
        /// 무기 등급
        /// 0: 일반, 1: 레어, 2: 에픽, 3: 전설
        /// </summary>
        public int Rarity { get; set; }

        private static string[] RarityNames = { "일반", "레어", "에픽", "전설" };

        /// <summary>
        /// 무기 생성
        /// </summary>
        public Weapon(
            string id,
            string name,
            int attack,
            float criticalChance = 0.1f,
            int rarity = 0,
            string desc = "")
            : base(id, name, desc)
        {
            AttackPower = attack;
            CriticalChance = criticalChance;
            Rarity = Mathf.Clamp(rarity, 0, 3);
            Quantity = 1;

            this.DropPosition = Vector3.zero;
        }

        /// <summary>
        /// 무기 정보 반환
        /// </summary>
        public override string GetDescription()
        {
            return $"{ItemName} [{GetRarityName()}]\n" +
                   $"공격력: +{AttackPower}\n" +
                   $"치명타: {CriticalChance * 100:F1}%\n" +
                   $"\n{Description}";
        }

        /// <summary>
        /// 등급 이름 반환
        /// </summary>
        public string GetRarityName()
        {
            return RarityNames[Rarity];
        }

        /// <summary>
        /// 등급별 색상 반환
        /// </summary>
        public Color GetRarityColor()
        {
            return Rarity switch
            {
                0 => Color.gray,
                1 => new Color(0, 1, 0),
                2 => new Color(1, 0, 1),
                3 => new Color(1, 0.84f, 0),
                _ => Color.gray
            };
        }
    }
}