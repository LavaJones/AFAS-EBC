using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using Afas.ImportConverter.Domain.POCO;
using Afc.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter
{
    public static class Mapping
    {

        public static void ConfigureMapper()
        {

            Mapper.CreateMap<AImportFormatCommand, AImportFormatCommand>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<AddNewColumnFormatCommand, AddNewColumnFormatCommand>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<AutoGenerateDefaultValuesFormatCommand, AutoGenerateDefaultValuesFormatCommand>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<DefaultColumnSingleFormatCommand, DefaultColumnSingleFormatCommand>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<DefaultColumnValueFormatCommand, DefaultColumnValueFormatCommand>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<DeleteByValueFormatCommand, DeleteByValueFormatCommand>()
                .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<DeleteColumnFormatCommand, DeleteColumnFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<DeleteColumnIfBlankFormatCommand, DeleteColumnIfBlankFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<DeleteRowsFormatCommand, DeleteRowsFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<FormatColumnValuesFormatCommand, FormatColumnValuesFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<MergeColumnsFormatCommand, MergeColumnsFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<RenameColumnFormatCommand, RenameColumnFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<ReplaceByValueFormatCommand, ReplaceByValueFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<UnifyColumnsFormatCommand, UnifyColumnsFormatCommand>()
                 .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                 .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<ImportData, ImportData>()
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<ImportFileMetaData, ImportFileMetaData>()
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<ImportFormatCommandScope, ImportFormatCommandScope>()
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<ImportFormatFileCommandScope, ImportFormatFileCommandScope>()
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<ImportMetaData, ImportMetaData>()
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<StagingImport, StagingImport>()
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<BaseUploadedDataInfo, BaseUploadedDataInfo>()
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());

            Mapper.CreateMap<UploadedFileInfo, UploadedFileInfo>()
                  .ForMember(baseModel => baseModel.ArchiveFileInfo, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ID, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ModifiedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedBy, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.CreatedDate, opt => opt.Ignore())
                  .ForMember(baseModel => baseModel.ResourceId, opt => opt.Ignore());



            Mapper.AssertConfigurationIsValid();
        }
    }
}
