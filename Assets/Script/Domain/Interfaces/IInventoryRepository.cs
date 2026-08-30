using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// 인터페이스
    /// 인벤토리 및 장비 저장 및 로드 기능 정의
    /// 구현체: InventoryRepository
    /// </summary>
    public interface IInventoryRepository
    {
        /// <summary>
        /// 인벤토리 저장
        /// 모든 아이템 정보 저장
        /// </summary>
        void SaveInventory(Inventory inventory);

        /// <summary>
        /// 인벤토리 로드
        /// 저장된 아이템 복원
        /// </summary>
        Inventory LoadInventory();

        /// <summary>
        /// 장비 저장
        /// 현재 장착 무기 정보 저장
        /// </summary>
        void SaveEquipment(Equipment equipment);

        /// <summary>
        /// 장비 로드
        /// 저장된 장착 무기 복원
        /// </summary>
        Equipment LoadEquipment();
    }
}