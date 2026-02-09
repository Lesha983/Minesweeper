namespace MineSweeper.UI
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UINumericSprites : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> digits;
        [Space] 
        [SerializeField] 
        private Image hundreds;
        [SerializeField] 
        private Image tens;
        [SerializeField] 
        private Image ones;

        public void SetValue(int value)
        {
            value = Mathf.Clamp(value, 0, 999);

            var h = value / 100;
            var t = (value / 10) % 10;
            var o = value % 10;
            
            hundreds.sprite = digits[h];
            tens.sprite     = digits[t];
            ones.sprite     = digits[o];
            
        }

        public void ResetValue()
        {
            SetValue(0);
        }
        
    }
}