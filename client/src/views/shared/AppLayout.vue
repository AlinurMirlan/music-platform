<script setup lang="ts"></script>

<template>
    <div class="min-h-screen flex flex-col">
        <!-- Shared layout -->
        <header class="p-5 bg-green-500">
            <nav class="flex flex-row justify-between text-white font-medium text-xl">
                <a href="">Recommendations</a>
                <div class="relative">
                    <button id="profileExpandButton">Profile</button>
                    <div
                        id="profileExpand"
                        class="hidden fixed w-32 px-4 py-2 bg-green-100 right-0 text-lg"
                    >
                        <a
                            class="block after:block after:content-[''] after:h-1 after:w-full after:bg-white"
                            href="#"
                            >Favorites</a
                        >
                        <a
                            class="block after:block after:content-[''] after:h-1 after:w-full after:bg-white"
                            href=""
                            >Settings</a
                        >
                        <button
                            class="after:block after:content-[''] after:h-1 after:w-full after:bg-white w-full text-left"
                        >
                            Logout
                        </button>
                    </div>
                </div>
            </nav>
        </header>
        <router-view></router-view>
    </div>
</template>
