using System;
using Application;
using Presentation.Views;
using UniRx;
using Zenject;

namespace Presentation.Presenters
{
    /// <summary>
    /// 프레젠터
    /// 게임 완료 조건을 감시하고 클리어 뷰 제어
    /// 생명주기(Initialize/Dispose)는 Zenject 컨테이너가 관리
    /// </summary>
    public class ClearPresenter : IInitializable, IDisposable
    {
        private readonly BattleService battle;
        private readonly ClearView view;

        private readonly CompositeDisposable disposables = new();

        public ClearPresenter(BattleService battle, ClearView view)
        {
            this.battle = battle;
            this.view = view;
        }

        /// <summary>
        /// 게임 클리어 상태 감시
        /// </summary>
        public void Initialize()
        {
            battle.IsGameClear
                .Where(cleared => cleared)
                .Subscribe(_ => view.Show())
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
