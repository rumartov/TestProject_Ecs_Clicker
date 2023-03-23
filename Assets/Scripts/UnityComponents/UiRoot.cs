using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace UnityComponents
{
    public class UiRoot : MonoBehaviour
    {
        public Transform businessCardContainer;
        public GameObject hud;
        public EcsUguiEmitter uiEmitter;

        private void Awake()
        {
            uiEmitter = hud.GetComponent<EcsUguiEmitter>();
        }
    }
}