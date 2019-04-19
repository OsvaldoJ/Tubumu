﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// 后台：通知
    /// </summary>
    public partial class AdminController
    {
        #region 通知

        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost("GetNotifications")]
        public async Task<ApiResultData<Page<NotificationUser>>> GetNotifications(NotificationPageSearchCriteria criteria)
        {
            criteria.ToUserId = HttpContext.User.GetUserId();
            var page = await _notificationService.GetPageAsync(criteria);
            var result = new ApiResultData<Page<NotificationUser>>
            {
                Code = 200,
                Message = "获取通知列表成功",
                Data = page,
            };
            return result;
        }

        /// <summary>
        /// 设置通知已读
        /// </summary>
        /// <param name="notificationIdListInput"></param>
        /// <returns></returns>
        [HttpPost("ReadNotifications")]
        public async Task<ApiResult> ReadNotifications(NotificationIdListInput notificationIdListInput)
        {
            int userId = int.Parse(HttpContext.User.Identity.Name);
            var result = new ApiResult();
            if (!await _notificationService.ReadAsync(userId, notificationIdListInput.NotificationIds, ModelState))
            {
                result.Code = 400;
                result.Message = "设置已读失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "设置已读成功";
            return result;
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="notificationIdListInput"></param>
        /// <returns></returns>
        [HttpPost("DeleteNotifications")]
        public async Task<ApiResult> DeleteNotifications(NotificationIdListInput notificationIdListInput)
        {
            var result = new ApiResult();
            if (!await _notificationService.DeleteAsync(HttpContext.User.GetUserId(), notificationIdListInput.NotificationIds, ModelState))
            {
                result.Code = 400;
                result.Message = "删除失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "删除成功";
            return result;
        }

        /// <summary>
        /// 获取最新通知
        /// </summary>
        /// <param name="currentNotificationId"></param>
        /// <returns></returns>
        [HttpPost("GetNewestNotification")]
        public async Task<ApiResultData<NotificationUser>> GetNewestNotification(int? currentNotificationId)
        {
            int userId = int.Parse(HttpContext.User.Identity.Name);
            var result = new ApiResultData<NotificationUser>
            {
                Code = 200,
                Message = "获取最新消息成功",
                Data = await _notificationService.GetNewestAsync(userId, currentNotificationId),
            };

            return result;
        }

        #endregion
    }
}
