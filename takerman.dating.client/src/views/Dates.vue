<template>
    <div class="container-fluid">
        <div class="row px-xl-5">
            <div class="col-lg-3 col-md-4">
                <h5 class="section-title position-relative text-uppercase mb-3"><span class="bg-secondary pr-3">{{ $t('dates.filter.type') }}</span></h5>
                <div class="bg-light p-4 mb-30">
                    <form>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" checked id="price-all">
                            <label class="custom-control-label" for="price-all">All Price</label>
                            <span class="badge border font-weight-normal">1000</span>
                        </div>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" id="price-1">
                            <label class="custom-control-label" for="price-1">$0 - $100</label>
                            <span class="badge border font-weight-normal">150</span>
                        </div>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" id="price-2">
                            <label class="custom-control-label" for="price-2">$100 - $200</label>
                            <span class="badge border font-weight-normal">295</span>
                        </div>
                    </form>
                </div>
                <h5 class="section-title position-relative text-uppercase mb-3"><span class="bg-secondary pr-3">{{ $t('dates.filter.ethnicity') }}</span></h5>
                <div class="bg-light p-4 mb-30">
                    <form>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" checked id="color-all">
                            <label class="custom-control-label" for="price-all">All Color</label>
                            <span class="badge border font-weight-normal">1000</span>
                        </div>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" id="color-1">
                            <label class="custom-control-label" for="color-1">Black</label>
                            <span class="badge border font-weight-normal">150</span>
                        </div>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" id="color-2">
                            <label class="custom-control-label" for="color-2">White</label>
                            <span class="badge border font-weight-normal">295</span>
                        </div>
                    </form>
                </div>
                <h5 class="section-title position-relative text-uppercase mb-3"><span class="bg-secondary pr-3">{{ $t('dates.filter.ages') }}</span></h5>
                <div class="bg-light p-4 mb-30">
                    <form>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" checked id="size-all">
                            <label class="custom-control-label" for="size-all">All Size</label>
                            <span class="badge border font-weight-normal">1000</span>
                        </div>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" id="size-1">
                            <label class="custom-control-label" for="size-1">XS</label>
                            <span class="badge border font-weight-normal">150</span>
                        </div>
                        <div class="custom-control custom-checkbox d-flex align-items-center justify-content-between mb-3">
                            <input type="checkbox" class="custom-control-input" id="size-2">
                            <label class="custom-control-label" for="size-2">S</label>
                            <span class="badge border font-weight-normal">295</span>
                        </div>
                    </form>
                </div>
            </div>
            <div class="col-lg-9 col-md-8">
                <div class="row pb-3">
                    <div class="col-12 pb-1">
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
                    </div>
                    <loader v-if="this.loading" />
                    <div v-else-if="!dates || dates.length == 0" class="text-center">
                        <h3>{{ $t('dates.status.nodates') }}</h3>
                    </div>
                    <div v-else v-for="(date, dateKey) in this.dates" :key="dateKey" class="col-lg-4 col-md-6 col-sm-6 pb-1">
                        <div class="product-item bg-light mb-4">
                            <div class="product-img position-relative overflow-hidden">
                                <img class="img-fluid w-100" src="../assets/img/date/date-thumbnail.jpeg" alt="">
                                <div class="product-action">
                                    <router-link class="btn btn-outline-dark btn-square" to=""><i class="fa fa-shopping-cart"></i></router-link>
                                    <router-link class="btn btn-outline-dark btn-square" to=""><i class="far fa-heart"></i></router-link>
                                </div>
                            </div>
                            <div class="text-center py-4">
                                <router-link class="h6 text-decoration-none text-truncate" :to="'date?id=' + date.id">{{ date.title }}</router-link>
                                <div class="d-flex align-items-center justify-content-center mt-2">
                                    <table class="tbl tbl-fluid">
                                        <tr>
                                            <td>{{ $t('dates.card.ethnicity') }}:</td>
                                            <td><strong>{{ date.ethnicity }}</strong></td>
                                        </tr>
                                        <tr>
                                            <td>{{ $t('dates.card.price') }}:</td>
                                            <td><strong>{{ date.price }} {{ this.currency }}</strong></td>
                                        </tr>
                                        <tr>
                                            <td>{{ $t('dates.card.ages') }}:</td>
                                            <td><strong>{{ date.minAges }} - {{ date.maxAges }}</strong></td>
                                        </tr>
                                        <tr>
                                            <td>{{ $t('dates.card.menCount') }}:</td>
                                            <td><strong>{{ date.minMen }}/{{ date.menCount }}</strong></td>
                                        </tr>
                                        <tr>
                                            <td>{{ $t('dates.card.womenCount') }}:</td>
                                            <td><strong>{{ date.wominMen }}/{{ date.womenCount }}</strong></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-12">
                        <nav>
                            <ul class="pagination justify-content-center">
                                <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
                                <li class="page-item active"><a class="page-link" href="#">1</a></li>
                                <li class="page-item"><a class="page-link" href="#">2</a></li>
                                <li class="page-item"><a class="page-link" href="#">3</a></li>
                                <li class="page-item"><a class="page-link" href="#">Next</a></li>
                            </ul>
                        </nav>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import card from '../components/Card.vue';
import loader from '../components/Loader.vue';
import cookies from '../helpers/cookies';

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
            dates: [],
            currency: ''
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
        this.currency = cookies.get('currency');
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