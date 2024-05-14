<template>
    <div :class="'col card' + (this.isPast ? ' bg-light' : '')" style="margin: 15px; width: 18rem; display: inline-block;">
        <router-link :to="'date?id=' + this.date?.id + ''">
            <img v-if="this.date && this.date.pictures && this.date.pictures.length > 0" class="card-img-top" :src="this.date?.pictures[0]?.url" alt="Date">
        </router-link>
        <div class="card-body">
            <h4 class="card-title text-center">
                <router-link :to="'date?id=' + this.date?.id + ''">{{ this.date?.title }}</router-link>
            </h4>
            <h6 class="card-title text-center">
                за <strong>{{ this.date?.ethnicity }}</strong> - {{ this.date?.minAges }}-{{ this.date?.maxAges }} год.</h6>
            <table class="table">
                <tbody>
                    <tr>
                        <td>Мъже {{ this.date?.menCount }}/{{ this.date?.minMen }}</td>
                        <td>Жени {{ this.date?.womenCount }}/{{ this.date?.minWomen }}</td>
                    </tr>
                    <tr>
                        <td>{{ this.date?.startsOn ? moment(new Date(this.date?.startsOn)).format("DD MMM, HH:mm") :
        'Предстои' }}
                        </td>
                        <td>Цена {{ this.date?.price }}лв.</td>
                    </tr>
                </tbody>
            </table>
            <p v-if="this.date?.status === 'NotApproved'" class="text-center">
                <a @click="saveSpot(this.date)" class="btn btn-primary">Запази място</a>
            </p>
            <p v-else-if="this.date?.status === 'SavedSpot'" class="text-center">
                <a @click="unsaveSpot(this.date)" class="btn btn-danger">Няма да присъствам</a>
            </p>
            <div v-else-if="this.date?.status === 'Approved' && this.date?.startsOn > new Date()" class="text-center">
                <PayButton v-if="this.path === '/date' && this.date?.price > 0" :date-id="this.date.id"
                           :on-approve="onApprove" :on-error="onError" class="pay-button">Купи</PayButton>
                <router-link v-else class="btn btn-success" :to="'date?id=' + this.date.id + ''">Купи
                    срещата</router-link>
                <div v-if="this.paymentStatus === 'success'" class="alert alert-success" role="alert">
                    <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                    <span class="sr-only"></span> Закупихте срещата успешно. Можете да я видите от менюто <router-link
                                 to="orders">'Мои срещи'</router-link>
                </div>
                <div v-else-if="this.paymentStatus === 'failed'" class="alert alert-danger" role="alert">
                    <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                    <span class="sr-only"></span> Стана грешка при плащането. Моля опитайте пак или се свържете с нас през
                    контактната форма или чата
                </div>
            </div>
            <p v-else-if="this.date?.status === 'Bought'" class="text-center">
                <strong>Закупили сте тази среща</strong>
            </p>
            <p v-else-if="this.date?.status === 'Started'" class="text-center">
                <strong>Срещата е започнала</strong>
            </p>
            <p v-else class="text-center">
                <strong>Срещата е завършила</strong>
            </p>
        </div>
    </div>
</template>
<script lang="js">
import moment from 'moment';
import { useRoute } from 'vue-router';
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import PayButton from './PayButton.vue';
import { router } from '@/helpers';

export default {
    props: {
        id: Number
    },
    data() {
        return {
            date: null,
            moment: moment,
            paymentStatus: '',
            userId: 0
        }
    },
    computed: {
        path() {
            var route = useRoute();
            return route.path;
        },
        isPast() {
            return new Date(this.date?.startsOn) < new Date();
        }
    },
    components: {
        PayButton
    },
    async mounted() {
        const authStore = useAuthStore();
        if (authStore.user)
            this.userId = authStore.user.id;
        this.date = await fetchWrapper.get('Dates/Get?id=' + this.id + '&userId=' + this.userId);
    },
    methods: {
        async onApprove(e, o) {
            this.paymentStatus = 'success';

            const data = {
                dateId: this.date?.id,
                userId: this.userId,
                paymentId: e.paymentID,
                payerId: e.payerID,
                orderId: e.orderID,
                paymentSource: e.paymentSource
            }

            await fetchWrapper.post('Order/Create', data);

            const payButton = document.getElementsByClassName('pay-button');
            for (let i = 0; i < payButton.length; i++) {
                const element = payButton[i];
                element.style.display = 'none';
            }
        },
        onError(e) {
            this.paymentStatus = 'fail';
        },
        async saveSpot(date) {
            const authStore = useAuthStore();
            if (!authStore.user) {
                router.push('/login?returnUrl=/date?id=' + this.date?.id);
            } else {
                await fetchWrapper.get('Dates/SaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + this.date.id))
                    .then((result) => {
                        this.date.status = result.status;
                        this.date.menCount = result.menCount;
                        this.date.womenCount = result.womenCount;
                        this.emitter.emit('addToSpotCount', { 'eventContent': 1 });
                    });
            }
        },
        async unsaveSpot(date) {
            const authStore = useAuthStore();
            if (!authStore.user) {
                router.push('/login?returnUrl=/date?id=' + this.date?.id);
            } else {
                await fetchWrapper.get('Dates/UnsaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + this.date.id))
                    .then((result) => {
                        this.date.status = result.status;
                        this.date.menCount = result.menCount;
                        this.date.womenCount = result.womenCount;
                        this.emitter.emit('addToSpotCount', { 'eventContent': -1 });
                    });
            }
        }
    }
}
</script>
<style scoped></style>