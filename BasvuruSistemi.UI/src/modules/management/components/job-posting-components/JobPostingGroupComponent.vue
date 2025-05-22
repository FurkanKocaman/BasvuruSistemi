<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import { useDropdown } from "../../composables/useDropdown";
import { useRoute } from "vue-router";
import jobPostingService from "../../services/job-posting.service";
import { PostingGroupSummaryGetModel } from "../../models/posting-group-summaries.model";

const route = useRoute();
const postingGroupId = route.query.postingGroupId;

const postingGroups = ref<PostingGroupSummaryGetModel[]>([]);

const isGroup = ref(false);
const groupDropdown = useDropdown();

const props = defineProps<{
  groupId: string | undefined;
}>();

const emit = defineEmits<{
  (e: "groupIdSelected", groupId?: string): void;
}>();

onMounted(() => {
  getPostingGroups();
  if (postingGroupId) {
    isGroup.value = true;
    selectGroup(postingGroupId.toLocaleString());
  }
});

const getPostingGroups = async () => {
  const res = await jobPostingService.getPostingGroups();
  if (res) {
    postingGroups.value = res;
    if (postingGroupId || props.groupId) {
      var group = res.find((p) => p.id == postingGroupId || p.id == props.groupId);
      if (group) {
        groupDropdown.selectOption(group.name);
      }
    }
  }
};

const getPostingGroupByName = (name: string) => {
  const group = postingGroups.value.find((p) => p.name == name);
  if (group) {
    selectGroup(group.id);
  }
};

function selectGroup(id?: string) {
  emit("groupIdSelected", id);
}

watch(groupDropdown.selectedLabel, () => {
  getPostingGroupByName(groupDropdown.selectedLabel.value);
});
watch(isGroup, () => {
  selectGroup(undefined);
  groupDropdown.selectOption("");
});
</script>

<template>
  <!-- FormGroup start -->
  <div class="px-10 py-5">
    <div
      class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200 flex flex-col px-20"
    >
      <div class="my-3">
        <span class="text-lg dark:text-gray-200 text-gray-800">İlan Grubu</span>
      </div>
      <div class="mb-3">
        <label class="inline-flex items-center cursor-pointer">
          <input type="checkbox" v-model="isGroup" class="sr-only peer" />
          <div
            class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
          ></div>
          <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700">Gruba ekle</span>
        </label>

        <div class="flex w-full">
          <div
            v-if="isGroup"
            class="flex-1 flex w-full pr-3 border-r dark:border-gray-700 border-gray-200"
          >
            <div class="flex-1 flex flex-col my-2 items-start w-full">
              <label for="formTeplate" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                >İlan Grubu</label
              >
              <input
                type="text"
                name="formTeplate"
                id="formTeplate"
                :ref="groupDropdown.inputRef"
                v-model="groupDropdown.selectedLabel.value"
                @focus="groupDropdown.handleFocus"
                @blur="groupDropdown.handleBlur"
                readonly
                placeholder="İlanın ekleneceği grubu seçin..."
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
            </div>
            <div
              v-if="groupDropdown.isOpen.value"
              class="absolute w-fit mt-18 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
            >
              <div
                v-for="option in postingGroups"
                :key="option.name"
                @mousedown.prevent="groupDropdown.selectOption(option.name)"
                class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
              >
                {{ option.name }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- FormGroup end -->
</template>
