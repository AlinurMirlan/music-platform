import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import AppLayout from '@/views/shared/AppLayout.vue';
import IdentityLayout from '@/views/shared/IdentityLayout.vue';
import Recommendations from '@/views/Recommendations.vue';
import Registration from '@/views/identity/Registration.vue';
import Login from '@/views/identity/Login.vue';
import { useAccountStore } from '@/stores/account';

const routes: RouteRecordRaw[] = [
    {
        path: '/',
        redirect: '/recommendations/1',
        component: AppLayout,
        children: [
            {
                name: 'recommendations',
                path: 'recommendations/:page(\\d*)',
                component: Recommendations,
                meta: {
                    requiresAuth: true
                },
                props: (route) => ({ page: Number(route.params.page) })
            }
        ]
    },
    {
        path: '/account',
        redirect: '/account/login',
        component: IdentityLayout,
        children: [
            {
                path: 'register',
                name: 'userRegister',
                component: Registration
            },
            {
                path: 'login',
                name: 'userLogin',
                component: Login
            }
        ]
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes: routes,
    scrollBehavior() {
        return { top: 0 };
    }
});

router.beforeEach((to, from) => {
    if (to.meta.requiresAuth) {
        const accountStore = useAccountStore();
        if (!accountStore.isAuthenticated) {
            accountStore.redirectPath = to.path;
            return { name: 'userLogin' };
        }
    }
});

export default router;
