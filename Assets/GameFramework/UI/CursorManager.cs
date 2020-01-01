using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.GameFramework.UI
{
    public class CursorManager : MonoBehaviour
    {
        public Texture2D defaultCursor;
        public Texture2D hoverItemCursor;
        public Texture2D removeItemCursor;

        public Texture2D selectedCursor;
        public GameObject selectedEntity;
        public Canvas canvas;

        public bool removeEntity;

        private Vector2 hotspot = Vector2.zero;
        private CursorMode cursorMode = CursorMode.Auto;

        [HideInInspector]
        public bool keepLastTexture = false;

        //private static CursorManager _instance;
        //public static CursorManager Instance { get => _instance; }

        public bool RemoveCursor { get => selectedCursor == removeItemCursor; }

        void Awake()
        {
            //if (_instance != null && _instance != this)
            //    Destroy(this.gameObject);
            //else
            //    _instance = this;

            SetCursor(defaultCursor);
        }

        private void Update()
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, 1000f))
            //{
            //    if (hit.collider != null && hit.collider.GetComponent<IDetectable>() != null)
            //        Cursor.SetCursor(hoverItemCursor, hotspot, cursorMode);
            //    else
            //        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
            //}

            //if (Input.GetMouseButton(0))
            //{
            //    if (hit.collider != null && hit.collider.GetComponent<IDetectable>() != null)
            //    {
            //        itemSelected = hit.collider.GetComponent<IDetectable>().GetGameObject();
            //    }
            //}

            //if (Input.GetMouseButton(1))
            //{
            //    Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
            //    keepLastTexture = false;
            //    selectedEntity = null;
            //}

            //if (itemSelected != null)
            //    itemSelected.transform.position = new Vector3(hit.point.x, itemSelected.transform.position.y, hit.point.z);

        }


        public void SpawnGameObject(GameObject entity, ref bool spawned)
        {
            if (entity == null) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000) && hit.collider.gameObject.CompareTag("Ground"))
            {
                var newEntity = Instantiate(entity, new Vector3(hit.point.x, entity.transform.position.y, hit.point.z), Quaternion.identity);
                newEntity.SetActive(true);
                spawned = true;
            }
        }

        // Called from "Event trigger" component in inspector
        public void KeepLastTexture() => keepLastTexture = !keepLastTexture;

        public void SetCursor(Texture2D texture)
        {
            if (keepLastTexture || selectedEntity != null)
                return;

            Cursor.SetCursor(texture, hotspot, cursorMode);
            selectedCursor = texture;
        }
    }

}