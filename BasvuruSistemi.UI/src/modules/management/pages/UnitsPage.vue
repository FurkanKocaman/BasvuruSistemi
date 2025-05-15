<script setup lang="ts">
import unitService from "../services/unit.service";
import { onMounted, ref } from "vue";
import { Unit } from "../models/unit-node.model";
import UnitNode from "../components/unit-components/UnitNode.vue";
import { buildUnitTree } from "../services/build-unit-tree";
import UnitCreateModal from "../modals/UnitCreateModal.vue";

const unListedUnits = ref<Unit[]>([]);
const units = ref<Unit[]>([]);

const selectedUnitId = ref("");
const selectedUnit = ref<Unit>({
  id: "",
  name: "",
});
const updatedUnit = ref<Unit | undefined>({
  id: "",
  name: "",
});

const isUnitCreateModalOpen = ref(false);

onMounted(() => {
  getUnits();
});

const getUnits = async () => {
  const res = await unitService.getUnitsByTenant();
  if (res) {
    unListedUnits.value = res;
    getUnitTree();
  }
};

const getUnitTree = () => {
  units.value = buildUnitTree(unListedUnits.value);
};

async function handleEdit(unitId: string) {
  updatedUnit.value = getUnitById(unitId);
  if (updatedUnit.value.parentId) selectedUnit.value = getUnitById(updatedUnit.value.parentId);
  isUnitCreateModalOpen.value = true;
}

async function handleRemove(unitId: string) {
  await unitService.deleteUnit(unitId);
  unListedUnits.value = unListedUnits.value.filter((p) => p.id != unitId);
  getUnitTree();
}

function handleAdd(unitId: string) {
  updatedUnit.value = undefined;
  selectedUnitId.value = unitId;
  selectedUnit.value = getUnitById(selectedUnitId.value);
  isUnitCreateModalOpen.value = true;
}

const getUnitById = (id: string) => {
  return unListedUnits.value.find((p) => p.id == id)!;
};

const closeUnitCreateodal = () => {
  isUnitCreateModalOpen.value = false;
};

const addUnit = () => {
  getUnits();
};
const updateUnit = (unit: Unit) => {
  const existingUnit = unListedUnits.value.find((p) => p.id == unit.id);
  if (existingUnit) {
    existingUnit.name = unit.name;
    existingUnit.parentId = unit.parentId;
    existingUnit.code = unit.code;
  }
  getUnitTree();
};
</script>
<template>
  <div class="flex pt-20">
    <UnitCreateModal
      v-if="isUnitCreateModalOpen"
      :parent-unit="selectedUnit"
      :unit="updatedUnit"
      @on-close="closeUnitCreateodal"
      @add-unit="addUnit"
      @update-unit="updateUnit"
    />
    <div class="flex-1">
      <UnitNode
        v-for="unit in units"
        :key="unit.id"
        :unit="unit"
        @edit="handleEdit"
        @add="handleAdd"
        @remove="handleRemove"
      />
    </div>
    <div class="flex-1"></div>
  </div>
</template>
