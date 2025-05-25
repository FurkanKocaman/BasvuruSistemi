<script setup lang="ts">
import { computed } from 'vue';

interface TableColumn {
  key: string;
  label: string;
  formatter?: (value: unknown) => string;
}

interface TableProps {
  title: string;
  data: Record<string, unknown>[];
  columns: TableColumn[];
  loading?: boolean;
}

const props = defineProps<TableProps>();

const formattedData = computed(() => {
  return props.data.map(item => {
    const formattedItem: Record<string, unknown> = {};
    
    props.columns.forEach(column => {
      const value = item[column.key as keyof typeof item];
      formattedItem[column.key] = column.formatter ? column.formatter(value) : value;
    });
    
    return formattedItem;
  });
});
</script>

<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden mb-6">
    <div class="p-4 border-b border-gray-200 dark:border-gray-700">
      <h3 class="text-lg font-medium text-gray-800 dark:text-white">{{ title }}</h3>
    </div>
    
    <div class="overflow-x-auto">
      <table class="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
        <thead class="bg-gray-50 dark:bg-gray-700">
          <tr>
            <th
              v-for="column in columns"
              :key="column.key"
              scope="col"
              class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
            >
              {{ column.label }}
            </th>
          </tr>
        </thead>
        
        <tbody v-if="!loading" class="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
          <tr v-for="(item, index) in formattedData" :key="index" class="hover:bg-gray-50 dark:hover:bg-gray-700">
            <td
              v-for="column in columns"
              :key="column.key"
              class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-gray-200"
            >
              {{ item[column.key] }}
            </td>
          </tr>
          
          <tr v-if="formattedData.length === 0">
            <td :colspan="columns.length" class="px-6 py-4 text-center text-sm text-gray-500 dark:text-gray-400">
              Veri bulunamadı
            </td>
          </tr>
        </tbody>
        
        <tbody v-else class="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
          <tr v-for="i in 5" :key="i">
            <td
              v-for="column in columns"
              :key="column.key"
              class="px-6 py-4 whitespace-nowrap"
            >
              <div class="h-4 bg-gray-200 dark:bg-gray-600 rounded animate-pulse"></div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
