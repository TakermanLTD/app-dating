<template>
  <breadcrumbs :paths="breadcrumbs" />
  <div class="container-fluid">
    <loader v-if="this.loading == true" />
    <div v-else class="row">
      <div class="col-2 text-center">
        <div v-for="(match, matchKey) in this.matches" :key="matchKey">
          <div>
            <img @click="chat(match.userId)" :src="match.avatarUrl" class="img match-avatar" width="120" height="120" />
            <br />
            <router-link v-if="match.avatarUrl" :to="'/user-profile?id=' + match.userId">{{ match.name }}</router-link>
          </div>
        </div>
      </div>
      <div class="col-10">
        <Chat v-if="this.toUserId" :toUserId="this.toUserId" />
      </div>
    </div>
  </div>
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
      for (let i = 0; i < this.matches.length; i++) {
        let result = await fetch('Cdn/GetAvatar?userId=' + this.matches[i].userId);
        this.matches[i].avatarUrl = await result.text();
      }
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

<style scoped>
.match-avatar {
  cursor: pointer;
}
</style>