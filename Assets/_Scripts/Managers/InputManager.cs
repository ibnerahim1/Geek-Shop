using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tools;

namespace Game.Managers
{

    public class InputManager : Singleton<InputManager>
    {
        public Vector3 Drag => drag;
        public bool OnMouse => Input.GetMouseButton(0);

        private Vector3 mouseDownPosition;
        private Vector3 mousePosition;
        private Vector3 drag;

        public delegate void OnMouseButtonDown();
        public static event OnMouseButtonDown OnMouseDown;

        public delegate void OnMouseButtonUp();
        public static event OnMouseButtonUp OnMouseUp;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown?.Invoke();
                //mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
                mouseDownPosition = Helpers.MainCamera.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(0))
            {
                mousePosition = Helpers.MainCamera.ScreenToViewportPoint(Input.mousePosition);
                drag = (mousePosition - mouseDownPosition) * 10;
                if (drag.sqrMagnitude < 0.1f)
                    drag = Vector3.zero;
                //if (Vector3.Distance(mouseDownPosition, mousePosition) > 0.05f)
                //{
                //    mouseDownPosition = Vector3.Lerp(mouseDownPosition, mousePosition, Time.fixedDeltaTime * 2);
                //}
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp?.Invoke();
                drag = Vector3.zero;
            }
        }
    }
}