using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class LmsViewableLiteratureVM : GridMetadata
    {
        [Key]
        public int LiteratureId { get; set; }
        public string? ISBN { get; set; }

        public string? LiteratureName { get; set; }
        public string? DeweyBookNumber { get; set; }

        public string? LiteratureAuthor_En { get; set; }
        public string? LiteratureAuthor_Ar { get; set; }

        public string? EditionNumber { get; set; }
        public DateTime? EditionYear { get; set; }
        public string? DeletedBy { get; set; }

        public int? CopyCount { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public string? IndexName_En { get; set; }
        public string? IndexName_Ar { get; set; }
        public  string? StoragePath { get; set; } 
        public  string? DocType { get; set; }   
        public  string? FileName { get; set; }   
        public  int? AttachmentTypeId { get; set; }   
        public  int? UploadedDocumentId { get; set; }   

        [NotMapped]
        public int? ClassificationId { get; set; }
        public string? IndexNumber { get; set; }
        [NotMapped]
        public string? Name_En { get; set; }
        [NotMapped]
        public string? Name_Ar { get; set; }
        [NotMapped]
        public string? DivisionNumber { get; set; }
        [NotMapped]
        public string? AisleNumber { get; set; }
        [NotMapped]
        public string? Name { get; set; }

        [NotMapped]
        public int? NumberOfBorrowedBooks { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        // NEW


    }





}