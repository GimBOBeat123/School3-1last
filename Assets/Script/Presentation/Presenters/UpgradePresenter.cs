using System;
using Application;
using Domain.Entities;
using Presentation.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Presentation.Presenters
{
    /// <summary>
    /// 프레젠터
    /// 업그레이드 뷰와 서비스를 연결
    /// 생명주기(Initialize/Dispose)는 Zenject 컨테이너가 관리
    /// </summary>
    public class UpgradePresenter : IInitializable, IDisposable
    {
        private readonly UpgradeView view;
        private readonly UpgradeService service;
        private readonly Hero hero;

        private readonly CompositeDisposable disposables = new();

        public UpgradePresenter(
            UpgradeView view,
            UpgradeService service,
            Hero hero)
        {
            this.view = view;
            this.service = service;
            this.hero = hero;
        }

        /// <summary>
        /// 뷰 이벤트 구독
        /// </summary>
        public void Initialize()
        {
            if (view == null)
            {
                Debug.LogError("[UpgradePresenter] 뷰가 널!");
                return;
            }

            Debug.Log("[UpgradePresenter] 초기화");

            // 업그레이드 버튼 클릭 감시
            view.OnUpgradeClicked
                .Subscribe(_ => OnUpgradeButtonClicked())
                .AddTo(disposables);

            // 영웅 공격력 변경 감시
            hero.Attack
                .Subscribe(attack => Debug.Log($"[UpgradePresenter] 영웅 공격력: {attack}"))
                .AddTo(disposables);

            // 영웅 골드 변경 감시
            hero.Gold
                .Subscribe(gold => Debug.Log($"[UpgradePresenter] 영웅 골드: {gold}"))
                .AddTo(disposables);

            Debug.Log("[UpgradePresenter] 설정 완료");
        }

        /// <summary>
        /// 업그레이드 버튼 클릭 처리
        /// </summary>
        private void OnUpgradeButtonClicked()
        {
            Debug.Log("========== [UpgradePresenter] 업그레이드 버튼 클릭! ==========");

            int currentAttack = hero.Attack.Value;
            int currentGold = hero.Gold.Value;
            int cost = service.UpgradeCost;

            Debug.Log($"[UpgradePresenter] 업그레이드 전 - 공격력: {currentAttack}, 골드: {currentGold}");
            Debug.Log($"[UpgradePresenter] 필요 비용: {cost}");

            if (currentGold < cost)
            {
                Debug.Log($"[UpgradePresenter] 골드 부족! 필요: {cost}, 보유: {currentGold}");
                return;
            }

            bool success = service.Upgrade();

            Debug.Log($"[UpgradePresenter] 업그레이드 결과: {success}");
            Debug.Log($"[UpgradePresenter] 업그레이드 후 - 공격력: {hero.Attack.Value}, 골드: {hero.Gold.Value}");
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
