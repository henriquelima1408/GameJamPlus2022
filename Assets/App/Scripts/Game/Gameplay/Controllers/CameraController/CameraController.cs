

using App.Game.Services;
using System;
using UnityEngine;

namespace App.Game.Gameplay
{
    public class CameraController : IDisposable
    {
        readonly Camera camera;
        readonly IUpdaterService updaterService;

        const string tileLayerMask = "Tile";

        public CameraController(Camera camera, IUpdaterService updaterService)
        {
            this.camera = camera;
            this.updaterService = updaterService;

            updaterService.OnUpdate += OnUpdate;
        }

        public void Dispose()
        {
            updaterService.OnUpdate -= OnUpdate;
        }

        void OnUpdate() {

            //Vector2 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            //cellSelector.SelectCell(pos, gameplayManager.CurrentSelectedBuildData);

            //float hAxis = Input.GetAxis(h);
            //float vAxis = Input.GetAxis(v);
            //var inputAxis = (new Vector3(hAxis, vAxis, 0) * pCameraSpd) * Time.deltaTime;

            //transform.position += inputAxis;

        }

        void CameraMovement() {
            //float hAxis = Input.GetAxis(h);
            //float vAxis = Input.GetAxis(v);
            //var inputAxis = (new Vector3(hAxis, vAxis, 0) * pCameraSpd) * Time.deltaTime;

            //transform.position += inputAxis; 
        }
    }
}
