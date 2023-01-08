using App.Game.Services;
using UnityEngine;

namespace App.Game
{
    public class ViewRoot : MonoBehaviour
    {
        [SerializeField] Transform screenRoot;
        [SerializeField] Transform popupRoot;
        [SerializeField] Canvas canvas;

        public Transform ScreenRoot { get => screenRoot; }
        public Transform PopupRoot { get => popupRoot; }
        public Canvas Canvas { get => canvas; }
    }
}
