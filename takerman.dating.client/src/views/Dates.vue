<template>
    <div class="container-fluid">
        <div class="row px-xl-5">
            <div class="col-lg-3 col-md-4">
                <div class="text-left">
                    <label>Тип среща</label> <br />
                    <select id="frmDateType" placeholder="Тип среща" @change="applyFilter" v-model="filter.dateType"
                            class="form-control">
                        <option :value="0">{{ $t('dates.filter.all') }}</option>
                        <option v-for="dateType in dateTypes" :value="dateType.key">{{ dateType.value }}</option>
                    </select>
                </div>
                <br />
                <div class="row">
                    <div class="col text-left">
                        <label>Мин. години</label> <br />
                        <input id="frmMinAges" placeholder="Мин. години" @change="applyFilter" type="number" class="form-control" v-model="filter.minAges" />
                    </div>
                    <div class="col text-left">
                        <label>Макс. години</label> <br />
                        <input id="frmMaxAges" placeholder="Макс. години" @change="applyFilter" type="number" class="form-control" v-model="filter.maxAges" />
                    </div>
                </div>
                <br />
                <div class="text-left">
                    <label>Макс. цена {{ $t('common.currencySign') }}</label> <br />
                    <input id="frmMaxPrice" placeholder="Макс. цена" @change="applyFilter" type="number" class="form-control" v-model="filter.maxPrice" />
                </div>
            </div>
            <br />
            <div class="col-lg-9 col-md-8">
                <div class="row pb-3 card-deck">
                    <loader v-if="this.loading" />
                    <div v-else-if="!dates || dates.length == 0" class="text-center">
                        <h3>{{ $t('dates.status.nodates') }}</h3>
                    </div>
                    <div v-else>
                        <card :id="date?.id" v-for="(date, dateKey) in this.dates" :key="dateKey" />
                        <!-- <div class="col-12 pb-1">
                        <div class="d-flex align-items-center justify-content-between mb-4">
                            <div>
                                <button class="btn btn-sm btn-light"><i class="fa fa-th-large"></i></button>
                                <button class="btn btn-sm btn-light ml-2"><i class="fa fa-bars"></i></button>
                            </div>
                            <div class="ml-2">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-sm btn-light dropdown-toggle" data-toggle="dropdown">Sorting</button>
                                    <div class="dropdown-menu dropdown-menu-right">
                                        <router-link class="dropdown-item" to="/">Latest</router-link>
                                        <router-link class="dropdown-item" to="/">Popularity</router-link>
                                        <router-link class="dropdown-item" to="/">Best Rating</router-link>
                                    </div>
                                </div>
                                <div class="btn-group ml-2">
                                    <button type="button" class="btn btn-sm btn-light dropdown-toggle" data-toggle="dropdown">Showing</button>
                                    <div class="dropdown-menu dropdown-menu-right">
                                        <router-link class="dropdown-item" to="/">20</router-link>
                                        <router-link class="dropdown-item" to="/">50</router-link>
                                        <router-link class="dropdown-item" to="/">100</router-link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div> -->
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="js">
import card from '../components/Card.vue';
import loader from '../components/Loader.vue';
import cookies from '../helpers/cookies';
import { useAuth0 } from '@auth0/auth0-vue';

export default {
    setup(props) {
        debugger;
        const { user } = useAuth0();

        return {
            user: user,
            userId: user?.id,
            loading: false,
            dateTypes: [],
            filter: {
                minAges: null,
                maxAges: null,
                maxPrice: null,
                dateType: 0
            },
            dates: [],
            currency: ''
        }
    },
    components: {
        card,
        loader
    },
    async mounted() {
        await this.applyFilter();
        this.currency = cookies.get('currency');
        this.dateTypes = await fetch('Options/GetDateTypes');
    },
    methods: {
        async applyFilter() {
            try {
                this.loading = true;
                const { user, isAuthenticated } = useAuth0();
                if (isAuthenticated.value) {
                    this.dates = await (await fetch('Dates/GetAllAsCards' + (user == null ? '' : '?userId=' + user.id), this.filter)).json();
                }
                this.loading = false;
            } catch (error) {
                this.loading = false;
            }
        }
    }
}
</script>

<style scoped></style>