using Assets.GameFramework;
using Assets.GameFramework.UI;
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

namespace Assets.GameProject_1.UI
{

    public class UIAddButton : MonoBehaviour, IPointerClickHandler
    {
        private void OnMouseDown()
        {
            Debug.Log("MouseDown");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("entra");
        }
    }

}