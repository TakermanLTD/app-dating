<template>
    <div class="col card" style="margin: 15px; width: 18rem; display: inline-block;">
        <router-link :to="'date?id=' + date.id + ''">
            <img class="card-img-top" src="/src/assets/img/date/date-thumbnail.jpeg" alt="Date">
        </router-link>
        <div class="card-body">
            <h4 class="card-title text-center">
                <router-link :to="'date?id=' + date.id + ''">{{ date?.title }}</router-link>
            </h4>
            <h6 class="card-title text-center">
                за <strong>{{ date?.ethnicity }}</strong> - {{ date?.minAges }}-{{ date?.maxAges }} год.</h6>
            <table class="table">
                <tbody>
                    <tr>
                        <td>Мъже {{ date?.menCount }}/{{ date?.minMen }}</td>
                        <td>Жени {{ date?.womenCount }}/{{ date?.minWomen }}</td>
                    </tr>
                    <tr>
                        <td>{{ date?.startsOn ? moment(new Date(date?.startsOn)).format("DD MMM, hh:mm") : 'Предстои' }}
                        </td>
                        <td>Цена {{ date?.price }}лв.</td>
                    </tr>
                </tbody>
            </table>
            <!-- <p class="card-text text-center">
                {{ date.description }}
            </p> -->
            <p v-if="date?.status === 'NotApproved'" class="text-center">
                <a @click="date?.isSpotSaved ? unsaveSpot(date) : saveSpot(date)"
                    :class="date?.isSpotSaved ? 'btn btn-danger' : 'btn btn-primary'">
                    {{ date?.isSpotSaved ? 'Няма да присъствам' : 'Запази място' }}</a>
            </p>
            <p v-else-if="false && date?.status === 'Approved'" class="text-center">
                <PayButton v-if="status === 'notBought'" :date-id="date.id" :on-approve="onApprove">Купи</PayButton>
            <div v-else-if="status === 'bought'" class="alert alert-success" role="alert">
                <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                <span class="sr-only"></span> Закупихте срещата успешно. Можете да я видите от менюто <router-link
                    to="orders">'Мои срещи'</router-link>
            </div>
            <div v-else-if="status === 'failed'" class="alert alert-danger" role="alert">
                <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                <span class="sr-only"></span> Стана грешка при плащането. Моля опитайте пак или се свържете с нас през
                контактната форма или чата
            </div>
            </p>
            <p v-else-if="date?.status === 'Approved'" class="text-center">
                <router-link class="btn btn-success" :to="'date?id=' + date.id + ''">Купи
                    срещата</router-link>
            </p>
            <p v-else-if="date?.status === 'Started'" class="text-center">
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
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import PayButton from './PayButton.vue';
import { router } from '@/helpers';

export default {
    props: {
        date: null
    },
    data() {
        return {
            moment: moment,
            status: 'notBought',
            userId: 0
        }
    },
    components: {
        PayButton
    },
    beforeMount() {
        const authStore = useAuthStore();
        if (authStore.user) {
            this.userId = authStore.user.id;
        }
    },
    methods: {
        onApprove() {
            this.status = 'bought';
        },
        async saveSpot(date) {
            const authStore = useAuthStore();
            if (!authStore.user) {
                router.push('/login');
            } else {
                await fetchWrapper.get('Dates/SaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + date.id))
                    .then((result) => {
                        date.isSpotSaved = result.isSpotSaved;
                        date.menCount = result.menCount;
                        date.womenCount = result.womenCount;
                    });
            }
        },
        async unsaveSpot(date) {
            const authStore = useAuthStore();
            if (!authStore.user) {
                router.push('/login');
            } else {
                await fetchWrapper.get('Dates/UnsaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + date.id))
                    .then((result) => {
                        date.isSpotSaved = result.isSpotSaved;
                        date.menCount = result.menCount;
                        date.womenCount = result.womenCount;
                    });
            }
        }
    }
}
</script>
<style scoped></style>