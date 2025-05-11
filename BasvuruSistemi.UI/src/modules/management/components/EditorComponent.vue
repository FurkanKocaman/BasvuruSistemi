<script setup lang="ts">
import { Editor } from "@tiptap/vue-3";
import { EditorContent } from "@tiptap/vue-3";
import StarterKit from "@tiptap/starter-kit";
import Underline from "@tiptap/extension-underline";
import TextAlign from "@tiptap/extension-text-align";
import Color from "@tiptap/extension-color";
import Highlight from "@tiptap/extension-highlight";
import { onBeforeUnmount, onMounted, ref } from "vue";
import {
  Bold,
  Italic,
  Underline as UnderlineIcon,
  AlignLeft,
  AlignCenter,
  AlignRight,
} from "lucide-vue-next";

const editor = ref<Editor | null>(null);

onMounted(() => {
  createEditor();
});

defineProps<{ modelValue: string; label: string }>();
const emit = defineEmits<{
  (e: "update:modelValue", value: string): void;
}>();

const createEditor = () => {
  editor.value = new Editor({
    content: "",
    extensions: [
      StarterKit,
      Underline,
      Color,
      Highlight,
      TextAlign.configure({
        types: ["heading", "paragraph"],
      }),
    ],
    editorProps: {
      attributes: {
        class: "focus:outline-none w-full prose max-w-none dark:prose-invert min-h-[200px] p-4",
      },
    },
    onUpdate({ editor }) {
      emit("update:modelValue", editor.getHTML());
    },
  });
};

onBeforeUnmount(() => {
  editor.value?.destroy();
});

const commands = [
  {
    icon: Bold,
    action: () => editor.value?.chain().focus().toggleBold().run(),
    isActive: () => editor.value?.isActive("bold"),
  },
  {
    icon: Italic,
    action: () => editor.value?.chain().focus().toggleItalic().run(),
    isActive: () => editor.value?.isActive("italic"),
  },
  {
    icon: UnderlineIcon,
    action: () => editor.value?.chain().focus().toggleUnderline().run(),
    isActive: () => editor.value?.isActive("underline"),
  },
  {
    icon: AlignLeft,
    action: () => editor.value?.chain().focus().setTextAlign("left").run(),
    isActive: () => editor.value?.isActive({ textAlign: "left" }),
  },
  {
    icon: AlignCenter,
    action: () => editor.value?.chain().focus().setTextAlign("center").run(),
    isActive: () => editor.value?.isActive({ textAlign: "center" }),
  },
  {
    icon: AlignRight,
    action: () => editor.value?.chain().focus().setTextAlign("right").run(),
    isActive: () => editor.value?.isActive({ textAlign: "right" }),
  },
];
</script>

<template>
  <div class="flex flex-col mt-5">
    <label for="editor" class="w-full text-md my-1 dark:text-gray-300 text-gray-800">{{
      label
    }}</label>
    <div
      class="dark:bg-gray-900/50 bg-gray-50 border rounded-md dark:border-gray-700 border-gray-200"
    >
      <div class="flex flex-wrap gap-2 p-2 border-b dark:border-gray-700 border-gray-200">
        <button
          v-for="(cmd, index) in commands"
          :key="index"
          @click="cmd.action"
          :class="[
            'p-1 rounded-md hover:bg-gray-200 dark:hover:bg-gray-700',
            cmd.isActive?.() ? 'bg-blue-500 text-white' : 'text-gray-700 dark:text-gray-300',
          ]"
        >
          <component :is="cmd.icon" class="w-5 h-5" />
        </button>
      </div>

      <EditorContent v-if="editor" :editor="editor" class="overflow-x-auto" />
    </div>
  </div>
</template>
