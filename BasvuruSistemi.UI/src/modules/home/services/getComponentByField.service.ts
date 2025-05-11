import FileInput from "../components/form-components/FileInput.vue";
import TextareaInput from "../components/form-components/TextareaInput.vue";
import TextBoxInput from "../components/form-components/TextBoxInput.vue";

export const getComponentByFieldType = (type: number) => {
  switch (type) {
    case 1:
      return TextBoxInput;
    case 2:
      return TextareaInput;
    case 6:
      return FileInput;
  }
};
