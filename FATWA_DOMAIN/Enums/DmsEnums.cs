using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Enums
{
	//<History Author = 'Hassan Abbas' Date='2023-06-16' Version="1.0" Branch="master"> Document Management Enums</History>
	public class DmsEnums
	{
		public enum DocumentClassificationEnum
		{
			External = 1,
			FreeEditor = 2,
			PredefinedTemplate = 4
		}

		public enum DocumentStatusEnum
		{
			Draft = 1,
			InReview = 2,
            Rejected = 4,
            Approved = 8,
			Published = 16,
		}

		public enum DocumentLinkModuleEnum
		{
			Literature = 1,
			LegalDocument = 2,
			LegalPrinciple = 4,
			CaseManagement = 8,
			ConsultationManagement = 16,
		}

		public enum DocumentLinkSubModuleEnum
		{
			CaseRequest = 1,
			CaseFile = 2,
			Case = 4,
			ConsultationRequest = 8,
			ConsultationFile = 16,
		}

        public enum SigningRequestStatusEnum
        {
            [Display(Name = "Initiated")]
            Initiated = 1,
            [Display(Name = "Approved")]
            Approved = 2,
            [Display(Name = "Declined")]
            Declined = 4,
            [Display(Name = "TimeOut")]
            TimeOut = 8,
            [Display(Name = "Failed")]
            Failed = 16,
        }

        public enum SigningMethodEnum
        {
            [Display(Name = "LocalSigning")]
            LocalSigning = 1,
            [Display(Name = "RemoteSigning")]
            RemoteSigning = 2,
            [Display(Name = "ExternalSigning")]
            ExternalSigning = 3,
            
        }

        public enum SigningTaskStatusEnum
        {
            [Display(Name = "UnSigned")]
            UnSigned = 1,
            [Display(Name = "Send For Signing")]
            SendForSigning = 2,
            [Display(Name = "Rejected")]
            Rejected = 3,
            [Display(Name = "Signed")]
            Signed = 4,
            [Display(Name = "Failed")]
            Failed = 5,
        }
    }
}
