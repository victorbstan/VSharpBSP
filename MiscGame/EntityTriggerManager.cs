using System;
using UnityEngine;

namespace VSharpBSP
{
    [Serializable]
    public class EntityTriggerManager : MonoBehaviour
    {
        private void Awake()
        {
            SetLayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"ON TRIGGER ENTER {this}: {other}");
            if (other.CompareTag("Player"))
                SendMessageUpwards("EntityTriggerEnter", other);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"ON TRIGGER EXIT {this}: {other}");
            if (other.CompareTag("Player"))
                SendMessageUpwards("EntityTriggerExit", other);
        }

        private void SetLayer()
        {
            int layerInt = LayerMask.NameToLayer("Interaction");
            gameObject.layer = layerInt;
        }
    }
}