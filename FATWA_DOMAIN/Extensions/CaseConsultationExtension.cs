using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Extensions
{
    //<History Author = 'Hassan Abbas' Date='2023-12-20' Version="1.0" Branch="master"> General Extension for defining shared methods related to Case and Consultation</History>
    public static class CaseConsultationExtension
    {
        //<History Author = 'Hassan Abbas' Date='2023-12-20' Version="1.0" Branch="master"> Get Sector Id based on Request And Court for Cases</History>
        public static int GetSectorIdBasedOnRequestAndCourtId(int requestTypeId, int courtTypeId)
        {
            int sectorTypeId = 0;
            if (requestTypeId == (int)RequestTypeEnum.Administrative)
            {
                if (courtTypeId == (int)CourtTypeEnum.Regional)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Appeal)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeAppealCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Supreme)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeSupremeCases;
                }
            }
            else if (requestTypeId == (int)RequestTypeEnum.CivilCommercial)
            {
                if (courtTypeId == (int)CourtTypeEnum.Regional)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Appeal)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialAppealCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Supreme)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases;
                }
            }
            else if (requestTypeId == (int)RequestTypeEnum.LegalAdvice)
            {
                sectorTypeId = (int)OperatingSectorTypeEnum.LegalAdvice;
            }
            else if (requestTypeId == (int)RequestTypeEnum.Legislations)
            {
                sectorTypeId = (int)OperatingSectorTypeEnum.Legislations;
            }
            else if (requestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
            {
                sectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeComplaints;
            }
            else if (requestTypeId == (int)RequestTypeEnum.Contracts)
            {
                sectorTypeId = (int)OperatingSectorTypeEnum.Contracts;
            }
            else if (requestTypeId == (int)RequestTypeEnum.InternationalArbitration)
            {
                sectorTypeId = (int)OperatingSectorTypeEnum.InternationalArbitration;
            }
            return sectorTypeId;
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-17' Version="1.0" Branch="master"> Get Sector Id based on Request And Court for Cases/MOJ RPA</History>
        public static int GetSectorIdBasedOnRequestAndCourtIdForMojRPA(int requestTypeId, int courtTypeId)
        {
            int sectorTypeId = 0;
            if (requestTypeId == (int)RequestTypeEnum.Administrative)
            {
                if (courtTypeId == (int)CourtTypeEnum.Regional)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeRegionalCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Appeal)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeAppealCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Supreme)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeSupremeCases;
                }
            }
            else if (requestTypeId == (int)RequestTypeEnum.CivilCommercial)
            {
                if (courtTypeId == (int)CourtTypeEnum.Regional)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Appeal)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialAppealCases;
                }
                else if (courtTypeId == (int)CourtTypeEnum.Supreme)
                {
                    sectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases;
                }
            }
            return sectorTypeId;
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-21' Version="1.0" Branch="master"> Get Request Type Id based on Sector Type Id</History>
        public static int GetRequestTypeIdBasedOnSectorIdForCases(int sectorTypeId)
        {
            int requestTypeId = 0;
            if (sectorTypeId >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && sectorTypeId <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
            {
                requestTypeId = (int)RequestTypeEnum.Administrative;
            }
            if (sectorTypeId >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && sectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
            {
                requestTypeId = (int)RequestTypeEnum.CivilCommercial;
            }
            return requestTypeId;
        }
		public static int GetFatwaSectorTypeBasedOnSectorId(int sectorTypeId)
        {
            int fatwaSectorType = 0;
            if(sectorTypeId >=(int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && sectorTypeId<=(int)OperatingSectorTypeEnum.Execution)
            {
				fatwaSectorType = (int)FatwaSectorTypeEnum.Case;
            }
            else if(sectorTypeId >=(int)OperatingSectorTypeEnum.LegalAdvice && sectorTypeId<=(int)OperatingSectorTypeEnum.InternationalArbitration)
            {
				fatwaSectorType = (int)FatwaSectorTypeEnum.Consultation;
            }
            else
            {
				fatwaSectorType = (int)FatwaSectorTypeEnum.Others;

			}
            return fatwaSectorType;
		}

		public static int GetCourtTypeIdBasedOnSectorId(int sectorTypeId)
        {
            int courtTypeId = 0;
            if (sectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases || sectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases || sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases || sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases)
            {
                courtTypeId = (int)CourtTypeEnum.Regional;
            }
            else if (sectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases || sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
            {
                courtTypeId = (int)CourtTypeEnum.Appeal;
            }
            else if (sectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases || sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)
            {
                courtTypeId = (int)CourtTypeEnum.Supreme;
            }
            else if (sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
            {
                courtTypeId = (int)CourtTypeEnum.PartialUrgent;
            }
            return courtTypeId;
		}


	}
}
