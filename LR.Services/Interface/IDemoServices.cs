using LR.DTO;
using LR.DTO.DemoModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Services.Interface
{
    public interface IDemoServices
    {
        /// <summary>
        /// Get all Profiles
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        List<DemoDTO> FindDemos(int pageIndex, int pageCount);

        /// <summary>
        /// Find Profile by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DemoDTO FindDemoById(int id);

        /// <summary>
        /// To Delete a Profile
        /// </summary>
        /// <param name="profileId"></param>
        void DeleteDemo(int profileId);

        /// <summary>
        /// Add new profile
        /// </summary>
        /// <param name="profileDTO"></param>
        /// <returns></returns>
        void SaveDemo(DemoDTO profileDTO);

        /// <summary>
        /// Update existing profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileDTO"></param>
        void UpdateDemo(int id, DemoDTO DemoDTO);

        /// <summary>
        /// Get all initialization data for Contact page
        /// </summary>
        /// <returns></returns>
        SubDemoDTO InitializePageData();
    }
}
