using System;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class TestingEventSubscriber : MonoBehaviour
    {
        void Start()
        {
            BoyController boyController = GetComponent<BoyController>();
            boyController.OnActionEvent += BoyControllerOnActionEvent;
        }

        private void BoyControllerOnActionEvent(bool arg1, int arg2)
        {
            Debug.Log(arg1 + " " + arg2);
        }
    }
}