using Assets.GameFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameFramework
{
    public class GFrameworkEntityBase : MonoBehaviour, IGFrameworkEntityBase
    {
        public Texture2D entityTexture;
        public Texture2D cursorTexture;

        public CursorManager cursorManager;
        protected bool cursorBinding = false;
        public Collider groundCollider;

        //protected MapGenerator

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

        public GameObject GetGameObject() => gameObject;
        public virtual string GetOriginalName()
        {
            var index = gameObject.name.IndexOf(" (");
            var res = gameObject.name.Substring(0, index < 0 ? gameObject.name.Length : index);
            return res;
        }

        public virtual List<KeyValuePair<string, object>> GetEntityFields()
        {
            return new List<KeyValuePair<string, object>>();
        }

        public virtual bool Equals(IGFrameworkEntityBase other)
        {
            return false;
        }

        public virtual GFrameworkEntityBase DeepCopy()
        {
            var entity = Instantiate(this);
            entity.entityTexture = entityTexture;
            entity.cursorTexture = cursorTexture;
            entity.cursorManager = cursorManager;
            entity.cursorBinding = cursorBinding;
            entity.groundCollider = groundCollider;
            entity.OnUpdateEntity = OnUpdateEntity;
            return entity;
        }

        public virtual GFrameworkEntityBase DeepCopy(GFrameworkEntityBase entity)
        {
            entity.entityTexture = entityTexture;
            entity.cursorTexture = cursorTexture;
            entity.cursorManager = cursorManager;
            entity.cursorBinding = cursorBinding;
            entity.groundCollider = groundCollider;
            entity.OnUpdateEntity = OnUpdateEntity;
            return entity;
        }

        public void SetCollider(Collider newCollider)
        {
            groundCollider = newCollider;
        }

        // Mouse events/functions

        private void OnMouseDown()
        {
            if (cursorManager.RemoveCursor)
            {
                cursorManager.removeEntity = true;
                cursorManager.selectedEntity = this;
            }
            else
                cursorManager.selectedEntity = this;
        }


        private void OnMouseEnter()
        {
            cursorManager.SetCursor(cursorManager.hoverItemCursor);
            cursorManager.hoverEntity = this;
        }

        private void OnMouseExit()
        {
            cursorManager.SetCursor(cursorManager.defaultCursor);
            cursorManager.hoverEntity = null;
        }

        private void OnMouseDrag()
        {
            DraggingItem();
        }

        protected virtual void DraggingItem()
        {
            if (groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 200.0F) &&
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
