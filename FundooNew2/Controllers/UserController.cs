using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace FundooNew2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBusiness business;
        private readonly IUserLoginBusiness login;
        private readonly IPasswordBusiness password;
        private readonly IBus bus;
        private readonly ILogger<UserController> _logger;
        private static TokenEmailClass tokenEmailClass;
        public UserController(IUserBusiness business, IUserLoginBusiness login, IPasswordBusiness passwordBusiness,
            IBus bus, ILogger<UserController> logger)
        {
            this.business = business;
            this.login = login; // 
            password = passwordBusiness;
            this.bus = bus;
            _logger = logger;
        }



        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Get All method started");
            return Ok(business.GetAll());
        }

        //[Authorize]
        [HttpGet("FirstName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUser(string FirstName)
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                _logger.LogWarning("Bad Request, Give Correct Name");
                return BadRequest();
            }
            var user = business.GetUser(FirstName);
            if (user == null)
            {
                _logger.LogError("User Not Found With given Name");
                return NotFound();
            }
            //int Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //string email=HttpContext.Session.GetString("UserEmail");
            return Ok(user);
        }
        //C:\Users\rajas\source\repos\FundooNew2\FundooNew2\FundooNew2.csproj
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(UserModel model)
        {

            bool flag = business.Insert(model);
            if (flag) return Ok(new {result=true, message="Registered"});

            return BadRequest(new { result = false, message = "Register Unsuccessful" });
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(string PEmail, UserUpdateModel model)
        {
            _logger.LogInformation(tokenEmailClass.Email);
            if (tokenEmailClass.Email != PEmail) { return BadRequest("Just Now Loggined is Not You, Please Login, To Update Your details"); }
            if (business.Update(PEmail, model) == false)
                return NotFound("Not Matched with any name");

            return NoContent();
        }

        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string firstname)
        {
            bool flag = business.DeleteUser(firstname);
            if (flag == false) return NotFound("Not fount delails");

            return NoContent();
        }

        /*
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Login(UserLoginModel model)
        {
            IActionResult responce = Unauthorized();
            var loginUser_ = login.LoginMethod(model);
            if(loginUser_ == null)
            {
                return NotFound();
            }  
            var token=login.GenerateToken(Id,model);
            responce=Ok(new { token = token });
            return responce;
           
        }*/

        [HttpPut]
        [Route("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult resetPasswordAction(ResetPassword resetPassword)
        {


            string result = password.ResetPasswordMethod(resetPassword);
            if (result.Equals("NotFound")) return NotFound(new {message= "User Not Found, Give Correct Email" , result=false});
            else if (result.Equals("PasswordNotEqual")) return BadRequest(new { message = "Password not matched", result = false });
            else return Ok(new { message = "Password Changed successfully", result = true });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Login(UserLoginModel model)
        {


            IActionResult responce = Unauthorized();
            tokenEmailClass = login.LoginMethod(model);
            if (tokenEmailClass == null)
            {
                responce= NotFound(new {result = false, message="Not found"});
            }
            else
            {
                responce = Ok(new { user = tokenEmailClass, result = true, message = "User Found" });
            }

            HttpContext.Session.SetInt32("UserId", tokenEmailClass.Id);
            HttpContext.Session.SetString("UserEmail", tokenEmailClass.Email);

            //responce = Ok(new { token = tokenEmailClass.Token });
            
            return responce;
        }

        

        [HttpPut("ForgetPassword")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                var result = password.FogetPasswordMethod(email);
                if (result != null)
                {
                    Send send = new Send();

                    send.SendingMail(result.Email, "Password is Trying to Changed is that you....! " + result.Token);//result

                    Uri uri = new Uri("rabbitmq://localhost/NotesEmail_Queue");
                    var endPoint = bus.GetSendEndpoint(uri);

                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        [HttpPost("UpdateOrCreate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateorUpdate(int Id, UserModel userModel)
        {
            if (Id <= 0 || userModel == null) return BadRequest();

            string result = business.CreateOrUpdate(Id, userModel);
            if (result.Equals("Created")) return Ok("Created successfully");
            else if (result.Equals("Updated")) return Ok("Updated SuccessFully");
            else return BadRequest("Create or Update is not Successfull");
        }






    }
}
