<template>
    <div class="card-deck">
        <div v-for="date in dates" :key="date.id" class="card" style="margin: 15px; width: 18rem;">
            <img class="card-img-top" src="/src/assets/img/portfolio/portfolio-1.jpg" alt="Card image cap">
            <div class="card-body">
                <h4 class="card-title text-center">{{ date.title }}</h4>
                <h6 class="card-title text-center">
                    за {{ date.ethnicity }} - {{ date.minAges }}-{{ date.maxAges }} год.</h6>
                <table class="table">
                    <tbody>
                        <tr>
                            <td>Мъже {{ date.menCount }}/{{ date.minMen }}</td>
                            <td>Жени {{ date.womenCount }}/{{ date.minWomen }}</td>
                        </tr>
                        <tr>
                            <td>{{ moment(date.startsOn).format("DD MMM, hh:mm") }}</td>
                            <td>Цена {{ date.price }}лв.</td>
                        </tr>
                    </tbody>
                </table>
                <p class="card-text text-center">
                    {{ date.shortDescription }}
                </p>
                <p class="text-center">
                    <a @click="date.isSpotSaved ? unsaveSpot(date) : saveSpot(date)" :class="date.isSpotSaved ? 'btn btn-danger' : 'btn btn-success' ">
                        {{ date.isSpotSaved ? 'Не се интересувам' : 'Запази място' }}</a>
                </p>
            </div>
        </div>
    </div>
</template>

<script lang="js">
import moment from 'moment';
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import { router } from '@/helpers';

export default {
    data() {
        return {
            moment: moment,
            dates: []
        }
    },
    async beforeMount() {
        const authStore = useAuthStore();
        this.dates = await fetchWrapper.get('Dates/GetAll' + (authStore.user == null ? '' : '?userId=' + authStore.user.id));
    },
    methods: {
        async saveSpot(date) {
            const authStore = useAuthStore();
            if (!authStore.user) {
                router.push('/login');
            } else {
                await fetchWrapper.get('Dates/SaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + date.id))
                    .then(() => {
                        date.isSpotSaved = true;
                    });
            }
        },
        async unsaveSpot(date) {
            const authStore = useAuthStore();
            if (!authStore.user) {
                router.push('/login');
            } else {
                await fetchWrapper.get('Dates/UnsaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + date.id))
                    .then(() => {
                        date.isSpotSaved = false;
                    });
            }
        }
    }
}
</script>

<style scoped></style>