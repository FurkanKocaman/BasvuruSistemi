<script setup lang="ts">
import { onMounted, ref, Ref } from "vue";
import { Unit } from "../models/unit-node.model";
import { UnitCreateModel } from "../models/unit-create.model";
import unitService from "../services/unit.service";

const emit = defineEmits(["onClose", "addUnit", "updateUnit"]);

const errorMessages = ref<string[]>([]);

const props = defineProps<{
  parentUnit: Unit | undefined;
  unit: Unit | undefined;
}>();

onMounted(() => {
  if (props.unit) {
    request.value.name = props.unit.name;
    request.value.code = props.unit.code;
  }
});

const request: Ref<UnitCreateModel> = ref({
  id: undefined,
  parentId: undefined,
  name: "",
  code: undefined,
});

const createUnit = async () => {
  if (props.parentUnit && props.parentUnit.id.trim() != "") {
    request.value.parentId = props.parentUnit.id;
  } else {
    request.value.parentId = undefined;
  }
  if (request.value.name.trim() !== "") {
    const res = await unitService.createUnit(request.value);
    if (res) {
      emit("addUnit");
      closeModal();
    }
  } else {
    addError("Ad kısmı boş olamaz");
  }
};

const addError = (error: string) => {
  if (!errorMessages.value.includes(error)) {
    errorMessages.value.push(error);
  }
};
const updateUnit = async () => {
  if (props.unit) {
    request.value.id = props.unit.id;

    if (props.parentUnit) request.value.parentId = props.parentUnit.id;
    const res = await unitService.updateUnit(props.unit?.id, request.value);
    if (res) {
      const createdUnit: Unit = {
        id: request.value.id,
        name: request.value.name,
        code: request.value.code,
        parentId: request.value.parentId,
      };
      emit("updateUnit", createdUnit);
      closeModal();
    }
  }
};

function closeModal() {
  emit("onClose");
}
</script>
<template>
  <div
    class="fixed top-0 z-[99] flex justify-center items-center w-full h-full bg-gray-500/60 dark:bg-neutral-900/80 backdrop-blur-md"
  >
    <div class="rounded-md bg-gray-50 dark:bg-gray-900 min-w-[40rem] px-5 py-8">
      <div class="flex flex-col justify-center w-full mb-3">
        <span
          v-for="error in errorMessages"
          :key="error"
          class="text-center border rounded-md border-red-600 text-red-600 py-1"
          >{{ error }}</span
        >
      </div>
      <span class="text-xl dark:text-gray-100 font-semibold">{{
        props.unit ? "Birim Güncelle" : "Birim Oluştur"
      }}</span>

      <div class="flex flex-row mt-5">
        <div class="flex flex-col w-full">
          <div v-if="props.parentUnit" class="flex flex-col mb-3">
            <label for="tenantName" class="text-sm dark:text-gray-400 text-gray-700 mb-1.5"
              >Üst Birim</label
            >
            <input
              type="text"
              name="tenantName"
              id="tenantName"
              :value="props.parentUnit.name"
              autocomplete="off"
              placeholder="Birim1"
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm text-gray-800 dark:text-gray-200 border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              readonly
              required
            />
          </div>
          <div class="flex flex-col mb-3">
            <label for="tenantName" class="text-sm dark:text-gray-400 text-gray-700 mb-1.5"
              >Ad</label
            >
            <input
              type="text"
              name="tenantName"
              id="tenantName"
              v-model="request.name"
              autocomplete="off"
              placeholder="Birim1"
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm text-gray-800 dark:text-gray-200 border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            />
          </div>
          <div class="flex flex-col mb-3">
            <label for="tenantCode" class="text-sm dark:text-gray-400 text-gray-700 mb-1.5"
              >Kod</label
            >
            <input
              type="text"
              name="tenantCode"
              id="tenantCode"
              v-model="request.code"
              autocomplete="off"
              placeholder="BRM"
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm text-gray-800 dark:text-gray-200 border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            />
          </div>
          <div class="flex flex-row justify-end mt-5">
            <button
              class="px-2 py-1 border rounded-md border-gray-200 dark:border-gray-700 text-gray-700 dark:text-gray-300 cursor-pointer hover:bg-gray-400/10 mr-5 text-sm"
              @click="closeModal()"
            >
              İptal
            </button>
            <button
              class="px-2 py-1 border rounded-md border-blue-600 dark:border-blue-700 text-gray-700 dark:text-gray-300 cursor-pointer hover:bg-blue-600 hover:text-white text-sm"
              @click="
                () => {
                  if (props.unit) {
                    updateUnit();
                  } else {
                    createUnit();
                  }
                }
              "
            >
              {{ props.unit ? "Güncelle" : "Oluştur" }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
