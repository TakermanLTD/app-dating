<template>
    <div class="container">
        <div class="row">
            <input class="form-control" v-model="message" @keyup.enter="sendMessage" placeholder="Message" />
        </div>
        <div style="overflow-y: auto; height: 600px;" class="row">
            <span :title="moment(new Date(msg.sentOn)).format('DD MMM, HH:mm')" v-for="(msg, index) in messages" :key="index" :style="(msg.toUserId == this.userId ? 'text-align: right' : 'text-align: left')">
                {{ msg.message }} <br />
            </span>
        </div>
    </div>
</template>
<script>
import moment from "moment";
import * as signalR from "@microsoft/signalr";
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
export default {
    props: {
        toUserId: Number
    },
    data() {
        return {
            userId: "",
            message: "",
            moment: moment,
            messages: [],
            connection: null
        };
    },
    async created() {
        const authStore = useAuthStore();
        this.userId = authStore.user.id;

        this.messages = await fetchWrapper.get('Notification/GetChatMessagesAsync?userId=' + this.userId + '&toUserId=' + this.toUserId);

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub")
            .build();

        this.connection.on("ReceiveMessage", (userId, toUserId, message) => {
            this.messages.push({
                userId: userId,
                toUserId: toUserId,
                message: message,
                sentOn: new Date()
            });
        });

        await this.connection.start().catch((err) => console.Error(err));
    },
    methods: {
        async sendMessage() {
            this.connection.invoke("SendMessage", this.userId, this.toUserId, this.message)
                .then(async (res) => {
                    await fetchWrapper.post('Notification/SendChatMessageAsync', {
                        userId: this.userId,
                        toUserId: this.toUserId,
                        message: this.message
                    });
                    this.message = "";
                }).catch(err =>
                    console.log(err)
                );
        }
    }
};
</script>