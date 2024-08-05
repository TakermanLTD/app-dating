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
      <div class="row rounded-lg overflow-hidden">
        <div class="col-3 px-0">
          <div class="bg-white">
            <div class="bg-gray px-4 py-2 bg-light">
              <p class="h5 mb-0 py-1">Recent</p>
            </div>
            <div class="messages-box">
              <div class="list-group rounded-0">
                <a @click="loadMessages(match.userId)" v-for="(match, matchKey) in this.matches" :key="matchKey" class="list-group-item list-group-item-action text-white rounded-0" :class="this.toUserId == match.userId ? 'active' : ''">
                  <div class="media">
                    <img :src="match.avatarUrl" class="img match-avatar rounded-circle" width="50" height="50" alt="user" />
                    <div class="media-body ml-4">
                      <div class="d-flex align-items-center justify-content-between mb-1">
                        <router-link v-if="match.avatarUrl" :to="'/user-profile?id=' + match.userId">
                          <h6 class="mb-0">{{ match.name }}</h6>
                        </router-link>
                      </div>
                      <!--<p style="color: black;" class="font-italic mb-0 text-small">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore.</p>-->
                    </div>
                  </div>
                </a>
              </div>
            </div>
          </div>
        </div>
        <div class="col-9 px-0">
          <div class="px-4 py-5 chat-box bg-white">
            <div v-for="(msg, index) in messages" :key="index" class="chat-message">
              <div v-if="msg.toUserId == this.userId" class="media w-50 mb-3"><img :src="this.toUser.avatarUrl" alt="user" width="50" class="rounded-circle">
                <div class="media-body ml-3">
                  <div class="bg-light rounded py-2 px-3 mb-2">
                    <p class="text-small mb-0 text-muted">{{ msg.message }}</p>
                  </div>
                  <p class="small text-muted">{{ moment(new Date(msg.sentOn)).format('DD MMM, HH:mm') }}</p>
                </div>
              </div>
              <div v-else class="media w-50 ml-auto mb-3">
                <div class="media-body">
                  <div class="bg-primary rounded py-2 px-3 mb-2">
                    <p class="text-small mb-0 text-white">{{ msg.message }}</p>
                  </div>
                  <p class="small text-muted">{{ moment(new Date(msg.sentOn)).format('DD MMM, HH:mm') }}</p>
                </div>
              </div>
            </div>
          </div>
          <div class="input-group">
            <input type="text" placeholder="Type a message" aria-describedby="button-addon2" v-model="message" @keyup.enter="sendMessage" style="background-color: #f2f1df !important;" class="form-control rounded-0 border-0 py-4 bg-light">
            <div class="input-group-append">
              <button id="button-addon2" class="btn btn-link" @click="sendMessage"> <i class="fa fa-paper-plane"></i></button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="js">
import breadcrumbs from '../components/Breadcrumbs.vue';
import loader from '../components/Loader.vue';
import heading from '../components/Heading.vue';
import Avatar from '../components/Avatar.vue';
import Heading from '../components/Heading.vue';
import moment from "moment";
import * as signalR from "@microsoft/signalr";

// chat design: https://bootstrapious.com/p/bootstrap-chat

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
    const { user } = useAuth0();
    this.userId = user.id;
    this.matches = await fetch('Dates/GetMatches?userId=' + this.userId);

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/chatHub")
      .build();

    if (this.matches?.length > 0) {
      for (let i = 0; i < this.matches.length; i++) {
        let result = await fetch('Cdn/GetAvatar?userId=' + this.matches[i].userId);
        this.matches[i].avatarUrl = await result.text();
      }

      await this.loadMessages(this.matches[0].userId);
    }

    await this.connection.start().catch((err) => console.Error(err));

    this.loading = false;
  },
  methods: {
    async sendMessage() {
      this.connection.invoke("SendMessage", this.userId, this.toUser.id, this.message)
        .then(async (res) => {
          await fetch('Notification/SendChatMessageAsync', {
            userId: this.userId,
            toUserId: this.toUser.id,
            message: this.message
          });
          this.message = "";
          this.messages = await fetch('Notification/GetChatMessagesAsync?userId=' + this.userId + '&toUserId=' + this.toUser.id);
        }).catch(err =>
          console.log(err)
        );
    },
    async loadMessages(toUserId) {
      this.messages = [];
      this.toUserId = toUserId;
      this.toUser = await fetch('User/Get?id=' + toUserId);
      let toUserAvatar = await fetch('Cdn/GetAvatar?userId=' + toUserId);
      this.toUser.avatarUrl = await toUserAvatar.text();
      this.messages = await fetch('Notification/GetChatMessagesAsync?userId=' + this.userId + '&toUserId=' + toUserId);

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

/*
*
* ==========================================
* FOR DEMO PURPOSES
* ==========================================
*
*/
body {
  background-color: #74EBD5;
  background-image: linear-gradient(90deg, #74EBD5 0%, #9FACE6 100%);

  min-height: 100vh;
}

::-webkit-scrollbar {
  width: 5px;
}

::-webkit-scrollbar-track {
  width: 5px;
  background: #f5f5f5;
}

::-webkit-scrollbar-thumb {
  width: 1em;
  background-color: #ddd;
  outline: 1px solid slategrey;
  border-radius: 1rem;
}

.text-small {
  font-size: 0.9rem;
}

.messages-box,
.chat-box {
  height: 600px;
  overflow-y: scroll;
}

.rounded-lg {
  border-radius: 0.5rem;
}

input::placeholder {
  font-size: 0.9rem;
  color: #999;
}
</style>
