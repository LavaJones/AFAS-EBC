using Afas.AfComply.Domain;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.UI.Areas.ViewModels;
using Afas.AfComply.UI.Areas.ViewModels.Reporting;
using Afas.Application.CSV;
using Afas.Domain;
using Afas.Domain.POCO;
using Afc.Marketing.Models;
using AutoMapper;
using System;
using System.Linq;
using Afas.AfComply.UI.Areas.ViewModels.FileCabinet;

namespace Afas.AfComply.UI.Areas
{

    public static class Mapping
    {

        public static void ConfigureMapper()
        {

            Mapper.CreateMap<BaseViewModel, Model>()
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceStatus, opt => opt.UseValue("Active"))
                .Include<TimeFrameViewModel, TimeFrameModel>()
                .Include<Employee1095detailsPart2ViewModel, Employee1095detailsPart2Model>()
                .Include<Employee1095detailsPart3ViewModel, Employee1095detailsPart3Model>()
                .Include<Employer1094detailsPart3ViewModel, Employer1094detailsPart3Model>()
                .Include<Employer1094detailsPart4ViewModel, Employer1094detailsPart4Model>()
                .Include<Employee1095summaryViewModel, Employee1095summaryModel>()
                .Include<Employer1094SummaryViewModel, Employer1094SummaryModel>()
                .Include<Approved1095FinalPart2ViewModel, Approved1095FinalPart2Model>()
                .Include<Approved1095FinalPart3ViewModel, Approved1095FinalPart3Model>()
                .Include<Approved1095FinalViewModel, Approved1095FinalModel>()
                .Include<Approved1094FinalPart3ViewModel, Approved1094FinalPart3Model>()
                .Include<Approved1094FinalPart4ViewModel, Approved1094FinalPart4Model>()
                .Include<Approved1094FinalViewModel, Approved1094FinalPart1Model>()
                .Include<FileCabinetInfoViewModel, FileCabinetInfoModel>()
                .Include<FileCabinetFolderInfoViewModel, FileCabinetFolderInfoModel>()
                .Include<FileCabinetAccessViewModel, FileCabinetAccessModel>()
                 .Include<FileCabinetFolderAccessInfoViewModel, FileCabinetAccessModel>();



            Mapper.CreateMap<TimeFrameViewModel, TimeFrameModel>();
            Mapper.CreateMap<Employee1095detailsPart2ViewModel, Employee1095detailsPart2Model>().ForMember(baseModel => baseModel.EmployeeId, opt => opt.Ignore());
            Mapper.CreateMap<Employee1095detailsPart3ViewModel, Employee1095detailsPart3Model>();
            Mapper.CreateMap<Employee1095summaryViewModel, Employee1095summaryModel>()
                .ForMember(model => model.ResourceStatus, opt => opt.Ignore())
                .ForMember(model => model.EmployeeID, opt => opt.Ignore())
                .ForMember(model => model.EmployerID, opt => opt.Ignore())
                .ForMember(model => model.EmployeeResourceId, opt => opt.Ignore())
                .ForMember(model => model.Ssn, opt => opt.Ignore());

            Mapper.CreateMap<Employer1094detailsPart3ViewModel, Employer1094detailsPart3Model>();
            Mapper.CreateMap<Employer1094detailsPart4ViewModel, Employer1094detailsPart4Model>();
            Mapper.CreateMap<FileCabinetInfoViewModel, FileCabinetInfoModel>().ForMember(baseModel => baseModel.ArchiveFileInfo, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.OwnerResourceId, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ApplicationId, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.OtherResourceId, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.FileCabinetFolderInfo, opt => opt.Ignore());

            Mapper.CreateMap<FileCabinetAccessViewModel, FileCabinetAccessModel>()
                .ForMember(baseModel => baseModel.OwnerResourceId, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.FileCabinetFolderInfo, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.children, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ApplicationId, opt => opt.Ignore());

