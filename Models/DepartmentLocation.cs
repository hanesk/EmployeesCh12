using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.ComponentModel;

namespace EmployeesCh12.Models
{
    public class DepartmentLocation
    {
        [DisplayName("Department ID")]
        public int DepartmentID { get; set; }

        [DisplayName("Location ID")]
        public int LocationID { get; set; }

        public Department? Department { get; set; }
        public Location? Location { get; set; }
    }
}
