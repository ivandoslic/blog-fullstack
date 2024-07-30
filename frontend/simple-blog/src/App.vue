<script lang="ts">
import { defineComponent } from "vue";
import { useAuthStore } from './stores';
import { RouterView, RouterLink } from 'vue-router';

export default defineComponent({
  name: "App",
  setup() {
    const authStore = useAuthStore();

    console.log(JSON.stringify(authStore.user));

    return {
      authStore
    };
  }
});
</script>

<template>
  <main>
    <nav v-show="authStore.user" class="w-full h-[10vh] flex flex-row justify-between items-center px-3">
      <div class="h-full flex-1">
        <img src="./assets/images/logo.png" class="h-full mb-12"/>
      </div>
      <div class="px-3 flex-1 flex flex-row justify-between">
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/">Search</RouterLink>
        <RouterLink to="/">About</RouterLink>
      </div>
      <div class="flex-1 flex flex-row justify-end">
        <p v-if="authStore.user">{{ authStore.user.userName }}</p>
        <a @click="authStore.logout()" class="ml-3">Logout</a>
      </div>
    </nav>
    <div class="w-full">
      <RouterView />
    </div>
  </main>
</template>

<style scoped>

</style>