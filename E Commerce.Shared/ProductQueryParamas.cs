using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }
        public ProductSortingOptions Sort { get; set; }

        private int _PageIndex = 1;
        public int PageIndex 
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = (value <= 0) ? 1 : value; 
            }
        }


        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;

        private int _PageSize = DefaultPageSize;
        public int PageSize
        {
            get 
            {
                return _PageSize;
            }
            set 
            {
                if (value <= 0) _PageSize = DefaultPageSize;
              else if (value > MaxPageSize) _PageSize = MaxPageSize;
                else _PageSize = value;
            }
        }
    }
}
