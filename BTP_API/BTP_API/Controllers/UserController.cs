using BTP_API.Helpers;
using BTP_API.Models;
using BTP_API.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BTPContext _context;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _config;

        public UserController(BTPContext context, IOptionsMonitor<AppSettings> optionsMonitor, IConfiguration config)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginVM loginVM)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == loginVM.Email);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy tài khoản!"
                    });
                }

                if (user.IsVerify == false)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Tài khoản chưa được xác thực!"
                    });
                }

                if (user.IsActive == false)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Tài khoản của bạn đã bị khóa!"
                    });
                }
                bool isValid = BCrypt.Net.BCrypt.Verify(loginVM.Password, user.Password);
                if (!isValid)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Mật khẩu không chính xác!"
                    });
                }

                //Cấp token
                var token = GenerateToken(user);
                SetaccessToken(token);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    Data = token
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1);
                option.Secure = true;
                option.IsEssential = true;
                Response.Cookies.Append("accessToken", string.Empty, option);
                Response.Cookies.Append("refreshToken", string.Empty, option);
                //Then delete the cookie
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đăng xuất thành công"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] RegisterVM registerVM)
        {
            try
            {
                var user = _context.Users.Any(u => u.Email == registerVM.Email);
                if (user == true)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Username đã tồn tại",
                    });
                }
                int costParameter = 12;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerVM.Password, costParameter);
                //bool test = BCrypt.Net.BCrypt.Verify(registerVM.Password, hashedPassword);

                var newUser = new User
                {
                    RoleId = 3,
                    Email = registerVM.Email,
                    Password = hashedPassword,
                    VerificationToken = CreateRandomToken(),
                    Fullname = registerVM.Fullname,
                    Phone = registerVM.Phone,
                    AddressMain = registerVM.AddressMain,
                    IsActive = true
                };
                _context.Add(newUser);
                _context.SaveChanges();

                var getNewUser = _context.Users.SingleOrDefault(u => u.Email == registerVM.Email);
                if (getNewUser != null)
                {
                    var newShippingUser = new ShipInfo
                    {
                        UserId = getNewUser.Id,
                        IsUpdate = false
                    };
                    _context.Add(newShippingUser);
                    _context.SaveChanges();
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đăng ký thành công",
                    Data = newUser
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("verify-email")]
        public IActionResult Verify(string token)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.VerificationToken == token);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid token"
                    });
                }
                user.IsVerify = true;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Xác thực thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng"
                    });
                }
                user.ForgotPasswordCode = CreateRandomCode();
                _context.SaveChanges();

                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
                mail.To.Add(MailboxAddress.Parse(user.Email.Trim()));
                mail.Subject = "[Trạm Sách] - Cấp mã code đổi lại mật khẩu";
                mail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "<h3>Xin chào " + user.Fullname + "!</h3>" +
                    "<p>Mã code để bạn đặt lại mật khẩu là: " + user.ForgotPasswordCode + "</p>" +
                    "<p>Hãy thực hiện thay đổi ngay lập tức trước khi mã code hết hạn rồi đăng nhập lại để kiểm tra!</p>" +
                    "<p>Nếu có vấn đề phát sinh xảy ra, hãy liên hệ chúng tôi qua hotline: 0961284654</p>" +
                    "<p>Trân trọng!</p>" +
                    "<p>Hỗ trợ từ Trạm Sách!</p>"
                };


                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(mail);
                smtp.Disconnect(true);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đã gửi code đổi lại mật khẩu qua email của bạn!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("reset-password")]
        public IActionResult ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == resetPasswordVM.Email && u.ForgotPasswordCode == resetPasswordVM.ForgotPasswordCode);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không hợp lệ"
                    });
                }

                int costParameter = 12;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordVM.NewPassword, costParameter);

                user.Password = hashedPassword;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Rest mật khẩu thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("new-token")]
        public IActionResult RenewToken(TokenModel tokenModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.Now)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == tokenModel.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                _context.SaveChanges();

                //create new token
                var user = _context.Users.SingleOrDefault(nd => nd.Id == storedToken.UserId);
                var token = GenerateToken(user);
                SetaccessToken(token);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private TokenModel GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        new Claim(ClaimTypes.Name, user.Fullname),
                        new Claim("Phone", user.Phone),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     //Roles
                }),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //lưu database
            var refreshTokenEntity = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssueDate = DateTime.Now,
                ExpiredDate = DateTime.Now.AddDays(3)
            };

            _context.Add(refreshTokenEntity);
            _context.SaveChanges();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }

        private void SetaccessToken(TokenModel token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(1),
            };
            Response.Cookies.Append("accessToken", token.AccessToken, cookieOptions);
            Response.Cookies.Append("refreshToken", token.RefreshToken, cookieOptions);
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        private string CreateRandomCode()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(4));
        }
    }
}


