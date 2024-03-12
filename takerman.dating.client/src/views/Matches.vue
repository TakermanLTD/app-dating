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
          <div v-for="(match, matchKey) in this.matches" :key="matchKey">
            <div>
              <img @click="chat(match.userId)"
                :src="match.avatar"
                class="img" width="150" height="150" />
              <br />
              <span>{{ match.name }}</span>
            </div>
            <router-link :to="'/user-profile?id=' + match.userId">Виж профил</router-link>
          </div>
        </div>
        <div class="col-9">
          <Chat v-if="this.toUserId" :toUserId="this.toUserId" />
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
import Chat from '../components/Chat.vue';

export default {
  data() {
    return {
      userId: null,
      toUserId: null,
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
    this.userId = authStore.user.id;
    this.matches = await fetchWrapper.get('Dates/GetMatches?userId=' + this.userId);
    if (this.matches?.length > 0) {
      this.toUserId = this.matches[0].userId;
    }
    this.loading = false;
  },
  methods: {
    chat(userId) {
      this.toUserId = userId;
    }
  },
  components: { breadcrumbs, loader, heading, Chat }
}
</script>

<style scoped></style>