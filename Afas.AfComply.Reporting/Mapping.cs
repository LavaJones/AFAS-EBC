using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Corrections;
using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afas.AfComply.Reporting.Domain.Transmission;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting
{
    public static class Mapping
    {

        public static void ConfigureMapper()
        {

            // NOTE: Sorry, you have to copy and paste this. Automapper wont Inherit "Ignore" because there is no unignore option.

            Mapper.CreateMap<TimeFrame, TimeFrame>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Void1095, Void1095>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Void1094, Void1094>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Transmission1095, Transmission1095>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Transmission1094, Transmission1094>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<PrintBatch, PrintBatch>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Print1095, Print1095>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Print1094, Print1094>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Correction1095, Correction1095>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Correction1094, Correction1094>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());


            Mapper.CreateMap<Approved1095Final, Approved1095Final>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore())
                 .ForMember(model => model.FileName, opt => opt.Ignore())
                 .ForMember(model => model.Suffix, opt => opt.Ignore())
                 .ForMember(model => model.DOB, opt => opt.Ignore());

            Mapper.CreateMap<Approved1095FinalPart2, Approved1095FinalPart2>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Approved1095FinalPart3, Approved1095FinalPart3>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Approved1094FinalPart1, Approved1094FinalPart1>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            //Mapper.CreateMap <, > ()
            //     .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
            //     .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
            //     .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
            //     .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
            //     .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
            //     .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<Employee1095summaryModel, Approved1095Final>()
               .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.FileName, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.Suffix, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EmployeeID, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.TaxYear, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EmployerID, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EmployeeResourceId, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.SelfInsured, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.part2s, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.part3s, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EntityStatus, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.StreetAddress, opt => opt.MapFrom(src => src.Address))
               .ForMember(baseModel => baseModel.DOB, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.Printed, opt => opt.Ignore());


            Mapper.CreateMap<Employee1095detailsPart2Model, Approved1095FinalPart2>()
             .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
             .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
             .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
             .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
             .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
             .ForMember(baseModel => baseModel.EntityStatus, opt => opt.Ignore())
             .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());


            Mapper.CreateMap<Employee1095detailsPart3Model, Approved1095FinalPart3>()
              .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
              .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
              .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
              .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
              .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
              .ForMember(baseModel => baseModel.EntityStatus, opt => opt.Ignore())
              .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore())
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
              //.ForMember(baseModel => baseModel.EnrolledAll12, opt => opt.MapFrom(src => src.Enrolled[12]))
              ;
            Mapper.CreateMap<Employer1094SummaryModel, Approved1094FinalPart1>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.Address, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.Approved1094FinalPart3s, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.Approved1094FinalPart4s, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.TaxYearId, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EmployerId, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.City, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.DgeCity, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.DgeAddress, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.DgeContactName, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EntityStatus, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.DgeContactPhoneNumber, opt => opt.MapFrom(src => src.DgeContactPhone))
               .ForMember(baseModel => baseModel.DgeEIN, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.DgeName, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.DgeState, opt => opt.Ignore())
            ;
            Mapper.CreateMap<Employer1094detailsPart3Model, Approved1094FinalPart3>()
               .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EntityStatus, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.Approved1094FinalPart1, opt => opt.Ignore())
               ;
            Mapper.CreateMap<Employer1094detailsPart4Model, Approved1094FinalPart4>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.EntityStatus, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore())
               .ForMember(baseModel => baseModel.Approved1094FinalPart1, opt => opt.Ignore())
               ;

            Mapper.AssertConfigurationIsValid();
        }
    }
}
