<template>
    <div class="card-deck">
        <div v-if="this.loading" class="text-center">
            <span>Loading...</span>
        </div>
        <div v-else v-for="date in dates" :key="date.id" class="card"
            style="margin: 15px; width: 18rem; display: inline-block;">
            <img class="card-img-top" src="/src/assets/img/date/date-thumbnail.jpeg" alt="Date">
            <div class="card-body">
                <h4 class="card-title text-center">{{ date.title }}</h4>
                <h6 class="card-title text-center">
                    за <strong>{{ date.ethnicity }}</strong> - {{ date.minAges }}-{{ date.maxAges }} год.</h6>
                <table class="table">
                    <tbody>
                        <tr>
                            <td>Мъже {{ date.menCount }}/{{ date.minMen }}</td>
                            <td>Жени {{ date.womenCount }}/{{ date.minWomen }}</td>
                        </tr>
                        <tr>
                            <td>{{ date.startsOn ? moment(date.startsOn).format("DD MMM, hh:mm") : 'Предстои' }}</td>
                            <td>Цена {{ date.price }}лв.</td>
                        </tr>
                    </tbody>
                </table>
                <p class="card-text text-center">
                    {{ date.shortDescription }}
                </p>
                <p v-if="date.startsOn" class="text-center">
                    <a @click="buy(date)" class="btn btn-success">Купи достъп</a>
                </p>
                <p v-else class="text-center">
                    <a @click="date.isSpotSaved ? unsaveSpot(date) : saveSpot(date)"
                        :class="date.isSpotSaved ? 'btn btn-danger' : 'btn btn-primary'">
                        {{ date.isSpotSaved ? 'Няма да присъствам' : 'Запази място' }}</a>
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
            loading: false,
            moment: moment,
            dates: []
        }
    },
    async beforeMount() {
        try {
            this.loading = true;
            const authStore = useAuthStore();
            this.dates = await fetchWrapper.get('Dates/GetAll' + (authStore.user == null ? '' : '?userId=' + authStore.user.id));
            this.loading = false;
        } catch (error) {
            this.loading = false;
        }
    },
    methods: {
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
        },
        async buy(date) {
            console.log('Buy access');
        }
    }
}
</script>

<style scoped></style>