using UnityEngine;

namespace Presentation.Views
{
    /// <summary>
    /// 뷰
    /// 게임 완료 시 클리어 패널을 표시
    /// </summary>
    public class ClearView : MonoBehaviour
    {
        [SerializeField]
        private GameObject clearPanel;

        private void Awake()
        {
            clearPanel.SetActive(false);
        }

        /// <summary>
        /// 클리어 패널을 화면에 표시
        /// </summary>
        public void Show()
        {
            clearPanel.SetActive(true);
        }
    }
}