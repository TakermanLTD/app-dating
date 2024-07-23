<template>
    <div v-if="this.date" :class="'col date-card card' + (this.isPast ? ' bg-light' : '')">
        <div class="date-card-image">
            <router-link :to="'date?id=' + this.date.id + ''">
                <img v-if="this.date && this.date.pictures && this.date.pictures.length > 0" class="card-img-top" :src="this.date.pictures[0]?.url" alt="Date">
            </router-link>
            <h5 class="date-card-icon">{{ this.date.dateType }}</h5>
        </div>
        <div class="card-body date-card-body">
            <h4 class="card-title text-center">
                <router-link class="date-card-title" :to="'date?id=' + this.date.id + ''">{{ this.date.title }}</router-link>
            </h4>
            <table class="table">
                <tbody>
                    <tr>
                        <td class="date-card-cell">
                            <h5><i class="bi bi-person-standing"></i> {{ this.date.menCount }}/{{ this.date.minMen }}</h5>
                        </td>
                        <td class="date-card-cell">
                            <h5><i class="bi bi-person-standing-dress"></i> {{ this.date.womenCount }}/{{ this.date.minWomen }}</h5>
                        </td>
                    </tr>
                    <tr>
                        <td class="date-card-cell">{{ this.date.minAges }}-{{ this.date.maxAges }} год.</td>
                        <td class="date-card-cell">{{ this.date.startsOn ? moment(new Date(this.date.startsOn)).format("DD MMM, HH:mm") :
                            'Предстои' }}</td>
                    </tr>
                </tbody>
            </table>
            <div class="text-center">
                <h3 class="font-weight-semi-bold">{{ $t('common.currencySign') == '$' ? $t('common.currencySign') +  this.date.price : this.date.price + $t('common.currencySign') }}</h3>
                <p v-if="this.date.status === 'NotApproved'">
                    <a v-if="this.isSpotSaved" @click="this.unsaveSpot(this.date)" class="btn btn-danger">Няма да присъствам</a>
                    <a v-else @click="this.saveSpot(this.date)" class="btn btn-primary">Запази място</a>
                </p>
                <div v-if="this.date.status === 'Approved'">
                    <div v-if="this.isBought">
                        <p>
                            Вече сте купили срещата
                        </p>
                        <a v-if="moment(this.date.startsOn).add(-15, 'minutes') < moment()" :href="this.date.videoLink" class="btn btn-success btn-lg" target="_blank">Влез в срещата</a>
                    </div>
                    <div v-else>
                        <router-link class="btn btn-success" :to="this.user ? '/date?id=' + this.date.id : '/login'">Купи срещата</router-link>
                    </div>
                </div>
                <div v-if="this.date.status === 'Started'">
                    <strong>Срещата е започнала</strong><br />
                    <div v-if="this.isBought">
                        <a :href="this.date.videoLink" class="btn btn-success btn-lg" target="_blank">Влез в срещата</a>
                    </div>
                </div>
                <div v-if="this.date.status === 'Finished' || this.date.status === 'ResultsRevealed'">
                    <p>Срещата е завършила</p>
                </div>
            </div>
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
            user: null,
            isBought: false,
            isSpotSaved: false,
        }
    },
    computed: {
        path() {
            var route = useRoute();
            return route.path;
        },
        isPast() {
            return new Date(this.date.startsOn) < new Date();
        }
    },
    components: {
        PayButton
    },
    async mounted() {
        const authStore = useAuthStore();
        if (authStore.user)
            this.user = authStore.user;
        this.date = await fetchWrapper.get('Dates/GetDate?id=' + this.id);
        this.isBought = this.user && await (await fetch('Dates/IsBought?dateId=' + this.id + (this.user.id ? '&userId=' + this.user.id : ''))).json();
        this.isSpotSaved = this.user && await (await fetch('Dates/IsSpotSaved?dateId=' + this.id + (this.user.id ? '&userId=' + this.user.id : ''))).json();
    },
    methods: {
        async onApprove(e, o) {
            this.paymentStatus = 'success';

            const data = {
                dateId: this.date.id,
                userId: this.user.id,
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
            if (!this.user) {
                router.push('/login?returnUrl=/date?id=' + this.date.id);
            } else {
                await fetchWrapper.get('Dates/SaveSpot' + (this.user == null ? '' : '?userId=' + this.user.id + '&dateId=' + this.date.id))
                    .then((result) => {
                        this.date.status = result.status;
                        this.date.menCount = result.menCount;
                        this.date.womenCount = result.womenCount;
                        this.emitter.emit('addToSpotCount', { 'eventContent': 1 });
                        this.isSpotSaved = true;
                    });
            }
        },
        async unsaveSpot(date) {
            if (!this.user) {
                router.push('/login?returnUrl=/date?id=' + this.date.id);
            } else {
                await fetchWrapper.get('Dates/UnsaveSpot' + (this.user == null ? '' : '?userId=' + this.user.id + '&dateId=' + this.date.id))
                    .then((result) => {
                        this.date.status = result.status;
                        this.date.menCount = result.menCount;
                        this.date.womenCount = result.womenCount;
                        this.emitter.emit('addToSpotCount', { 'eventContent': -1 });
                        this.isSpotSaved = false;
                    });
            }
        }
    }
}
</script>
<style scoped>
.date-card {
    margin: 15px;
    width: 18rem;
    display: inline-block;
    padding: 0px;
    text-align: center;
}

.date-card-title {
    color: black;
}

.date-card-body {
    padding: 20px;
    text-align: center;
}

.date-card-cell {
    width: 50%;
    text-align: center;
}

.date-card-image {
    position: relative;
}

.date-card-icon {
    position: absolute;
    top: 0;
    right: 0;
    background-color: white;
    color: black;
}
</style>