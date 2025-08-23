using Databases;
using DG.Tweening;
using Enums;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hud.Slots
{
    public class InventorySlotUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public Image icon;
        public TextMeshProUGUI countText;

        private SourceType _sourceType;
        private int _currentCount;

        public void Setup(SourceType type, int count)
        {
            _sourceType = type;
            icon.sprite = ServiceLocator.Get<SourceDatabase>().Get(type).icon;
            _currentCount = count;
            countText.text = _currentCount.ToString();
        }
        public void UpdateCount(int newCount)
        {
            // اگه تعداد تغییر نکرده کاری نکن
            if (newCount == _currentCount) return;

            // Tween عدد از _currentCount به newCount
            DOTween.To(() => _currentCount, x => {
                _currentCount = x;
                countText.text = _currentCount.ToString();
            }, newCount, 0.5f).SetEase(Ease.OutBounce);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ServiceLocator.Get<InventoryUI>().ShowTooltip(ServiceLocator.Get<SourceDatabase>().Get(_sourceType).enumType.ToString(), Input.mousePosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ServiceLocator.Get<InventoryUI>().HideTooltip();
        }
    }
}
