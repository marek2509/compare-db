using Porównator_Baz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Middleware
{
    public class ParcelEqualityComparer : IEqualityComparer<ParcelsDto>
    {
        readonly bool _ignorKW;
        readonly bool _ignorArea;
        public ParcelEqualityComparer(bool ignorKW, bool ignoreArea)
        {
            _ignorArea = ignoreArea;
            _ignorKW = ignorKW;
        }


        public bool Equals(ParcelsDto x, ParcelsDto y)
        {
            return x.Equals(y, _ignorKW, _ignorArea);
        }

        public int GetHashCode(ParcelsDto obj)
        {
            throw new NotImplementedException();
        }
    }
}
