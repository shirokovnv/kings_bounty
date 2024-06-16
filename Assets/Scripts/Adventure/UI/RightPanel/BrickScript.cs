using Assets.Scripts.Adventure.Logic.Continents.Interactors.Puzzle;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Adventure.UI.RightPanel
{
    public class BrickScript : MonoBehaviour
    {
        public PuzzlePiece PuzzlePiece;
        public int X, Y;

        public void Toggle(bool value)
        {
            gameObject.GetComponent<Image>().enabled = value;
        }
    }
}