            Mapper.CreateMap<FileCabinetFolderAccessInfoViewModel, FileCabinetAccessModel>()
                 .ForMember(baseModel => baseModel.ApplicationId, opt => opt.Ignore())
                   .ForMember(baseModel => baseModel.FileCabinetFolderInfo, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.OwnerResourceId, opt => opt.Ignore());

            Mapper.CreateMap<FileCabinetFolderInfoViewModel, FileCabinetFolderInfoModel>().ForMember(baseModel => baseModel.ApplicationId, opt => opt.Ignore());
            Mapper.CreateMap<FileCabinetAccessModel, FileCabinetFolderAccessInfoViewModel>()
                .ForMember(baseModel => baseModel.children, opt => opt.MapFrom(src => src.children))
                .ForMember(baseModel => baseModel.FolderName, opt => opt.MapFrom(src => src.FileCabinetFolderInfo.FolderName))
                .ForMember(baseModel => baseModel.FolderDepth, opt => opt.MapFrom(src => src.FileCabinetFolderInfo.FolderDepth));



            Mapper.CreateMap<Employer1094SummaryViewModel, Employer1094SummaryModel>()
                .ForMember(model => model.EmployerID, opt => opt.Ignore())
                .ForMember(dest => dest.TransmissionTotal1095Forms, opt => opt.MapFrom(src => src.TransmissionTotal1095Forms))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(dest => dest.DgeZipCode, opt => opt.MapFrom(src => src.DgeZipCode))
                .ForMember(dest => dest.DgeStateId, opt => opt.MapFrom(src => src.DgeStateId));

            Mapper.CreateMap<Approved1095FinalPart2ViewModel, Approved1095FinalPart2Model>()
                .ForMember(baseModel => baseModel.employeeID, opt => opt.Ignore());

            Mapper.CreateMap<Approved1095FinalPart3ViewModel, Approved1095FinalPart3Model>()
                .ForMember(model => model.InsuranceCoverageRowID, opt => opt.Ignore())
                .ForMember(model => model.EmployeeID, opt => opt.Ignore())
                .ForMember(model => model.DependantID, opt => opt.Ignore())
                .ForMember(model => model.TaxYear, opt => opt.Ignore());

