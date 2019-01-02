using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CrystalScript : MonoBehaviour
    {
        public void ChangeItemCount()
        {
            var mineralPercent = (int)gameObject.GetComponent<Slider>().value;
            Map.CountTypeObjects[(int)Cell.TypeOfCell.Mineral] = (int)(Map.AllCellCount * mineralPercent / 100.0f);
            Map.CleanMap((int)Cell.TypeOfCell.Mineral);
        }
    }
}
