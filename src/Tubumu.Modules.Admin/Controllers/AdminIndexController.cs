﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.UI.Navigation;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// 后台：首页
    /// </summary>
    public partial class AdminController
    {
        private readonly string[] _imageExtensions = new[] { ".jpg", ".png" };

        #region Index

        /// <summary>
        /// 获取资料
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProfile")]
        public async Task<ApiResultData<Profile>> GetProfile()
        {
            var result = new ApiResultData<Profile>();
            var userInfo = await _userService.GetItemByUserIdAsync(HttpContext.User.GetUserId(), UserStatus.Normal);
            if (userInfo == null)
            {
                result.Code = 400;
                result.Message = "获取用户信息失败";
                return result;
            }
            var profile = new Profile
            {
                UserId = userInfo.UserId,
                Username = userInfo.Username,
                DisplayName = userInfo.DisplayName,
                AvatarUrl = userInfo.AvatarUrl,
                LogoUrl = userInfo.LogoUrl,
                Groups = await _groupService.GetInfoPathAsync(userInfo.Group.GroupId),
                Role = userInfo.Role,
            };

            result.Code = 200;
            result.Message = "获取用户信息成功";
            result.Data = profile;
            return result;
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ChangeProfile")]
        public async Task<ApiResult> ChangeProfile(UserChangeProfileInput input)
        {
            var result = new ApiResult();
            var changeProfileResult = await _adminUserService.ChangeProfileAsync(HttpContext.User.GetUserId(), input, ModelState);
            if (!changeProfileResult)
            {
                result.Code = 400;
                result.Message = "修改资料失败: " + ModelState.FirstErrorMessage();
            }
            else
            {
                result.Code = 200;
                result.Message = "修改资料成功";
            }

            return result;
        }

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("ChangeAvatar")]
        public async Task<ApiResultUrl> ChangeAvatar(IFormFile file)
        {
            var result = new ApiResultUrl();
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (!_imageExtensions.Contains(extension))
            {
                result.Code = 400;
                result.Message = "修改头像失败：图片格式错误(仅支持 jpg 或 png)";
                return result;
            }

            if (file.Length > 1024 * 1024)
            {
                result.Code = 400;
                result.Message = "修改头像失败：请保持在 1M 以内";
                return result;
            }
            var uploadFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "Upload", "Avatar");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var userId = HttpContext.User.GetUserId();
            var fileName =  userId + extension;
            using (var stream = System.IO.File.Create(Path.Combine(uploadFolder, fileName)))
            {
                file.CopyTo(stream);
            }
            var webUrl = $"/Upload/Avatar/{fileName}";
            if (!await _userService.ChangeAvatarAsync(userId, webUrl, ModelState))
            {
                result.Code = 400;
                result.Message = "修改头像失败：" + ModelState.FirstErrorMessage();
            }

            result.Url = webUrl;
            result.Code = 200;
            result.Message = "修改头像成功";
            return result;
        }

        /// <summary>
        /// 修改用户 Logo
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("ChangeLogo")]
        public async Task<ApiResultUrl> ChangeLogo(IFormFile file)
        {
            var result = new ApiResultUrl();
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (!_imageExtensions.Contains(extension))
            {
                result.Code = 400;
                result.Message = "修改 Logo 失败：图片格式错误(仅支持 jpg 或 png)";
                return result;
            }

            if (file.Length > 1024 * 1024)
            {
                result.Code = 400;
                result.Message = "修改 Logo 失败：请保持在 1M 以内";
                return result;
            }
            var uploadFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "Upload", "Logo");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var userId = HttpContext.User.GetUserId();
            var fileName =  userId + extension;
            using (var stream = System.IO.File.Create(Path.Combine(uploadFolder, fileName)))
            {
                file.CopyTo(stream);
            }
            var webUrl = $"/Upload/Logo/{fileName}";
            if (!await _userService.ChangeLogoAsync(userId, webUrl, ModelState))
            {
                result.Code = 400;
                result.Message = "修改 Logo 失败：" + ModelState.FirstErrorMessage();
            }

            result.Url = webUrl;
            result.Code = 200;
            result.Message = "修改 Logo 成功";
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        public async Task<ApiResult> ChangePassword(UserChangePasswordInput input)
        {
            var result = new ApiResult();
            if (!await _adminUserService.ChangePasswordAsync(HttpContext.User.GetUserId(), input, ModelState))
            {
                result.Code = 400;
                result.Message = "修改密码失败" + ModelState.FirstErrorMessage();
            }
            else
            {
                result.Code = 200;
                result.Message = "修改密码成功";
            }

            return result;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenus")]
        public ApiResultData<List<Menu>> GetMenus()
        {
            var list = new List<Menu>();
            var menuProviders = _menuProviders.OrderBy(m => m.Order);
            foreach (var menuProvider in menuProviders)
            {
                var items = menuProvider.GetModuleMenus();
                foreach (var item in items)
                {
                    AddMenuToList(list, item);
                }
            }

            var result = new ApiResultData<List<Menu>>
            {
                Code = 200,
                Message = "获取菜单成功",
                Data = list
            };
            return result;
        }

        #endregion

        #region Private Methods 

        private void AddMenuToList(List<Menu> list, Menu item)
        {
            if (!ValidateMenu(item)) return;

            if (item.Directly.HasValue && !item.Directly.Value)
            {
                // 避免无意义的序列化
                item.Directly = null;
            }

            if (item.Type == MenuType.Item && !item.Children.IsNullOrEmpty())
            {
                throw new Exception($"菜单项【{item.Title}】不能包含子项");
            }
            if (item.Type == MenuType.Sub || item.Type == MenuType.Group)
            {
                if (!item.LinkRouteName.IsNullOrWhiteSpace())
                {
                    throw new Exception($"{(item.Type == MenuType.Sub ? "子菜单" : "菜单组")}【{item.Title}】不能设置路由");
                }
                if (item.Directly.HasValue && item.Directly.Value)
                {
                    throw new Exception($"{(item.Type == MenuType.Sub ? "子菜单" : "菜单组")}【{item.Title}】不能设置为直接访问");
                }

                if (item.Children.IsNullOrEmpty())
                {
                    // 如果类型是 Sub 或 Group 并且没有任何 Child，则本菜单也不用显示。
                    return;
                }

                var newChildren = new List<Menu>();
                item.Children.ForEach(m => AddMenuToList(newChildren, m));
                if (newChildren.IsNullOrEmpty())
                {
                    // 如果经过全选过滤，子菜单或菜单组已无子项，则本子菜单或菜单组也不用显示。
                    return;
                }
                item.Children = newChildren;
            }

            if (item.Type == MenuType.Item && ValidateMenu(item))
            {
                item.Link = Url.RouteUrl(item.LinkRouteName, item.LinkRouteValues);
            }
            list.Add(item);

        }

        private bool ValidateMenu(Menu item)
        {
            if (item.Permission.IsNullOrWhiteSpace() && item.Role.IsNullOrWhiteSpace() && item.Group.IsNullOrWhiteSpace() && item.Validator == null)
            {
                return true;
            }

            var user = HttpContext.User;
            if (item.Validator != null)
            {
                return item.Validator(user);
            }

            if (item.Permission != null)
            {
                var perArray = item.Permission.Split('|', ';', ',');
                foreach (var it in perArray)
                {
                    if (user.HasPermission(it))
                    {
                        return true;
                    }
                }
            }
            if (item.Role != null)
            {
                var rolArray = item.Role.Split('|', ';', ',');
                foreach (var it in rolArray)
                {
                    if (HttpContext.User.IsInRole(it))
                    {
                        return true;
                    }
                }
            }
            if (item.Group != null)
            {
                var grpArray = item.Group.Split('|', ';', ',');
                foreach (var it in grpArray)
                {
                    if (user.IsInGroup(it))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
