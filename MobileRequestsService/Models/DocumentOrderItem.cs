using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRequestsService.Models
{
    public class DocumentOrderItem
    {
        public int DocumentOrderId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Username { get; set; }
        public string DocumentType { get; set; }
        public string Department { get; set; }
        public string OrderStatus { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
