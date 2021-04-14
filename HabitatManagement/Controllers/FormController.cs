using HabitatManagement.BusinessEntities;
using HabitatManagement.BusinessLogic;
using HabitatManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HabitatManagement.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = FormLogic.BlockFetchPermitFormScreenDesignTemplate(1, Int32.MaxValue, out int totalRecords, "");
            List<SelectListItem> forms = new List<SelectListItem>();
            forms = listPermitFormScreenDesignTemplate.Select(m => new SelectListItem()
            {
                Text = m.Design,
                Value = m.FormID.ToString()
            }).ToList();

            forms.Insert(0, new SelectListItem { Text = "--Select Form--", Value = "-1" });
            ViewData["FormList"] = forms;
            return View();
        }
    }
}