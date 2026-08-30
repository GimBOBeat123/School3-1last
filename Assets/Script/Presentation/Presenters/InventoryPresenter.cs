using Application;
using Presentation.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Presentation.Presenters
{
    /// <summary>
    /// 프레젠터
    /// 인벤토리 뷰와 서비스 연결
    /// </summary>
    public class InventoryPresenter
    {
        private InventoryView view;
        private InventoryService inventoryService;
        private CompositeDisposable disposables = new();

        [Inject]
        public void Construct(
            InventoryService inventoryService,
            InventoryView view)
        {
            this.inventoryService = inventoryService;
            this.view = view;

            Debug.Log("[InventoryPresenter] 주입 완료");
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
                Debug.LogError("[InventoryPresenter] 뷰가 널!");
                return;
            }

            view.SetPresenter(this);
            Debug.Log("[InventoryPresenter] 초기화");

            // 인벤토리 아이템 개수 변경 감시
            inventoryService.InventoryCount
                .Subscribe(count =>
                {
                    Debug.Log($"[InventoryPresenter] 아이템 개수 변경: {count}");
                    OnInventoryChanged();
                })
                .AddTo(disposables);

            // 각 슬롯 아이템 변경 감시
            foreach (var slot in inventoryService.Inventory.Slots)
            {
                slot.Item
                    .Subscribe(item =>
                    {
                        OnInventoryChanged();
                    })
                    .AddTo(disposables);
            }

            OnInventoryChanged();
            Debug.Log("[InventoryPresenter] 설정 완료");
        }

        /// <summary>
        /// 자동 정렬 버튼 클릭 처리
        /// 수동으로만 정렬 수행
        /// </summary>
        public void OnAutoArrangeButtonClicked()
        {
            Debug.Log("[InventoryPresenter] 자동 정렬 클릭");

            Debug.Log("[정렬 전]");
            int index = 0;
            foreach (var slot in inventoryService.Inventory.Slots)
            {
                if (!slot.IsEmpty)
                {
                    Debug.Log($"  슬롯 {index}: {slot.Item.Value.ItemName} (공격력: {(slot.Item.Value is Domain.Entities.Weapon w ? w.AttackPower.ToString() : "없음")})");
                }
                index++;
                if (index >= 5) break;
            }

            // 정렬 실행
            inventoryService.AutoArrangeInventory();

            Debug.Log("[정렬 후]");
            index = 0;
            foreach (var slot in inventoryService.Inventory.Slots)
            {
                if (!slot.IsEmpty)
                {
                    Debug.Log($"  슬롯 {index}: {slot.Item.Value.ItemName} (공격력: {(slot.Item.Value is Domain.Entities.Weapon w ? w.AttackPower.ToString() : "없음")})");
                }
                index++;
                if (index >= 5) break;
            }

            OnInventoryChanged();
        }

        /// <summary>
        /// 인벤토리 변경 시 뷰 업데이트
        /// </summary>
        private void OnInventoryChanged()
        {
            if (view == null) return;

            view.DisplayInventory(inventoryService.Inventory);
            view.UpdateInventoryCount(inventoryService.GetItemCount());
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