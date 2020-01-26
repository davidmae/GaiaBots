using Assets.GameFramework;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameProject_1.UI
{
    public class UIPanelInfo : MonoBehaviour
    {
        public GameObject header;
        public GameObject body;

        private CursorManager cursorManager;
        private Text headerContent;
        private Text bodyContent;
        private bool containsInfo;

        void Awake()
        {
            cursorManager = GameObject.FindGameObjectWithTag("CursorManager").GetComponent<CursorManager>();
            headerContent = header.GetComponent<Text>();
            bodyContent = body.GetComponent<Text>();
            containsInfo = false;
        }

        void Update()
        {
            SetTitle(cursorManager.hoverEntity ?? null);
        }

        public void SetTitle(GFrameworkEntityBase entity)
        {
            if (entity == null)
            {
                headerContent.text = "";
                bodyContent.text = "";
                containsInfo = false;
                return;
            }

            if (containsInfo)
                return;

            if (entity != null)
            {
                headerContent.text = entity.GetOriginalName();
                entity.GetEntityFields().ForEach(field =>
                {
                    bodyContent.text += $"{field.Key}: {field.Value} \n";
                });
                containsInfo = true;
            }
        }
    }

}