using UnityEngine;

namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 게임의 모든 아이템 기본 클래스
    /// </summary>
    [System.Serializable]
    public class Item
    {
        /// <summary>
        /// 아이템 고유 ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 아이템 이름
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 아이템 설명
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 수량
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 드롭 위치
        /// </summary>
        [System.NonSerialized]
        public Vector3 DropPosition = Vector3.zero;

        /// <summary>
        /// 아이템 생성
        /// </summary>
        public Item(string id, string name, string desc = "")
        {
            ItemId = id;
            ItemName = name;
            Description = desc;
            Quantity = 1;
        }

        /// <summary>
        /// 아이템 설명 반환
        /// </summary>
        public virtual string GetDescription()
        {
            return $"{ItemName}\n{Description}";
        }
    }
}