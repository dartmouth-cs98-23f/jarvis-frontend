#region Includes
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#endregion

namespace TS.PageSlider
{
    public class PageDot : MonoBehaviour
    {
        #region Variables

        [Header("Events")]
        public UnityEvent<bool> OnActiveStateChanged;
        public UnityEvent<int> OnPressed;

        public bool IsActive { get; private set; }
        public int Index { get; set; }

        #endregion

        private void Start()
        {
            ChangeActiveState(Index == 1);
        }

        public virtual void ChangeActiveState(bool active)
        {
            IsActive = active;
            OnActiveStateChanged?.Invoke(active);
            Debug.Log("ChangeActiveState: " + active);
        }
        public void Press()
        {
            OnPressed?.Invoke(Index);
        }

        public void SetColor(Color color)
        {
            Debug.Log("Setting color: " + color);
            GetComponent<Image>().color = color;
        }
    }
}