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
        GameplayDatasheet gameplayDatasheet;

        const string h = "Horizontal";
        const string v = "Vertical";

        const string tileLayerMask = "Tile";


        public void Init(CellSelector cellSelector, float pCameraSpd, GameplayDatasheet gameplayDatasheet)
        {
            this.pCameraSpd = pCameraSpd;
            this.cellSelector = cellSelector;
            this.gameplayDatasheet = gameplayDatasheet;
        }

        private void Update()
        {
            if (gameplayDatasheet == null) return;

            Vector2 pos = pCamera.ScreenToWorldPoint(Input.mousePosition);
            cellSelector.SelectCell(pos, gameplayDatasheet.BuildDatas[0]);

            float hAxis = Input.GetAxis(h);
            float vAxis = Input.GetAxis(v);
            var inputAxis = (new Vector3(hAxis, vAxis, 0) * pCameraSpd) * Time.deltaTime;

            transform.position += inputAxis;
        }
    }
}
