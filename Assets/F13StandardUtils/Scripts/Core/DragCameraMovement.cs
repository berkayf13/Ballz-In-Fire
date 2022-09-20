using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class DragCameraMovement : BaseCameraMovementStrategy
    {
        private Vector3 lastPosition;
        private void FixedUpdate()
        {
            var mousePosition = Input.mousePosition;


            if (Input.GetMouseButtonDown(0))
            {
                lastPosition = mousePosition;
            }
            if (Input.GetMouseButton(0))
            {

                if (lastPosition == Vector3.zero)
                {
                    lastPosition = mousePosition;
                }
                var delta = mousePosition - lastPosition;
                if (delta.magnitude > 0)
                {
                    var translate =  (Vector3.right*delta.x+Vector3.forward*delta.y)* speed * Time.deltaTime;
                    var relativeTranslate=transform.TransformDirection(-translate);
                    var position = transform.position;
                    position += relativeTranslate;
                    position.x = Mathf.Clamp(position.x, minClampX, maxClampX);
                    position.z = Mathf.Clamp(position.z, minClampZ, maxClampZ);
                    transform.position = position;
                }
            }
            lastPosition = mousePosition;

        }
        

    }
}