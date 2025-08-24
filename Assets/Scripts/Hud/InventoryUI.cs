using System.Collections.Generic;
using Databases;
using DG.Tweening;
using Enums;
using Hud.Slots;
using Interfaces;
using Services;
using TMPro;
using UnityEngine;

namespace Hud
{
    public class InventoryUI : MonoBehaviour,IInitializable
    {
        public RectTransform panel;
        public GameObject slotPrefab;
        public Transform slotParent;
        [SerializeField] private TextMeshProUGUI tooltipText;
        [SerializeField] private SourceDatabase sourceDatabase;

        private readonly Dictionary<SourceType, InventorySlotUI> _slots = new();

        private bool _isOpen = false;
        private  Vector2 _cashedPanelPosition; 
        [Tooltip("Using this offset for padding from left or right. use negative value when panel is in right.")]
        public float xOffset=-10; 
        public void Initialize()
        {
            sourceDatabase=ServiceLocator.Get<SourceDatabase>();
            RefreshInventory(ServiceLocator.Get<PlayerInventory>().GetInventory());
            panel.anchoredPosition = new Vector2(+panel.rect.width, 0);
            _cashedPanelPosition = new Vector2(+panel.rect.width, 0);
        }

        public void OnDestroy() { }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
        }

        private void ToggleInventory()
        {
            _isOpen = !_isOpen;
            if (_isOpen)
                panel.DOAnchorPos((-_cashedPanelPosition / 2)+Vector2.right*xOffset, 0.5f).SetEase(Ease.OutExpo);
            else
                panel.DOAnchorPos(_cashedPanelPosition, 0.5f).SetEase(Ease.InExpo);
        }

        public void RefreshInventory(Dictionary<SourceType, int> inventoryData)
        {
            
            foreach (var item in inventoryData)
            {
                if (_slots.TryGetValue(item.Key, out var slot))
                {
                    // if slot already exists, just update the count
                    slot.UpdateCount(item.Value);
                }
                else
                {
                    // if slot doesn't exist, create a new one
                    GameObject slotObj = Instantiate(slotPrefab, slotParent);
                    slot = slotObj.GetComponent<InventorySlotUI>();
                    slot.Setup(item.Key, item.Value);
                    _slots[item.Key] = slot;
                }
            }

            // if any slots exist that are not in the inventory data, remove them
            var keysToRemove = new List<SourceType>();
            foreach (var existing in _slots)
            {
                if (!inventoryData.ContainsKey(existing.Key))
                {
                    Destroy(existing.Value.gameObject);
                    keysToRemove.Add(existing.Key);
                }
            }

            foreach (var key in keysToRemove)
                _slots.Remove(key);
            
        }

        public void ShowTooltip(string text, Vector2 position)
        {
            tooltipText.text = text;
            tooltipText.transform.position = position;
            tooltipText.gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            tooltipText.gameObject.SetActive(false);
        }

       
    }
}
