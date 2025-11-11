namespace FATWA_DOMAIN.Enums.Consultation
{
    public class ConsultationEnums
    {
        //public enum ConsultationTypeEnum
        //{

        //    AdministrativeCase = 1,
        //    CivilAndCommercialCase = 2,
        //    AdministrativeCompaints = 4,
        //    LegalAdvice = 8,
        //    Legislation = 16,
        //    InternationalArbitration = 32,
        //    ContractReview = 64
        //}
        //public enum ContractTypeEnum
        //{
        //    Momarasa = 1,
        //    Tender = 2,
        //    Bidding = 4,
        //    ExtensionOrRenewal = 8,
        //    DesignAndBuild = 16,
        //    PPP = 32,

        //}
        //public enum LegalAdviceEnum
        //{
        //    HousingAllowance = 1,
        //    Employee = 2,
        //    Ministry = 4,
        //    Judgment = 8,
        //    Authorities = 16,
        //    Investigations = 32,
        //    ResidentialCare = 64,
        //    SubClassification = 128

        //}
        //public enum InternationArbitration
        //{
        //    //No Meta Data

        //}
        public enum ConsultationArticleStatusEnum
        {
            OurStatus = 1,
            New = 2,
            Modifiable = 4,
            Locked = 8,

        }
        public enum ConsultationRequestStatusEnum
        {
            Draft = 1,
            Submitted = 2,
            InReview = 4,
            ONHold = 8,
            Registered = 16,
            Withdrawn = 32,
            Cancelled = 64,
            ReSubmitted = 128,
           
        }
        public enum ConsultationPartyTypeEnum
        {
            Type1 = 1,
            Type2 = 2,
            
        }

        public enum ConsultationTemplateSectionEnum
        {
            Title = 1,
            Introduction = 2,
            Party1 = 4,
            Party2 = 8,
            Article = 16,
            LockedArticle = 32,
        }
        public enum ConsultationScreensEnum
        {
            ListConsultationRequest = 1,
            ListConsultationRequestPendingtransferRequests = 2,
            ListConsultationFilePendingAssignFilesRequest = 4,
            
        }


    }
}
