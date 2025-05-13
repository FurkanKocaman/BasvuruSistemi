<script setup lang="ts">
import { FormFieldDefinition } from "@/modules/management/models/form-field.model";
import { getComponentByFieldType } from "../services/getComponentByField.service";
import { ref } from "vue";

const values = ref([]);

const props = defineProps<{ fields: FormFieldDefinition[] }>();
</script>

<template>
  <div v-for="(field, index) in props.fields" :key="index">
    <component :is="getComponentByFieldType(field.type)" v-model="values[index]" :field="field" />
  </div>
</template>
