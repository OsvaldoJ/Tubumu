﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Tubumu.Core.Extensions;

namespace Tubumu.DataAnnotations
{
    /// <summary>
    /// MutexAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class MutexAttribute : ValidationAttribute
    {
        private readonly bool _canBeNull;
        private const string DefaultErrorMessage = "对{0}和{1}只能有一个输入";
        private const string DefaultNullErrorMessage = "对{0}和{1}需要有一个输入";

        /// <summary>
        /// OtherProperty
        /// </summary>
        public string OtherProperty { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalProperty"></param>
        /// <param name="canBeNull"></param>
        public MutexAttribute(string originalProperty, bool canBeNull)
            : base(DefaultErrorMessage)
        {
            OtherProperty = originalProperty;
            _canBeNull = canBeNull;
        }

        /// <summary>
        /// FormatErrorMessage
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, OtherProperty);
        }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = null;
            var originalProperty = validationContext.ObjectType.GetProperty(OtherProperty);
            if (originalProperty == null)
            {
                throw new NullReferenceException(nameof(originalProperty));
            }
            var otherValue = originalProperty.GetValue(validationContext.ObjectInstance, null);

            var valueString = (value ?? String.Empty).ToString();
            var otherValueString = (otherValue ?? String.Empty).ToString();

            if (valueString.IsNullOrEmpty() && otherValueString.IsNullOrEmpty())
            {
                if (_canBeNull)
                {
                    return ValidationResult.Success;
                }

                validationResult = new ValidationResult(String.Format(CultureInfo.CurrentUICulture, DefaultNullErrorMessage, validationContext.DisplayName, OtherProperty));
            }
            else if (!valueString.IsNullOrEmpty() && !otherValueString.IsNullOrEmpty())
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            if (validationResult != null)
                return validationResult;

            return ValidationResult.Success;
        }
    }
}
