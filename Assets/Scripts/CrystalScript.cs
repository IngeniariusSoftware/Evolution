using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CrystalScript:MonoBehaviour
    {
        public int CRYSTAL_INT = (int)Cell.TypeOfCell.Mineral;

        public void ChangeItemCount()
        {
            var percent = (int)gameObject.GetComponent<Slider>().value;
            Map.PercentObjects[CRYSTAL_INT] = (float)(percent / 100.0);
            Map.RecalculateSingleTypeObject(CRYSTAL_INT);
            Map.CleanMap(CRYSTAL_INT);
        }
    }
}
