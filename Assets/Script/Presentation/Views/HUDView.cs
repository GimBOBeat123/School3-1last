using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Views
{
    /// <summary>
    /// 뷰
    /// 게임의 라운드, 골드, 공격력, 몬스터 HP를 화면에 표시
    /// </summary>
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private TMP_Text roundText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text attackText;

        [SerializeField] private TMP_Text hpText;
        [SerializeField] private Slider hpSlider;

        /// <summary>
        /// 라운드 번호를 화면에 표시
        /// </summary>
        public void SetRound(int value)
        {
            Debug.Log($"Round UI {value}");
            roundText.text = $"Round : {value}";
        }

        /// <summary>
        /// 골드 수량을 화면에 표시
        /// </summary>
        public void SetGold(int value)
        {
            goldText.text = $"Gold : {value}";
        }

        /// <summary>
        /// 영웅의 공격력을 화면에 표시
        /// </summary>
        public void SetAttack(int value)
        {
            attackText.text = $"ATK : {value}";
        }

        /// <summary>
        /// 몬스터의 현재 체력을 화면에 표시
        /// </summary>
        public void SetMonsterHp(int current, int max)
        {
            hpText.text = $"{current} / {max}";

            hpSlider.maxValue = max;
            hpSlider.value = current;
        }
    }
}