using Application;
using Presentation.Views;
using UniRx;

namespace Presentation.Presenters
{
    /// <summary>
    /// 프레젠터
    /// 게임 완료 조건을 감시하고 클리어 뷰 제어
    /// </summary>
    public class ClearPresenter
    {
        /// <summary>
        /// 프레젠터 생성 및 초기화
        /// 게임 클리어 이벤트 구독
        /// </summary>
        public ClearPresenter(
            BattleService battle,
            ClearView view)
        {
            // 게임 클리어 상태 감시
            battle.IsGameClear
                .Where(x => x)
                .Subscribe(_ =>
                {
                    // 클리어 뷰 표시
                    view.Show();
                });
        }
    }
}