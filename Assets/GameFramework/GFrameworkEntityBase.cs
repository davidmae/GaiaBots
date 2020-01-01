using Assets.GameFramework.UI;
using System;
using UnityEngine;

namespace Assets.GameFramework
{
    public class GFrameworkEntityBase : MonoBehaviour, IGFrameworkEntityBase
    {
        public Texture2D entityTexture;
        public Texture2D cursorTexture;

        protected CursorManager cursorManager;
        protected bool cursorBinding = false;
        protected Collider groundCollider;


        public event Action OnUpdateEntity;

        public void OnUpdate()
        {
            if (OnUpdateEntity != null)
                OnUpdateEntity();
        }

        public void InvokeUpdateEntityOverTime(float time = 0f, float repeatRate = 1f)
        {
            if (!IsInvoking("OnUpdate"))
                InvokeRepeating("OnUpdate", time, repeatRate);
        }

        public void CancelUpdateEntityOverTime()
        {
            CancelInvoke("OnUpdate");
            //if (OnUpdateEntity == null && OnUpdateEntity.Target == null)
            //    OnUpdateEntity = null;
        }

        public void OnUpdateEntityToNull()
        {
            OnUpdateEntity = null;
            CancelInvoke("OnUpdate");
        }

        public void DestroyEntity()
        {
            Destroy(gameObject);
        }

        public Action GetOnUpdateVal()
        {
            return OnUpdateEntity;
        }


        // Mouse events/functions

        private void OnMouseDown()
        {
            if (cursorManager.RemoveCursor)
            {
                cursorManager.removeEntity = true;
                cursorManager.selectedEntity = this.gameObject;
            }
        }

        private void OnMouseEnter()
        {
            cursorManager.SetCursor(cursorManager.hoverItemCursor);
        }

        private void OnMouseExit()
        {
            cursorManager.SetCursor(cursorManager.defaultCursor);
        }

        private void OnMouseDrag()
        {
            DraggingItem();
        }

        protected void DraggingItem()
        {
            RaycastHit hit;
            if (groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0F) &&
                hit.collider.gameObject.CompareTag("Ground"))
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }

        public void BindToCursor(bool bind)
        {
            cursorBinding = bind;
        }

        private void Update()
        {
            if (cursorBinding)
                DraggingItem();
        }

    }
}
