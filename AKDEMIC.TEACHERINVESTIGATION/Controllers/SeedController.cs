using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/seed")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SeedController(
            AkdemicContext context,
            RoleManager<ApplicationRole> roleManager
        )
        {
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> SeedRoles()
        {
            //Seed para roles de la Base de datos
            var data = new List<dynamic>();

            var roles = new List<string>
            {
                GeneralConstants.ROLES.SUPERADMIN,
                GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN,

                GeneralConstants.ROLES.RESEARCHERS,
                GeneralConstants.ROLES.TECHNICAL_COMMITTEE,
                GeneralConstants.ROLES.EVALUATOR_COMMITTEE,
                GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_MONITOR,
                GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_COORDINATORMONITOR,
                GeneralConstants.ROLES.INCUBATORCONVOCATION_MONITOR,
                GeneralConstants.ROLES.INCUBATORCONVOCATION_COORDINATORMONITOR,

                GeneralConstants.ROLES.TECHNICAL_COLLABORATOR,
                GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT,
                GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT,
                GeneralConstants.ROLES.PUBLICATION_UNIT,
                GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT,


                GeneralConstants.ROLES.EXTERNAL_EVALUATOR,

                GeneralConstants.ROLES.APPLICANT_TEACHER,
                GeneralConstants.ROLES.CONVOCATION_PRESIDENT,
                GeneralConstants.ROLES.CONVOCATION_MEMBER,
                GeneralConstants.ROLES.EXTERNAL_JURY,
                GeneralConstants.ROLES.TEACHING_SECRETARY
            };

            for (int i = 0; i < roles.Count; i++)
            {
                var rolName = roles[i];
                int priority = 0;

                if (rolName == GeneralConstants.ROLES.SUPERADMIN ||
                    rolName == GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN)
                {
                    priority = 1;
                }

                var roleExist = await _roleManager.RoleExistsAsync(rolName);
                if (!roleExist)
                {
                    var applicationRole = new ApplicationRole
                    {
                        Name = rolName,
                        Priority = priority
                    };

                    var result = await _roleManager.CreateAsync(applicationRole);
                    if (!result.Succeeded)
                    {
                        data.Add(new 
                        {
                            Rol = rolName,
                            Info = "No se ha podido crear el rol"
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            Rol = rolName,
                            Info = "Se ha creado el rol"
                        });

                    }
                }
                else
                {
                    data.Add(new
                    {
                        Rol = rolName,
                        Info = "Ya existe el rol"
                    });
                }
            }


            return Ok(data);
        }
    }
}
