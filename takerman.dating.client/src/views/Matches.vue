<template>
  <breadcrumbs :paths="breadcrumbs" />
  <div class="container-fluid">
    <Heading heading="Съвпадения" />
    <p class="text-center">
      Тук ще намерите своите съвпадения. Можете да чатите с тях и да разгледате
      профила им.
    </p>
    <br />
    <loader v-if="this.loading == true" />
    <div v-else-if="!this.matches || this.matches.length == 0">
      <h2 class="text-center">
        Все още нямате съвпадения. Ходете на срещи и все някога ще имате
        съвпадение.
      </h2>
    </div>
    <div v-else>
      <div class="row">
        <div class="col-2 text-center">
          <div
            v-if="this.matches && this.matches.length > 0"
            v-for="(match, matchKey) in this.matches"
            :key="matchKey"
          >
            <div>
              <img
                @click="loadMessages(match.userId)"
                :src="match.avatarUrl"
                class="img match-avatar"
                width="120"
                height="120"
              />
              <br />
              <router-link
                v-if="match.avatarUrl"
                :to="'/user-profile?id=' + match.userId"
                >{{ match.name }}</router-link
              >
            </div>
          </div>
          <div v-else>
            <h3 class="text-center">Няма намерени съвпадения</h3>
          </div>
        </div>
        <div class="col-10">
          <h4 class="text-center">
            {{
              this.toUser
                ? "В момента чатите със " +
                  this.toUser.firstName +
                  " " +
                  this.toUser.lastName
                : "Изберете някой с когото да чатите"
            }}
          </h4>
          <div :if="this.toUser">
            <input
              class="form-control"
              v-model="message"
              @keyup.enter="sendMessage"
              placeholder="Message"
            />
            <button class="btn btn-success" @click="sendMessage">Send</button>
            <br />
          </div>
          <div style="overflow-y: auto; height: 600px">
            <span
              class="chat-message"
              :title="moment(new Date(msg.sentOn)).format('DD MMM, HH:mm')"
              v-for="(msg, index) in messages"
              :key="index"
              :style="
                msg.toUserId == this.userId
                  ? 'text-align: right'
                  : 'text-align: left'
              "
            >
              {{ msg.message }} <br />
            </span>
          </div>
        </div>
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
import Avatar from '../components/Avatar.vue';
import Heading from '../components/Heading.vue';
import moment from "moment";
import * as signalR from "@microsoft/signalr";

export default {
  data() {
    return {
      userId: null,
      toUser: null,
      toUserId: null,
      matches: null,
      loading: false,
      message: "",
      moment: moment,
      messages: [],
      connection: null,
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
      for (let i = 0; i < this.matches.length; i++) {
        let result = await fetch('Cdn/GetAvatar?userId=' + this.matches[i].userId);
        this.matches[i].avatarUrl = await result.text();
      }
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/chatHub")
      .build();

    await this.connection.start().catch((err) => console.Error(err));

    this.loading = false;
  },
  methods: {
    async sendMessage() {
            this.connection.invoke("SendMessage", this.userId, this.toUser.id, this.message)
                .then(async (res) => {
                    await fetchWrapper.post('Notification/SendChatMessageAsync', {
                        userId: this.userId,
                        toUserId: this.toUser.id,
                        message: this.message
                    });
                    this.message = "";
          this.messages = await fetchWrapper.get('Notification/GetChatMessagesAsync?userId=' + this.userId + '&toUserId=' + this.toUser.id);
                }).catch(err =>
                    console.log(err)
                );
        },
        async loadMessages(toUserId) {
          this.messages = [];
          this.toUserId = toUserId;
          this.toUser = await fetchWrapper.get('User/Get?id=' + toUserId);
          this.messages = await fetchWrapper.get('Notification/GetChatMessagesAsync?userId=' + this.userId + '&toUserId=' + toUserId);

          this.connection.on("ReceiveMessage", (userId, toUserId, message) => {
              this.messages.unshift({
                  userId: userId,
                  toUserId: toUserId,
                  message: message,
                  sentOn: new Date()
              });
          });
        }
  },
  components: { breadcrumbs, loader, heading, Avatar, Heading }
}
</script>

<style scoped>
.match-avatar {
  cursor: pointer;
}
.chat-message {
  display: block;
}
</style>
