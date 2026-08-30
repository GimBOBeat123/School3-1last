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
    /// 전투 시스템과 영웅 정보를 뷰에 반영
    /// </summary>
    public class GamePresenter
    {
        private HUDView hudView;
        private ClearView clearView;

        private BattleService battleService;
        private Hero hero;
        private CompositeDisposable disposables = new();

        [Inject]
        public void Construct(
            HUDView hudView,
            ClearView clearView,
            BattleService battleService,
            Hero hero)
        {
            this.hudView = hudView;
            this.clearView = clearView;
            this.battleService = battleService;
            this.hero = hero;

            Debug.Log("[GamePresenter] 주입 완료");
            Initialize();
        }

        /// <summary>
        /// 프레젠터 초기화
        /// 모든 이벤트 구독 설정
        /// </summary>
        private void Initialize()
        {
            if (hudView == null || clearView == null)
            {
                Debug.LogError("[GamePresenter] 뷰가 널!");
                return;
            }

            Debug.Log("[GamePresenter] 초기화");

            // 라운드 변경 감시
            battleService.CurrentRound
                .Subscribe(round =>
                {
                    hudView.SetRound(round);
                    Debug.Log($"[GamePresenter] 라운드: {round}");
                })
                .AddTo(disposables);

            // 몬스터 체력 변경 감시
            battleService.CurrentMonster
                .Where(m => m != null)
                .Subscribe(monster =>
                {
                    monster.CurrentHp
                        .Subscribe(hp =>
                        {
                            hudView.SetMonsterHp(hp, monster.MaxHp.Value);
                        })
                        .AddTo(disposables);
                })
                .AddTo(disposables);

            // 영웅 공격력 변경 감시
            hero.Attack
                .Subscribe(attack =>
                {
                    hudView.SetAttack(attack);
                    Debug.Log($"[GamePresenter] 공격력: {attack}");
                })
                .AddTo(disposables);

            // 영웅 골드 변경 감시
            hero.Gold
                .Subscribe(gold =>
                {
                    hudView.SetGold(gold);
                    Debug.Log($"[GamePresenter] 골드: {gold}");
                })
                .AddTo(disposables);

            // 게임 클리어 감시
            battleService.IsGameClear
                .Where(x => x)
                .Subscribe(_ =>
                {
                    clearView.Show();
                    Debug.Log("[GamePresenter] 게임 완료!");
                })
                .AddTo(disposables);

            Debug.Log("[GamePresenter] 설정 완료");
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