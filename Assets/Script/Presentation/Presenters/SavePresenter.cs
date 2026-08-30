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
    /// 게임 데이터 저장과 로드 기능 제어
    /// 생명주기(Initialize/Dispose)는 Zenject 컨테이너가 관리
    /// </summary>
    public class SavePresenter : IInitializable, IDisposable
    {
        private readonly GameControlView view;
        private readonly SaveService saveService;
        private readonly BattleService battle;

        private readonly CompositeDisposable disposables = new();

        public SavePresenter(
            GameControlView view,
            SaveService saveService,
            BattleService battle)
        {
            this.view = view;
            this.saveService = saveService;
            this.battle = battle;
        }

        /// <summary>
        /// 저장, 로드 버튼 이벤트 구독
        /// </summary>
        public void Initialize()
        {
            // 저장 버튼 클릭 처리
            view.OnSaveClicked
                .Subscribe(_ =>
                {
                    Debug.Log("[SavePresenter] 저장 클릭");
                    saveService.Save();
                    Debug.Log("[SavePresenter] 저장 완료");
                })
                .AddTo(disposables);

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
                    Debug.Log("[SavePresenter] 로드 완료");
                })
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
