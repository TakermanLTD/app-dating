<template>
    <div class="row form-row">
        <div class="col">
            <label for="frmDateType">Тип среща</label>
            <select id="frmDateType" placeholder="Тип среща" @change="applyFilter" v-model="filter.dateType"
                class="form-control">
                <option :value="0"></option>
                <option v-for="dateType in dateTypes" :value="dateType.key">{{ dateType.value }}</option>
            </select>
        </div>
        <div class="col">
            <label for="frmEthnicity">Етнос</label>
            <select id="frmEthnicity" placeholder="Етнос" @change="applyFilter" v-model="filter.ethnicity"
                class="form-control">
                <option :value="0"></option>
                <option v-for="ethnicity in ethnicities" :value="ethnicity.key">{{ ethnicity.value }}</option>
            </select>
        </div>
        <div class="col">
            <label for="frmMinAges">Мин. години</label>
            <input id="frmMinAges" placeholder="Мин. години" @change="applyFilter" type="number" class="form-control"
                v-model="filter.minAges" />
        </div>
        <div class="col">
            <label id="frmMaxAges" for="frmMaxAges">макс години</label>
            <input placeholder="макс години" @change="applyFilter" type="number" class="form-control"
                v-model="filter.maxAges" />
        </div>
        <div class="col">
            <label for="frmMaxPrice">Макс цена</label>
            <input id="frmMaxPrice" placeholder="Макс цена" @change="applyFilter" type="number" class="form-control"
                v-model="filter.maxPrice" />
        </div>
    </div>
    <div class="card-deck">
        <div v-if="this.loading" class="text-center">
            <h3>Зареждане...</h3>
        </div>
        <div v-else-if="!dates" class="text-center">
            <h3>There are not dates upcomming</h3>
        </div>
        <div v-else v-for="date in dates" :key="date.id" class="card"
            style="margin: 15px; width: 18rem; display: inline-block;">
            <router-link :to="'/date?id=' + date.id + '&userId=' + this.userId"><img class="card-img-top" src="/src/assets/img/date/date-thumbnail.jpeg" alt="Date"></router-link>
            <div class="card-body">
                <h4 class="card-title text-center"><router-link :to="'/date?id=' + date.id + '&userId=' + this.userId">{{ date.title }}</router-link>
                </h4>
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
                <p v-if="date.status === 'NotApproved'" class="text-center">
                    <a @click="date.isSpotSaved ? unsaveSpot(date) : saveSpot(date)"
                        :class="date.isSpotSaved ? 'btn btn-danger' : 'btn btn-primary'">
                        {{ date.isSpotSaved ? 'Няма да присъствам' : 'Запази място' }}</a>
                </p>
                <p v-else-if="date.status === 'Approved'" class="text-center">
                    <a @click="buy(date)" class="btn btn-success">Купи достъп</a>
                </p>
                <p v-else-if="date.status === 'Started'" class="text-center">
                    <strong>Срещата е започнала</strong>
                </p>
                <p v-else class="text-center">
                    <strong>Срещата е завършила</strong>
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
            userId: null,
            loading: false,
            ethnicities: [],
            dateTypes: [],
            moment: moment,
            filter: {
                minAges: 0,
                maxAges: 0,
                maxPrice: 0,
                ethnicity: null,
                dateType: null,
            },
            dates: []
        }
    },
    async beforeMount() {
        const authStore = useAuthStore();
        this.userId = authStore.user?.id;
        await this.applyFilter();
        this.ethnicities = await fetchWrapper.get('Options/GetEthnicities');
        this.dateTypes = await fetchWrapper.get('Options/GetDateTypes');
    },
    methods: {
        async applyFilter() {
            try {
                this.loading = true;
                const authStore = useAuthStore();
                this.dates = await fetchWrapper.post('Dates/GetAll' + (authStore.user == null ? '' : '?userId=' + authStore.user.id), this.filter);
                this.loading = false;
            } catch (error) {
                this.loading = false;
            }
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
        },
        async buy(date) {
            console.log('Buy access');
        }
    }
}
</script>

<style scoped></style>