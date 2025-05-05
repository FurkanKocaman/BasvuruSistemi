using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;


namespace BasvuruSistemi.Server.Domain.ApplicationFieldValues;
public sealed class ApplicationFieldValue : Entity
{
    public Guid ApplicationId { get; private set; }
    public Guid FieldDefinitionId { get; private set; }
    public string Value { get; private set; } = default!;   // Metin, sayı, tarih veya dosya yolu / JSON

    public Application Application { get; private set; } = default!;
    public FormFieldDefinition FieldDefinition { get; private set; } = default!;

    private ApplicationFieldValue() { }
    public ApplicationFieldValue(Guid applicationId, Guid fieldDefinitionId, string value)
    {
        ApplicationId = applicationId;
        FieldDefinitionId = fieldDefinitionId;
        Value = value;
    }
}
