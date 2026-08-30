using System;
using Application;
using Presentation.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Presentation
{
    /// <summary>
    /// 프레젠터
    /// 1초마다 자동으로 공격 실행
    /// 생명주기(Initialize/Dispose)는 Zenject 컨테이너가 관리
    /// </summary>
    public class AutoAttackRunner : IInitializable, IDisposable
    {
        private readonly BattleService battleService;
        private readonly SettingsView settingsView;

        private readonly CompositeDisposable disposables = new();

        public AutoAttackRunner(BattleService battleService, SettingsView settingsView)
        {
            this.battleService = battleService;
            this.settingsView = settingsView;
        }

        /// <summary>
        /// 자동 공격 타이머 설정
        /// </summary>
        public void Initialize()
        {
            if (settingsView == null)
            {
                Debug.LogError("[AutoAttackRunner] 설정 뷰가 널!");
                return;
            }

            // 1초마다 공격 실행 (토글로 제어)
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Where(_ => !battleService.IsGameClear.Value &&   // 게임 클리어 전
                            settingsView.AutoAttackEnabled.Value) // 자동공격 켜짐
                .Subscribe(_ => battleService.Attack())
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
