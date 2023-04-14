import { reactive, ref } from 'vue';
import type z from 'zod';

export function useUserErrors<TUser extends z.ZodTypeAny>(userShape: z.infer<TUser>) {
    type UserShape = typeof userShape;
    type UserErrors = Partial<Record<keyof UserShape, string[]>>;

    const user: UserShape = reactive(userShape);
    let errors = ref<UserErrors>({});
    return { user, errors };
}
