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
using TMPro;

namespace Assets.GameProject_1.UI
{

    public class UIRootManager : MonoBehaviour /*IPointerClickHandler*/
    {
        public GFrameworkEntityBase entityToSpawn;
        public bool showSidebar;
        public List<UIDropSlot> itemsSlots;
        public UIDropSlot selectedSlot;
        public UIDragItem selectedDragItem;

        public TextMeshProUGUI huntCounter;


        private Vector3 screenPoint;
        private Vector3 offset;

        private CursorManager cursorManager;
        private GraphicRaycaster graphicRaycaster;
        private Animator animator;

        private bool spawn = false;

        private Action onRestoreItemAfterSpawn;


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
                    onRestoreItemAfterSpawn();
                }
            }

        }


        // -----------------
        public void RestoreValuesDragItemAfterSpawn()
        {
            if (Convert.ToInt32(selectedDragItem.counter.text) <= 1)
            {
                Destroy(selectedDragItem.gameObject);
                spawn = false;
                entityToSpawn = null;
                cursorManager.selectedEntity = null;
                cursorManager.keepLastTexture = false;
                cursorManager.SetCursor(cursorManager.defaultCursor);
                selectedSlot.currentItem = null;
                selectedSlot = null;
                selectedDragItem = null;
            }
            else
                selectedDragItem.counter.text = (Convert.ToInt32(selectedDragItem.counter.text) - 1).ToString();
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

            onRestoreItemAfterSpawn = RestoreValuesDragItemAfterSpawn;
        }
        // -----------------


        // -----------------
        public void RestoreValuesHuntItemAfterSpawn()
        {
            if (Convert.ToInt32(huntCounter.text) <= 1)
            {
                spawn = false;
                entityToSpawn = null;
                cursorManager.selectedEntity = null;
                cursorManager.keepLastTexture = false;
                cursorManager.SetCursor(cursorManager.defaultCursor);
            }
         
            var count = Convert.ToInt32(huntCounter.text) - 1;
            huntCounter.text = count.ToString();
        }

        // Called from button event
        public void ChangeCursorToHuntMode()
        {
            if (Convert.ToInt32(huntCounter.text) == 0) return;

            entityToSpawn = Resources.Load<GFrameworkEntityBase>("Celltrap");
            cursorManager.SetCursor(entityToSpawn.cursorTexture);
            spawn = true;
            cursorManager.selectedEntity = entityToSpawn;
            cursorManager.keepLastTexture = true;
            cursorManager.removeEntity = false;

            onRestoreItemAfterSpawn = RestoreValuesHuntItemAfterSpawn;

        }
        // -----------------



        // Called from button event
        public void SetRemoveEntity(bool val)
        {
            if (cursorManager.selectedEntity == null)
                return;

            cursorManager.removeEntity = val;
        }

        /// Actualiza el sidebar con los objetos que recogemos/destruimos en el mapa
        public void UpdateItems(GFrameworkEntityBase entity)
        {
            var slotFilledWithSameItem = itemsSlots.FirstOrDefault(x => x.currentItem != null &&
                                                                        x.currentItem.GetCurrentEntity().Equals(entity));

            if (slotFilledWithSameItem != null)
            {
                slotFilledWithSameItem.currentItem.counter.text = (Convert.ToInt32(slotFilledWithSameItem.currentItem.counter.text) + 1).ToString();
                Destroy(cursorManager.selectedEntity.gameObject);
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
            dragItemScript.prefab = entityBase.DeepCopy();
            dragItemScript.prefab.gameObject.name = dragItemScript.prefab.gameObject.name.Replace("(Clone)", "");
            dragItemScript.prefab.gameObject.SetActive(false);
            dragItemScript.prefabCursor = entityBase.cursorTexture;
            dragItemScript.counter.text = (Convert.ToInt32(dragItemScript.counter.text) + 1).ToString();
            dragItemScript.name = dragItemScript.name.Replace("(Clone)", "");

            // Indica la textura que tendrá el item en la UI del slot
            dragItemImage.texture = entityBase.entityTexture;

            slotEmpty.currentItem = dragItemScript;

            // Destruye el item que acabamos de recoger
            Destroy(cursorManager.selectedEntity.gameObject);
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