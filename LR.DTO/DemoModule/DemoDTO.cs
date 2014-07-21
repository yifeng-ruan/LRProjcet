using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.DTO.DemoModule
{
    public class DemoDTO
    {
        public int DemoId { get; set; }
        public string DemoName { get; set; }
        public List<SubDemoDTO> SubDemoDTO { get; set; }
    }
}
