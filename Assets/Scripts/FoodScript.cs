using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;


namespace Assets.Scripts
{
    public class FoodScript : MonoBehaviour
    {
        public int BERRY_INT = (int)CellEnum.TypeOfCell.Berry;

        public void ChangeItemCount()
        {
            var berryPercent = (int)gameObject.GetComponent<Slider>().value;
            Map.PercentObjects[BERRY_INT] = (float)(berryPercent / 100.0);
            Map.RecalculateSingleTypeObject(BERRY_INT);
            Map.CleanMap(BERRY_INT);
        }
    }
}