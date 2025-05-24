using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.Evaluations;
public sealed class EvaluationFormFieldDefinition
{
    public Guid Id { get; private set; }

    public Guid EvaluationFormId { get; private set; }
    public EvaluationForm EvaluationForm { get; private set; } = default!;

    public string Label { get; private set; } = default!; // Arayüzde gösterilecek etiket
    public FieldTypeEnum FieldType { get; private set; } // Enum: TextBox, TextArea, Dropdown, Checkbox, RadioButton, Number, Date etc.
    public string? Options { get; private set; } // JSON formatında seçenekler (Dropdown, RadioButton için)
    public bool IsRequired { get; private set; } = false;
    public int Order { get; private set; } // Form içindeki gösterim sırası
    public string? Placeholder { get; private set; }
    public string? HelpText { get; private set; }
    public string? ValidationRules { get; private set; } // JSON formatında validasyon kuralları (regex, minLength, maxLength vb.)

    private EvaluationFormFieldDefinition() { }
    public EvaluationFormFieldDefinition(Guid evaluationFormId, string label, FieldTypeEnum fieldType,
        string? options = null, bool isRequired = false, int order = 0, string? placeholder = null,
        string? helpText = null, string? validationRules = null)
    {
        Id = Guid.CreateVersion7();
        EvaluationFormId = evaluationFormId;
        Label = label;
        FieldType = fieldType;
        Options = options;
        IsRequired = isRequired;
        Order = order;
        Placeholder = placeholder;
        HelpText = helpText;
        ValidationRules = validationRules;
    }
}
