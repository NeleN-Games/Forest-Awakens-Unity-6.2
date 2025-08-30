using Databases;
using DG.Tweening;
using Enums;
using Interfaces;
using Managers;
using Models.Data;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hud.Slots
{
    public class InventorySlotUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image icon;
        public TextMeshProUGUI countText;

        private IEquippable _equippable;
        private int _currentCount;
        
        private GameObject _dragIcon;
        private RectTransform _dragIconRect;
        
        public void Setup(IEquippable data, int count)
        {
            _equippable = data;  
            icon.sprite = _equippable.GetIcon();
            _currentCount = count;
            countText.text = _currentCount.ToString();
        }
        public void UpdateCount(int newCount)
        {
            if (newCount == _currentCount) return;

            DOTween.To(() => _currentCount, x => {
                _currentCount = x;
                countText.text = _currentCount.ToString();
            }, newCount, 0.5f).SetEase(Ease.OutBounce);
        }
        public IEquippable GetEquippable() => _equippable;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_equippable is SourceData sourceData)
            {
                ServiceLocator.Get<InventoryUI>().ShowTooltip
                    (ServiceLocator.Get<SourceDatabase>().Get(sourceData.GetEnum()).enumType.ToString(), Input.mousePosition);
            } 
            if (_equippable is ItemData itemData)
            {
                ServiceLocator.Get<InventoryUI>().ShowTooltip
                    (ServiceLocator.Get<ItemDatabase>().Get(itemData.GetEnum()).enumType.ToString(), Input.mousePosition);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ServiceLocator.Get<InventoryUI>().HideTooltip();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragIcon = ServiceLocator.Get<EquipmentManager>().cashedDragIcon;
            _dragIcon.SetActive(true); 
            _dragIcon.transform.SetParent(transform.root, false);
            var img = _dragIcon.GetComponent<Image>();
            img.raycastTarget = false;
            img.sprite = icon.sprite;
            _dragIconRect = _dragIcon.GetComponent<RectTransform>();
            _dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragIconRect != null)
                _dragIconRect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragIcon != null)
                _dragIcon.SetActive(false); 
        }
    }
}
