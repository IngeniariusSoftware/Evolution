using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

namespace Assets.Scripts
{
    public class PoisonScript:MonoBehaviour
    {
        public void ChangeItemCount()
        {
            var berryPercent = (int)gameObject.GetComponent<Slider>().value;
            Map.CountTypeObjects[(int)Cell.TypeOfCell.Poison] = (int)(Map.AllCellCount * berryPercent / 100.0f);
            Map.CleanMap((int)Cell.TypeOfCell.Poison);
        }
    }
}
