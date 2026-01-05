<template>
  <button
    :class="[
      baseClasses,
      variantClasses,
      sizeClasses,
      {
        'w-full': block,
        'opacity-50 cursor-not-allowed': disabled || loading,
      },
    ]"
    :disabled="disabled || loading"
    @click="handleClick"
  >
    <span v-if="loading" class="animate-spin mr-2 w-4 h-4 border-2 border-t-transparent rounded-full border-current" />
    <slot name="icon" v-if="!loading && hasIcon" />
    <span v-if="!loading">
      <slot />
    </span>
  </button>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, computed } from 'vue'

const props = defineProps({
  variant: {
    type: String as () => 'primary' | 'secondary' | 'danger' | 'outline',
    default: 'primary',
  },
  size: {
    type: String as () => 'sm' | 'md' | 'lg',
    default: 'md',
  },
  disabled: {
    type: Boolean,
    default: false,
  },
  loading: {
    type: Boolean,
    default: false,
  },
  block: {
    type: Boolean,
    default: false,
  },
  hasIcon: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['click'])

const handleClick = (e: MouseEvent) => {
  if (!props.disabled && !props.loading) {
    emit('click', e)
  }
}

const baseClasses = 'inline-flex items-center justify-center font-medium rounded-md transition-colors duration-200 focus:outline-none dark:focus:ring-white focus:ring-2 focus:ring-offset-2'

const variantClasses = computed(() => {
  const common = 'focus:ring-offset-2'

  return {
    primary: `bg-blue-600 text-white hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 ${common}`,
    secondary: `bg-gray-100 text-gray-800 hover:bg-gray-200 dark:bg-gray-700 dark:text-white dark:hover:bg-gray-600 ${common}`,
    danger: `bg-red-600 text-white hover:bg-red-700 dark:bg-red-500 dark:hover:bg-red-600 ${common}`,
    outline: `border border-gray-300 text-gray-700 hover:bg-gray-50 dark:border-gray-500 dark:text-white dark:hover:bg-gray-700 ${common}`,
  }[props.variant]
})

const sizeClasses = computed(() => {
  return {
    sm: 'px-3 py-1.5 text-sm',
    md: 'px-4 py-2 text-base',
    lg: 'px-5 py-3 text-lg',
  }[props.size]
})
</script>
