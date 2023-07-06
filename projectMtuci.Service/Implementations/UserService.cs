﻿using projectMtuci.DAL.Interfaces;
using projectMtuci.Domain.Entity;
using projectMtuci.Domain.Enum;
using projectMtuci.Domain.Response;
using projectMtuci.Domain.ViewModels.Login;
using projectMtuci.Domain.ViewModels.User;
using projectMtuci.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace projectMtuci.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(UserViewModel userViewModel)
        {
            try
            {
                var user = await _userRepository.GetByName(userViewModel.Name);
                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с таким именем уже есть",
                    };
                }

                user = new User()
                {
                    Name = userViewModel.Name,
                    Role = "User",
                    Password = userViewModel.Password,
                };

                await _userRepository.Create(user);
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.IternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetByName(model.Name);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден",
                    };
                }

                if (user.Password.Trim() != model.Password)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пароль не верный",
                    };
                }
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.IternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
