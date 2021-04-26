using HabitatManagement.BusinessEntities;
using HabitatManagement.WebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class SecureFormController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [DisableRequestSizeLimit]
        [EnableCors("AllowOrigin")]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DBConfiguration.JWTKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            DateTime expireTokenTime = DateTime.Now.AddMinutes(DBConfiguration.JWTExpireTokenTimeInMinutes);
            var token = new JwtSecurityToken(
                issuer: DBConfiguration.JWTIssuer,
                audience: DBConfiguration.JWTIssuer,
                claims: claims,
                expires: expireTokenTime,
                signingCredentials: credentials);
            response = Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = expireTokenTime.ToString("dd/MM/yyyy hh:mm") });
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [EnableCors("AllowOrigin")]
        [Route("GetFormList")]
        public IEnumerable<PermitFormScreenDesignTemplateBE> GetFormList()
        {
            IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = FormLogic.BlockFetchPermitFormScreenDesignTemplate(1, Int32.MaxValue, out int totalRecords, "");       
            return listPermitFormScreenDesignTemplate;
        }

        [HttpGet]
        [Authorize]
        [EnableCors("AllowOrigin")]
        [Route("GetFormHtmlList")]
        public List<SelectListItem> GetFormHtmlList()
        {
            List<SelectListItem> formHtmlList = new List<SelectListItem>();
            IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = FormLogic.BlockFetchPermitFormScreenDesignTemplate(1, Int32.MaxValue, out int totalRecords, "");

            if (listPermitFormScreenDesignTemplate != null && listPermitFormScreenDesignTemplate.Count() > 0)
            {
                foreach (var permitFormScreenDesignTemplate in listPermitFormScreenDesignTemplate)
                {
                    List<PermitFormScreenDesignTemplateDetailBE> templateDetails = FormLogic.FetchAllPermitFormScreenDesignTemplateDetail(permitFormScreenDesignTemplate.FormID);
                    List<TemplateFormFieldDataBE> templateFormFieldData = new List<TemplateFormFieldDataBE>();  //FormLogic.FetchAllTemplateFormFieldData(permitFormScreenDesignTemplate.FormID, templateDetails);
                    FormDesignTemplateModelBE model = new FormDesignTemplateModelBE(templateDetails, templateFormFieldData);
                    model.FormID = permitFormScreenDesignTemplate.FormID;
                    model.RenderForDragnDrop = false;

                    formHtmlList.Add(new SelectListItem
                    {
                        Text = model.FormSectionFields(),
                        Value = model.FormID.ToString()
                    });
                }
            }

            return formHtmlList;
        }

        #region Private Methods

        private LoginModel AuthenticateUser(LoginModel login)
        {
            //Validate the User Credentials     
            if (login != null && login.UserName == "rahul" && login.Password == "rahul")
            {
                return new LoginModel { UserName = "Rahul Verma", EmailAddress = "rahul.verma@rsk-bsl.com" };
            }
            return null;
        }

        #endregion
    }
}