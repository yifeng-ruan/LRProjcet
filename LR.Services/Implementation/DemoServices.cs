
using LR.DTO;
using LR.Services.Interface;
using LR.Services.Resources;
using LR.Repository.DemoModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LR.Core.DemoModule.DemoAggregate;
using LR.Utils;
using LR.DTO.DemoModule;


namespace LR.Services.Implementation
{
    public class DemoServices:IDemoServices
    {
        #region 全局变量

        DemoRepository _demoRepository;

        #endregion 全局变量

        #region 构造函数

        public DemoServices(DemoRepository demoRepository)
        {
            if (demoRepository == null)
                throw new ArgumentNullException("demoRepository");

            _demoRepository = demoRepository;
        }

        #endregion 构造函数

        #region 接口实现
        public DemoDTO FindDemos(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException(Messages.warning_CannotAddDemoWithNullInformatio);

            int total=0;
            var demos = _demoRepository.GetAll(new Demo(), new PageInfo() { PageIndex = pageIndex, PageSize = pageCount }, ref total).ToList();

            if (demos != null
                &&
                demos.Any())
            {
                return Conversion.Mapping.DemoToDemoDTO(demos.First(), demos);
            }
            else // no data
                return new DemoDTO();
        }

        public DTO.DemoModule.DemoDTO FindDemoById(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteDemo(int profileId)
        {
            throw new NotImplementedException();
        }

        public void SaveDemo(DTO.DemoModule.DemoDTO profileDTO)
        {
            throw new NotImplementedException();
        }

        public void UpdateDemo(int id, DTO.DemoModule.DemoDTO DemoDTO)
        {
            throw new NotImplementedException();
        }

        public DTO.DemoModule.SubDemoDTO InitializePageData()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 私有方法

        #endregion


        List<DemoDTO> IDemoServices.FindDemos(int pageIndex, int pageCount)
        {
            throw new NotImplementedException();
        }
    }
}
