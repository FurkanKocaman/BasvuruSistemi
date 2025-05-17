<script setup lang="ts">
import { type PropType } from "vue";
import { useTenantStore } from "@/stores/tenant";
import { Unit } from "../../models/unit-node.model";

const tenantStore = useTenantStore();

const props = defineProps({
  unit: {
    type: Object as PropType<Unit>,
    required: true,
  },
});
</script>

<template>
  <div class="pl-4">
    <div
      class="flex items-center justify-between gap-2 w-full border rounded-md dark:border-gray-800 border-gray-200 text-gray-800 dark:text-gray-200 text-md font-semibold my-3 py-3 px-2 cursor-pointer hover:bg-gray-400/10 select-none"
      @click="
        () => {
          if (tenantStore.tenantId != props.unit.id) $emit('edit', unit.id);
        }
      "
    >
      <span>{{ unit.name }}</span>
      <div v-if="tenantStore.tenantId != unit.id" class="flex items-center">
        <button @click.stop="$emit('add', unit.id)" class="group cursor-pointer mr-3">
          <svg class="size-6 fill-none" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <circle
              cx="12"
              cy="12"
              r="10"
              class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
              stroke-width="2"
            />
            <path
              d="M15 12L12 12M12 12L9 12M12 12L12 9M12 12L12 15"
              class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
              stroke-width="2"
              stroke-linecap="round"
            />
          </svg>
        </button>
        <button @click.stop="$emit('edit', unit.id)" class="group cursor-pointer mr-3">
          <svg
            class="size-6 dark:fill-gray-400 fill-gray-600 group-hover:fill-blue-600 dark:group-hover:fill-blue-600"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M18.111,2.293,9.384,11.021a.977.977,0,0,0-.241.39L8.052,14.684A1,1,0,0,0,9,16a.987.987,0,0,0,.316-.052l3.273-1.091a.977.977,0,0,0,.39-.241l8.728-8.727a1,1,0,0,0,0-1.414L19.525,2.293A1,1,0,0,0,18.111,2.293ZM11.732,13.035l-1.151.384.384-1.151L16.637,6.6l.767.767Zm7.854-7.853-.768.767-.767-.767.767-.768ZM3,5h8a1,1,0,0,1,0,2H4V20H17V13a1,1,0,0,1,2,0v8a1,1,0,0,1-1,1H3a1,1,0,0,1-1-1V6A1,1,0,0,1,3,5Z"
            />
          </svg>
        </button>
        <button @click.stop="$emit('remove', unit.id)" class="cursor-pointer">
          <svg
            class="size-6 group"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M20.5001 6H3.5"
              class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
              stroke-width="2"
              stroke-linecap="round"
            />
            <path
              d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
              class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
              stroke-width="2"
              stroke-linecap="round"
            />
            <path
              d="M9.5 11L10 16"
              class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
              stroke-width="2"
              stroke-linecap="round"
            />
            <path
              d="M14.5 11L14 16"
              class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
              stroke-width="2"
              stroke-linecap="round"
            />
            <path
              d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65651 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
              class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
              stroke-width="2"
            />
          </svg>
        </button>
      </div>
    </div>

    <UnitNode
      v-for="child in unit.children"
      :key="child.id"
      :unit="child"
      @add="$emit('add', $event)"
      @edit="$emit('edit', $event)"
      @remove="$emit('remove', $event)"
    />
  </div>
</template>
