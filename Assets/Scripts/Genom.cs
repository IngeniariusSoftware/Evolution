using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Genom : IEnumerable,ICloneable
    {
        public List<int> genom { get; set; }

        public IEnumerator GetEnumerator()
        {
            foreach (var gen in genom) yield return gen;
        }

        /// <summary>
        /// Реализация IClonable для глубокого копирования
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new List<int>(genom);
        }
        /// <summary>
        /// Клонирование генома в виде List<int>, чтобы не выполнялось клонирование по ссылке 
        /// </summary>
        /// <returns></returns>
        public List<int> Clone(bool flag)
        {
            return new List<int>(genom); 
        }
    }
}