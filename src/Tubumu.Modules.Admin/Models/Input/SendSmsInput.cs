﻿using System.ComponentModel.DataAnnotations;
using Tubumu.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 发送短信 Input
    /// </summary>
    public class SendSmsInput
    {
        /// <summary>
        /// 手机号码(多个手机号以半角逗号分隔)
        /// </summary>
        [Required(ErrorMessage = "请输入手机号码")]
        [ChineseMobilePeriod(ErrorMessage = "请输入以半角逗号分隔的手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 内容(短信内容请保持在 70 个字符以内)
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        [StringLength(70, ErrorMessage = "短信内容请保持在 70 个字符以内")]
        public string Text { get; set; }
    }
}
