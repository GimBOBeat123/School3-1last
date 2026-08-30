using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Views
{
    /// <summary>
    /// 뷰
    /// 공격력을 업그레이드하는 버튼을 관리
    /// </summary>
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private Button upgradeButton;

        private readonly Subject<Unit> upgradeClicked = new();

        /// <summary>
        /// 업그레이드 버튼 클릭 이벤트를 반환
        /// </summary>
        public IObservable<Unit> OnUpgradeClicked =>
            upgradeClicked;

        private void Start()
        {
            upgradeButton.onClick.AddListener(() =>
            {
                upgradeClicked.OnNext(Unit.Default);
            });
        }
    }
}