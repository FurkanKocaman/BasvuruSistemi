import { ref, computed } from "vue";

export function useDropdown(initialValue = "") {
  const isOpen = ref(false);
  const _selectedLabel = ref(initialValue);
  const inputRef = ref<HTMLInputElement | null>(null);

  const selectedLabel = computed({
    get: () => _selectedLabel.value,
    set: (val) => {
      _selectedLabel.value = val;
      isOpen.value = false;
    },
  });

  function selectOption(label: string) {
    selectedLabel.value = label;

    inputRef.value?.blur();
  }

  function handleFocus() {
    isOpen.value = true;
  }

  function handleBlur() {
    setTimeout(() => {
      isOpen.value = false;
    }, 200);
  }

  return {
    isOpen,
    selectedLabel,
    inputRef,
    selectOption,
    handleFocus,
    handleBlur,
  };
}
