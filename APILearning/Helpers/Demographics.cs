using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace APILearning.Helpers
{
    public class Demographics
    {
        public string Title { get; set; }
        public string TitleCode { get; set; }
        [StringLength(41)]
        public string Forename { get; set; }

        private string _othernames;
        [StringLength(41)]
        public string Othernames
        {
            get => _othernames;
            set => _othernames = (!string.IsNullOrWhiteSpace(value)) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower()) : "";
        }

        [StringLength(20)]
        public string Surname { get; set; }
      //  public string FullName => string.Format("{0} {1}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Forename.ToLower()), Surname);

        public string Address
        {
            get
            {
                return (string.IsNullOrWhiteSpace(Address1) ? string.Empty : Address1) +
                       (string.IsNullOrWhiteSpace(Address2) ? string.Empty : ", " + Address2) +
                       (string.IsNullOrWhiteSpace(Address3) ? string.Empty : ", " + Address3) +
                       (string.IsNullOrWhiteSpace(Address4) ? string.Empty : ", " + Address4);
            }
        }
        [StringLength(25)]
        public string Address1 { get; set; }
        [StringLength(25)]
        public string Address2 { get; set; }
        [StringLength(25)]
        public string Address3 { get; set; }
        [StringLength(25)]
        public string Address4 { get; set; }
        [StringLength(8)]
        public string Postcode { get; set; }
    }
}