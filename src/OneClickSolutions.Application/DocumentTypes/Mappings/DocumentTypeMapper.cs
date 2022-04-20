using AutoMapper;
using OneClickSolutions.Application.DocumentTypes.Commands;
using OneClickSolutions.Application.DocumentTypes.ViewModels;
using OneClickSolutions.Domain.DocumentTypes;
using OneClickSolutions.Domain.DocumentTypes.Catalogs;

namespace OneClickSolutions.Application.DocumentTypes.Mappings
{
    public class DocumentTypeMapper:Profile
    {
        
        public DocumentTypeMapper(IDocumentTypePolicy _documentTypePolicy)
        {
            CreateMap<AddDocumentTypeViewModel, CreateDocumentTypeCommand>();
            CreateMap<CreateDocumentTypeCommand, DocumentType>().ConstructUsing(x => DocumentType.CreateInstance(x.Name,x.RegularExpression,x.DefaultDocumentType,x.Active,
                x.ZUGFeRdDocumentType,_documentTypePolicy));
        }
    }
}
