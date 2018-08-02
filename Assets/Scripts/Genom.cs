using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Genom : IEnumerable
    {
        public List<int> genom { get; set; }

        public IEnumerator GetEnumerator()
        {
            foreach (var gen in genom) yield return gen;
        }
    }
}