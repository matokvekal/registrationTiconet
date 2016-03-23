namespace Business_Logic
{
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(tblFamilyMetaData))]
    public partial class tblFamily
    {
    }
    public class tblFamilyMetaData
    {
        [Editable(false)]
        [localizedSystemDisplayName("tblFamily.familyId")]
        public object familyId { get; set; }

        [Required]
        //[Display(Name = "מספר זהות")]
        [localizedSystemDisplayName("tblFamily.ParentId")]
        public object ParentId { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1Type")]
        [Required]
        public object parent1Type { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1FirstName")]
        [Required]
        public object parent1FirstName { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1LastName")]
        [Required]
        public object parent1LastName { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1Email")]
        [Required]
        public object parent1Email { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1EmailConfirm")]
        public object parent1EmailConfirm { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1GetAlertByEmail")]
        public object parent1GetAlertByEmail { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1CellPhone")]
        [Required]
        public object parent1CellPhone { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1CellConfirm")]
        public object parent1CellConfirm { get; set; }

        [localizedSystemDisplayName("tblFamily.parent1GetAlertBycell")]
        public object parent1GetAlertBycell { get; set; }



        [localizedSystemDisplayName("tblFamily.parent2Type")]
        public object parent2Type { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2FirstName")]
        public object parent2FirstName { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2LastName")]
        public object parent2LastName { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2Email")]
        public object parent2Email { get; set; }
        [localizedSystemDisplayName("tblFamily.paymentOk")]
        public object paymentOk { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2EmailConfirm")]
        public object parent2EmailConfirm { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2GetAlertByEmail")]
        public object parent2GetAlertByEmail { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2CellPhone")]
        public object parent2CellPhone { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2CellConfirm")]
        public object parent2CellConfirm { get; set; }
        [localizedSystemDisplayName("tblFamily.parent2GetAlertBycell")]
        public object parent2GetAlertBycell { get; set; }

        [localizedSystemDisplayName("tblFamily.date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public object date { get; set; }

        [localizedSystemDisplayName("tblFamily.payMentDateConfirm")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public object payMentDateConfirm { get; set; }

        [Required]
        [localizedSystemDisplayName("tblFamily.iAgree")]
        public object iAgree { get; set; }
    }
}




