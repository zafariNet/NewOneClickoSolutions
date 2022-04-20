using OneClickSolutions.Infrastructure.Cqrs.Commands;

namespace OneClickSolutions.Application.DocumentTypes.Commands
{
    public class CreateDocumentTypeCommand:ICommand
    {
        public CreateDocumentTypeCommand(string name, string regularExpression, bool defaultDocumentType, bool active, bool zUGFeRdDocumentType)
        {
            Name = name;
            RegularExpression = regularExpression;
            DefaultDocumentType = defaultDocumentType;
            Active = active;
            ZUGFeRdDocumentType = zUGFeRdDocumentType;
        }
        public string Name { get; set; }
        public string Identifier { get; protected set; }
        public string RegularExpression { get; protected set; }
        public bool DefaultDocumentType { get; protected set; }
        public bool Active { get; protected set; }
        public bool ZUGFeRdDocumentType { get; protected set; }
    }
}
