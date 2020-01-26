using Assets.GameFramework;
using Assets.GameFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.GameProject_1.UI
{

    public class UIDropSlot : GFrameworkEntityBase, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // Reference to the item inside slot.
        public UIDragItem currentItem;

        private UIRootManager uiRoot;
        private CursorManager cursorManager;

        private void Awake()
        {
            uiRoot = GameObject.FindGameObjectWithTag("UIRoot").GetComponent<UIRootManager>();
            cursorManager = GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>();
        }

        // Tells if slot is filled by other item.
        public bool SlotFilled => currentItem;

        public void OnPointerClick(PointerEventData eventData)
        {
            uiRoot.ChangeCursorToCurrentPrefab(this);
            cursorManager.keepLastTexture = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cursorManager.SetCursor(cursorManager.defaultCursor);
            cursorManager.hoverEntity = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            cursorManager.SetCursor(cursorManager.hoverItemCursor);
            cursorManager.hoverEntity = currentItem?.prefab;
        }

    }

}