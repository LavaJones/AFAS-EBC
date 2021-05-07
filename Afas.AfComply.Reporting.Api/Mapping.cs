using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Approvals.FileCabinet;
using Afas.AfComply.Reporting.Domain.Corrections;
using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afas.AfComply.Reporting.Domain.Transmission;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Core.Domain;
using Afc.Marketing.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.Reporting.Api
{

    public static class Mapping
    {

        public static void ConfigureMapper()
        {

            Afas.AfComply.Reporting.Mapping.ConfigureMapper();

            Mapper.CreateMap<Model, BaseReportingModel>()
                .ForMember(am => am.EntityStatus, opt => opt.MapFrom(a => Enum.Parse(typeof(EntityStatusEnum), a.ResourceStatus, true)))
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                ;

            Mapper.CreateMap<TimeFrameModel, TimeFrame>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Transmission1095Model, Transmission1095>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Transmission1094Model, Transmission1094>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Void1095Model, Void1095>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Void1094Model, Void1094>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<PrintBatchModel, PrintBatch>().IncludeBase<Model, BaseReportingModel>()
                .ForMember(dest => dest.ArchivedFile, opt => opt.Ignore());
            Mapper.CreateMap<Print1095Model, Print1095>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Print1094Model, Print1094>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Correction1095Model, Correction1095>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Correction1094Model, Correction1094>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Approved1095FinalModel, Approved1095Final>().IncludeBase<Model, BaseReportingModel>()
                .ForMember(model => model.FileName, opt => opt.Ignore())
                .ForMember(model => model.Suffix, opt => opt.Ignore())
                .ForMember(model => model.DOB, opt => opt.Ignore())
                .ForMember(dest => dest.part2s, opt => opt.MapFrom(src => src.part2s))
                .ForMember(dest => dest.part3s, opt => opt.MapFrom(src => src.part3s));
            Mapper.CreateMap<Approved1095FinalPart2Model, Approved1095FinalPart2>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Approved1095FinalPart3Model, Approved1095FinalPart3>().IncludeBase<Model, BaseReportingModel>()
              .ForMember(baseModel => baseModel.SSN, opt => opt.MapFrom(src => src.SSN))
              .ForMember(baseModel => baseModel.EnrolledJan, opt => opt.MapFrom(src => src.Enrolled[0]))
              .ForMember(baseModel => baseModel.EnrolledFeb, opt => opt.MapFrom(src => src.Enrolled[1]))
              .ForMember(baseModel => baseModel.EnrolledMar, opt => opt.MapFrom(src => src.Enrolled[2]))
              .ForMember(baseModel => baseModel.EnrolledApr, opt => opt.MapFrom(src => src.Enrolled[3]))
              .ForMember(baseModel => baseModel.EnrolledMay, opt => opt.MapFrom(src => src.Enrolled[4]))
              .ForMember(baseModel => baseModel.EnrolledJun, opt => opt.MapFrom(src => src.Enrolled[5]))
              .ForMember(baseModel => baseModel.EnrolledJul, opt => opt.MapFrom(src => src.Enrolled[6]))
              .ForMember(baseModel => baseModel.EnrolledAug, opt => opt.MapFrom(src => src.Enrolled[7]))
              .ForMember(baseModel => baseModel.EnrolledSep, opt => opt.MapFrom(src => src.Enrolled[8]))
              .ForMember(baseModel => baseModel.EnrolledOct, opt => opt.MapFrom(src => src.Enrolled[9]))
              .ForMember(baseModel => baseModel.EnrolledNov, opt => opt.MapFrom(src => src.Enrolled[10]))
              .ForMember(baseModel => baseModel.EnrolledDec, opt => opt.MapFrom(src => src.Enrolled[11]))
              .ForMember(baseModel => baseModel.EnrolledAll12, opt => opt.Ignore())
              ;
            Mapper.CreateMap<Employee1095summaryModel, Approved1095Final>().IncludeBase<Model, BaseReportingModel>()
              .ForMember(dest => dest.FileName, opt => opt.Ignore())
              .ForMember(dest => dest.Suffix, opt => opt.Ignore())
              .ForMember(dest => dest.DOB, opt => opt.Ignore())
              .ForMember(dest => dest.TaxYear, opt => opt.MapFrom(src => src.TaxYear))
              .ForMember(dest => dest.EmployeeID, opt => opt.MapFrom(src => src.EmployeeID))
              .ForMember(dest => dest.EmployerID, opt => opt.MapFrom(src => src.EmployerID))
              .ForMember(dest => dest.EmployeeResourceId, opt => opt.MapFrom(src => src.EmployeeResourceId))
              .ForMember(dest => dest.part2s, opt => opt.MapFrom(src => src.EmployeeMonthlyDetails))
              .ForMember(dest => dest.EntityStatus, opt => opt.MapFrom(o => EntityStatusEnum.Active))
              .ForMember(dest => dest.part3s, opt => opt.MapFrom(src => src.CoveredIndividuals));

            Mapper.CreateMap<Employer1094SummaryModel, Approved1094FinalPart1>().IncludeBase<Model, BaseReportingModel>()
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                 .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                 .ForMember(dest => dest.DgeCity, opt => opt.MapFrom(src => src.DgeCity))
                 .ForMember(dest => dest.EmployerId, opt => opt.MapFrom(src => src.EmployerID))
                 .ForMember(dest => dest.TaxYearId, opt => opt.MapFrom(src => src.TaxYearId))
                 .ForMember(dest => dest.EntityStatus, opt => opt.MapFrom(o => EntityStatusEnum.Active))
                 .ForMember(dest => dest.DgeContactPhoneNumber, opt => opt.MapFrom(src => src.DgeContactPhone))
                 .ForMember(dest => dest.DgeContactName, opt => opt.MapFrom(src => src.DgeContactName))
                 .ForMember(dest => dest.DgeZipCode, opt => opt.MapFrom(src => src.DgeZipCode))
                 .ForMember(dest => dest.DgeAddress, opt => opt.MapFrom(src => src.DgeAddress))
                 .ForMember(dest => dest.TransmissionTotal1095Forms, opt => opt.MapFrom(src => src.TransmissionTotal1095Forms))
                 .ForMember(dest => dest.Approved1094FinalPart3s, opt => opt.MapFrom(source => source.Employer1094Part3s))
                 .ForMember(dest => dest.Approved1094FinalPart4s, opt => opt.MapFrom(source => source.Employer1094Part4s));

            Mapper.CreateMap<Core.Models.EmployerModel, ApprovedEmployer>().IncludeBase<Model, BaseReportingModel>();


            Mapper.CreateMap<Employee1095detailsPart2Model, Approved1095FinalPart2>().IncludeBase<Model, BaseReportingModel>()
                .ForMember(dest => dest.EntityStatus, opt => opt.MapFrom(o => EntityStatusEnum.Active));
            Mapper.CreateMap<Employee1095detailsPart3Model, Approved1095FinalPart3>().IncludeBase<Model, BaseReportingModel>()
              .ForMember(dest => dest.EntityStatus, opt => opt.MapFrom(o => EntityStatusEnum.Active))
              .ForMember(baseModel => baseModel.SSN, opt => opt.MapFrom(src => src.SsnHidden))
              .ForMember(baseModel => baseModel.EnrolledJan, opt => opt.MapFrom(src => src.Enrolled[0]))
              .ForMember(baseModel => baseModel.EnrolledFeb, opt => opt.MapFrom(src => src.Enrolled[1]))
              .ForMember(baseModel => baseModel.EnrolledMar, opt => opt.MapFrom(src => src.Enrolled[2]))
              .ForMember(baseModel => baseModel.EnrolledApr, opt => opt.MapFrom(src => src.Enrolled[3]))
              .ForMember(baseModel => baseModel.EnrolledMay, opt => opt.MapFrom(src => src.Enrolled[4]))
              .ForMember(baseModel => baseModel.EnrolledJun, opt => opt.MapFrom(src => src.Enrolled[5]))
              .ForMember(baseModel => baseModel.EnrolledJul, opt => opt.MapFrom(src => src.Enrolled[6]))
              .ForMember(baseModel => baseModel.EnrolledAug, opt => opt.MapFrom(src => src.Enrolled[7]))
              .ForMember(baseModel => baseModel.EnrolledSep, opt => opt.MapFrom(src => src.Enrolled[8]))
              .ForMember(baseModel => baseModel.EnrolledOct, opt => opt.MapFrom(src => src.Enrolled[9]))
              .ForMember(baseModel => baseModel.EnrolledNov, opt => opt.MapFrom(src => src.Enrolled[10]))
              .ForMember(baseModel => baseModel.EnrolledDec, opt => opt.MapFrom(src => src.Enrolled[11]))
              .ForMember(baseModel => baseModel.EnrolledAll12, opt => opt.Ignore())
              ;
            Mapper.CreateMap<Approved1094FinalPart1Model, Approved1094FinalPart1>().IncludeBase<Model, BaseReportingModel>()
                 .ForMember(dest => dest.DgeState, opt => opt.MapFrom(source => source.DgeStateId))
                 .ForMember(dest => dest.DgeContactPhoneNumber, opt => opt.MapFrom(source => source.DgeContactPhone))
                 .ForMember(dest => dest.Approved1094FinalPart3s, opt => opt.MapFrom(source => source.Approved1094FinalPart3Models))
                 .ForMember(dest => dest.TransmissionTotal1095Forms, opt => opt.MapFrom(source => source.TransmissionTotal1095Forms))
                 .ForMember(dest => dest.Approved1094FinalPart4s, opt => opt.MapFrom(source => source.Approved1094FinalPart4Models));

            Mapper.CreateMap<Approved1094FinalPart3Model, Approved1094FinalPart3>().IncludeBase<Model, BaseReportingModel>();
            Mapper.CreateMap<Approved1094FinalPart4Model, Approved1094FinalPart4>().IncludeBase<Model, BaseReportingModel>();

            Mapper.CreateMap<BaseReportingModel, Model>()
               .ForMember(am => am.ResourceStatus, opt => opt.MapFrom(a => a.EntityStatus.ToString()))
               ;

            Mapper.CreateMap<TimeFrame, TimeFrameModel>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Transmission1095Model, Transmission1095>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Transmission1094Model, Transmission1094>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Void1095, Void1095Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Void1094, Void1094Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<PrintBatch, PrintBatchModel>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Print1095, Print1095Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Print1094, Print1094Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Correction1095, Correction1095Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Correction1094, Correction1094Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Approved1095Final, Approved1095FinalModel>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Approved1095FinalPart2, Approved1095FinalPart2Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Approved1095FinalPart3, Approved1095FinalPart3Model>().IncludeBase<BaseReportingModel, Model>()
              .ForMember(baseModel => baseModel.Enrolled, opt => opt.MapFrom(src => new bool[] { src.EnrolledJan, src.EnrolledFeb, src.EnrolledMar, src.EnrolledApr, src.EnrolledMay, src.EnrolledJun, src.EnrolledJul, src.EnrolledAug, src.EnrolledSep, src.EnrolledOct, src.EnrolledNov, src.EnrolledDec, src.EnrolledAll12.HasValue ? src.EnrolledAll12.Value : false }));

            Mapper.CreateMap<Approved1094FinalPart1, Approved1094FinalPart1Model>().IncludeBase<BaseReportingModel, Model>()
                .ForMember(dest => dest.Approved1094FinalPart3Models, opt => opt.MapFrom(source => source.Approved1094FinalPart3s))
                 .ForMember(dest => dest.Approved1094FinalPart4Models, opt => opt.MapFrom(source => source.Approved1094FinalPart4s))
                 .ForMember(dest => dest.DgeStateId, opt => opt.MapFrom(source => source.DgeState))
                 .ForMember(dest => dest.TransmissionTotal1095Forms, opt => opt.MapFrom(source => source.TransmissionTotal1095Forms))
                 .ForMember(dest => dest.DgeContactPhone, opt => opt.MapFrom(source => source.DgeContactPhoneNumber));

            Mapper.CreateMap<Approved1094FinalPart3, Approved1094FinalPart3Model>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<Approved1094FinalPart4, Approved1094FinalPart4Model>().IncludeBase<BaseReportingModel, Model>();

            Mapper.CreateMap<FileCabinetInfo, FileCabinetInfoModel>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<FileCabinetFolderInfo, FileCabinetFolderInfoModel>().IncludeBase<BaseReportingModel, Model>();
            Mapper.CreateMap<FileCabinetAccess, FileCabinetAccessModel>().IncludeBase<BaseReportingModel, Model>();

            Mapper.CreateMap<FileCabinetInfoModel, FileCabinetInfo>().IncludeBase<Model, BaseReportingModel>()
                .ForMember(dest => dest.EntityStatus, opt => opt.MapFrom(o => EntityStatusEnum.Active));

            Mapper.CreateMap<FileCabinetFolderInfoModel, FileCabinetFolderInfo>().IncludeBase<Model, BaseReportingModel>()
               .ForMember(dest => dest.EntityStatus, opt => opt.MapFrom(o => EntityStatusEnum.Active));

            Mapper.CreateMap<FileCabinetAccessModel, FileCabinetAccess>().IncludeBase<Model, BaseReportingModel>()
               .ForMember(dest => dest.EntityStatus, opt => opt.MapFrom(o => EntityStatusEnum.Active));


            Mapper.AssertConfigurationIsValid();
        }

    }

}