using LR.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LR.Core.DemoModule.DemoAggregate
{
    public partial class DemoTB : Base.BaseEntity
    {
        #region 构造器

        public DemoTB()
        {
        }

        #endregion 构造器

        #region 属性
        [Key]
        public int? DemoId { get; set; }
        public string DemoName { get; set; }
        public string DemoEmail { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
        public string LastUpdateBy { get; set; }

        #endregion 属性

        #region IValidatableObject Members

        /// <summary>
        /// This will validate entity for all  the conditions
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            ////-->Check FirstName property
            //if (String.IsNullOrWhiteSpace(this.FirstName))
            //{
            //    validationResults.Add(new ValidationResult(Messages.validation_ProfileFirstNameCannotBeNull,
            //                                               new string[] { "AddressLine1" }));
            //}

            ////-->Check LastName property
            //if (String.IsNullOrWhiteSpace(this.LastName))
            //{
            //    validationResults.Add(new ValidationResult(Messages.validation_ProfileLastNameCannotBeBull,
            //                                               new string[] { "AddressLine2" }));
            //}

            ////-->Check Email property
            //if (String.IsNullOrWhiteSpace(this.Email))
            //{
            //    validationResults.Add(new ValidationResult(Messages.validation_ProfileEmailCannotBeBull,
            //                                               new string[] { "ZipCode" }));
            //}

            return validationResults;
        }

        #endregion
    }
}
