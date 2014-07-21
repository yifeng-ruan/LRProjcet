
using LR.DTO;
using LR.DTO.DemoModule;
using LR.Core.DemoModule.DemoAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Services.Conversion
{
    public static class Mapping
    {
        public static DemoDTO DemoToDemoDTO(Demo demo, List<Demo> demos)
        {
            DemoDTO objDemoDTO = new DemoDTO();
            objDemoDTO.DemoId = demo.DemoId.Value;

            foreach (var subDemo in demos)
            {
                SubDemoDTO objSubDemoDTO = DemoToSubDemoDTO(subDemo);
                objDemoDTO.SubDemoDTO.Add(objSubDemoDTO);
            }
            return objDemoDTO;
        }

        public static SubDemoDTO DemoToSubDemoDTO(Demo demo)
        {
            SubDemoDTO objSubDemoDTO = new SubDemoDTO();
            if (demo != null)
            {
                objSubDemoDTO.SubDemoName = demo.DemoName;
            }
            return objSubDemoDTO;
        }
    }
}
