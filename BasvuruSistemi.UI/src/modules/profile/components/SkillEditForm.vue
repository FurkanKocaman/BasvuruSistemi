<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { Skill } from "../models/profile.model";
import { SkillCreateModel } from "../models/skill-create.model";
import profileService from "../services/profile.service";
import { SkillLeveloptions } from "@/models/constants/skill-level";

const props = defineProps<{
  modelValue: boolean;
  skill?: Skill;
}>();

const emit = defineEmits(["update:modelValue", "save"]);

const isEditing = ref(false);

const formData = ref<SkillCreateModel>({
  id: undefined,
  name: "",
  description: undefined,
  level: 0,
});

onMounted(() => {
  loadFormData();
});

watch(
  () => props.skill,
  () => {
    loadFormData();
  },
  { deep: true }
);

const loadFormData = () => {
  if (props.skill) {
    isEditing.value = true;
    formData.value = {
      id: props.skill.id,
      name: props.skill.name,
      description: props.skill.description,
      level: props.skill.level,
    };
  } else {
    isEditing.value = false;
    formData.value = {
      id: undefined,
      name: "",
      description: undefined,
      level: 0,
    };
  }
};

// Yetenek bilgilerini kaydet
const saveSkill = async () => {
  if (props.skill) {
    await profileService.updateSkill(formData.value);
    const skill: Skill = {
      id: formData.value.id!,
      name: formData.value.name,
      description: formData.value.description,
      level: formData.value.level,
    };
    onSave(skill);
    onClose();
  } else {
    const res = await profileService.createSkill(formData.value);
    onSave(res);
    onClose();
  }
};

function onClose() {
  emit("update:modelValue", false);
}

function onSave(skill: Skill) {
  emit("save", skill);
}
</script>

<template>
  <div
    class="fixed inset-0 bg-gradient-to-br from-blue-500/20 to-purple-600/30 backdrop-blur-sm flex items-center justify-center z-50"
    v-if="modelValue"
  >
    <div
      class="bg-white dark:bg-gray-800 rounded-lg shadow-xl w-full max-w-lg max-h-[90vh] overflow-y-auto"
    >
      <div
        class="p-6 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center"
      >
        <h3 class="text-xl font-semibold text-gray-800 dark:text-white">
          {{ isEditing ? "Yetenek Düzenle" : "Yeni Yetenek Ekle" }}
        </h3>
        <button
          @click="$emit('update:modelValue', false)"
          class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-6 w-6"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
        </button>
      </div>

      <div class="p-6">
        <form @submit.prevent="saveSkill">
          <div class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Yetenek Adı
              </label>
              <input
                type="text"
                v-model="formData.name"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Yetenek adını giriniz"
                required
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Seviye
              </label>
              <select
                v-model="formData.level"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                required
              >
                <option v-for="level in SkillLeveloptions" :key="level.label" :value="level.value">
                  {{ level.label }}
                </option>
              </select>
            </div>
          </div>

          <div class="mt-6 flex justify-end space-x-3">
            <button
              type="button"
              @click="$emit('update:modelValue', false)"
              class="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700"
            >
              İptal
            </button>
            <button
              type="submit"
              class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors"
            >
              Kaydet
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
