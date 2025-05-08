<template>
  <span 
    :class="[
      'px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full',
      statusClasses[status]
    ]"
  >
    {{ statusText[status] }}
  </span>
</template>

<script setup lang="ts">
import { defineProps, defineComponent } from 'vue';

// TypeScript için Vue bileşeni tanımlama
defineComponent({
  name: 'StatusBadge'
});

// Props tanımlaması
defineProps<{
  status: 'Pending' | 'Approved' | 'Rejected'
}>();

// Durum metinleri
const statusText: Record<string, string> = {
  'Pending': 'Beklemede',
  'Approved': 'Onaylandı',
  'Rejected': 'Reddedildi'
};

// Durum sınıfları
const statusClasses: Record<string, string> = {
  'Pending': 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300',
  'Approved': 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300',
  'Rejected': 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300'
};
</script>
