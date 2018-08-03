using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Genome : IEnumerable,ICloneable
    {
        public List<int> genome { get; set; }

        public IEnumerator GetEnumerator()
        {
            foreach (var gene in genome) yield return gene;
        }

        /// <summary>
        /// Реализация IClonable для глубокого копирования
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new List<int>(genome);
        }
        /// <summary>
        /// Клонирование генома в виде List<int>, чтобы не выполнялось клонирование по ссылке 
        /// </summary>
        /// <returns></returns>
        public List<int> Clone(bool flag)
        {
            return new List<int>(genome); 
        }
    }
}