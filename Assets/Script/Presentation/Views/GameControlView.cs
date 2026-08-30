using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Views
{
    /// <summary>
    /// 뷰
    /// 저장과 로드 버튼을 관리
    /// </summary>
    public class GameControlView : MonoBehaviour
    {
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private readonly Subject<Unit> saveClicked = new();
        private readonly Subject<Unit> loadClicked = new();

        /// <summary>
        /// 저장 버튼 클릭 이벤트를 반환
        /// </summary>
        public IObservable<Unit> OnSaveClicked => saveClicked;

        /// <summary>
        /// 로드 버튼 클릭 이벤트를 반환
        /// </summary>
        public IObservable<Unit> OnLoadClicked => loadClicked;

        private void Start()
        {
            saveButton.onClick.AddListener(() =>
            {
                saveClicked.OnNext(Unit.Default);
            });

            loadButton.onClick.AddListener(() =>
            {
                loadClicked.OnNext(Unit.Default);
            });
        }
    }
}