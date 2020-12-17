using System.Collections;
using System.Collections.Generic;

namespace mtgroup.locacao.Internal
{
    public abstract class XUnitTheoryClassData<TData> : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IEnumerator<object[]> GetEnumerator()
        {
            return ConvertToEnumerableOfArrayObjects()
                .GetEnumerator();
        }

        private IEnumerable<object[]> ConvertToEnumerableOfArrayObjects()
        {
            var data = GetEnumerableData();

            foreach (var item in data)
            {
                if (item is object[] objArray)
                    yield return objArray;

                yield return new object[] { item };
            }
        }

        protected abstract IEnumerable<TData> GetEnumerableData();
    }
}