

namespace OneClickSolutions.Application.DocumentTypes.ViewModels
{
    public class AddDocumentTypeViewModel
    {
        public string Name { get; set; }
        public string Identifier { get;  set; }
        public string RegularExpression { get; set; }
        public bool DefaultDocumentType { get; set; }
        public bool Active { get; set; }
        public bool ZUGFeRdDocumentType { get; set; }
    }
}
