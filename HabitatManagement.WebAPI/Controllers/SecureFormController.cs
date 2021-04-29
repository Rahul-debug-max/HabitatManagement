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
                response = Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [EnableCors("AllowOrigin")]
        [Route("GetFormList")]
        public IEnumerable<FormDesignTemplateBE> GetFormList()
        {
            IEnumerable<FormDesignTemplateBE> listFormDesignTemplate = FormLogic.BlockFetchFormDesignTemplate(1, Int32.MaxValue, out int totalRecords, string.Empty);
            return listFormDesignTemplate;
        }

        [HttpGet]
        [Authorize]
        [EnableCors("AllowOrigin")]
        [Route("GetFormHtmlList")]
        public List<SelectListItem> GetFormHtmlList()
        {
            List<SelectListItem> formHtmlList = new List<SelectListItem>();
            IEnumerable<FormDesignTemplateBE> listFormDesignTemplate = FormLogic.BlockFetchFormDesignTemplate(1, Int32.MaxValue, out int totalRecords, string.Empty);

            if (listFormDesignTemplate != null && listFormDesignTemplate.Count() > 0)
            {
                foreach (var formDesignTemplate in listFormDesignTemplate)
                {
                    List<FormDesignTemplateDetailBE> templateDetails = FormLogic.FetchAllFormDesignTemplateDetail(formDesignTemplate.FormID);
                    List<TemplateFormFieldDataBE> templateFormFieldData = new List<TemplateFormFieldDataBE>();  //FormLogic.FetchAllTemplateFormFieldData(formDesignTemplate.FormID, templateDetails);
                    FormDesignTemplateModelBE model = new FormDesignTemplateModelBE(templateDetails, templateFormFieldData);
                    model.FormID = formDesignTemplate.FormID;
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