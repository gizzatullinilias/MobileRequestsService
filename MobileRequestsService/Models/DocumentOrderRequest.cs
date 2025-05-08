using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRequestsService.Models
{
    public class DocumentOrderRequest
    {
        public int DocumentTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
