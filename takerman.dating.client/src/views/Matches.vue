<template>
  <breadcrumbs :paths="breadcrumbs" />
  <section class="container">
    <loader v-if="this.loading == true" />
    <div v-else>
      <div class="container">
        <heading heading="Съвпадения" />
      </div>
      <div class="row">
        <div class="col-3 text-center">
          <div v-for="(user, userKey) in this.matches" :key="userKey">
            <div>
              <img @click="chat(user.userId)" :src="user.avatar == null ? 'defaultAvatar.png' : 'data:image/jpeg;base64,' + btoa(user.avatar)" class="img" width="150" height="150" />
              <br />
              <span>{{ user.name }}</span>
            </div>
            <router-link :to="'/userProfile?id=' + user.userId">Виж профил</router-link>
          </div>
        </div>
        <div class="col-9">
          <textarea class="form-control" style="width: 100%; height: 100%;"></textarea>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import breadcrumbs from '../components/Breadcrumbs.vue';
import loader from '../components/Loader.vue';
import heading from '../components/Heading.vue';

export default {
  data() {
    return {
      matches: null,
      loading: false,
      breadcrumbs: [
        { name: '/', title: 'Начало' },
        { name: 'matches', title: 'Съвпадения' }
      ]
    };
  },
  async created() {
    this.loading = true;
    const authStore = useAuthStore();
    this.matches = await fetchWrapper.get('Dates/GetMatches?userId=' + authStore.user.id);
    this.loading = false;
  },
  methods: {
    chat(userId) {
      
    }
  },
  components: { breadcrumbs, loader, heading }
}
</script>

<style scoped></style>