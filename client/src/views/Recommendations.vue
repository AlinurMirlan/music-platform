<script setup lang="ts">
import { onMounted, reactive, ref, watch } from 'vue';
import type { Genre, Song, PagedSongs, Page } from '@/assets/types/types.js';
import Pagination from '@/components/Pagination.vue';
import MusicCard from '@/components/MusicCard.vue';
import LoadSpinner from '@/components/LoadSpinner.vue';
import { useAccountStore } from '@/stores/account';
import { getJwtConfiguredAxios } from '@/assets/axios.js';
import Button from '@/components/Button.vue';

const accountStore = useAccountStore();
const axios = getJwtConfiguredAxios(accountStore.jwt.token);
const props = defineProps<{
    page: number;
}>();
const genreDialog = ref<HTMLDialogElement | null>(null);
let loading = ref<boolean>(false);
let genres = ref<Genre[]>([]);
let songs = ref<Song[]>([]);
const search = {
    gennreIds: ref<number[]>([]),
    term: ref<string>('')
}
const page: Page = reactive({
    currentPage: props.page,
    pageSize: 6,
    totalPages: -1
});

function onDialogReset() {
    search.gennreIds.value = [];
}
function onDialogOpen() {
    genreDialog.value?.showModal();
}
function onDialogClose() {
    genreDialog.value?.close();
}
function onGenreChecked(genre: Genre) {
    const index = search.gennreIds.value.indexOf(genre.id);
    if (index !== -1) {
        search.gennreIds.value.splice(index, 1);
    } else {
        search.gennreIds.value.push(genre.id);
    }
}
async function setSongsAndImages(responseData: PagedSongs) {
    const imagelessSongs = responseData.results;
    // On a note: array.forEach() doesn't wait for an async operation to finish.
    for (const song of imagelessSongs) {
        try {
            const response = await axios.get(`/music/image/${song.id}`, { responseType: 'blob' });
            const blob = response.data;
            song.imageFile = URL.createObjectURL(blob);
        } catch (error) {
            console.log(error);
        }
    }

    songs.value = imagelessSongs;
}
function updatePage(newPage: number) {
    page.currentPage = newPage;
}
async function getSongs() {
    try {
        let response = await axios.get<PagedSongs>(
            `/music/${encodeURIComponent(search.term.value)}`,
            {
                params: {
                    genreIds: search.gennreIds.value,
                    page: page.currentPage,
                    pageSize: page.pageSize
                },
                paramsSerializer: { indexes: null }
            }
        );

        const pagedSongs = response.data;
        page.totalPages = pagedSongs.totalPages;
        await setSongsAndImages(pagedSongs);
    } catch (error) {
        console.log(error);
    }
}
async function getGenres() {
    try {
        const response = await axios.get<Genre[]>('/music/genres');
        const genresData = response.data;
        genres.value = genresData;
    } catch (error) {
        console.log(error);
    }
}
async function onPlayMusic(song: Song) {
    try {
        const response = await axios.get(`/music/song/${song.id}`, { responseType: 'blob' });
        const blob = response.data;
        song.songFile = URL.createObjectURL(blob);
    } catch (error) {
        console.log(error);
    }
}

watch(
    () => page.currentPage,
    async () => {
        songs.value.length = 0;
        loading.value = true;
        await getSongs();
        loading.value = false;
    }
);

onMounted(async () => {
    loading.value = true;
    await getGenres();
    await getSongs();
    loading.value = false;
});
</script>

<template>
    <main id="index" class="h-full p-5 flex-grow flex flex-col">
        <!-- Control panel -->
        <section class="flex flex-row gap-x-3">
            <!-- Search panel -->
            <div class="flex text-[0px]">
                <form @submit.prevent="getSongs" class="">
                    <Button class="text-base h-full" name="Search" @click="getSongs"></Button>
                    <input
                        v-model="search.term.value" 
                        class="h-full text-base py-1 px-2 outline-none focus:outline-none bg-slate-100"
                        type="text"
                        name="searchTerm"
                        placeholder="Title, Author(s), Album"
                    />
                </form>
            </div>
            <!-- Genre dialog -->
            <dialog ref="genreDialog" id="genreDialog" class="max-w-lg">
                <form action="">
                    <div class="flex flex-row flex-wrap gap-2">
                        <!-- Genre tag -->
                        <label
                            v-for="genre in genres"
                            :for="genre.id.toString()"
                            :class="{
                                'bg-indigo-400 border-white text-slate-100': search.gennreIds.value.includes(genre.id)
                            }"
                            class="border border-indigo-300 rounded-lg py-1 px-2 select-none"
                        >
                            <input
                                type="checkbox"
                                name="checkbox"
                                :id="genre.id.toString()"
                                v-on:change="onGenreChecked(genre)"
                                class="hidden"
                            />
                            {{ genre.name }}
                        </label>
                    </div>
                    <div class="flex flex-row space-x-2 mt-3">
                        <Button
                            name="Reset"
                            @click="onDialogReset"
                            class="text-base px-3 !bg-red-500"
                        />
                        <Button
                            name="Ok"
                            @click="onDialogClose"
                            class="text-base px-3 py-1"
                        />
                    </div>
                </form>
            </dialog>
            <Button
                name="Genres"
                id="genreDialogButton"
                @click="onDialogOpen"
                class="px-3"
            />
        </section>
        <div class="w-full h-[1px] bg-teal-300 mb-4 mt-2"></div>
        <!-- Recommendation page -->
        <section class="flex flex-col justify-between flex-grow">
            <LoadSpinner v-if="loading" />
            <!-- Recommended songs -->
            <div class="flex flex-row flex-wrap gap-2">
                <MusicCard v-for="song in songs" :song="song" @play-click="onPlayMusic" />
            </div>
            <Pagination route-name="recommendations" :page="page" @page-click="updatePage" />
        </section>
    </main>
</template>
