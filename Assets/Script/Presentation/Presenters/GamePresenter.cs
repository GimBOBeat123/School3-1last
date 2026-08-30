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
    /// 전투 시스템과 영웅 정보를 뷰에 반영
    /// 생명주기(Initialize/Dispose)는 Zenject 컨테이너가 관리
    /// </summary>
    public class GamePresenter : IInitializable, IDisposable
    {
        private readonly HUDView hudView;
        private readonly ClearView clearView;
        private readonly BattleService battleService;
        private readonly Hero hero;

        private readonly CompositeDisposable disposables = new();

        public GamePresenter(
            HUDView hudView,
            ClearView clearView,
            BattleService battleService,
            Hero hero)
        {
            this.hudView = hudView;
            this.clearView = clearView;
            this.battleService = battleService;
            this.hero = hero;
        }

        /// <summary>
        /// 모든 이벤트 구독 설정
        /// 오브젝트 그래프가 완성된 뒤 Zenject가 호출
        /// </summary>
        public void Initialize()
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
            // 몬스터가 교체되면 Switch가 이전 구독을 정리해 누적을 막음
            battleService.CurrentMonster
                .Where(monster => monster != null)
                .Select(monster => monster.CurrentHp
                    .Select(currentHp => (currentHp, maxHp: monster.MaxHp.Value)))
                .Switch()
                .Subscribe(hp => hudView.SetMonsterHp(hp.currentHp, hp.maxHp))
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
                .Where(cleared => cleared)
                .Subscribe(_ =>
                {
                    clearView.Show();
                    Debug.Log("[GamePresenter] 게임 완료!");
                })
                .AddTo(disposables);

            Debug.Log("[GamePresenter] 설정 완료");
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
