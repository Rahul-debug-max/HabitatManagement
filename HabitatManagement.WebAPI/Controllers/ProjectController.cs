using HabitatManagement.Business;
using HabitatManagement.WebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitatManagement.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        [HttpGet]        
        [Route("GetClient")]
        public IEnumerable<SelectListItem> GetClient()
        {
            IEnumerable<ClientBE> list = FormLogic.BlockFetchClient(1, Int32.MaxValue, out int totalRecords);
            List<SelectListItem> client = new List<SelectListItem>();
            client = list.Select(m => new SelectListItem()
            {
                Text = m.Name,
                Value = m.ID.ToString()
            }).ToList();
            client.Insert(0, new SelectListItem { Text = "--Select Client--", Value = "-1" });
            return client;
        }

        [HttpGet]      
        [Route("GetProject")]
        public IEnumerable<SelectListItem> GetProject()
        {
            IEnumerable<ProjectBE> list = FormLogic.BlockFetchProject(1, Int32.MaxValue, out int totalRecords);
            List<SelectListItem> project = new List<SelectListItem>();
            project = list.Select(m => new SelectListItem()
            {
                Text = string.IsNullOrWhiteSpace(m.Project) ? m.ProjectName : m.Project + "-" + m.ProjectName,
                Value = m.ID.ToString()
            }).ToList();
            project.Insert(0, new SelectListItem { Text = "--Select Project--", Value = "-1" });
            return project;
        }
    }
}
