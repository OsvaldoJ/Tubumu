﻿using System.Collections.Generic;
using Tubumu.Modules.Admin.UI.Navigation;
using Tubumu.Modules.Framework.Authorization;

namespace Tubumu.Modules.Admin
{
    /// <summary>
    /// Menus
    /// </summary>
    public class Menus : IMenuProvider
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public int Order => -1000;

        /// <summary>
        /// 获取模块菜单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Menu> GetModuleMenus()
        {
            return new List<Menu>
            {
                /*
                new ModuleMenu
                {
                    Title = "管理首页",
                    Type = 1,
                    Children = new List<ModuleMenu> {
                        new ModuleMenu {
                            Title = "管理首页",
                            LinkRouteName = "Admin.View",
                            LinkRouteValues = new { IsCore = true, Title = "管理首页", Name = "welcome" }, // 因为 Webpack 调试服务器区分大小写，所以统一使用小写
                            Validator = u => u.HasPermission("后台管理"),
                        },
                        new ModuleMenu {
                            Title = "修改基本资料",
                            LinkRouteName = "Admin.ChangeProfile",
                            Validator = u => u.HasPermission("后台修改资料"),
                        },
                        new ModuleMenu {
                            Title = "修改登录密码",
                            LinkRouteName = "Admin.ChangePassword",
                            Validator = u => u.HasPermission("后台修改密码"),
                        },
                        new ModuleMenu {
                            Title = "退出登录",
                            LinkRouteName = "Admin.Api",
                            LinkRouteValues = new { Action = "Logout" }
                        },
                    }
                },*/
                new Menu
                {
                    Title = "系统管理",
                    Type = MenuType.Sub,
                    Children = new List<Menu> {
                         new Menu{
                             Type = MenuType.Group,
                             Title ="系统管理",
                             Children = new List<Menu> {
                                new Menu{ Title="系统公告", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "系统公告", Name = "bulletin" }, Validator = u => u.HasPermission("系统公告")},
                                new Menu{ Title="通知管理", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "通知管理", Name = "notificationmanage" }, Validator = u => u.HasPermission("通知管理")},
                             }
                         },
                         new Menu{
                             Type = MenuType.Group,
                             Title ="模块管理",
                             Children = new List<Menu> {
                                new Menu{ Title="模块元数据", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "模块元数据", Name = "modulemetadatas" }, Validator = u => u.HasPermission("模块元数据")},
                             }
                         },
                    },
                },
                new Menu
                {
                    Title = "组织架构管理",
                    Type = MenuType.Sub,
                    Children = new List<Menu> {
                         new Menu{ Title="用户列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "用户列表", Name = "user" }, Validator = u => u.HasPermission("用户管理")},
                         new Menu{ Title="分组列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "分组列表", Name = "group" }, Validator = u => u.HasPermission("分组管理")},
                         new Menu{ Title="角色列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "角色列表", Name = "role" }, Validator = u => u.HasPermission("角色管理")},
                         new Menu{ Title="权限列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "权限列表", Name = "permission" }, Validator = u => u.HasPermission("权限管理")},
                    }
                },
            };
        }
    }
}
