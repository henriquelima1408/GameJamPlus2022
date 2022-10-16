using App.Game.Data;
using UnityEngine;

namespace App.Game.Gameplay
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        Camera pCamera;

        public Camera PCamera { get => pCamera; }

        float pCameraSpd;
        CellSelector cellSelector;
        GameplayManager gameplayManager;


        const string h = "Horizontal";
        const string v = "Vertical";

        const string tileLayerMask = "Tile";


        public void Init(GameplayManager gameplayManager, CellSelector cellSelector, float pCameraSpd)
        {
            this.pCameraSpd = pCameraSpd;
            this.gameplayManager = gameplayManager;
            this.cellSelector = cellSelector;
        }

        private void Update()
        {
            if (gameplayManager.CurrentSelectedBuildData == null) return;

            Vector2 pos = pCamera.ScreenToWorldPoint(Input.mousePosition);
            cellSelector.SelectCell(pos, gameplayManager.CurrentSelectedBuildData);

            float hAxis = Input.GetAxis(h);
            float vAxis = Input.GetAxis(v);
            var inputAxis = (new Vector3(hAxis, vAxis, 0) * pCameraSpd) * Time.deltaTime;

            transform.position += inputAxis;
        }
    }
}
