<script setup lang="ts">
import { ref, watch } from 'vue';
import type { Page } from '@/assets/types/types.js';

interface Props {
    routeName: string;
    page: Page;
}

const props = defineProps<Props>();
const emit = defineEmits<{
    (emit: 'page-click', page: number): void;
}>();
const pages = ref<number[]>([]);

function onPageClick(page: number) {
    const lastElement = pages.value.slice(-1)[0];
    const firstElement = pages.value[0];
    if (page == lastElement && page < props.page.totalPages) {
        pages.value.shift();
        pages.value.push(page + 1);
    }
    if (page == firstElement && page != 1) {
        pages.value.pop();
        pages.value.unshift(firstElement - 1);
    }
    emit('page-click', page);
}

watch(
    () => props.page.totalPages,
    (newTotalPages: number) => {
        const currentPage = props.page.currentPage;
        const visiblePages = props.page.pageSize;
        let totalPages = newTotalPages;
        if (currentPage >= visiblePages && currentPage < totalPages) {
            const leftShift = currentPage - visiblePages;
            for (let i = 2 + leftShift; i <= currentPage + 1; i++) {
                pages.value.push(i);
            }
        } else if (currentPage == totalPages) {
            const leftShift = currentPage - visiblePages;
            for (let i = 1 + leftShift; i <= currentPage; i++) {
                pages.value.push(i);
            }
        } else {
            const rightmostPage = visiblePages < totalPages ? visiblePages : totalPages;
            for (let i = 1; i <= rightmostPage; i++) {
                pages.value.push(i);
            }
        }
    }
);
</script>
<template>
    <!-- Pagination -->
    <div class="flex flex-row">
        <div class="flex">
            <router-link
                tag="button"
                v-for="page in pages"
                :to="{ name: props.routeName, params: { page: page } }"
                :class="{ 'border border-indigo-300 bg-indigo-50': page === props.page.currentPage }"
                @click="onPageClick(page)"
                class="bg-indigo-100 py-1 px-3"
            >
                {{ page }}
            </router-link>
        </div>
    </div>
</template>
