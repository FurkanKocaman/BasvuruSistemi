using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.FormFieldDefinitions;

public sealed class FormFieldDefinition : Entity
{
    public Guid TemplateId { get; private set; }
    public string Label { get; private set; } = default!;
    public string? Description { get; private set; }
    public FieldTypeEnum Type { get; private set; } 
    public bool IsRequired { get; private set; }
    public int Order { get; private set; } // Alanların form üzerindeki sırasını belirlemek için eklendi
    public string? Placeholder { get; private set; } // Alan için örnek metin
    public string? OptionsJson { get; private set; } // Dropdown, RadioButton, Checkbox seçenekleri için JSON formatında


    public bool IsReadOnly { get; private set; } // Alanın sadece okunabilir olup olmadığını belirtir (örn: YÖKSİS'ten çekilen veri)
    public string? DefaultValue { get; private set; } // Alan için varsayılan değer

    // Doğrulama ve Veri Çekme ile İlgili Özellikler
    public VerificationSourceEnum VerificationSource { get; private set; } // e.g., EDevlet, Yoksis
    public string? VerificationParametersJson { get; private set; }
    // Örn: YÖKSİS ALES Puanı için: { "ExamYearRequired": true, "ScoreTypeRequired": false }
    // Örn: YÖKSİS KPSS Puanı için: { "ExamYearRequired": true, "ScoreTypeRequired": true, "AllowedScoreTypes": ["KPSSP3", "KPSSP94"] }
    // Örn: EDevletVerifiedFile için: { "AllowedFileTypes": [".pdf"], "MaxFileSizeMB": 5, "VerificationEndpoint": "https://api.turkiye.gov.tr/belge-dogrulama/v1/..." }

    // Dosya Alanları İçin Ek Özellikler (Type == File || Type == EDevletVerifiedFile || Type == YoksisAlesDocument ise anlamlı)
    public string? AllowedFileTypes { get; private set; } // Virgülle ayrılmış dosya uzantıları, örn: ".pdf,.jpg,.png"
    public int? MaxFileSizeMB { get; private set; }

    // YÖKSİS'ten veri çekme ile ilgili ek konfigürasyon (Eğer FieldTypeEnum'da ayrıştırmadıysak)
    // public YoksisDataTypeEnum? YoksisDataType { get; private set; } // Hangi YÖKSİS verisinin çekileceği

    public ApplicationFormTemplate Template { get; private set; } = default!;

    private FormFieldDefinition() { }

    public FormFieldDefinition(
        Guid templateId,
        string label,
        FieldTypeEnum type,
        bool isRequired,
        int order,
        string? description = null,
        string? placeholder = null,
        string? optionsJson = null,
        bool isReadOnly = false,
        string? defaultValue = null,
        string? allowedFileTypes = null,
        int? maxFileSizeMB = null)
    {
        TemplateId = templateId;
        Label = label;
        Description = description;
        Type = type;
        IsRequired = isRequired;
        Order = order;
        Placeholder = placeholder;
        OptionsJson = optionsJson;
        IsReadOnly = isReadOnly;
        DefaultValue = defaultValue;
        AllowedFileTypes = allowedFileTypes;
        MaxFileSizeMB = maxFileSizeMB;

        if (Type == FieldTypeEnum.EDevletVerifiedFile || Type == FieldTypeEnum.YoksisAlesDocument)
        {
            if (string.IsNullOrWhiteSpace(AllowedFileTypes)) AllowedFileTypes = ".pdf";
            if (!MaxFileSizeMB.HasValue) MaxFileSizeMB = 10;
        }
        if (Type == FieldTypeEnum.YoksisAlesScore || Type == FieldTypeEnum.YoksisKpssScore)
        {
            IsReadOnly = true;
            VerificationSource = VerificationSourceEnum.Yoksis;
        }
        if (Type == FieldTypeEnum.EDevletVerifiedFile)
        {
            VerificationSource = VerificationSourceEnum.EDevlet;
        }
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    // Update metotları eklenebilir
}