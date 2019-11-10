using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.GameProject_1.UI
{

    public class UIDropSlot : MonoBehaviour
    {
        // Reference to the item inside slot.
        public UIDragItem currentItem;

        // Tells if slot is filled by other item.
        public bool SlotFilled => currentItem;

    }

}