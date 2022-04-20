using OneClickSolutions.Domain.DocumentTypes.Catalogs;
using OneClickSolutions.Infrastructure.Domain;
using OneClickSolutions.Infrastructure.Extensions;

namespace OneClickSolutions.Domain.DocumentTypes;
public class DocumentType : Entity<Guid>, IAggregateRoot, IHasRowIntegrity, ICreationTracking, IModificationTracking, IHasValidationRule
{
    private DocumentType()
    {

    }
    private DocumentType(string name, string regularExpression, bool defaultDocumentType, bool active, bool zUGFeRdDocumentType)
    {
        Name = name;
        RegularExpression = regularExpression;
        DefaultDocumentType = defaultDocumentType;
        Active = active;
        ZUGFeRdDocumentType = zUGFeRdDocumentType;
    }

    public static DocumentType CreateInstance(string name, string regularExpression, bool defaultDocumentType, bool active,
        bool zUGFeRdDocumentType,IDocumentTypePolicy documentTypePolicy)
    {
        Check.NotNullOrEmpty(name, nameof(Name));
        Check.CheckregEx(regularExpression, "", nameof(RegularExpression));
        var instance = new DocumentType(name, regularExpression, defaultDocumentType, active, zUGFeRdDocumentType);
        Validate(instance);
        if(documentTypePolicy.DocumentTypeNameMustBeUnique(instance)) throw new Exception("Name must be unique");
        instance.AddDomainEvent(new DocumentTypeCreated(name));
        instance.AddDomainEvent(new DocumentTypeCreated0(regularExpression));
        return instance;
    }

    private static void Validate(DocumentType document)
    {
        if (document.Name.Length < 10)
            throw new ArgumentException($"{nameof(Name)} must have atleast 10 character!");
    }

    public string Name { get; set; }
    public string Identifier { get; protected set; }
    public string RegularExpression { get; protected set; }
    public bool DefaultDocumentType { get; protected set; }
    public bool Active { get; protected set; }
    public bool ZUGFeRdDocumentType { get; protected set; }

}
