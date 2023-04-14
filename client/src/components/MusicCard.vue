<script setup lang="ts">
import type { Song } from '@/assets/types/types.js';
import Button from './Button.vue';
defineEmits<{
    (emit: "play-click", song: Song): void
}>();
defineProps<{
    song: Song;
}>();

function nameosOfAuthors(song: Song) {
    return song.authors.map((a) => a.nickname).join(', ');
}
</script>

<template>
    <div class="max-w-[460px] text-indigo-950">
        <div class="border border-indigo-300 rounded-sm pt-3 px-3 pb-2">
            <div class="flex h-28 max-w-max">
                <img class="" :src="song.imageFile" alt="Song cover" />
                <div class="flex flex-col justify-between px-3">
                    <div class="flex flex-row justify-between">
                        <p class="font-bold">
                            {{ nameosOfAuthors(song) }}<br />
                            <span class="text-sm">Album: {{ song.album }}</span>
                        </p>
                        <span class="font-bold inline-block pl-1">{{ song.title }}</span>
                    </div>
                    <audio class="" :src="song.songFile" controls>Audio is not supported.</audio>
                </div>
            </div>
            <div class="mt-2 flex flex-wrap items-center gap-1">
                Genres:
                <span
                    v-for="genre in song.genres"
                    class="inline-block bg-indigo-400 text-slate-100 rounded-lg p-1 px-2"
                    >{{ genre.name }}</span
                >
                <Button 
                    class="ml-auto !py-1 !px-3"
                    name="Play"
                    @click="$emit('play-click', song)"
                />
            </div>
        </div>

        <span class="float-right text-slate-400 text-xs">Played: {{ song.popularity }}</span>
    </div>
</template>
