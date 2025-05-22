import { computed, Ref } from "vue";

type PageItem =
  | { type: "page"; page: number; label: number; key: string }
  | { type: "ellipsis"; key: string; label: "..." };

export function useVisiblePages(totalPages: Ref<number>, currentPage: Ref<number>) {
  const visiblePages = computed<PageItem[]>(() => {
    const pages: PageItem[] = [];
    const total = totalPages.value;
    const current = currentPage.value;

    const addPage = (page: number) => {
      pages.push({ type: "page", page, label: page, key: `page-${page}` });
    };

    const addEllipsis = (key: string) => {
      pages.push({ type: "ellipsis", key, label: "..." });
    };

    if (total <= 6) {
      for (let i = 1; i <= total; i++) {
        addPage(i);
      }
    } else {
      if (current <= 3) {
        // Başta
        for (let i = 1; i <= 3; i++) addPage(i);
        addEllipsis("mid");
        addPage(total);
      } else if (current >= total - 2) {
        // Sonda
        addPage(1);
        addEllipsis("mid");
        for (let i = total - 2; i <= total; i++) addPage(i);
      } else {
        // Ortada
        addPage(1);
        addEllipsis("start");
        addPage(current - 1);
        addPage(current);
        addPage(current + 1);
        addEllipsis("end");
        addPage(total);
      }
    }

    return pages;
  });

  return visiblePages;
}
