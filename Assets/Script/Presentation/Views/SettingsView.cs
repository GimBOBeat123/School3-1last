using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Views
{
    /// <summary>
    /// 뷰
    /// 자동공격 토글과 게임 속도 슬라이더를 관리
    /// </summary>
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private Toggle autoAttackToggle;
        [SerializeField] private Slider gameSpeedSlider;
        [SerializeField] private TextMeshProUGUI gameSpeedValueText;

        /// <summary>
        /// 자동공격 활성화 여부를 나타내는 반응형 속성
        /// </summary>
        public ReactiveProperty<bool> AutoAttackEnabled = new(true);

        /// <summary>
        /// 게임 속도를 나타내는 반응형 속성
        /// </summary>
        public ReactiveProperty<float> GameSpeed = new(1f);

        private void Start()
        {
            Debug.Log("[SettingsView] Start");

            // 자동공격 토글 설정
            if (autoAttackToggle != null)
            {
                autoAttackToggle.isOn = true;

                autoAttackToggle.onValueChanged.AddListener((isOn) =>
                {
                    AutoAttackEnabled.Value = isOn;
                    Debug.Log($"[SettingsView] 자동공격: {(isOn ? "켜짐" : "꺼짐")}");
                });

                Debug.Log("[SettingsView] 자동공격 토글 연결됨");
            }
            else
            {
                Debug.LogWarning("[SettingsView] 자동공격 토글 미할당!");
            }

            // 게임 속도 슬라이더 설정
            if (gameSpeedSlider != null)
            {
                gameSpeedSlider.minValue = 0.5f;
                gameSpeedSlider.maxValue = 2.0f;
                gameSpeedSlider.value = 1.0f;

                gameSpeedSlider.onValueChanged.AddListener((speed) =>
                {
                    GameSpeed.Value = speed;
                    Time.timeScale = speed;

                    if (gameSpeedValueText != null)
                        gameSpeedValueText.text = $"배속 {speed:F1}";

                    Debug.Log($"[SettingsView] 게임 속도: {speed:F1}");
                });

                if (gameSpeedValueText != null)
                    gameSpeedValueText.text = "배속 1.0";

                Debug.Log("[SettingsView] 게임 속도 슬라이더 연결됨");
            }
            else
            {
                Debug.LogWarning("[SettingsView] 게임 속도 슬라이더 미할당!");
            }

            Debug.Log("[SettingsView] Start 완료");
        }

        private void OnDestroy()
        {
            // 게임 속도를 원래대로 복구
            Time.timeScale = 1f;
        }
    }
}