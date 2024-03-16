<template>
  <breadcrumbs :paths="breadcrumbs" />
  <section class="container">
    <loader v-if="this.loading == true" />
    <div v-else class="row">
      <div class="col-2 text-center">
        <div v-for="(match, matchKey) in this.matches" :key="matchKey">
          <div>
            <Avatar @click="chat(match.userId)" :userId="match.userId" />
            <br />
            <span>{{ match.name }}</span>
          </div>
        </div>
      </div>
      <div class="col-10">
        <Chat v-if="this.toUserId" :toUserId="this.toUserId" />
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
import Avatar from '../components/Avatar.vue';

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
  components: { breadcrumbs, loader, heading, Chat, Avatar }
}
</script>

<style scoped></style>