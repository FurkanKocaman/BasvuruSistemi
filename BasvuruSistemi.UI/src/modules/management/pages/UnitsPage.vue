<script setup lang="ts">
import unitService from "../services/unit.service";
import { onMounted, ref } from "vue";
import { Unit } from "../models/unit-node.model";
import UnitNode from "../components/unit-components/UnitNode.vue";
import { buildUnitTree } from "../services/build-unit-tree";
import UnitCreateModal from "../modals/UnitCreateModal.vue";
import ConfirmModal from "@/components/ConfirmModal.vue";
import { useTenantStore } from "@/stores/tenant";

const unListedUnits = ref<Unit[]>([]);
const units = ref<Unit[]>([]);

const tenantStore = useTenantStore();

const confirmModal = ref();

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
  selectedUnit.value = {
    id: "",
    name: "",
  };
  updatedUnit.value = getUnitById(unitId);
  if (updatedUnit.value.parentId) selectedUnit.value = getUnitById(updatedUnit.value.parentId);
  isUnitCreateModalOpen.value = true;
}

async function handleRemove(unitId: string) {
  const result = await confirmModal.value.open();
  if (result) {
    await unitService.deleteUnit(unitId);
    unListedUnits.value = unListedUnits.value.filter((p) => p.id != unitId);
    getUnitTree();
  }
}

function handleAdd(unitId: string) {
  selectedUnit.value = {
    id: "",
    name: "",
  };
  if (unitId !== tenantStore.tenantId) {
    selectedUnitId.value = unitId;
    selectedUnit.value = getUnitById(selectedUnitId.value);
  }
  updatedUnit.value = undefined;

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
  getUnits();
  getUnitTree();
};
</script>
<template>
  <div class="flex pt-20">
    <ConfirmModal
      ref="confirmModal"
      title="Bu birimi silmek istediğinize emin misiniz?"
      description="Bu işlem geri alınamaz"
    />
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
