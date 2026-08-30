using System;
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
    /// 생명주기(Initialize/Dispose)는 Zenject 컨테이너가 관리
    /// </summary>
    public class InventoryPresenter : IInitializable, IDisposable
    {
        private readonly InventoryView view;
        private readonly InventoryService inventoryService;

        private readonly CompositeDisposable disposables = new();

        public InventoryPresenter(
            InventoryService inventoryService,
            InventoryView view)
        {
            this.inventoryService = inventoryService;
            this.view = view;
        }

        /// <summary>
        /// 뷰 설정 및 이벤트 구독
        /// </summary>
        public void Initialize()
        {
            if (view == null)
            {
                Debug.LogError("[InventoryPresenter] 뷰가 널!");
                return;
            }

            view.SetPresenter(this);

            // 인벤토리 아이템 개수 변경 감시
            inventoryService.InventoryCount
                .Subscribe(_ => OnInventoryChanged())
                .AddTo(disposables);

            // 각 슬롯 아이템 변경 감시
            foreach (var slot in inventoryService.Inventory.Slots)
            {
                slot.Item
                    .Subscribe(_ => OnInventoryChanged())
                    .AddTo(disposables);
            }

            OnInventoryChanged();
        }

        /// <summary>
        /// 자동 정렬 버튼 클릭 처리
        /// 수동으로만 정렬 수행
        /// </summary>
        public void OnAutoArrangeButtonClicked()
        {
            Debug.Log("[InventoryPresenter] 자동 정렬 클릭");

            Debug.Log("[정렬 전]");
            LogFirstSlots();

            inventoryService.AutoArrangeInventory();

            Debug.Log("[정렬 후]");
            LogFirstSlots();

            OnInventoryChanged();
        }

        /// <summary>
        /// 디버그용: 앞쪽 5개 슬롯 상태 출력
        /// </summary>
        private void LogFirstSlots()
        {
            int index = 0;
            foreach (var slot in inventoryService.Inventory.Slots)
            {
                if (!slot.IsEmpty)
                {
                    string atk = slot.Item.Value is Domain.Entities.Weapon w ? w.AttackPower.ToString() : "없음";
                    Debug.Log($"  슬롯 {index}: {slot.Item.Value.ItemName} (공격력: {atk})");
                }
                if (++index >= 5) break;
            }
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

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
