<script setup lang="ts">
import InputField from '@/components/form/Field.vue';
import Button from '@/components/Button.vue';
import z from 'zod';
import ErrorMessage from '@/components/form/ErrorMessage.vue';
import { getConfiguredAxios } from '@/assets/axios.js';
import { useAccountStore } from '@/stores/account';
import type { JwtRaw } from '@/assets/types/types.js';
import { formatErrors } from '@/assets/errorFormatter';
import { useUserErrors } from '@/composables/userErrors';
import { useRouter } from 'vue-router';
import axios from 'axios';

// STATE
const axiosBase = getConfiguredAxios();
const accountStore = useAccountStore();
const router = useRouter();

const userSchema = z.object({
    email: z.string().email(),
    password: z.string().min(8, { message: 'Password must be at least 8 characters long' })
});
const { user, errors } = useUserErrors<typeof userSchema>({
    email: '',
    password: ''
});

// METHODS
async function onFormSubmit() {
    const result = userSchema.safeParse(user);
    if (result.success) {
    } else {
        errors.value = result.error.flatten().fieldErrors;
        return;
    }

    try {
        const response = await axiosBase.post<JwtRaw>('/account/login', user);
        accountStore.setJwt(response.data);
        router.push(accountStore.getRedirect());
    } catch (error) {
        if (!axios.isAxiosError(error)) {
            console.log(error);
            throw error;
        }

        if (error.response?.status === 404) {
            errors.value.email = ['User with the given email does not exist'];
            return;
        }
        if (error.response?.status === 400) {
            errors.value.password = ['Incorrect password'];
            return;
        }
    }
}
</script>

<template>
    <main>
        <form class="max-w-lg mx-auto mt-6">
            <InputField class="mt-4" name="Email" type="email" v-model="user.email" />
            <ErrorMessage v-if="errors?.email" :message="formatErrors(errors.email)" />

            <InputField class="mt-4" name="Password" type="password" v-model="user.password" />
            <ErrorMessage v-if="errors?.password" :message="formatErrors(errors.password)"/>

            <Button class="mt-4 block" name="Sign in" @click="onFormSubmit()" />
        </form>
    </main>
</template>
