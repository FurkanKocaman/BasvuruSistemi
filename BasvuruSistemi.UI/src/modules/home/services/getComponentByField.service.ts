import CheckboxField from "../components/form-components/CheckboxField.vue";
import DatePickerInput from "../components/form-components/DatePickerInput.vue";
import DropdownInput from "../components/form-components/DropdownInput.vue";
import FileInput from "../components/form-components/FileInput.vue";
import RadioInput from "../components/form-components/RadioInput.vue";
import TextareaInput from "../components/form-components/TextareaInput.vue";
import TextBoxInput from "../components/form-components/TextBoxInput.vue";

export const getComponentByFieldType = (type: number) => {
  switch (type) {
    case 1:
      return TextBoxInput;
    case 2:
      return TextareaInput;
    case 3:
      return CheckboxField;
    case 4:
      return RadioInput;
    case 5:
      return DropdownInput;
    case 6:
      return FileInput;
    case 7:
      return DatePickerInput;
    case 8:
      return TextBoxInput;
    case 9:
      return TextBoxInput;
    case 10:
      return TextBoxInput;
    case 11:
      return TextBoxInput;
    case 12:
      return TextBoxInput;
    case 13:
      return FileInput;
    case 14:
      return TextBoxInput;
    case 15:
      return FileInput;
    case 16:
      return FileInput;
    case 17:
      return TextBoxInput;
    case 18:
      return TextBoxInput;
  }
};
