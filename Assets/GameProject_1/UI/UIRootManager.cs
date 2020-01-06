using Assets.GameFramework;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.UI;
using Assets.GameFramework.Item.Extensions;
using Assets.GameProject_1.Item.Scripts;
using Assets.GameProject_1.UI;
using Assets.GameProject_1.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Status.Core;

namespace Assets.GameProject_1.UI
{

    public class UIRootManager : MonoBehaviour /*IPointerClickHandler*/
    {
        public GameObject entityToSpawn;
        public bool showSidebar;
        public List<UIDropSlot> itemsSlots;
        public UIDropSlot selectedSlot;
        public UIDragItem selectedDragItem;

        private Vector3 screenPoint;
        private Vector3 offset;

        private CursorManager cursorManager;
        private GraphicRaycaster graphicRaycaster;
        private Animator animator;

        private bool spawn = false;

        private void Awake()
        {
            graphicRaycaster = GetComponent<GraphicRaycaster>();

            cursorManager = GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>();

            animator = GetComponent<Animator>();
            animator.SetBool("ShowSideBar", showSidebar);

            var sidebar = GameObject.FindGameObjectWithTag("SideBar");
            itemsSlots = sidebar.GetComponentsInChildren<UIDropSlot>().ToList();
        }

        private void Update()
        {
            if (cursorManager.removeEntity)
            {
                UpdateItems(cursorManager.selectedEntity);
                cursorManager.removeEntity = false;
                cursorManager.selectedEntity = null;
            }

            if (Input.GetMouseButton(1))
            {
                cursorManager.SetCursor(cursorManager.defaultCursor);
                cursorManager.keepLastTexture = false;
                cursorManager.selectedEntity = null;
            }

            if (!spawn)
                return;

            if (Input.GetMouseButtonDown(1)) spawn = false;

            if (Input.GetMouseButtonDown(0) == true)
            {
                bool spawned = false;
                cursorManager.SpawnGameObject(entityToSpawn, ref spawned);

                if (spawned)
                {
                    if (Convert.ToInt32(selectedDragItem.counter.text) <= 1)
                    {
                        Destroy(selectedDragItem.gameObject);
                        spawn = false;
                        entityToSpawn = null;
                        cursorManager.selectedEntity = null;
                        cursorManager.keepLastTexture = false;
                        cursorManager.SetCursor(cursorManager.defaultCursor);
                    }
                    else
                        selectedDragItem.counter.text = (Convert.ToInt32(selectedDragItem.counter.text) - 1).ToString();
                }
            }


        }

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    var results = new List<RaycastResult>();
        //    graphicRaycaster.Raycast(eventData, results);

        //    foreach (var hit in results)
        //    {
        //        var slot = hit.gameObject.GetComponent<UIDropSlot>();
        //        if (slot)
        //        {
        //            //itemToSpawn = Instantiate(itemToSpawn, new Vector3(hit.worldPosition.x, slot.currentItem.prefab.transform.position.y, hit.worldPosition.z), Quaternion.identity);
        //            //itemToSpawn.GetComponent<Apple>().BindToCursor(true);
        //            //Cursor.SetCursor(null, new Vector2(hit.worldPosition.x + 3f, hit.worldPosition.z + 3f), CursorMode.Auto);

        //            cursorManager.SetCursor(slot.currentItem.prefabCursor);
        //            spawn = true;
        //        }
        //    }
        //}

        /// Actualiza el sidebar con los objetos que recogemos/destruimos en el mapa
        public void UpdateItems(GameObject entity)
        {
            var slotFilledWithSameItem = itemsSlots.FirstOrDefault(x => x.currentItem != null &&
                                                                        x.currentItem.GetCurrentItem().Equals(entity.GetComponent<IItem>()));

            if (slotFilledWithSameItem != null)
            {
                slotFilledWithSameItem.currentItem.counter.text = (Convert.ToInt32(slotFilledWithSameItem.currentItem.counter.text) + 1).ToString();
                Destroy(cursorManager.selectedEntity);
                return;
            }

            var slotEmpty = itemsSlots.FirstOrDefault(x => !x.SlotFilled);
            if (slotEmpty == null)
                return;

            var entityBase = entity.GetComponent<GFrameworkEntityBase>();

            // Instancia un DragItem en la UI dentro del slot vacio
            var dragItem = Instantiate(Resources.Load("UI/DragItem") as GameObject, slotEmpty.transform);
            var dragItemScript = dragItem.GetComponent<UIDragItem>();
            var dragItemImage = dragItem.GetComponent<RawImage>();

            // Establece los valores del script del item de dentro del slot, y clona la entidad ocultandola (active = false)
            dragItemScript.currentSlot = slotEmpty;
            dragItemScript.prefab = Instantiate(entity);
            dragItemScript.prefab.SetActive(false);
            dragItemScript.prefabCursor = entityBase.cursorTexture;
            dragItemScript.counter.text = (Convert.ToInt32(dragItemScript.counter.text) +1).ToString();

            // Indica la textura que tendrá el item en la UI del slot
            dragItemImage.texture = entityBase.entityTexture;

            slotEmpty.currentItem = dragItemScript;

            // Destruye el item que acabamos de recoger
            Destroy(cursorManager.selectedEntity);

        }

        public void ChangeCursorToCurrentPrefab(UIDropSlot slot)
        {
            if (slot.currentItem == null)
                return;

            entityToSpawn = slot.currentItem.prefab;
            selectedSlot = slot;
            selectedDragItem = slot.currentItem;
            cursorManager.SetCursor(slot.currentItem.prefabCursor);
            spawn = true;

            cursorManager.selectedEntity = entityToSpawn;
        }

        public void ShowSidebar()
        {
            showSidebar = !showSidebar;
            animator.SetBool("ShowSideBar", showSidebar);
        }


        void OnMouseDown()
        {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }

        void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            transform.position = curPosition;
        }

    }


}