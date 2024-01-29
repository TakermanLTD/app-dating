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
        <loader v-if="this.loading" />
        <div v-else-if="!dates || dates.length == 0" class="text-center">
            <h3>There are not dates upcomming</h3>
        </div>
        <card :date="date" v-else v-for="date in dates" :key="date.id" />
    </div>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import card from '../components/Card.vue';
import loader from '../components/loader.vue';

export default {
    data() {
        return {
            userId: null,
            loading: false,
            ethnicities: [],
            dateTypes: [],
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
    components: {
        card,
        loader
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
        }
    }
}
</script>

<style scoped></style>