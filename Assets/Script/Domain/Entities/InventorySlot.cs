using UniRx;

namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 인벤토리의 개별 슬롯
    /// </summary>
    public class InventorySlot
    {
        /// <summary>
        /// 슬롯 번호
        /// </summary>
        public int SlotIndex { get; set; }

        /// <summary>
        /// 슬롯에 들어있는 아이템
        /// </summary>
        public ReactiveProperty<Item> Item = new();

        /// <summary>
        /// 슬롯이 비어있는지 여부
        /// </summary>
        public bool IsEmpty => Item.Value == null;

        /// <summary>
        /// 슬롯 생성
        /// </summary>
        public InventorySlot(int index)
        {
            SlotIndex = index;
            Item.Value = null;
        }

        /// <summary>
        /// 슬롯에 아이템 설정
        /// </summary>
        public void SetItem(Item item) => Item.Value = item;

        /// <summary>
        /// 슬롯의 아이템 반환
        /// </summary>
        public Item GetItem() => Item.Value;

        /// <summary>
        /// 슬롯의 아이템 제거
        /// </summary>
        public void RemoveItem() => Item.Value = null;

        /// <summary>
        /// 슬롯 비우기
        /// </summary>
        public void Clear() => Item.Value = null;

        /// <summary>
        /// 다른 슬롯과 아이템 교환
        /// </summary>
        public void SwapWith(InventorySlot otherSlot)
        {
            var temp = this.Item.Value;
            this.Item.Value = otherSlot.Item.Value;
            otherSlot.Item.Value = temp;
        }
    }
}