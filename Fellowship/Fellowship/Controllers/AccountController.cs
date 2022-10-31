using Fellowship.DataAccess.UnitOfWork.Interface;
using Fellowship.DTOs;
using Fellowship.Helper;
using Fellowship.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fellowship.Controllers
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IOptions<JwtSettings> jwtSettings;
        private readonly IConfiguration configuration;
        //private readonly IEloEmailService eloEmailServices;
        private readonly IWebHostEnvironment env;
        private readonly IUnitOfWork<Fellow> unitOfWorkFellow;


        /// <summary>
        /// Dependency Injections
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="jwtSettings"></param>
        /// <param name="configuration"></param>
        /// <param name="eloEmailServices"></param>
        /// <param name="env"></param>
        /// <param name="unitOfWorkFellow"></param>
        public AccountController
            (UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            IConfiguration configuration,
            //IEloEmailService eloEmailServices,
            IWebHostEnvironment env,
            IUnitOfWork<Fellow> unitOfWorkFellow
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtSettings = jwtSettings;
            this.configuration = configuration;
            //this.eloEmailServices = eloEmailServices;
            this.env = env;
            this.unitOfWorkFellow = unitOfWorkFellow;
        }



        /// <summary>
        /// Registers app users
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> SignUp(UserDto model)
        {

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return Ok(new ResponseModel { Response = "You need to enter email" });
            }

            //check if user exists
            var checkUser = await userManager.FindByEmailAsync(model.Email);
            if (checkUser != null)
            {
                return Ok(new ResponseModel
                {
                    Response = "Seems you have an account with us already. Please proceed to login with this email",
                    Status = false
                });
            }

            string rolenameToUse = "fellow";

            IdentityUser user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var isAddedToRole = await AssignUserToRole(user, rolenameToUse);

                Fellow fellow = new Fellow
                {
                    ID = Guid.Parse(user.Id),
                    Email = model.Email,
                    ApplyProgress = ApplicationProgress.SignUp
                };

                await unitOfWorkFellow.Repository.Create(fellow);
                await unitOfWorkFellow.Save();

                // send signup emails
                //MessagingViewModel messagingViewModel = new MessagingViewModel();
                //messagingViewModel.Contacts = new List<ContactsViewModel>();
                //var webRoot = env.WebRootPath;

                //var templateLocation = webRoot + Path.DirectorySeparatorChar
                //                               + Path.DirectorySeparatorChar
                //                               + "EmailTemplates"
                //                               + Path.DirectorySeparatorChar
                //                               + "Welcome.html";

                //string welc = "We're excited to have you registered on the platform.<br /> We are here to help reduce your stress and give you the best of the fresh items market can offer";
                //string promo = "<a href=\"https://littlethings.com/lifestyle/relieve-stress-without-leaving-your-chair\">here is what doctors say about stress</a>";

                //var emailBody = Helper.Utility.GenerateEmailContent(templateLocation, model.Name, welc, promo);

                //messagingViewModel.Contacts.Add(new ContactsViewModel { Email = user.Email, Name = model.Name });
                //messagingViewModel.EmailAddress = "admin@projectdriveng.com.ng";
                //messagingViewModel.EmailDisplayName = "Customer's Friend at Elo";
                //messagingViewModel.Message = emailBody;
                //messagingViewModel.Subject = "Welcome! Customer's Friend at Elo";

                //eloEmailServices.SendSingleEmailAsync(messagingViewModel);


                // sign the user in
                var loginResponse = await LogUserIn(new UserDto { Email = user.Email, Password = model.Password});

                return Ok(loginResponse);
            }
            string errors = string.Join(' ', result.Errors.Select(x => x.Description + Environment.NewLine));
            return Ok(new ResponseModel { Response = errors, Status = false });
        }

        private async Task<bool> AssignUserToRole(IdentityUser user, string roleName)
        {
            try
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    await userManager.AddToRoleAsync(user, roleName);
                    return true;
                }
                else
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    var roleResult = await userManager.AddToRoleAsync(user, roleName);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<ResponseModel> LogUserIn(UserDto model)
        {
            var emailToUse = model.Email;

            //get the user
            var user = await userManager.FindByEmailAsync(emailToUse);

            if (user == null)
            {
                return new ResponseModel { Status = false, Response = "Account with this email does not exist!" };
            }


            //get the user's roles
            var roles = await userManager.GetRolesAsync(user);

            Fellow fellowUser = await unitOfWorkFellow.Repository.GetByID(Guid.Parse(user.Id));

            //check that the user is not null and that his password is correct
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                DateTime expirationDay; // NEED TO SPECIFY EXPIRATION TIME
                JwtSecurityTokenHandler tokenHandler;
                SecurityToken token;
                GenerateToken(model, user, roles, out expirationDay, out tokenHandler, out token);
                var response = new LoginResponseDTO
                {
                    UserID = Guid.Parse(user.Id),
                    Token = tokenHandler.WriteToken(token),
                    Email = user.Email,
                    ExpiryTime = expirationDay.AddDays(3).ToUniversalTime(),
                    FirstName = fellowUser.FirstName,
                    Address = fellowUser.Address,
                    PhoneNumber = fellowUser.PhoneNumber,
                    Roles = roles.ToList()
                };
                return new ResponseModel { Status = true, Response = "Signed in successfully!", ReturnObj = response };
            }
            //return an authorization error if the checks fail
            return new ResponseModel { Response = "Username or password invalid, please try again with correct details.", Status = false };
        }

        private void GenerateToken(UserDto model, IdentityUser user, IList<string> roles, out DateTime expirationDay, out JwtSecurityTokenHandler tokenHandler, out SecurityToken token)
        {
            //Generate Token
            var configExpTime = Encoding.ASCII.GetBytes(configuration["JwtSettings:ExpirationTime"].ToString());
            var correctExp = Encoding.ASCII.GetChars(configExpTime);
            expirationDay = DateTime.UtcNow.AddHours(double.Parse(correctExp));
            tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"].ToString()));
            List<Claim> subjectClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            subjectClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            subjectClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            subjectClaims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            subjectClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            subjectClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, model.Email));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(subjectClaims.AsEnumerable()),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.Value.Site,
                Audience = jwtSettings.Value.Audience,
                Expires = expirationDay

            };

            //create the token 
            token = tokenHandler.CreateToken(tokenDescriptor);
        }

    }
}
