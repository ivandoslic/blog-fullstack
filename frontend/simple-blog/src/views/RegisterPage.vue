<template>
<div class="w-full h-full flex flex-row justify-center items-center">
    <div class="w-[70%] h-full">
        <img src="../assets/images/registerBackground.jpg" class="h-full" />
    </div>
    <div class="w-[30%] flex flex-col h-full center text-center px-10 justify-center items-center">
        <img src="../assets/images/logo.png" class="w-[35%]"/>
        <h1 class="text-4xl mb-3 font-bold">Create your account</h1>
        <input type="text" placeholder="Enter your username" v-model="username" class="textinp" :class="{ 'textinp-error' : errorOccured }" required/>
        <input type="email" placeholder="Enter your email address" v-model="email" class="textinp" :class="{ 'textinp-error' : errorOccured }" required/>
        <input type="password" placeholder="Enter your password" v-model="password" class="textinp" :class="{ 'textinp-error' : errorOccured }" required/>
        <input type="password" placeholder="Confirm your password" v-model="confPassword" class="textinp" :class="{ 'textinp-error' : errorOccured }" required/>
        <span v-if="errorOccured" class="text-sm text-red-700 w-full text-start mb-3">Error: {{ errorMessage }}</span>
        <button @click="onSubmit" :disabled="buttonDisabled" class="button" :class="{ 'button-disabled' : buttonDisabled }">
            <a v-if="!buttonDisabled">Submit</a>
            <div v-if="buttonDisabled" role="status">
                <svg aria-hidden="true" class="inline w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-gray-600 dark:fill-gray-300" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="currentColor"/>
                    <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentFill"/>
                </svg>
                <span class="sr-only">Loading...</span>
            </div>
        </button>
        <span class="text-sm text-gray-700">Already have an account?</span>
        <hr class="border-gray-300 w-full mt-2 mb-1">
        <button @click="goToLogin" class="button">Login</button>
    </div>
</div>
</template>

<script>
import { defineComponent, ref } from "vue";
import { router, emailRegex } from '@/util';
import { useAuthStore } from "../stores";

export default defineComponent({
    name: "RegisterPage",
    setup() {
        const username = ref('');
        const email = ref('');
        const password = ref('');
        const confPassword = ref('');
        const errorMessage = ref('');
        const errorOccured = ref(false);
        const buttonDisabled = ref(false);

        const onSubmit = async () => {
            buttonDisabled.value = true;

            if (username.value.trim().length == 0) {
                errorOccured.value = true;
                errorMessage.value = 'You must pick an username';
                buttonDisabled.value = false;
                return;
            }

            if (!email.value.toLowerCase().match(emailRegex)) {
                errorOccured.value = true;
                errorMessage.value = 'Invalid email';
                buttonDisabled.value = false;
                return;
            }

            if (password.value.trim().length == 0) {
                errorOccured.value = true;
                errorMessage.value = 'You must pick a password';
                buttonDisabled.value = false;
                return;
            }

            if (password.value !== confPassword.value) {
                errorOccured.value = true;
                errorMessage.value = 'Passwords do not match';
                buttonDisabled.value = false;
                return;
            }

            const authStore = useAuthStore();

            try {
                await authStore.register(username.value, email.value, password.value);
                buttonDisabled.value = false;
            } catch (e) {
                errorOccured.value = true;
                errorMessage = e.message;
                buttonDisabled.value = false;
            }
        }

        const goToLogin = () => {
            router.push('/login');
        }

        return {
            username,
            email,
            password,
            confPassword,
            errorOccured,
            errorMessage,
            buttonDisabled,
            onSubmit,
            goToLogin
        }
    }
});
</script>

<style>

</style>