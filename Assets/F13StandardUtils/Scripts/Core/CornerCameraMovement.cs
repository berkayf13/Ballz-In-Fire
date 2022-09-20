using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class CornerCameraMovement : BaseCameraMovementStrategy
    {
        [SerializeField] private float xRatio =0.1f;
        [SerializeField] private float yRatio =0.1f;
        

        private void FixedUpdate()
        {
            var mousePosition = Input.mousePosition;
            if (mousePosition.x > 0 &&
                mousePosition.x < Screen.width &&
                mousePosition.y > 0 &&
                mousePosition.y < Screen.height)
            {
                
                var currentRatioX = mousePosition.x / Screen.width;
                var currentRatioY = mousePosition.y / Screen.height;

                var translate = Vector3.zero;
                if (currentRatioX < xRatio)
                {
                    translate +=  Vector3.left * speed * Time.deltaTime;
                }
                else if (1-currentRatioX < xRatio)
                {
                    translate +=  Vector3.right * speed * Time.deltaTime;

                }
            
                if (currentRatioY < yRatio)
                {
                    translate +=  Vector3.back * speed * Time.deltaTime;
                }
                else if (1-currentRatioY < yRatio)
                {
                    translate +=  Vector3.forward * speed * Time.deltaTime;

                }

                if (translate != Vector3.zero)
                {
                    var relativeTranslate=transform.TransformDirection(translate);
                    var position = transform.position+relativeTranslate;
                    position.x = Mathf.Clamp(position.x, minClampX, maxClampX);
                    position.z = Mathf.Clamp(position.z, minClampZ, maxClampZ);
                    transform.position = position;
                }
                
            }

            
        }
        

    }
}