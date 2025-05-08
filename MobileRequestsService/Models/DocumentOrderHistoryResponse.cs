using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRequestsService.Models
{
    public class DocumentOrderHistoryResponse
    {
        public int TotalCount { get; set; }
        public List<DocumentOrderItem> Data { get; set; }
        public bool IsHaveNextPage { get; set; }
        public bool IsHavePrevPage { get; set; }
    }

}
