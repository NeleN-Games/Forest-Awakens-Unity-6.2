using System;
using Enums;
using Interfaces;
using Models.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hud.Slots
{
    public class CraftableSlotUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image icon;
        private Action onClicked;
        private ICraftable _craftable;

        public void Setup(ICraftable craftable)
        {
            _craftable = craftable;
            title.text = _craftable.UniqueId.UniqueName;
            Sprite sprite = null;
            if (_craftable is CraftableAssetData<ItemType> item)
            {
                sprite = item.icon;
            }
            else if (_craftable is CraftableAssetData<ItemType> building)
            {
                sprite = building.icon;
            }
            else
            {
                Debug.LogError("Unknown ICraftable type");
            }
            if (sprite != null)
            {
                SetIcon(sprite);
            }
            onClicked += _craftable.Craft;
        }
        private void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
            float aspect = sprite.rect.width / sprite.rect.height;
            RectTransform rt = icon.GetComponent<RectTransform>();
            float targetHeight = icon.rectTransform.rect.height;
            float targetWidth = targetHeight * aspect;

            rt.sizeDelta = new Vector2(targetWidth, targetHeight);
            icon.sprite = sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClicked?.Invoke();
        }

        public void ChangeAvailability(bool isAvailable)
        {
            if (isAvailable)
            {
                ChangeToAvailable();
            }
            else
            {
                ChangeToUnavailable();
            }
        }

        private void ChangeToAvailable()
        {
            icon.color = Color.white;
        }
        private void ChangeToUnavailable()
        {
            icon.color = Color.gray;
        }
    }
}
