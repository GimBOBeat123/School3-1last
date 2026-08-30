using Application;
using Presentation.Views;
using UniRx;
using UnityEngine;

namespace Presentation.Presenters
{
    /// <summary>
    /// 프레젠터
    /// 게임 데이터 저장과 로드 기능 제어
    /// </summary>
    public class SavePresenter
    {
        /// <summary>
        /// 프레젠터 생성 및 초기화
        /// 저장, 로드 버튼 이벤트 구독
        /// </summary>
        public SavePresenter(
            GameControlView view,
            SaveService saveService,
            BattleService battle,
            InventoryService inventoryService,
            EquipmentService equipmentService)
        {
            // 저장 버튼 클릭 처리
            view.OnSaveClicked
                .Subscribe(_ =>
                {
                    Debug.Log("[SavePresenter] 저장 클릭");
                    saveService.Save();
                    Debug.Log("[SavePresenter] 저장 완료");
                });

            // 로드 버튼 클릭 처리
            view.OnLoadClicked
                .Subscribe(_ =>
                {
                    Debug.Log("[SavePresenter] 로드 클릭");

                    // 게임 데이터 로드
                    var data = saveService.Load();
                    Debug.Log($"[SavePresenter] 데이터 로드 - 라운드: {data.Round}, 골드: {data.Gold}");

                    // 전투 시스템 복구
                    battle.Restore(data);
                    Debug.Log("[SavePresenter] 전투 시스템 복구");

                    Debug.Log("[SavePresenter] 로드 완료");
                });
        }
    }
}