            Mapper.CreateMap<Approved1095FinalViewModel, Approved1095FinalModel>()
                .ForMember(model => model.EmployerID, opt => opt.Ignore())
                .ForMember(model => model.EmployeeID, opt => opt.Ignore())
                .ForMember(model => model.EmployeeResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Model, BaseViewModel>()
                .Include<TimeFrameModel, TimeFrameViewModel>()
                .Include<Employee1095detailsPart2Model, Employee1095detailsPart2ViewModel>()
                .Include<Employee1095detailsPart3Model, Employee1095detailsPart3ViewModel>()
                .Include<Employee1095summaryModel, Employee1095summaryViewModel>()
                .Include<Employer1094detailsPart3Model, Employer1094detailsPart3ViewModel>()
                .Include<Employer1094detailsPart4Model, Employer1094detailsPart4ViewModel>()
                .Include<Employer1094SummaryModel, Employer1094SummaryViewModel>()
                .Include<Approved1095FinalPart2Model, Approved1095FinalPart2ViewModel>()
                .Include<Approved1095FinalPart3Model, Approved1095FinalPart3ViewModel>()
                .Include<Approved1095FinalModel, Approved1095FinalViewModel>()
                .Include<FileCabinetInfoModel, FileCabinetInfoViewModel>()
                 .Include<FileCabinetFolderInfoModel, FileCabinetFolderInfoViewModel>()
                .Include<FileCabinetAccessModel, FileCabinetAccessViewModel>()
                .Include<FileCabinetAccessModel, FileCabinetFolderAccessInfoViewModel>()
                .Include<AfComply.Reporting.Core.Models.EmployerModel, EmployerViewModel>()
                ;

            Mapper.CreateMap<TimeFrameModel, TimeFrameViewModel>();

            Mapper.CreateMap<Employee1095detailsPart2Model, Employee1095detailsPart2ViewModel>()
                .ForMember(model => model.SummaryEncyptedParameters, opt => opt.Ignore());

            Mapper.CreateMap<Employee1095detailsPart3Model, Employee1095detailsPart3ViewModel>();
            Mapper.CreateMap<FileCabinetInfoModel, FileCabinetInfoViewModel>();
            Mapper.CreateMap<FileCabinetInfoModel, FileCabinetInfoViewModel>();
            Mapper.CreateMap<FileCabinetFolderInfoModel, FileCabinetFolderInfoViewModel>();
            Mapper.CreateMap<FileCabinetAccessModel, FileCabinetAccessViewModel>();


            Mapper.CreateMap<Employee1095summaryModel, Employee1095summaryViewModel>()
                .ForMember(model => model.LoadPart2ItemLink, opt => opt.Ignore())
                .ForMember(model => model.LoadPart3ItemLink, opt => opt.Ignore());
            Mapper.CreateMap<Employer1094SummaryModel, Employer1094SummaryViewModel>()
                .ForMember(model => model.Finalize1094ItemLink, opt => opt.Ignore())
                .ForMember(dest => dest.DgeCity, opt => opt.MapFrom(src => src.DgeCity))
                .ForMember(dest => dest.EmployerName, opt => opt.MapFrom(src => src.EmployerName))
                .ForMember(dest => dest.DgeAddress, opt => opt.MapFrom(src => src.DgeAddress))
                .ForMember(dest => dest.DgeCity, opt => opt.MapFrom(src => src.DgeCity))
                .ForMember(dest => dest.DgeState, opt => opt.MapFrom(src => src.DgeState))
                .ForMember(dest => dest.DgeStateId, opt => opt.MapFrom(src => src.DgeStateId))
                .ForMember(dest => dest.DgeContactName, opt => opt.MapFrom(src => src.DgeContactName))
                .ForMember(dest => dest.DgeZipCode, opt => opt.MapFrom(src => src.DgeZipCode))
                .ForMember(dest => dest.DgeAddress, opt => opt.MapFrom(src => src.DgeAddress));

            Mapper.CreateMap<Approved1095FinalPart2Model, Approved1095FinalPart2ViewModel>();

            Mapper.CreateMap<Approved1095FinalPart3Model, Approved1095FinalPart3ViewModel>();

            Mapper.CreateMap<Approved1095FinalModel, Approved1095FinalViewModel>();
            Mapper.CreateMap<AfComply.Reporting.Core.Models.EmployerModel, EmployerViewModel>();


            Mapper.CreateMap<Employer1094detailsPart3Model, Employer1094detailsPart3ViewModel>();
            Mapper.CreateMap<Employer1094detailsPart4Model, Employer1094detailsPart4ViewModel>();
            Mapper.CreateMap<FileCabinetInfoModel, FileCabinetInfoViewModel>();
            Mapper.CreateMap<FileCabinetAccessModel, FileCabinetAccessViewModel>();
            Mapper.CreateMap<FileCabinetFolderInfoModel, FileCabinetFolderInfoViewModel>();
            Mapper.CreateMap<FileCabinetAccessModel, FileCabinetFolderAccessInfoViewModel>();
            Mapper.CreateMap<classification, ConfirmationClassificationViewModel>()
                 .ForMember(baseModel => baseModel.CLASS_2GInValidPrice, opt => opt.Ignore());



            Mapper.CreateMap<Approved1095FinalPart3, CoveredIndividualMonthlyIndGrp>()
                .ForMember(covIndiv => covIndiv.JanuaryInd, opt => opt.MapFrom(from => from.EnrolledJan.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.FebruaryInd, opt => opt.MapFrom(from => from.EnrolledFeb.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.MarchInd, opt => opt.MapFrom(from => from.EnrolledMar.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.AprilInd, opt => opt.MapFrom(from => from.EnrolledApr.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.MayInd, opt => opt.MapFrom(from => from.EnrolledMay.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.JuneInd, opt => opt.MapFrom(from => from.EnrolledJun.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.JulyInd, opt => opt.MapFrom(from => from.EnrolledJul.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.AugustInd, opt => opt.MapFrom(from => from.EnrolledAug.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.SeptemberInd, opt => opt.MapFrom(from => from.EnrolledSep.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.OctoberInd, opt => opt.MapFrom(from => from.EnrolledOct.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.NovemberInd, opt => opt.MapFrom(from => from.EnrolledNov.BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.DecemberInd, opt => opt.MapFrom(from => from.EnrolledDec.BoolToOneZero()));

            Mapper.CreateMap<Approved1095FinalPart3, CoveredIndividualGrp>()
                .ForMember(covIndiv => covIndiv.employee_id, opt => opt.MapFrom(from => from.EmployeeID))
                .ForMember(covIndiv => covIndiv.covered_individual_id, opt => opt.MapFrom(from => from.DependantID))
                .ForMember(covIndiv => covIndiv.PersonFirstNm, opt => opt.MapFrom(from => from.FirstName.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19)))
                .ForMember(covIndiv => covIndiv.PersonMiddleNm, opt => opt.MapFrom(from => from.MiddleName.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19)))
                .ForMember(covIndiv => covIndiv.PersonLastNm, opt => opt.MapFrom(from => from.LastName.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19)))
                .ForMember(covIndiv => covIndiv.SSN, opt => opt.MapFrom(from => AesEncryption.Decrypt(from.SSN).Replace("l", "").RemoveDashes().Trim().ZeroPadSsn()))
                .ForMember(covIndiv => covIndiv.DOB, opt => opt.MapFrom(from => from.Dob ?? new DateTime()))
                .ForMember(covIndiv => covIndiv.BirthDt, opt => opt.MapFrom(from =>
                    from.SSN.IsNullOrEmpty() && from.Dob.HasValue && from.Dob.Value > new DateTime(1920, 1, 1) ? from.Dob.Value.ToString("yyyy-MM-dd") : string.Empty))
                .ForMember(covIndiv => covIndiv.CoveredIndividualAnnualInd, opt => opt.MapFrom(from => from.EnrolledAll12.HasValue ? from.EnrolledAll12.Value.BoolToOneZero() : null))
                .ForMember(covIndiv => covIndiv.CoveredIndividualMonthlyIndGrp, opt => opt.MapFrom(from => from.EnrolledAll12.HasValue ? null : from))
                .ForMember(covIndiv => covIndiv.PersonNameControlTxt, opt => opt.Ignore())
                .ForMember(covIndiv => covIndiv.SuffixNm, opt => opt.Ignore());

            Mapper.CreateMap<Approved1095Final, MonthlyEmployeeRequiredContriGrp>()
                .ForMember(covIndiv => covIndiv.JanuaryAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 1).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 1).Line14)))
                .ForMember(covIndiv => covIndiv.FebruaryAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 2).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 2).Line14)))
                .ForMember(covIndiv => covIndiv.MarchAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 3).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 3).Line14)))
                .ForMember(covIndiv => covIndiv.AprilAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 4).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 4).Line14)))
                .ForMember(covIndiv => covIndiv.MayAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 5).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 5).Line14)))
                .ForMember(covIndiv => covIndiv.JuneAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 6).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 6).Line14)))
                .ForMember(covIndiv => covIndiv.JulyAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 7).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 7).Line14)))
                .ForMember(covIndiv => covIndiv.AugustAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 8).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 8).Line14)))
                .ForMember(covIndiv => covIndiv.SeptemberAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 9).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 9).Line14)))
                .ForMember(covIndiv => covIndiv.OctoberAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 10).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 10).Line14)))
                .ForMember(covIndiv => covIndiv.NovemberAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 11).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 11).Line14)))
                .ForMember(covIndiv => covIndiv.DecemberAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 12).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 12).Line14)));






            Mapper.CreateMap<Approved1095Final, MonthlyOfferCoverageGrp>()
                .ForMember(covIndiv => covIndiv.JanOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 1).Line14))
                .ForMember(covIndiv => covIndiv.FebOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 2).Line14))
                .ForMember(covIndiv => covIndiv.MarOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 3).Line14))
                .ForMember(covIndiv => covIndiv.AprOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 4).Line14))
                .ForMember(covIndiv => covIndiv.MayOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 5).Line14))
                .ForMember(covIndiv => covIndiv.JunOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 6).Line14))
                .ForMember(covIndiv => covIndiv.JulOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 7).Line14))
                .ForMember(covIndiv => covIndiv.AugOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 8).Line14))
                .ForMember(covIndiv => covIndiv.SepOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 9).Line14))
                .ForMember(covIndiv => covIndiv.OctOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 10).Line14))
                .ForMember(covIndiv => covIndiv.NovOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 11).Line14))
                .ForMember(covIndiv => covIndiv.DecOfferCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 12).Line14));





            Mapper.CreateMap<Approved1095Final, MonthlySafeHarborGrp>()
                .ForMember(covIndiv => covIndiv.JanSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 1).Line16))
                .ForMember(covIndiv => covIndiv.FebSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 2).Line16))
                .ForMember(covIndiv => covIndiv.MarSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 3).Line16))
                .ForMember(covIndiv => covIndiv.AprSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 4).Line16))
                .ForMember(covIndiv => covIndiv.MaySafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 5).Line16))
                .ForMember(covIndiv => covIndiv.JunSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 6).Line16))
                .ForMember(covIndiv => covIndiv.JulSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 7).Line16))
                .ForMember(covIndiv => covIndiv.AugSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 8).Line16))
                .ForMember(covIndiv => covIndiv.SepSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 9).Line16))
                .ForMember(covIndiv => covIndiv.OctSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 10).Line16))
                .ForMember(covIndiv => covIndiv.NovSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 11).Line16))
                .ForMember(covIndiv => covIndiv.DecSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 12).Line16));

            Mapper.CreateMap<Approved1095Final, Form1095CUpstreamDetail>()
                /* These ones still need to figure out a mapping */
                .ForMember(covIndiv => covIndiv.RecordId, opt => opt.Ignore())          
                .ForMember(covIndiv => covIndiv.TestScenarioId, opt => opt.Ignore())  
                .ForMember(covIndiv => covIndiv.CorrectedInd, opt => opt.MapFrom(from => 0))    
                .ForMember(covIndiv => covIndiv.USZIPExtensionCd, opt => opt.Ignore())  
                .ForMember(covIndiv => covIndiv.PersonNameControlTxt, opt => opt.Ignore())  
                .ForMember(covIndiv => covIndiv.ALEContactPhoneNum, opt => opt.Ignore())    
                .ForMember(covIndiv => covIndiv.StartMonthNumberCd, opt => opt.MapFrom(from => "01"))  
                .ForMember(covIndiv => covIndiv.CorrectedRecordInfoGrp, opt => opt.Ignore())    
                .ForMember(covIndiv => covIndiv.CoveredIndividualInd, opt => opt.MapFrom(from => (from.part3s.Count > 0).BoolToOneZero()))
                .ForMember(covIndiv => covIndiv.employer_id, opt => opt.MapFrom(from => from.EmployerID))
                .ForMember(covIndiv => covIndiv.employee_id, opt => opt.MapFrom(from => from.EmployeeID))
                .ForMember(covIndiv => covIndiv.ResourceId, opt => opt.MapFrom(from => from.ResourceId))
                .ForMember(covIndiv => covIndiv.TaxYr, opt => opt.MapFrom(from => from.TaxYear))
                .ForMember(covIndiv => covIndiv.OtherCompletePersonFirstNm, opt => opt.MapFrom(from => from.FirstName.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19)))
                .ForMember(covIndiv => covIndiv.OtherCompletePersonLastNm, opt => opt.MapFrom(from => from.LastName.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19)))
                .ForMember(covIndiv => covIndiv.SSN, opt => opt.MapFrom(from => AesEncryption.Decrypt(from.SSN).Replace("l", "").RemoveDashes().Trim().ZeroPadSsn()))
                .ForMember(covIndiv => covIndiv.DOB, opt => opt.MapFrom(from => from.DOB ?? new DateTime()))
                .ForMember(covIndiv => covIndiv.BirthDt, opt => opt.MapFrom(from =>
                    from.SSN.IsNullOrEmpty() && from.DOB.HasValue && from.DOB.Value > new DateTime(1920, 1, 1) ? from.DOB.Value.ToString("yyyy-MM-dd") : string.Empty))
                .ForMember(covIndiv => covIndiv.AddressLine1Txt, opt => opt.MapFrom(from => from.StreetAddress.RemoveIllegalAddressIRSCharacters().RemoveDoubleSpaces().Trim().TruncateLongString(35)))
                .ForMember(covIndiv => covIndiv.CityNm, opt => opt.MapFrom(from => from.City.RemoveIllegalCityIRSCharacters().RemoveNumbers().Trim().TruncateLongString(22)))
                .ForMember(covIndiv => covIndiv.USStateCd, opt => opt.MapFrom(from => ((UsStateAbbreviationEnum)int.Parse(from.State)).ToString()))
                .ForMember(covIndiv => covIndiv.USZIPCd, opt => opt.MapFrom(from => from.Zip.ZeroPadZip().Substring(0, 5)))


                .ForMember(covIndiv => covIndiv.AnnualOfferOfCoverageCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 0).Line14.ReturnNullIfEmpty()))
                .ForMember(covIndiv => covIndiv.AnnlEmployeeRequiredContriAmt, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 0).Line15.ContribForCode(from.part2s.Find(month => month.MonthId == 0).Line14.ReturnNullIfEmpty())))
                .ForMember(covIndiv => covIndiv.AnnualSafeHarborCd, opt => opt.MapFrom(from => from.part2s.Find(month => month.MonthId == 0).Line16.SafeHarborForCode(from.part2s.Find(month => month.MonthId == 0).Line14.ReturnNullIfEmpty())))


                .ForMember(covIndiv => covIndiv.MonthlyEmployeeRequiredContriGrp, opt => opt.MapFrom(from =>
               from.part2s.Find(month => month.MonthId == 0).Line14.IsNullOrEmpty() ? from : null))       

                .ForMember(covIndiv => covIndiv.MonthlyOfferCoverageGrp, opt => opt.MapFrom(from =>
               from.part2s.Find(month => month.MonthId == 0).Line14.IsNullOrEmpty() ? from : null))

                .ForMember(covIndiv => covIndiv.MonthlySafeHarborGrp, opt => opt.MapFrom(from =>
               from.part2s.Find(month => month.MonthId == 0).Line14.IsNullOrEmpty() ? from : null))


                .ForMember(covIndiv => covIndiv.CoveredIndividualGrps, opt => opt.MapFrom(from => from.part3s));


            Mapper.CreateMap<Approved1094FinalPart1, GovtEntityEmployerInfoGrp>()  
                .ForMember(govtEnt => govtEnt.GovtAddressLine1Txt, opt => opt.MapFrom(from => from.DgeAddress.RemoveIllegalAddressIRSCharacters().RemoveDoubleSpaces().Trim().TruncateLongString(35)))
                .ForMember(govtEnt => govtEnt.GovtBusinessNameLine1Txt, opt => opt.MapFrom(from => from.DgeName.RemoveIllegalBusinessIRSCharacters().Trim().TruncateLongString(74)))
                .ForMember(govtEnt => govtEnt.GovtCityNm, opt => opt.MapFrom(from => from.DgeCity))
                .ForMember(govtEnt => govtEnt.GovtContactPhoneNum, opt => opt.MapFrom(from => from.DgeContactPhoneNumber))
                .ForMember(govtEnt => govtEnt.GovtEmployerEIN, opt => opt.MapFrom(from => from.DgeEIN))


                .ForMember(govtEnt => govtEnt.GovtPersonFirstNm, opt => opt.MapFrom(from => from.DgeContactName.Split(' ').First()))
                .ForMember(govtEnt => govtEnt.GovtPersonLastNm, opt => opt.MapFrom(from => string.Empty))
                .ForMember(govtEnt => govtEnt.GovtPersonMiddleNm, opt => opt.MapFrom(from => from.DgeContactName.Split(' ').Last()))


                .ForMember(govtEnt => govtEnt.GovtUSStateCd, opt => opt.MapFrom(from => ((UsStateAbbreviationEnum)from.DgeState).ToString()))
                .ForMember(govtEnt => govtEnt.GovtUSZIPCd, opt => opt.MapFrom(from => from.DgeZipCode.ZeroPadZip().Substring(0, 5)))
                .ForMember(govtEnt => govtEnt.GovtAddressLine2Txt, opt => opt.Ignore())
                .ForMember(govtEnt => govtEnt.GovtBusinessNameLine2Txt, opt => opt.Ignore())
                .ForMember(govtEnt => govtEnt.GovtSuffixNm, opt => opt.Ignore())
                .ForMember(govtEnt => govtEnt.GovtUSZIPExtensionCd, opt => opt.Ignore())
                .ForMember(govtEnt => govtEnt.GovtBusinessNameControlTxt, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Approved1094FinalPart3, ALEMemberMonthlyInfo>()
                .ForMember(final => final.employer_id, opt => opt.Ignore())
                .ForMember(final => final.time_frame_id, opt => opt.MapFrom(from => from.MonthId))
                .ForMember(final => final.MinEssentialCvrOffrCd, opt => opt.MapFrom(from => from.MinimumEssentialCoverageOfferIndicator.BoolToTwoOneZero()))
                .ForMember(final => final.ALEMemberFTECnt, opt => opt.MapFrom(from => from.FullTimeEmployeeCount))
                .ForMember(final => final.TotalEmployeeCnt, opt => opt.MapFrom(from => from.TotalEmployeeCount))
                .ForMember(final => final.MecEmployeeCnt, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Approved1094FinalPart1, ALEMemberInformationGrp>()
                .ForMember(final => final.employer_id, opt => opt.MapFrom(from => from.EmployerId))
                .ForMember(final => final.MonthlyInfo, opt => opt.MapFrom(from => from.Approved1094FinalPart3s))
                .ForMember(final => final.EmployerIsAggrAle, opt => opt.MapFrom(from => from.IsAggregatedAleGroup))
                ;

            Mapper.CreateMap<Approved1094FinalPart4, OtherALEMembersGrp>()
                .ForMember(final => final.OtherALEBusinessNameLine1Txt, opt => opt.MapFrom(from => from.EmployerName.RemoveIllegalBusinessIRSCharacters().Trim().TruncateLongString(74)))
                .ForMember(final => final.OtherALEEIN, opt => opt.MapFrom(from => from.EIN.Replace("-", "")))
                .ForMember(final => final.OtherALEBusinessNameLine2Txt, opt => opt.Ignore())
                .ForMember(final => final.OtherALEBusinessNameControlTxt, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Approved1094FinalPart1, Form1094CUpstreamDetail>()
                .ForMember(final => final.employer_id, opt => opt.MapFrom(from => from.EmployerId))
                .ForMember(final => final.dge, opt => opt.MapFrom(from => from.IsDge))

                .ForMember(final => final.SubmissionId, opt => opt.Ignore()) 
                .ForMember(final => final.OriginalUniqueSubmissionId, opt => opt.Ignore()) 
                .ForMember(final => final.TestScenarioId, opt => opt.Ignore()) 
                .ForMember(final => final.CorrectedInd, opt => opt.MapFrom(from => 0))
                .ForMember(final => final.AuthoritativeTransmittalInd, opt => opt.UseValue("1")) 
                .ForMember(final => final.JuratSignaturePIN, opt => opt.Ignore()) 
                .ForMember(final => final.PersonTitleTxt, opt => opt.Ignore())
                .ForMember(final => final.SignatureDt, opt => opt.Ignore()) 
                .ForMember(final => final.dtSignature, opt => opt.Ignore()) 
                .ForMember(final => final.Form1095CUpstreamDetails, opt => opt.Ignore())
                .ForMember(final => final.CorrectedSubmissionInfoGrp, opt => opt.Ignore()) 

                .ForMember(final => final.TaxYr, opt => opt.MapFrom(from => from.TaxYearId))
                .ForMember(final => final.BusinessNameLine1Txt, opt => opt.MapFrom(from => from.EmployerName.RemoveIllegalBusinessIRSCharacters().Trim().TruncateLongString(74)))
                .ForMember(final => final.EmployerEIN, opt => opt.MapFrom(from => from.EIN.RemoveDashes().Trim()))

                .ForMember(final => final.PersonFirstNm, opt => opt.MapFrom(from => from.IrsContactName.Split(' ').First().RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19)))
                .ForMember(final => final.PersonMiddleNm, opt => opt.MapFrom(from => string.Empty))
                .ForMember(final => final.PersonLastNm, opt => opt.MapFrom(from => from.IrsContactName.Split(' ').Last().RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19)))

                .ForMember(final => final.ContactPhoneNum, opt => opt.MapFrom(from => from.IrsContactPhone.RemoveDashes().Trim()))
                .ForMember(final => final.AddressLine1Txt, opt => opt.MapFrom(from => from.Address.RemoveIllegalAddressIRSCharacters().RemoveDoubleSpaces().Trim().TruncateLongString(35)))
                .ForMember(final => final.CityNm, opt => opt.MapFrom(from => from.City.RemoveIllegalCityIRSCharacters().RemoveNumbers().Trim().TruncateLongString(22)))
                .ForMember(final => final.USStateCd, opt => opt.MapFrom(from => ((UsStateAbbreviationEnum)from.StateId).ToString()))
                .ForMember(final => final.USZIPCd, opt => opt.MapFrom(from => from.ZipCode.ZeroPadZip().Substring(0, 5)))
                .ForMember(final => final.Form1095CAttachedCnt, opt => opt.MapFrom(from => from.TransmissionTotal1095Forms))
                .ForMember(final => final.GovtEntityEmployerInfoGrp, opt => opt.MapFrom(from => from.IsDge ? from : null))

                .ForMember(final => final.TotalForm1095CALEMemberCnt, opt => opt.MapFrom(from => from.Total1095Forms))
                .ForMember(final => final.AggregatedGroupMemberCd, opt => opt.MapFrom(from => from.IsAggregatedAleGroup.BoolToTwoOneZero()))

                .ForMember(final => final.QualifyingOfferMethodInd, opt => opt.MapFrom(from => string.Empty))
                .ForMember(final => final.QlfyOfferMethodTrnstReliefInd, opt => opt.MapFrom(from => string.Empty))
                .ForMember(final => final.Section4980HReliefInd, opt => opt.MapFrom(from => string.Empty))
                .ForMember(final => final.NinetyEightPctOfferMethodInd, opt => opt.MapFrom(from => string.Empty))
                .ForMember(final => final.ALEMemberInformationGrp, opt => opt.MapFrom(from => from))
                .ForMember(final => final.OtherALEMembersGrps, opt => opt.MapFrom(from => from.Approved1094FinalPart4s))

                .ForMember(final => final.BusinessNameLine2Txt, opt => opt.Ignore())  
                .ForMember(final => final.AddressLine2Txt, opt => opt.Ignore())
                .ForMember(final => final.BusinessNameControlTxt, opt => opt.Ignore())
                .ForMember(final => final.SuffixNm, opt => opt.Ignore())
                .ForMember(final => final.USZIPExtensionCd, opt => opt.Ignore())
                .ForMember(final => final.dge_ein, opt => opt.Ignore())
                ;



            Mapper.AssertConfigurationIsValid();

        }

    }

}