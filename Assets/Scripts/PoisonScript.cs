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

        public int POISON_INT = (int)CellEnum.TypeOfCell.Poison;

        public void ChangeItemCount()
        {
            var poisonPercent = (int)gameObject.GetComponent<Slider>().value;
            Map.PercentObjects[POISON_INT] = (float)(poisonPercent / 100.0);
            Map.RecalculateSingleTypeObject(POISON_INT);
            Map.CleanMap(POISON_INT);
        }
    }
}
