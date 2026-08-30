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
    /// </summary>
    public class AutoAttackRunner
    {
        private BattleService battleService;
        private SettingsView settingsView;
        private CompositeDisposable disposables = new();

        [Inject]
        public void Construct(BattleService battleService, SettingsView settingsView)
        {
            this.battleService = battleService;
            this.settingsView = settingsView;

            Debug.Log("[AutoAttackRunner] 주입 완료");
            Initialize();
        }

        /// <summary>
        /// 자동 공격 초기화
        /// 타이머 설정 및 필터 구성
        /// </summary>
        private void Initialize()
        {
            if (settingsView == null)
            {
                Debug.LogError("[AutoAttackRunner] 설정 뷰가 널!");
                return;
            }

            Debug.Log("[AutoAttackRunner] 초기화");

            // 1초마다 공격 실행 (토글로 제어)
            Observable.Interval(System.TimeSpan.FromSeconds(1))
                .Where(_ =>
                {
                    // 안전성 검사
                    if (battleService == null || settingsView == null)
                        return false;

                    return !battleService.IsGameClear.Value &&  // 게임 클리어 전
                           settingsView.AutoAttackEnabled.Value;  // 자동공격 켜짐
                })
                .Subscribe(_ =>
                {
                    battleService.Attack();
                    Debug.Log("[AutoAttackRunner] 공격!");
                })
                .AddTo(disposables);

            Debug.Log("[AutoAttackRunner] 설정 완료");
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