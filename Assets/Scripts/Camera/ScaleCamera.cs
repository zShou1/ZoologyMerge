using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class ScaleCamera : MonoBehaviour
 {
     private float defaultCameraFieldOfView;
     
     private Vector2 defaultResolution= new Vector2(1920, 1080);

     private Vector2 screenResolution;
     private void Awake()
     {
         Application.targetFrameRate = 60;
         defaultCameraFieldOfView = 60;
     }

     private void Start()
     {
         screenResolution= new Vector2(Screen.height, Screen.width);
         if (screenResolution.x != defaultResolution.x || screenResolution.y != defaultResolution.y)
         {
             FitCamera();
         }
     }
     
     private void FitCamera()
     {
         float defaultRatio = defaultResolution.x / defaultResolution.y;
         float screenRatio = screenResolution.x / screenResolution.y;
         float ratio = screenRatio / defaultRatio;
         Camera.main.fieldOfView *= ratio;
     }
 }
