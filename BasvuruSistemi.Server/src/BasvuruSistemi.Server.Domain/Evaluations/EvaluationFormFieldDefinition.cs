using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.Evaluations;
public sealed class EvaluationFormFieldDefinition
{
    public Guid Id { get; private set; }

    public Guid EvaluationFormId { get; private set; }
    public EvaluationForm EvaluationForm { get; private set; } = default!;

    public string Label { get; private set; } = default!; // Arayüzde gösterilecek etiket
    public string? Description { get; private set; }
    public FieldTypeEnum Type { get; private set; }
    public bool IsRequired { get; private set; }
    public int Order { get; private set; } // Alanların form üzerindeki sırasını belirlemek için eklendi
    public string? Placeholder { get; private set; } // Alan için örnek metin
    public string? OptionsJson { get; private set; } // Dropdown, RadioButton, Checkbox seçenekleri için JSON formatında


    public bool IsReadOnly { get; private set; } // Alanın sadece okunabilir olup olmadığını belirtir (örn: YÖKSİS'ten çekilen veri)
    public string? DefaultValue { get; private set; } // Alan için varsayılan değer

    public string? AllowedFileTypes { get; private set; } // Virgülle ayrılmış dosya uzantıları, örn: ".pdf,.jpg,.png"
    public int? MaxFileSizeMB { get; private set; }

    private EvaluationFormFieldDefinition() { }
    public EvaluationFormFieldDefinition(Guid evaluationFormId, string label, string? description, FieldTypeEnum type, bool isRequired, int order, string? placeholder, string? optionsJson, bool isReadOnly, string? defaultValue, string? allowedFileTypes, int? maxFileSizeMB)
    {
        Id = Guid.CreateVersion7();
        EvaluationFormId = evaluationFormId;
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
    }

    public void Update(string label, string? description, FieldTypeEnum type, bool isRequired, int order, string? placeholder, string? optionsJson, bool isReadOnly, string? defaultValue, string? allowedFileTypes, int? maxFileSizeMB)
    {
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
    }

    public void SetOrder(int order)
    {
        Order = order;
    }
}
