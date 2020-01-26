using Assets.GameFramework;
using Assets.GameFramework.Item.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GameProject_1.UI
{

    public class UIDragItem : GFrameworkEntityBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UIDropSlot currentSlot;
        public GFrameworkEntityBase prefab;
        public Texture2D prefabCursor;

        [HideInInspector] 
        public TextMeshProUGUI counter;

        // Reference to the canvas.
        private Canvas canvas;
        // Reference to UI raycaster.
        private GraphicRaycaster graphicRaycaster;

        private void Awake()
        {
            counter = GetComponentInChildren<TextMeshProUGUI>();
        }

        public IItem GetCurrentItem()
        {
            if (prefab == null) return null;
            return prefab.GetComponent<IItem>();
        }

        public IGFrameworkEntityBase GetCurrentEntity()
        {
            if (prefab == null) return null;
            return prefab.GetComponent<IGFrameworkEntityBase>();
        }

        public override List<KeyValuePair<string, object>> GetEntityFields()
        {
            return GetCurrentEntity().GetEntityFields();
        }

        public override string GetOriginalName()
        {
            return GetCurrentEntity().GetOriginalName();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Start moving object from the beginning!
            transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0) / transform.lossyScale.x;
            // Thanks to the canvas scaler we need to devide pointer delta by canvas scale to match pointer movement.

            // We need a few references from UI.
            if (!canvas)
            {
                canvas = GetComponentInParent<Canvas>();
                graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            }

            // Change parent of our item to the canvas.
            transform.SetParent(canvas.transform, true);
            // And set it as last child to be rendered on top of UI.
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Continue moving object around screen.
            transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0) / transform.lossyScale.x;
            // Thanks to the canvas scaler we need to devide pointer delta by canvas scale to match pointer movement.
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // On end we need to test if we can drop item into new slot.
            var results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            // Check all hits.
            foreach (var hit in results)
            {
                // If we found slot.
                var slot = hit.gameObject.GetComponent<UIDropSlot>();
                if (slot)
                {
                    // We should check if we can place ourselves there.
                    if (!slot.SlotFilled)
                    {
                        // Swapping references.
                        currentSlot.currentItem = null;
                        currentSlot = slot;
                        currentSlot.currentItem = this;
                    }

                    // In either cases we should break check loop.
                    break;
                }
            }

            // Changing parent back to slot.
            transform.SetParent(currentSlot.transform);

            // And centering item position.
            transform.localPosition = Vector3.zero;
        }
    }

}