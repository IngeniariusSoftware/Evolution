using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;


namespace Assets.Scripts
{
    public class FoodScript : MonoBehaviour
    {
        public void ChangeItemCount()
        {
            var berryPercent = (int)gameObject.GetComponent<Slider>().value;
            Map.CountTypeObjects[(int)Cell.TypeOfCell.Berry] = (int)(Map.AllCellCount * berryPercent / 100.0f);
            Map.CleanMap((int)Cell.TypeOfCell.Berry);
        }
    }
}